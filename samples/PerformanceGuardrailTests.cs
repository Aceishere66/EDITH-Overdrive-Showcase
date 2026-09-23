using SentinelPC.Application.Telemetry;

namespace SentinelPC.Tests;

/// <summary>
/// Curated excerpts from the private EDITH Overdrive regression suite.
/// These tests demonstrate the low-overhead invariants protected by the project.
/// </summary>
public sealed class PerformanceGuardrailTests
{
    [Fact]
    public async Task FastTelemetrySnapshot_ReusesDashboardCacheWithinTtl()
    {
        var sensorProvider = new CountingSensorSnapshotService();
        var networkProvider = new CountingNetworkTelemetryProvider();
        var storageProvider = new CountingStorageTelemetryProvider();

        var service = new FastTelemetryService(
            sensorProvider,
            networkProvider,
            storageProvider,
            new StubTelemetrySnapshotCache());

        _ = await service.GetSnapshotAsync();
        _ = await service.GetSnapshotAsync();

        Assert.Equal(1, sensorProvider.CaptureCount);
        Assert.Equal(1, networkProvider.CaptureCount);
        Assert.Equal(1, storageProvider.CaptureCount);
    }

    [Fact]
    public async Task SharedSensorSnapshot_ReusesFreshTelemetrySnapshotWithoutSecondHardwarePoll()
    {
        var sensorProvider = new CountingSensorSnapshotService();

        var service = new FastTelemetryService(
            sensorProvider,
            new CountingNetworkTelemetryProvider(),
            new CountingStorageTelemetryProvider(),
            new StubTelemetrySnapshotCache());

        _ = await service.GetSnapshotAsync();
        _ = await service.GetSharedSensorSnapshotAsync();

        Assert.Equal(1, sensorProvider.CaptureCount);
    }
}

// Helper fakes/stubs live in the private test suite.
// This public file is an excerpt intended to show the verified invariant.
