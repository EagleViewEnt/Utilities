# TaxStationPro

Modern .NET 9 platform for self-service tax collection and kiosk operations, built with Blazor (Server and RCLs), EF Core, and strong hardware integrations (bill acceptors, coin dispensers, scanners, printers, cameras, LED boards, etc.). This repo uses **Central Package Management** and GitHub Actions CI.

> Org: `EagleViewEnt` • Repo: `TaxStationPro`

---

## Contents

- [Getting Started](#getting-started)
- [Repo Layout](#repo-layout)
- [Build & Test](#build--test)
- [Central Package Management](#central-package-management)
- [NuGet Sources & Mapping](#nuget-sources--mapping)
- [Blazor Server vs WebAssembly](#blazor-server-vs-webassembly)
- [EF Core Migrations](#ef-core-migrations)
- [Contributing](#contributing)
- [License](#license)

---

## Getting Started

### Prereqs
- **.NET SDK 9.0** (latest 9.x)
- **Visual Studio 2022 17.14+** or JetBrains Rider
- Optional: **SQL Server** (Developer/Express) for data projects

### Quickstart
```bash
git clone https://github.com/EagleViewEnt/TaxStationPro.git
cd TaxStationPro

# First restore/build
dotnet restore
dotnet build -m:1

# Run the primary app (adjust path to your app project)
dotnet run --project src/Apps/TaxStationPro.App/TaxStationPro.App.csproj
