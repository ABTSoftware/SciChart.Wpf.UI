param ([Parameter(Mandatory=$true)][string]$version)

$slnDir = "$PSScriptRoot/.."

# Import the CleanProject function
. ./CleanProject.ps1 $slnDir

# Clean solution-level output directories
$projectsToClean = "SciChart.UI.Bootstrap","SciChart.UI.Reactive","SciChart.Wpf.UI","SciChart.Wpf.UI.Transitionz"

$projectsToClean | CleanProject

# Import the SafeRemoveDir function
. ./SafeRemoveDir.ps1

# Clean the root output directories
SafeRemoveDir("$slnDir/Build")

# Set version number
./UpdateAssemblyInfo.ps1 "$slnDir/AssemblyInfoCommon.cs" $version

# Requires MSBuild tools VS2022
$msbuild = & "./FindLatestMSBuild.ps1"
if (-not (Test-Path $msbuild)) {
	Write-Host "Error finding MSBuild"
	exit -1
}

# Clean, Restore packages
$restoreMsBuildArgs = @("$slnDir/SciChart.Wpf.UI.sln",
	"-target:Clean;Restore",
	"-verbosity:normal"
)
& $msbuild $restoreMsBuildArgs
if ($LastExitCode -ne 0) {
	Write-Host "Error cleaning solution, code" $LastExitCode
	exit $LastExitCode
}

# Rebuild
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
	exit $LastExitCode
}