# Needed because running 'psake.NET.bat -doc' result in $psake.build_success being false
$psake.build_success = $true

Import-Properties
Import-Tasks Version, Clean, Build, Test

Task Default -Depends Clean, Build, Test