# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2026-10-21

### Changed

- Rebuilt the desktop client on **standard Windows Forms**, removing the
  commercial UI suite whose assemblies were not shipped with the release.
- The API key is now resolved at runtime instead of being compiled into the
  binary, with a first-run dialog for entering it.

### Fixed

- **The application now starts.** The `v1.0.0` executable referenced the UI
  suite assemblies that were not included in the release, so it terminated
  before showing its window.
- Removed the stale Google TTS fallback so the documented speech engine matches
  the code.
- Corrected the release asset name and refreshed the update date.

### Removed

- The `v1.0.0` release and its tag, which could not start.
