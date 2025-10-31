# EagleViewEnt.Utilities

This repository tree contains the **EagleViewEnt.Utilities** platform, a collection of utilities developed by **EagleViewEnt**.

Each subfolder represents a dedicated GitHub repository with its own solution, README, license, and CI/CD workflows.

## Repositories

- **Core/** (`EagleViewEnt.Utilities.Core`)
  - Common Extentions, Types, Converters, Mapping 
- **External/** (`EagleViewEnt.Utilities.External`)
  - Common use external useful API's  
  - Example NOAAForcast api
- **Windows/** (`EagleViewEnt.Utilities.Windows`)
  - Windows specific customizations, Speech, Audion, Encryption 
- **Serialization/** (`EagleViewEnt.Utilities.Serialization`)
  - XML/Json serialization extensions
- **Security/** (`EagleViewEnt.Utilities.Security`)
  - Application authentication/authorization 
- **Media/** (`EagleViewEnt.Utilities.Media`)
  - Image manipulation tools
- **Localization/** (`EagleViewEnt.Utilities.Localization`)
  - Language/Localization tools
- **Data/** (`EagleViewEnt.Utilities.Data`)
  - Common types and extensions for database access
- **Testing/** (`EagleViewEnt.Utilities.Testing`)
  - Unit test types, extensions

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

