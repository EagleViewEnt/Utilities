param(
  [string]$Root = 'D:\Development\EagleViewEnt\TaxStationPro',
  [switch]$Overwrite
)

$ErrorActionPreference = 'Stop'

function New-Dir($p) { if (-not (Test-Path $p)) { New-Item -ItemType Directory -Force -Path $p | Out-Null } }
function Write-IfAllowed([string]$path, [string]$content, [bool]$overwrite) {
  New-Dir (Split-Path -Parent $path)
  if ((-not (Test-Path $path)) -or $overwrite) {
    Set-Content -Path $path -Value $content -Encoding UTF8
    Write-Host "Wrote: $path"
  } else {
    Write-Host "Skipped (exists): $path"
  }
}

# ---- Shared NOTICE for all repos and root ----
$NOTICE = @'
# NOTICE

Copyright (c) 2025 EagleViewEnt.
All rights reserved.

This repository is part of the EagleViewEnt **TaxStationPro** platform and may contain components licensed under separate terms by their respective authors.

Where applicable, third-party components are credited below. If you believe attribution is missing or incomplete, please contact EagleViewEnt.

## Third-Party Notices

- Microsoft .NET SDKs and packages © Microsoft Corporation — Licensed under their respective licenses.
- YamlDotNet © Contributors — Licensed under the MIT License.
- Other transitive dependencies — See package metadata in `Directory.Packages.props` and NuGet package pages.

This software and all modifications remain the exclusive property of EagleViewEnt per the accompanying `LICENSE.txt`.
'@

# ---- Root README (umbrella) ----
$Readme_Root = @'
# EagleViewEnt.TaxStationPro (Umbrella)

This repository tree contains the **TaxStationPro** platform, a Blazor Server–based tax payment kiosk solution developed by **EagleViewEnt**.

Each subfolder represents a dedicated GitHub repository with its own solution, README, license, and CI/CD workflows.

## Repositories

- **App/**
  - Main application (`EagleViewEnt.TaxStationPro.App`)
- **Vendors/**
  - Vendor-specific integrations  
  - Example: `EagleViewEnt.TaxStationPro.Vendors.Kiosk`, `EagleViewEnt.TaxStationPro.Vendors.PointAndPay`
- **Customers/**
  - Customer-specific modules and customizations  
  - Example: `Riverside`, `Merced`
- **Profiles/**
  - Profiles to connect applications, vendors, and customers

## Development

```powershell
# Restore and build everything from the umbrella solution (if present)
dotnet restore TaxStationPro-Full.sln
dotnet build   TaxStationPro-Full.sln -c Release

# Or use solution filters (*.slnf) for faster scoped work
'@