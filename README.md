[//]: # (GNU Terry Pratchett)
# LibNuclearesWeb
[![GitHub License](https://img.shields.io/github/license/ggppjj/LibNuclearesWeb)](./LICENSE )
### Nucleares Webserver Library
#### !!! WIP, NOT AT ALL READY FOR ANYTHING YET. !!!

This is an experimental library for interacting with the webserver for Nucleares, a game on Steam about managing an experimental Pressurized Water Reactor.

It aims to:

* Provide an object that represents the current overall available states exposed by the webapi of the game.
* Provide asynchronous and synchronous methods to load data from the game manually.
* Provide a datasource that auto-refreshes data from the game.
* Be AOT compatible.
* Be generally serialiazable.

Current usage example, for an app targeting localhost with auto-refresh enabled:
```csharp
using System;
using System.Threading.Tasks;
using LibNuclearesWeb;

namespace LibNuclearesWebExample;

var nuclearesWeb = new NuclearesWeb();

Console.WriteLine(nuclearesWeb.MainPlant.MainReactor.MainCore.Pressure);

```

If you wish to use your own network location or port, or to not load data automatically:
```csharp
var nuclearesWeb = new NuclearesWeb(networkLocation:"localhost", port:5000, refreshAutomatically:false);

var generator0Voltage = await nuclearesWeb.MainPlant.SteamGeneratorList[0].RefreshAllDataAsync().ActivePowerV;

Console.WriteLine(generator0Voltage);
```

This library is not affiliated with the game or its developers. It is an independent project.

Please report any bugs or issues you find here, although keep in mind enhancements may require enhancements to the game itself. All reasonable attempts will be made to keep this library up to date with the game, patience is appreciated, I'm just a dude.

---
Please consider joining the game's Discord server:  
[![Discord](https://img.shields.io/badge/Nucleares-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/nucleares) 


Also consider joining the developer's Patreon:  
[![Patreon](https://img.shields.io/badge/Nucleares-F96854?style=for-the-badge&logo=patreon&logoColor=white)](https://www.patreon.com/Nucleares)
