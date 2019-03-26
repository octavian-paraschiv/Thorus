$version = $args[0]

$templateFile = ".\ThorusCommon.IO\Properties\VersionTemplate.cs"
$versionFile = ".\ThorusCommon.IO\Properties\Version.cs"

$content = (Get-Content $templateFile).replace('VERSION', $version)
$content | Set-Content $versionFile
