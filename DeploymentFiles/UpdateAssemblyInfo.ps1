param([string]$filePath, [string]$version)

#e.g. [assembly: AssemblyVersion("4.0.0")]

$regex = "([0-9]+)\.([0-9]+)\.([0-9]+)"
$beforeContent = (Get-Content $filePath)
$replace = $version 
write-host "regex: " $regex
write-host "replace: " $replace
$afterContent = $beforeContent -replace $regex, $replace
#write-host $afterContent
$afterContent | Out-File $filePath -Encoding "UTF8"