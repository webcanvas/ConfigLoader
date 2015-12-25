# ConfigLoader
Uses reflection with some conventions to load settings from web or app configs, json files and more importantly environment variables.

[![Build status](https://ci.appveyor.com/api/projects/status/udshi1cj8aq8y1xd/branch/master?svg=true)](https://ci.appveyor.com/project/chriskolenko/configloader/branch/master)
[![Coverage Status](https://coveralls.io/repos/webcanvas/ConfigLoader/badge.svg?branch=master&service=github)](https://coveralls.io/github/webcanvas/ConfigLoader?branch=master)

||Stable|Pre-release|
|:--:|:--:|:--:|
|NuGet (ConfigLoader)|[![NuGet](https://img.shields.io/nuget/v/ConfigLoader.svg)](https://www.nuget.org/packages/ConfigLoader)|[![NuGet](https://img.shields.io/nuget/vpre/ConfigLoader.svg)](https://www.nuget.org/packages/ConfigLoader)|

## Usage

```C#
var config = ConfigLoader.LoadConfig<TestConfig>(".\\testdata.json");
var config2 = ConfigLoader.LoadConfig<TestConfig>();
```
