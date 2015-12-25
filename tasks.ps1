# Needed because running 'psake.NET.bat -doc' result in $psake.build_success being false
$psake.build_success = $true

Import-Properties
Include '.\invoke-nunit3.ps1'
Import-Tasks Clean, Test, Build, Pack-Nuspec, Tag, Push-Local, Push, Push-Tag, Version, Version-BuildServer, Set-NuGetApiKey, Send-OpenCoverResultToCoveralls

Task Default -Depends Clean, Build, Test, Pack-Nuspec
Task Release-Local -Depends Version, Clean, Build, Test, Pack-Nuspec, Push-Local -Description "Release 'ConfigLoader' to the local NuGet feed"
