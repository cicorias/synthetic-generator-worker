# Synthetic IoT Data Generator

This is a simple IoT data generator that creates synthetic data for a set of IoT devices. The data is generated in a way that it can be used to test and demonstrate the capabilities of the IoT platform.

## Program Parameters

The generator makes use of dotnet configuration and can utilize either a `appsettings.json`, command line parameters, or environment variables. See the article [Configuration providers in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration-providers).  The solution makes use of:
1. File Configuration
1. Environment Variable Configuration
1. Command Line Configuration


### Existing `SyntheticGenerator\appsettings.Development.json` file

The deployment uses the following defaults that can be overwritten as needed via config or via command paramters.

```json
  "ReplayOrdersOptions": {
    "WindowStartTimeStr": "2024-03-15T12:00:00",
    "WindowEndTimeStr": "2024-03-15T12:15:00",
    "Lambda": 120,
    "NumberOfEvents": 100,
```    

## Running via GitHub Container

### bash/zsh

```sh
# nix
docker run -it --rm -v /tmp:/out ghcr.io/cicorias/synthetic-generator-worker:main --ReplayOrdersOptions:OutputFile="/out/foobar.jsonl" --ReplayOrdersOptions:WindowStartTimeStr="2024-03-15T00:00:00" --ReplayOrdersOptions:WindowEndTimeStr="2024-03-16T00:00:00" --ReplayOrdersOptions:NumberOfEvents=2000 --ReplayOrdersOptions:Lambda=60
```
### PowerShell

```sh
# windows pwsh
docker run -it --rm -v c:\temp:/out ghcr.io/cicorias/synthetic-generator-worker:main --ReplayOrdersOptions:OutputFile="/out/foobar.jsonl" --ReplayOrdersOptions:WindowStartTimeStr="2024-03-15T00:00:00" --ReplayOrdersOptions:WindowEndTimeStr="2024-03-16T00:00:00" --ReplayOrdersOptions:NumberOfEvents=2000 --ReplayOrdersOptions:Lambda=600
```

## Running as native dotnet app

If the package is provided and unzipped, you can affect the local `appsettings.Development.json` as needed or use parameters as follows:

```sh
#pwsh windows
.\SyntheticGenerator.exe --ReplayOrdersOptions:WindowStartTimeStr="2024-03-15T00:00:00" --ReplayOrdersOptions:WindowEndTimeStr="2024-03-16T00:00:00" --ReplayOrdersOptions:NumberOfEvents=2000 --ReplayOrdersOptions:Lambda=600 --ReplayOrdersOptions:OutputFile=c:\temp\f.json
```


## TODO:
- [x] externalize configuartion and command line parameters
- [ ] package in pipeline
- [ ] fix signing issue of image
- [ ] add emit file every N minutes option
- [ ] have file emitted with YYYY... format
- [ ] provide a replay of existing input file -- modifying dates only; using time delta on base time

