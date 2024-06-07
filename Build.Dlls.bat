@echo off

SET version=3.1.0
ECHO Parameters = %1

if [%1] NEQ [] SET version=%1

ECHO Deploying SciChart.WPF.UI DLLs v%version%

REM Setup directory structure
rmdir Build /S /Q


REM Prepare for deployment
call powershell.exe ".\DeploymentFiles\UpdateAssemblyInfoCommon.ps1" "AssemblyInfoCommon.cs" %version%
if ERRORLEVEL 1 goto :powershellError

REM Restore nuget packages 
REM Set MSBUILDPATH2022=C:\Program Files (x86)\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin
call "%MSBUILDPATH2022%\msbuild.exe" -target:Restore -restore SciChart.Wpf.UI.sln -p:Configuration=Release;Platform="Any CPU";WarningLevel=0
if ERRORLEVEL 1 goto :restoreError

REM -------------------------------------------
REM Build all target frameworks 
REM -------------------------------------------

call "%MSBUILDPATH2022%\msbuild.exe" -target:Build SciChart.Wpf.UI.sln -p:Configuration=Release;Platform="Any CPU";WarningLevel=0
if ERRORLEVEL 1 goto :msBuildError


Echo SUCCESS KID!
exit 0

:powershellError
echo One or more Powershell Scripts Failed
exit %ERRORLEVEL%

:restoreError
echo Failed to restore packages 
exit %ERRORLEVEL%

:msBuildError
echo Failed to Build .NET
exit %ERRORLEVEL%
