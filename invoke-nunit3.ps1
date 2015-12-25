
function Invoke-NUnitTests {
  param([PSCustomObject[]]$assemblies)

  if($assemblies.Count -gt 0) {
    "`r`nRunning NUnit3 Tests"
    $nunit = Join-Path (Get-PackageDir "NUnit.Console") "tools\nunit3-console.exe"
    $framework_version = Get-FrameworkVersion

    #$include = @{$true=@("/include",$nunit_categories);$false=""}[-not[String]::IsNullOrWhiteSpace($nunit_categories)]
    #$exclude = @{$true=@("/exclude",$nunit_exclude_categories);$false=""}[-not[String]::IsNullOrWhiteSpace($nunit_exclude_categories)]

    if ($run_dotcover -eq $true) {
      $scope = ($assemblies | foreach { Join-Path (Split-Path -Path $_) "**\*.dll" }) -Join ';'
      exec { & $(Get-dotCover) cover /TargetExecutable="$nunit" /TargetArguments="--work=`"`"$test_results_dir`"`" --result=`"`"NUnit.xml`"`" --framework=`"`"net-$framework_version`"`" --noh `"`"$($assemblies -join '`"`" `"`"')`"`"" /Output="$(Join-Path $test_results_dir 'NUnit.dotCover.Snapshot.dcvr')" /Scope="$scope" /Filters="`"$script:dotcover_filters`"" /AttributeFilters="`"$script:dotcover_attribute_filters`"" /ReturnTargetExitCode }
    } elseif ($run_opencover -eq $true) {
      exec { & $(Get-OpenCover) "-target:$nunit" "-targetargs:--work=`"`"$test_results_dir`"`" --result=`"`"NUnit.xml`"`" --framework=`"`"net-$framework_version`"`" --noh `"`"$($assemblies -join '`"`" `"`"')`"`"" "-output:$(Join-Path $test_results_dir $opencover_result_name)" -mergeoutput "-filter:$script:opencover_filters" "-excludebyattribute:$script:opencover_excluded_attributes" "-register:Path$opencover_platform" -returntargetcode }
    } else {
      exec { & $nunit "$assemblies" --work="$test_results_dir" --result="NUnit.xml" --framework="net-$framework_version" --noh }
    }
  }
}
