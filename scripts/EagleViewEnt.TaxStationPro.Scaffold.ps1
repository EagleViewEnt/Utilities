param(
  [string]$Root = 'D:\Development\EagleViewEnt\TaxStationPro',
  [switch]$Zip
)

$ErrorActionPreference = 'Stop'

function New-Dir([string]$p) { if (-not (Test-Path $p)) { New-Item -ItemType Directory -Force -Path $p | Out-Null } }
function Write-Text([string]$path, [string]$content) { New-Dir (Split-Path -Parent $path); Set-Content -Path $path -Value $content -Encoding UTF8 }

# ---------- Common text ----------
$LICENSE_TXT = @'
LICENSE (Proprietary)

Copyright (c) 2025 EagleViewEnt. All rights reserved.

1) Ownership
This software (the "Software") is the exclusive property of EagleViewEnt.

2) Permitted Use
Use is permitted solely within the scope of agreements with EagleViewEnt.
No license is granted to copy, modify, merge, publish, distribute, sublicense, sell, or create derivative works outside authorized agreements.

3) Derivative Works
Any modifications or derivative works become the exclusive property of EagleViewEnt.

4) Disclaimer of Warranty
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED.

5) Limitation of Liability
In no event shall EagleViewEnt be liable for any claim, damages, or other liability arising from the Software.

6) Governing Law
This License is governed by the laws applicable to agreements with EagleViewEnt.

Proprietary and Confidential — Unauthorized use is prohibited.
'@

$DIR_PACKAGES_PROPS = @'
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    <CentralPackageTransitivePinningEnabled>true</CentralPackageTransitivePinningEnabled>
  </PropertyGroup>
  <ItemGroup>
    <PackageVersion Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="9.0.9" />
    <PackageVersion Include="Microsoft.Extensions.Options" Version="9.0.9" />
    <PackageVersion Include="Microsoft.Extensions.Options.ConfigurationExtensions" Version="9.0.9" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="9.0.9" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.9" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.9" />
    <PackageVersion Include="YamlDotNet" Version="15.1.4" />
    <PackageVersion Include="xunit" Version="2.9.2" />
    <PackageVersion Include="xunit.runner.visualstudio" Version="3.0.0" />
    <PackageVersion Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
  </ItemGroup>
</Project>
'@

$BUILD_YML = @'
name: CI Build
on:
  push: { branches: [ main ] }
  pull_request: { branches: [ main ] }
jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with: { dotnet-version: 9.0.x }
      - name: Restore
        run: dotnet restore
      - name: Build
        run: dotnet build -c Release --no-restore
      - name: Test
        run: dotnet test -c Release --no-build
'@

