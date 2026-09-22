using SentinelPC.Application.Abstractions;
using SentinelPC.Application.Telemetry;

namespace SentinelPC.Infrastructure.Sensors;

public sealed class TelemetrySnapshotCache : ITelemetrySnapshotCache
{
    private readonly object _sync = new();
    private TelemetryCacheEntry? _latest;

    public TelemetryCacheEntry? GetLatest()
    {
        lock (_sync)
        {
            return _latest;
        }
    }

    public void Store(TelemetryCacheEntry entry)
    {
        lock (_sync)
        {
            _latest = entry;
        }
    }
}
