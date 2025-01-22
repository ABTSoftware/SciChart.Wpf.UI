param ([Parameter(Mandatory=$true)][string]$version)

$currentDir = Get-Location
$deployDir = "$PSScriptRoot"
$slnDir = "$PSScriptRoot/.."
$packagesDir = "$slnDir/Packages"

# Switch to the Deployment folder
Set-Location $deployDir

# Import the SafeRemoveDir function
. ./SafeRemoveDir.ps1

# Clean the root packages directory
Write-Host "Clearing folders..."
SafeRemoveDir $packagesDir

function PackNuget {
    param (
        [string]$project,
        [string]$nuspec
    )
	
    dotnet pack -c:Release "$slnDir/$project/$project.csproj" "-p:NuspecFile=$slnDir/$nuspec.nuspec" "-p:NuspecProperties=`"version=$version`"" -o "$packagesDir" --no-build
}

Write-Host "Creating packages..."

PackNuget "SciChart.UI.Bootstrap" "SciChart.UI.Bootstrap"
PackNuget "SciChart.UI.Reactive" "SciChart.UI.Reactive"
PackNuget "SciChart.Wpf.UI" "SciChart.Wpf.UI"
PackNuget "SciChart.Wpf.UI.Transitionz" "SciChart.Wpf.UI.Transitionz"

Write-Host "Success!"

Set-Location $currentDir