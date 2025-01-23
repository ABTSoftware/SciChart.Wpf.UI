param ([Parameter(Mandatory=$true)][string]$version)

$currentDir = Get-Location
$deployDir = "$PSScriptRoot"
$slnDir = "$PSScriptRoot/.."

Write-Host "Current folder:" $currentDir
Write-Host "Deployment folder" $PSScriptRoot

# Switch to the Deployment folder
Set-Location $deployDir

# Import the CleanProject function
. ./CleanProject.ps1 $slnDir

# Clean solution-level output directories
Write-Host "Clearing projects..."
$projectsToClean = "SciChart.UI.Bootstrap","SciChart.UI.Reactive","SciChart.Wpf.UI","SciChart.Wpf.UI.Transitionz"
$projectsToClean | CleanProject

# Import the SafeRemoveDir function
. ./SafeRemoveDir.ps1

# Clean the root output directories
Write-Host "Clearing folders..."
SafeRemoveDir("$slnDir/Build")

# Set version number
Write-Host "Setting build version..." $version
./UpdateAssemblyInfo.ps1 "$slnDir/AssemblyInfoCommon.cs" $version

# Requires MSBuild tools VS2022
$msbuild = & "./FindLatestMSBuild.ps1"
if (-not (Test-Path $msbuild)) {
	Write-Host "Error finding MSBuild"
	exit -1
}

# Clean, Restore packages
Write-Host "Clearing SciChart.Wpf.UI.sln..."
$restoreMsBuildArgs = @("$slnDir/SciChart.Wpf.UI.sln",
	"-target:Clean;Restore",
	"-verbosity:normal"
)
& $msbuild $restoreMsBuildArgs
if ($LastExitCode -ne 0) {
	Write-Host "Error cleaning solution, code" $LastExitCode
	exit -1
}

# Rebuild
Write-Host "Building SciChart.Wpf.UI.sln..."
$buildMsBuildArgs = @("$slnDir/SciChart.Wpf.UI.sln",
	"-target:Build",
	"-toolsVersion:Current",
	"-p:Configuration=Release;Platform=`"Any CPU`";WarningLevel=0",
	"-m",
	"-verbosity:normal"
)
& $msbuild $buildMsBuildArgs
if ($LastExitCode -ne 0) {
	Write-Host "Error building solution, code" $LastExitCode
	exit -1
}

Write-Host "Success!"

Set-Location $currentDir