# EagleViewEnt.Utilities.Core

This repository tree contains the **EagleViewEnt.Utilities.Core** platform, a collection of utilities developed by **EagleViewEnt**.

Each subfolder represents a dedicated GitHub repository with its own solution, README, license, and CI/CD workflows.

## Repositories

- **Core/** (`EagleViewEnt.Utilities.Core`)
  - Common Extentions, Types, Converters, Mapping 

## Development

# Restore and build everything from the umbrella solution (if present)
```powershell
dotnet restore EagleViewEnt.Utilities-Full.sln
dotnet build   EagleViewEnt.Utilities.sln -c Release

```
# Or use solution filters (*.slnf) for faster scoped work
```powershell
dotnet restore EagleViewEnt.Utilities-Full.slnf
dotnet build   EagleViewEnt.Utilities-Full.slnf -c Release

