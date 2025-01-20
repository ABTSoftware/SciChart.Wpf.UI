param ([Parameter(Mandatory=$true)][string]$version)

$slnDir = "$PSScriptRoot/.."
$packagesDir = "$slnDir/Packages"

. ./SafeRemoveDir.ps1

SafeRemoveDir $packagesDir

function PackNuget {
    param (
        [string]$project,
        [string]$nuspec
    )
	
    dotnet pack -c:Release "$slnDir/$project/$project.csproj" "-p:NuspecFile=$slnDir/$nuspec.nuspec" "-p:NuspecProperties=`"version=$version`"" -o "$packagesDir" --no-build
}

PackNuget "SciChart.UI.Bootstrap" "SciChart.UI.Bootstrap"
PackNuget "SciChart.UI.Reactive" "SciChart.UI.Reactive"
PackNuget "SciChart.Wpf.UI" "SciChart.Wpf.UI"
PackNuget "SciChart.Wpf.UI.Transitionz" "SciChart.Wpf.UI.Transitionz"