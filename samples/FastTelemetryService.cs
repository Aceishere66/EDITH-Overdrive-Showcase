using SentinelPC.Application.Abstractions;
using SentinelPC.Domain.Dashboard;
using SentinelPC.Domain.Sensors;

namespace SentinelPC.Application.Telemetry;

public sealed class FastTelemetryService(
    ISensorSnapshotService sensorSnapshotService,
    INetworkTelemetryProvider networkTelemetryProvider,
    IStorageTelemetryProvider storageTelemetryProvider,
    ITelemetrySnapshotCache telemetrySnapshotCache)
{
    private static readonly TimeSpan DashboardCacheTtl = TimeSpan.FromMilliseconds(900);
    private static readonly TimeSpan LoggerCacheTtl = TimeSpan.FromSeconds(2);
    private readonly SemaphoreSlim _refreshGate = new(1, 1);

    public async Task<FastTelemetrySnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default)
    {
        var cached = telemetrySnapshotCache.GetLatest();
        if (cached is not null && DateTimeOffset.UtcNow - cached.FastTelemetrySnapshot.Timestamp <= DashboardCacheTtl)
        {
            return cached.FastTelemetrySnapshot;
        }

        return await RefreshAsync(cancellationToken);
    }

    public async Task<SensorSnapshot> GetSharedSensorSnapshotAsync(CancellationToken cancellationToken = default)
    {
        var cached = telemetrySnapshotCache.GetLatest();
        if (cached is not null && DateTimeOffset.UtcNow - cached.RawSensorSnapshot.Timestamp <= LoggerCacheTtl)
        {
            return cached.RawSensorSnapshot;
        }

        return (await RefreshCoreAsync(cancellationToken)).RawSensorSnapshot;
    }

    public async Task<FastTelemetrySnapshot> RefreshAsync(CancellationToken cancellationToken = default)
        => (await RefreshCoreAsync(cancellationToken)).FastTelemetrySnapshot;

    private async Task<TelemetryCacheEntry> RefreshCoreAsync(CancellationToken cancellationToken)
    {
        await _refreshGate.WaitAsync(cancellationToken);
        try
        {
            var cached = telemetrySnapshotCache.GetLatest();
            if (cached is not null && DateTimeOffset.UtcNow - cached.FastTelemetrySnapshot.Timestamp <= DashboardCacheTtl)
            {
                return cached;
            }

            var sensorSnapshot = await sensorSnapshotService.CaptureAsync(cancellationToken);
            var networkSnapshot = await networkTelemetryProvider.CaptureAsync(cancellationToken);
            var storageSnapshot = await storageTelemetryProvider.CaptureAsync(cancellationToken);

            var fastTelemetrySnapshot = new FastTelemetrySnapshot(
                sensorSnapshot.Timestamp,
                BuildCards(sensorSnapshot, networkSnapshot, storageSnapshot),
                networkSnapshot);

            var entry = new TelemetryCacheEntry(sensorSnapshot, fastTelemetrySnapshot);
            telemetrySnapshotCache.Store(entry);
            return entry;
        }
        finally
        {
            _refreshGate.Release();
        }
    }

    // The complete private implementation also contains the card-building and
    // sensor-selection logic. This showcase keeps the concurrency/caching core
    // because it is the architecture-relevant part of the sample.
    private static IReadOnlyList<HealthCard> BuildCards(
        SensorSnapshot sensorSnapshot,
        NetworkTelemetrySnapshot networkSnapshot,
        StorageTelemetrySnapshot storageSnapshot)
        => [];
}
