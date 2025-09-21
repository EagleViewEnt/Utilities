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