$RELEASE_YML = @'
name: Release
on: { push: { tags: [ "v*.*.*" ] } }
jobs:
  publish:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with: { dotnet-version: 9.0.x }
      - name: Restore
        run: dotnet restore
      - name: Publish
        run: dotnet publish ./src/EagleViewEnt.TaxStationPro.App/EagleViewEnt.TaxStationPro.App.csproj -c Release -o artifacts
      - name: Upload artifact
        uses: actions/upload-artifact@v4
        with: { name: TaxStationPro_App_Publish, path: artifacts/** }
'@

$PACK_YML = @'
name: Pack and Publish
on:
  push: { tags: [ "v*.*.*" ] }
  workflow_dispatch: {}
jobs:
  pack:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with: { dotnet-version: 9.0.x }
      - name: Restore
        run: dotnet restore
      - name: Build
        run: dotnet build -c Release --no-restore
      - name: Test
        run: dotnet test -c Release --no-build
      - name: Pack libraries
        run: |
          Get-ChildItem src -Recurse -Filter *.csproj | ForEach-Object {
            dotnet pack $_.FullName -c Release -o artifacts --no-build
          }
      - name: Push to GitHub Packages
        env: { NUGET_AUTH_TOKEN: ${{ secrets.GITHUB_TOKEN }} }
        run: |
          dotnet nuget push "artifacts\*.nupkg" --api-key "$env:NUGET_AUTH_TOKEN" --source "https://nuget.pkg.github.com/EagleViewEnt/index.json" --skip-duplicate
'@

$NUGET_CONFIG_APP = @'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="github" value="https://nuget.pkg.github.com/EagleViewEnt/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="EagleViewEnt" />
      <add key="ClearTextPassword" value="YOUR_PERSONAL_ACCESS_TOKEN" />
    </github>
  </packageSourceCredentials>
</configuration>
'@

# ---------- Helpers to create projects ----------
function Ensure-Platforms([string]$csprojPath) {
  [xml]$xml = Get-Content $csprojPath
  $ns = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
  $pg = $xml.Project.PropertyGroup | Select-Object -First 1
  if (-not $pg) { $pg = $xml.CreateElement("PropertyGroup"); $xml.Project.AppendChild($pg) | Out-Null }
  $plat = $pg.Platforms
  if (-not $plat) { $plat = $xml.CreateElement("Platforms"); $pg.AppendChild($plat) | Out-Null }
  $plat.InnerText = "AnyCPU;x64"
  $xml.Save($csprojPath)
}

function New-ClassLib([string]$path, [string]$name) {
  New-Dir $path
  dotnet new classlib -f net9.0 -n $name -o $path --no-restore | Out-Null
  Ensure-Platforms (Join-Path $path "$name.csproj")
}
function New-RazorLib([string]$path, [string]$name) {
  New-Dir $path
  dotnet new razorclasslib -f net9.0 -n $name -o $path --no-restore | Out-Null
  Ensure-Platforms (Join-Path $path "$name.csproj")
}
function New-BlazorServer([string]$path, [string]$name) {
  New-Dir $path
  dotnet new blazor --interactivity Server -f net9.0 -n $name -o $path --no-restore | Out-Null
  Ensure-Platforms (Join-Path $path "$name.csproj")
}

function New-XUnit([string]$path, [string]$name) {
  New-Dir $path
  dotnet new xunit -f net9.0  -n $name -o $path --no-restore | Out-Null
  # Add common test packages (central versions handle versions)
  dotnet add (Join-Path $path "$name.csproj") package xunit.runner.visualstudio | Out-Null
  dotnet add (Join-Path $path "$name.csproj") package Microsoft.NET.Test.Sdk | Out-Null
  Ensure-Platforms (Join-Path $path "$name.csproj")
}

function New-Solution([string]$repoRoot, [string]$slnName) {
  Push-Location $repoRoot
  if (Test-Path $slnName) { Remove-Item $slnName -Force }
  dotnet new sln -n ($slnName -replace '\.sln$','') | Out-Null
  Pop-Location
}
function Add-ToSolution([string]$repoRoot, [string]$slnName, [string[]]$projects) {
  Push-Location $repoRoot
  foreach ($p in $projects) { dotnet sln $slnName add $p | Out-Null }
  Pop-Location
}

Write-Host "Creating scaffold under: $Root" -ForegroundColor Cyan
New-Dir $Root

# =====================================
# APP REPO
# =====================================
$app = Join-Path $Root 'App'
New-Dir $app
Write-Text (Join-Path $app 'Directory.Packages.props') $DIR_PACKAGES_PROPS
Write-Text (Join-Path $app 'LICENSE.txt') $LICENSE_TXT
Write-Text (Join-Path $app 'NuGet.config') $NUGET_CONFIG_APP
Write-Text (Join-Path $app '.github\workflows\build.yml') $BUILD_YML
Write-Text (Join-Path $app '.github\workflows\release.yml') $RELEASE_YML

# Projects
$appProjPath = Join-Path $app 'src\EagleViewEnt.TaxStationPro.App'
$appAbsPath  = Join-Path $app 'src\EagleViewEnt.TaxStationPro.App.Abstractions'
$appTests    = Join-Path $app 'tests\EagleViewEnt.TaxStationPro.App.Tests'
$appAbsTests = Join-Path $app 'tests\EagleViewEnt.TaxStationPro.App.Abstractions.Tests'

New-BlazorServer $appProjPath  'EagleViewEnt.TaxStationPro.App'
New-ClassLib    $appAbsPath    'EagleViewEnt.TaxStationPro.App.Abstractions'
New-XUnit       $appTests      'EagleViewEnt.TaxStationPro.App.Tests'
New-XUnit       $appAbsTests   'EagleViewEnt.TaxStationPro.App.Abstractions.Tests'

# Wire references
dotnet add (Join-Path $appProjPath 'EagleViewEnt.TaxStationPro.App.csproj') reference (Join-Path $appAbsPath 'EagleViewEnt.TaxStationPro.App.Abstractions.csproj') | Out-Null
dotnet add (Join-Path $appTests    'EagleViewEnt.TaxStationPro.App.Tests.csproj') reference (Join-Path $appProjPath 'EagleViewEnt.TaxStationPro.App.csproj') | Out-Null
dotnet add (Join-Path $appAbsTests 'EagleViewEnt.TaxStationPro.App.Abstractions.Tests.csproj') reference (Join-Path $appAbsPath 'EagleViewEnt.TaxStationPro.App.Abstractions.csproj') | Out-Null

# Solution
New-Solution $app 'TaxStationPro.sln'
Add-ToSolution $app 'TaxStationPro.sln' @(
  'src\EagleViewEnt.TaxStationPro.App\EagleViewEnt.TaxStationPro.App.csproj',
  'src\EagleViewEnt.TaxStationPro.App.Abstractions\EagleViewEnt.TaxStationPro.App.Abstractions.csproj',
  'tests\EagleViewEnt.TaxStationPro.App.Tests\EagleViewEnt.TaxStationPro.App.Tests.csproj',
  'tests\EagleViewEnt.TaxStationPro.App.Abstractions.Tests\EagleViewEnt.TaxStationPro.App.Abstractions.Tests.csproj'
)

# =====================================
# VENDORS\KIOSK REPO (classlibs)
# =====================================
$vk = Join-Path $Root 'Vendors\Kiosk'
New-Dir $vk
Write-Text (Join-Path $vk 'Directory.Packages.props') $DIR_PACKAGES_PROPS
Write-Text (Join-Path $vk 'LICENSE.txt') $LICENSE_TXT
Write-Text (Join-Path $vk '.github\workflows\pack.yml') $PACK_YML

$vkBase = 'src\EagleViewEnt.TaxStationPro.Vendors\Kiosk'
$vkLibs = @('Abstractions','Core','Cash','Imaging','DocScan')
$vkProjPaths = @()
$vkTestPaths = @()

foreach ($lib in $vkLibs) {
  $p = Join-Path $vk "$vkBase\$lib"
  $n = "EagleViewEnt.TaxStationPro.Vendors.Kiosk.$lib"
  New-ClassLib $p $n
  $vkProjPaths += "$p\$n.csproj"

  $tp = Join-Path $vk ("tests\{0}.Tests" -f $n)
  $tn = "$n.Tests"
  New-XUnit $tp $tn
  $vkTestPaths += "$tp\$tn.csproj"

  dotnet add "$tp\$tn.csproj" reference "$p\$n.csproj" | Out-Null
}

New-Solution $vk 'EagleViewEnt.TaxStationPro.Vendors.Kiosk.sln'
Add-ToSolution $vk 'EagleViewEnt.TaxStationPro.Vendors.Kiosk.sln' ($vkProjPaths + $vkTestPaths)

# =====================================
# VENDORS\POINTANDPAY REPO (classlibs)
# =====================================
$vpnp = Join-Path $Root 'Vendors\PointAndPay'
New-Dir $vpnp
Write-Text (Join-Path $vpnp 'Directory.Packages.props') $DIR_PACKAGES_PROPS
Write-Text (Join-Path $vpnp 'LICENSE.txt') $LICENSE_TXT
Write-Text (Join-Path $vpnp '.github\workflows\pack.yml') $PACK_YML

$ppBase = 'src\EagleViewEnt.TaxStationPro.Vendors\PointAndPay'
$ppLibs = @('Abstractions','Core','Ach','Card')
$ppProjPaths = @()
$ppTestPaths = @()

foreach ($lib in $ppLibs) {
  $p = Join-Path $vpnp "$ppBase\$lib"
  $n = "EagleViewEnt.TaxStationPro.Vendors.PointAndPay.$lib"
  New-ClassLib $p $n
  $ppProjPaths += "$p\$n.csproj"

  $tp = Join-Path $vpnp ("tests\{0}.Tests" -f $n)
  $tn = "$n.Tests"
  New-XUnit $tp $tn
  $ppTestPaths += "$tp\$tn.csproj"

  dotnet add "$tp\$tn.csproj" reference "$p\$n.csproj" | Out-Null
}

New-Solution $vpnp 'EagleViewEnt.TaxStationPro.Vendors.PointAndPay.sln'
Add-ToSolution $vpnp 'EagleViewEnt.TaxStationPro.Vendors.PointAndPay.sln' ($ppProjPaths + $ppTestPaths)

# =====================================
# CUSTOMERS\RIVERSIDE (Components = RCL; others = classlibs)
# =====================================
$cr = Join-Path $Root 'Customers\Riverside'
New-Dir $cr
Write-Text (Join-Path $cr 'Directory.Packages.props') $DIR_PACKAGES_PROPS
Write-Text (Join-Path $cr 'LICENSE.txt') $LICENSE_TXT
Write-Text (Join-Path $cr '.github\workflows\pack.yml') $PACK_YML

$crBase = 'src\EagleViewEnt.TaxStationPro.Customers\Riverside'
$crSpecs = @(
  @{ Name='Data';          Kind='classlib' },
  @{ Name='Components';    Kind='razorlib' },
  @{ Name='Resources';     Kind='classlib' },
  @{ Name='Bootstrap';     Kind='classlib' },
  @{ Name='Customizations';Kind='classlib' }
)
$crProjPaths=@(); $crTestPaths=@()

foreach ($it in $crSpecs) {
  $p = Join-Path $cr "$crBase\$($it.Name)"
  $n = "EagleViewEnt.TaxStationPro.Customers.Riverside.$($it.Name)"
  if ($it.Kind -eq 'razorlib') { New-RazorLib $p $n } else { New-ClassLib $p $n }
  $crProjPaths += "$p\$n.csproj"

  $tp = Join-Path $cr ("tests\{0}.Tests" -f $n)
  $tn = "$n.Tests"
  New-XUnit $tp $tn
  $crTestPaths += "$tp\$tn.csproj"
  dotnet add "$tp\$tn.csproj" reference "$p\$n.csproj" | Out-Null
}

# Suggested internal references (Bootstrap depends on others)
dotnet add (Join-Path $cr "$crBase\Bootstrap\EagleViewEnt.TaxStationPro.Customers.Riverside.Bootstrap.csproj") reference `
  (Join-Path $cr "$crBase\Data\EagleViewEnt.TaxStationPro.Customers.Riverside.Data.csproj") `
  (Join-Path $cr "$crBase\Resources\EagleViewEnt.TaxStationPro.Customers.Riverside.Resources.csproj") `
  | Out-Null

New-Solution $cr 'EagleViewEnt.TaxStationPro.Customers.Riverside.sln'
Add-ToSolution $cr 'EagleViewEnt.TaxStationPro.Customers.Riverside.sln' ($crProjPaths + $crTestPaths)

# =====================================
# CUSTOMERS\MERCED (Components = RCL; others = classlibs)
# =====================================
$cm = Join-Path $Root 'Customers\Merced'
New-Dir $cm
Write-Text (Join-Path $cm 'Directory.Packages.props') $DIR_PACKAGES_PROPS
Write-Text (Join-Path $cm 'LICENSE.txt') $LICENSE_TXT
Write-Text (Join-Path $cm '.github\workflows\pack.yml') $PACK_YML

$cmBase = 'src\EagleViewEnt.TaxStationPro.Customers\Merced'
$cmSpecs = @(
  @{ Name='Data';       Kind='classlib' },
  @{ Name='Components'; Kind='razorlib' },
  @{ Name='Resources';  Kind='classlib' },
  @{ Name='Bootstrap';  Kind='classlib' }
)
$cmProjPaths=@(); $cmTestPaths=@()

foreach ($it in $cmSpecs) {
  $p = Join-Path $cm "$cmBase\$($it.Name)"
  $n = "EagleViewEnt.TaxStationPro.Customers.Merced.$($it.Name)"
  if ($it.Kind -eq 'razorlib') { New-RazorLib $p $n } else { New-ClassLib $p $n }
  $cmProjPaths += "$p\$n.csproj"

  $tp = Join-Path $cm ("tests\{0}.Tests" -f $n)
  $tn = "$n.Tests"
  New-XUnit $tp $tn
  $cmTestPaths += "$tp\$tn.csproj"
  dotnet add "$tp\$tn.csproj" reference "$p\$n.csproj" | Out-Null
}

# Suggested internal references (Bootstrap depends on others)
dotnet add (Join-Path $cm "$cmBase\Bootstrap\EagleViewEnt.TaxStationPro.Customers.Merced.Bootstrap.csproj") reference `
  (Join-Path $cm "$cmBase\Data\EagleViewEnt.TaxStationPro.Customers.Merced.Data.csproj") `
  (Join-Path $cm "$cmBase\Resources\EagleViewEnt.TaxStationPro.Customers.Merced.Resources.csproj") `
  | Out-Null

New-Solution $cm 'EagleViewEnt.TaxStationPro.Customers.Merced.sln'
Add-ToSolution $cm 'EagleViewEnt.TaxStationPro.Customers.Merced.sln' ($cmProjPaths + $cmTestPaths)

# =====================================
# PROFILES (classlibs)
# =====================================
$prof = Join-Path $Root 'Profiles'
New-Dir $prof
Write-Text (Join-Path $prof 'Directory.Packages.props') $DIR_PACKAGES_PROPS
Write-Text (Join-Path $prof 'LICENSE.txt') $LICENSE_TXT
Write-Text (Join-Path $prof '.github\workflows\pack.yml') $PACK_YML

$prBase = 'src\EagleViewEnt.TaxStationPro.Profiles'
$prNames = @('Riverside','Merced')
$prProjPaths=@(); $prTestPaths=@()

foreach ($c in $prNames) {
  $p = Join-Path $prof "$prBase\$c"
  $n = "EagleViewEnt.TaxStationPro.Profiles.$c"
  New-ClassLib $p $n
  $prProjPaths += "$p\$n.csproj"

  $tp = Join-Path $prof ("tests\{0}.Tests" -f $n)
  $tn = "$n.Tests"
  New-XUnit $tp $tn
  $prTestPaths += "$tp\$tn.csproj"
  dotnet add "$tp\$tn.csproj" reference "$p\$n.csproj" | Out-Null
}

New-Solution $prof 'EagleViewEnt.TaxStationPro.Profiles.TaxStationPro.sln'
Add-ToSolution $prof 'EagleViewEnt.TaxStationPro.Profiles.TaxStationPro.sln' ($prProjPaths + $prTestPaths)

# =====================================
# ROOT umbrella solution + filter
# =====================================
$rootScripts = Join-Path $Root 'scripts'
New-Dir $rootScripts
$CREATE_FULL_SOLUTION_PS1 = @'
$ErrorActionPreference = "Stop"
function Get-RelativePath([string]$BasePath,[string]$TargetPath){
  $baseUri=[System.Uri]((Resolve-Path $BasePath).Path.TrimEnd('\')+'\'); $targetUri=[System.Uri]((Resolve-Path $TargetPath).Path)
  ($baseUri.MakeRelativeUri($targetUri).ToString()) -replace '/', '\'
}
$root = Split-Path $PSScriptRoot -Parent
$slnName = "TaxStationPro-Full.sln"
$slnPath = Join-Path $root $slnName
$slnfPath = Join-Path $root "TaxStationPro-Full.slnf"
if (Test-Path $slnPath) { Remove-Item $slnPath -Force }
dotnet new sln -n "TaxStationPro-Full" -o $root | Out-Null
$projects = Get-ChildItem -Path $root -Recurse -Filter *.csproj | Where-Object { $_.FullName -notmatch '\\(bin|obj|\.git)\\' }
foreach ($proj in $projects) {
  $rel = Get-RelativePath $root $proj.FullName
  $folder = "Projects"; if ($rel -match 'src\\([^\\]+)') { $folder = $Matches[1] }
  dotnet sln $slnPath add $proj.FullName --solution-folder $folder | Out-Null
  Write-Host ("Added -> [{0}] {1}" -f $folder, $rel)
}
$relProjects = @(); foreach ($proj in $projects) { $relProjects += (Get-RelativePath $root $proj.FullName) }
$relProjects = $relProjects | Sort-Object -Unique
$slnf = @{ solution = @{ path = [IO.Path]::GetFileName($slnPath); projects = $relProjects } } | ConvertTo-Json -Depth 5
Set-Content -Path $slnfPath -Value $slnf -Encoding UTF8
Write-Host "Created: $slnPath"; Write-Host "Created: $slnfPath"
'@
Write-Text (Join-Path $rootScripts 'create-full-solution.ps1') $CREATE_FULL_SOLUTION_PS1

Write-Host "`nDONE. Scaffold created under: $Root" -ForegroundColor Green

# Optional zips
if ($Zip) {
  Add-Type -AssemblyName System.IO.Compression.FileSystem
  function New-Zip([string]$folder,[string]$zipPath){ if (Test-Path $zipPath){Remove-Item $zipPath -Force}; [System.IO.Compression.ZipFile]::CreateFromDirectory($folder,$zipPath) }
  New-Zip (Join-Path $Root 'App') (Join-Path $Root 'EagleViewEnt.TaxStationPro_App.zip')
  New-Zip (Join-Path $Root 'Vendors\Kiosk') (Join-Path $Root 'EagleViewEnt.TaxStationPro_Vendors.Kiosk.zip')
  New-Zip (Join-Path $Root 'Vendors\PointAndPay') (Join-Path $Root 'EagleViewEnt.TaxStationPro_Vendors.PointAndPay.zip')
  New-Zip (Join-Path $Root 'Customers\Riverside') (Join-Path $Root 'EagleViewEnt.TaxStationPro_Customers.Riverside.zip')
  New-Zip (Join-Path $Root 'Customers\Merced') (Join-Path $Root 'EagleViewEnt.TaxStationPro_Customers.Merced.zip')
  New-Zip (Join-Path $Root 'Profiles') (Join-Path $Root 'EagleViewEnt.TaxStationPro_Profiles.zip')
  Write-Host "Zips created next to repos."
}
