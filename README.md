# EDITH Overdrive — Performance Cockpit

Curated engineering showcase of **EDITH Overdrive**, a Windows performance cockpit developed within EDITH Dev Studio.

> This repository is a portfolio-oriented public edition of a larger private project. It intentionally exposes selected architecture, engineering decisions and representative source samples while excluding private configuration, local data, internal planning material and unrelated implementation detail.

## What the project is

EDITH Overdrive is a Windows 11 desktop application focused on **low-overhead telemetry, diagnostics, benchmarking and performance-state visibility**, especially for gaming and second-screen use.

The engineering problem is not simply reading hardware values. The application has to collect heterogeneous system data without making the monitoring tool itself a meaningful source of performance degradation.

## Engineering focus

- low-overhead live telemetry
- asynchronous/concurrent data acquisition
- separation of fast and slow paths
- snapshot caching and polling control
- graceful degradation when sensors are unavailable
- local persistence and historical telemetry
- benchmark ingestion and comparison
- performance guardrails
- explicit, review-first maintenance actions
- automated tests around caching, lifecycle and failure behavior

## Stack

- C#
- .NET 8
- WinUI 3
- SQLite
- MVVM
- LibreHardwareMonitor
- PresentMon integration

## Core architecture

```text
Hardware / OS providers
        │
        ▼
Fast telemetry services
        │
        ├── snapshot cache
        ├── network telemetry
        └── storage telemetry
        │
        ▼
Application aggregation
        │
        ├── Dashboard
        ├── History
        └── Background logging

Slow path
        │
        ├── diagnostics
        ├── maintenance
        ├── update review
        └── reports
```

The live dashboard and heavier maintenance/diagnostic work are intentionally separated so expensive operations do not run on each UI refresh.

## Representative engineering decisions

### Shared telemetry snapshots

The dashboard and background logger reuse a shared hardware snapshot instead of polling the same hardware independently. A refresh gate prevents concurrent refreshes from multiplying provider work.

### Missing data is normal

A missing or unsupported sensor does not cause the application to fail. Partial telemetry is represented explicitly as unavailable rather than replaced with fabricated values.

### Performance is a requirement

The project includes regression-oriented tests that verify cache reuse and protect the intended low-overhead architecture.

### Real measurements vs approximations

The UI distinguishes measured signals from derived or limited signals. Diagnostic estimates and unavailable data are not presented as stronger evidence than they are.

## Local baseline

A Windows 11 local reference sample recorded during development, with the default live dashboard refresh and no active benchmark capture, measured approximately:

- process CPU: **0.47% average** over a short post-warm-up idle sample
- working set: **263.5 MB**
- private memory: **191.9 MB**

These values are a development reference, not a guaranteed ceiling. Actual overhead depends on hardware, drivers, sensor availability, refresh cadence and benchmark capture state.

## Selected source

The `samples/` directory contains representative source taken from the private project:

- `FastTelemetryService.cs` — live telemetry aggregation, TTL caching and refresh serialization
- `TelemetrySnapshotCache.cs` — thread-safe shared snapshot storage

The private project still contains legacy `SentinelPC` namespaces from the previous project name. They are preserved in the samples so the source remains faithful to the implementation.

## Documentation

- [Architecture](docs/ARCHITECTURE.md)
- [Verification and engineering evidence](docs/VERIFICATION.md)
- [Public showcase scope](docs/PUBLIC_SCOPE.md)

## Related

- Engineering portfolio: https://github.com/Aceishere66/engineering-portfolio
- EDITH Dev Studio engineering page: https://edithdevstudio.com/engineering
