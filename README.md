[//]: # (GNU Terry Pratchett)
# LibNuclearesWeb
[![GitHub License](https://img.shields.io/github/license/ggppjj/LibNuclearesWeb)](./LICENSE )
## Nucleares Webserver Library
#### Status: WIP

This is an experimental library for interacting with the webserver for Nucleares, a game on Steam about managing an experimental Pressurized Water Reactor.  

> This library is ***not*** affiliated with the game or its developers. It is an independent project.

## LibNuclearesWeb provides:

- A **main entry point** (`NuclearesWeb`) that coordinates HTTP access to a running Nucleares server.  
- **Object models** that represent each major system exposed by the game’s web API (e.g., reactor core, coolant system, control rods, steam turbines, and more).  
- **Asynchronous** and **synchronous** methods to load data from the game.  
- An **auto-refresh** feature that periodically updates all data in the background.  
- **Property change notifications** (`INotifyPropertyChanged`) for each model – especially useful if you’re building UIs in WPF, MAUI, Blazor, or other .NET frameworks.

## Features:

- **Simple, Unified API**  
  Create a single NuclearesWeb instance, then access detailed models like MainPlant, MainReactor, Coolant, and SteamGeneratorList.
- **Auto-Refresh Capability**  
  Optionally let the library poll the game server at a specified interval, automatically updating all properties.
- **Async + Sync**  
  Each subsystem can be refreshed asynchronously (RefreshAllDataAsync) or via blocking calls (RefreshAllData).
- **MVVM-Friendly**  
  All model properties raise change notifications, so they can be bound directly to a UI with real-time updates.
- **Extendable**  
  Written in C#, easily integrable with other .NET projects.
- **Serialization and deserialization support.**
  

---
## Examples:
Current usage example, for an app targeting localhost with auto-refresh enabled:

```csharp
using System; 
using LibNuclearesWeb;

// By default, uses 127.0.0.1:8787 if no arguments are provided.
var nuclearesWeb = new NuclearesWeb(autoRefresh: true); 

// If autoRefresh is enabled, the library periodically updates data in the background.
Console.WriteLine(nuclearesWeb.MainPlant.MainReactor.MainCore.Pressure); 

```

If you wish to use your own network location or port, or to not load data automatically:

```csharp
using System; 
using System.Threading.Tasks; 
using LibNuclearesWeb;

var nuclearesWeb = new NuclearesWeb(networkLocation: "192.168.0.45", port: 5000);

await nuclearesWeb.MainPlant.SteamGeneratorList[0].RefreshAllDataAsync(); 
string generator0Voltage = nuclearesWeb.MainPlant.SteamGeneratorList[0].ActivePowerV; 
Console.WriteLine(generator0Voltage);
```
To access data:

```csharp
// Access Control Rod data 
var rodStatus = nuclearesWeb.MainPlant.MainReactor.MainCore.ControlRodBundles.Status; 
Console.WriteLine($"Rod Status: {rodStatus}");

// Access coolant pump speeds 
foreach (var pump in nuclearesWeb.MainPlant.MainReactor.MainCore.Coolant.PumpList) 
    => Console.WriteLine($"Pump {pump.Id} Speed: {pump.Speed}");
```

### A Note on Auto-Refresh

 If you pass autoRefresh: true into the constructor, LibNuclearesWeb starts a background task calling RefreshAllDataAsync at 5-second intervals (by default).
 
 You can manually disable/enable auto-refresh using:

```csharp
nuclearesWeb.EnableAutoRefresh(TimeSpan.FromSeconds(10));  // e.g., 10s
...
nuclearesWeb.DisableAutoRefresh();
```

Synchronous property reads remain safe to call but might reflect data from the last refresh.

---

Please report any bugs or issues you find here, although keep in mind enhancements may require enhancements to the game itself. All reasonable attempts will be made to keep this library up to date with the game, patience is appreciated, I'm just a dude.

---

### Final Disclaimer


  - This library is unofficial and not affiliated with Nucleares or its developers.
  - It may break if future game updates change the webserver’s behavior or data layout.

Use at your own risk. This code is a personal project and remains a work in progress.

---
Please consider joining the game's Discord server:  
[![Discord](https://img.shields.io/badge/Nucleares-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/nucleares) 


Also consider joining the game developer's Patreon:  
[![Patreon](https://img.shields.io/badge/Nucleares-F96854?style=for-the-badge&logo=patreon&logoColor=white)](https://www.patreon.com/Nucleares)
