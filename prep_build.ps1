$version = $args[0]

$versionFile = ".\ThorusCommon.IO\Properties\Version.cs"

$content = (Get-Content $versionFile).replace('1.0.0.0', $version)
$content | Set-Content $versionFile