param (
    [Parameter(Mandatory=$true)][string]$slnDir
)

. ./SafeRemoveDir.ps1

function CleanProject{
    param
    (
        [Parameter(ValueFromPipeline=$true)]
        [string] $name
    )
    process
    {
        SafeRemoveDir("$slnDir/$name/bin")
        SafeRemoveDir("$slnDir/$name/obj")
    }
}