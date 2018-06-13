@echo off

SET version=2.0.0
ECHO Parameters = %1

if [%1] NEQ [] SET version=%1

ECHO Deploying SciChart DLLs v%version%

REM Setup directory structure
rmdir Build /S /Q


REM Prepare for deployment
call powershell.exe ".\DeploymentFiles\UpdateAssemblyInfoCommon.ps1" "AssemblyInfoCommon.cs" %version%
if ERRORLEVEL 1 goto :powershellError

REM -------------------------------------------
REM Build .NET 4.0 AnyCPU
REM -------------------------------------------

call "%MSBUILDPATH%\msbuild.exe" /ToolsVersion:15.0 /p:Configuration="Release" SciChart.Wpf.UI.sln /p:Platform="Any CPU" /p:WarningLevel=0
if ERRORLEVEL 1 goto :msBuildErrorNet40

REM -------------------------------------------
REM Build .NET 4.5 AnyCPU
REM -------------------------------------------

call "%MSBUILDPATH%\msbuild.exe" /ToolsVersion:15.0 /p:Configuration="Release45" SciChart.Wpf.UI.sln /p:Platform="Any CPU" /p:WarningLevel=0
if ERRORLEVEL 1 goto :msBuildErrorNet45


REM -------------------------------------------
REM Build .NET 4.6 AnyCPU
REM -------------------------------------------

call "%MSBUILDPATH%\msbuild.exe" /ToolsVersion:15.0 /p:Configuration="Release46" SciChart.Wpf.UI.sln /p:Platform="Any CPU" /p:WarningLevel=0
if ERRORLEVEL 1 goto :msBuildErrorNet46

Echo SUCCESS KID!
exit 0

:powershellError
echo One or more Powershell Scripts Failed
exit %ERRORLEVEL%

:msBuildErrorNet40
echo Failed to Build .NET4.0
exit %ERRORLEVEL%

:msBuildErrorNet45
echo Failed to Build .NET4.5
exit %ERRORLEVEL%

:msBuildErrorNet46
echo Failed to Build .NET4.6
exit %ERRORLEVEL%
