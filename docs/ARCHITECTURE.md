# Architecture

## Design objective

EDITH Overdrive is designed around a simple constraint:

> A performance-monitoring application should not become a meaningful source of performance loss.

The codebase therefore separates frequently refreshed telemetry from heavier system inspection.

## Layering

```text
Presentation
    ↓
Application
    ↓
Domain abstractions
    ↑
Infrastructure providers
```

### Domain

Models, enums and contracts. It does not own UI or platform-specific implementations.

### Application

Coordinates use cases and prepares snapshots for consumers.

### Infrastructure

Contains Windows/hardware integrations, persistence, diagnostics and benchmark adapters.

### Presentation

Consumes prepared application state. Heavy platform queries do not belong directly in view code.

## Fast path

The fast path serves live telemetry.

Typical inputs:

- CPU/GPU sensor readings
- RAM/VRAM
- storage state
- network throughput
- lightweight foreground-session context

Key rules:

- reuse snapshots
- avoid duplicate hardware polling
- keep refresh cadence configurable
- do not run event-log scans, package review or heavy diagnostics inside dashboard refresh
- return partial state when a provider is unavailable

## Slow path

Heavier work is isolated and refreshed independently:

- diagnostics
- update review
- maintenance scans
- report generation
- broad process/system inspection

Slow-path state can be cached for longer because freshness requirements differ from live telemetry.

## Snapshot flow

```text
LibreHardwareMonitor
        │
        ▼
SensorSnapshot
        │
        ▼
FastTelemetryService
        │
        ├── NetworkTelemetryProvider
        ├── StorageTelemetryProvider
        └── TelemetrySnapshotCache
        │
        ▼
FastTelemetrySnapshot
        │
        ├── Dashboard
        ├── History
        └── Background logger
```

## Concurrency

The fast telemetry service uses a refresh gate so multiple consumers requesting stale data at the same time do not trigger multiple expensive refreshes.

Snapshot storage is synchronized and intentionally small.

## Failure model

The application prefers degraded operation over hard failure:

- unsupported sensor → unavailable value
- inaccessible event log → remaining diagnostics still work
- missing benchmark adapter → session metadata remains valid
- unavailable package manager → update review becomes limited
- provider failure → isolate and report instead of inventing data

## Transferable systems-engineering themes

Although EDITH Overdrive is a desktop application rather than an automotive system, the engineering work is directly useful for any software that must manage heterogeneous live data:

- asynchronous acquisition
- freshness and cache semantics
- bounded polling
- concurrency control
- provider abstraction
- partial-data handling
- observability
- testable data pipelines
