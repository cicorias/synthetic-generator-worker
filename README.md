# Synthetic IoT Data Generator

This is a simple IoT data generator that creates synthetic data for a set of IoT devices. The data is generated in a way that it can be used to test and demonstrate the capabilities of the IoT platform.


## TODO:
- [x] externalize configuartion and command line parameters
- [ ] add emit file every N minutes option
- [ ] have file emitted with YYYY... format
- [ ] 



## Running

```sh
# windows pwsh
docker run -it --rm -v c:\temp:/out syntheticgenerator --ReplayOrdersOptions:OutputFile="/out/foobar.jsonl" --ReplayOrdersOptions:WindowStartTimeStr="2024-03-15T00:00:00" --ReplayOrdersOptions:WindowEndTimeStr="2024-03-16T00:00:00" --ReplayOrdersOptions:NumberOfEvents=2000 --ReplayOrdersOptions:Lambda=600
```

```sh
docker run -it --rm -v c:\temp:/out ghcr.io/cicorias/synthetic-generator-worker:sha256-fc81a7d957d5a2da9c2871af04a4f779a221fcc0ccf1f4d8d11d0dfb20e6c9fc.sig --ReplayOrdersOptions:OutputFile="/out/foobar.jsonl" --ReplayOrdersOptions:WindowStartTimeStr="2024-03-15T00:00:00" --ReplayOrdersOptions:WindowEndTimeStr="2024-03-16T00:00:00" --ReplayOrdersOptions:NumberOfEvents=2000 --ReplayOrdersOptions:Lambda=600
```