# Fiona

---
![logo](assets/logo.jpg)

---
[![🚧 - Under Development](https://img.shields.io/badge/🚧-Under_Development-orange)](https://)
![Licence - MIT](https://img.shields.io/badge/Licence-MIT-2ea44f)


Fiona is a web api framework written in .Net. As a developer, you will be able to create API easily and quickly.
The planned key feature is a GUI where a developer will be able to create API using blocks like a blueprint in UE.

In the beginning, I used HttpListener, but in the distant future, I want to write my HTTP handshake.

The project was initiated as part of the "100 commitów" competition

The Repo will be renamed after 100 commits competition because I came up with the idea for the name after registering the repo.

## Is it ready for production

Nope, maybe in the future

## Why this project's name is Fiona
Because Fiona it's the name of my dog, which one I adopted from dog shelter few days before I start this project

## Why I want to do this ecosystem
There are two reasons
1. I want to learn new think like make own http server.
2. I want to create a competition for asp.net because I don't currently know a c# alternative to it.

## Link to nuget
[nuget](https://www.nuget.org/packages/Fiona.Hosting)

## OS Support

| OS     | Fiona.Hosting     | Fiona.Ui           |
|--------|-------------------|--------------------|
| Windows| :white_check_mark:| :white_check_mark: |
| Linux  | :white_check_mark:| :x:                |
| MacOs  | :x:               | :x:                |

## Road map

Must have - I want to finish it by to end of march
- [X] Server
	- [X] Folder structure 
    - [X] First Tests
	- [X] Simple HTTP server with host
	- [X] Startup builder
	- [X] Routing
	- [X] Simple Controller
	- [X] Parsing body to object
	- [X] Middleware
	- [X] Passing parameters and args from the route
	- [X] Response (status codes etc)
	- [X] Async handle request
	- [X] A configuration like a baseUrl, port ETC
	- [X] Cookies
	- [X] Logger
	- [X] Error handler
	- [X] Documentation :)
- [X] Tools
    - [X] Github actions to build it

Good to have
- [X] Tools
	- [X] Publish nuget package
- [X] Compiler
	- [X] Parsing class
 	- [X] Parsing Method
	- [X] Parsing Return type
    - [X] Parsing parameters
	- [X] Parsing DI
    - [X] Namespace
    - [X] Parsing body
    - [X] Parsing comment
- [ ] Console tool
    - [X] Create a solution from the console
    - [X] Compile one file
    - [X] Create file with template
    - [X] Compile solution
    - [ ] Run solution


Nice to have
- [ ] GUI (GUI will be reworked to another technology :), that's why I moved it to nice to have after the competition)
	- [X] Simple view where the user can drag and drop boxes
	- [X] Nav menu
	- [X] Creating project
    - [ ] Configuration endpoint class
    - [X] Save to file
    - [X] Remove file
    - [X] Load from file
    - [X] Users can connect boxes
    - [ ] Run project
    - [ ] Adding controller 
    - [ ] Adding service
    - [ ] Add a way to create a simple CRUD
    - [ ] Add logic statement
    - [ ] Add loop statement
- [ ] Tools
	- [ ] Docker image
- [ ] Server
    - [ ] File Upload
	- [ ] Inject service to a controller method
	- [ ] Auto impl register
	- [ ] Auto inject to method
	- [ ] Extension for EF
    - [ ] Support for cqrs
	- [ ] Security
		- [ ] CORS
	- [ ] Before calling the action, an attribute
	- [ ] Support for plain text in the body
- [ ] GUI
	- [ ] Database configuration

Would to have
- [ ] Debugger
- [ ] Own HTTP handshake
- [ ] Cache
- [ ] HttpClient - user can easily use it
- [ ] Add user login option


## How to run a simple server

in `Program.cs`

```c#
using Fiona.Hosting;
using Fiona.Hosting.Abstractions;

IFionaHostBuilder serviceBuilder = FionaHostBuilder.CreateHostBuilder();

using IFionaHost host = serviceBuilder.Build();

host.Run();
```

sample controller

```c#
using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;
using Microsoft.Extensions.Logging;

namespace SampleFionaServer.Controller;

[Controller]
public class HomeController(ILogger<HomeController> logger)
{
    private readonly ILogger<HomeController> _logger = logger;
    
    [Route(HttpMethodType.Get)]
    public Task<string> Index()
    {
        _logger.Log(LogLevel.Critical, "Home Controller Index");
        return Task.FromResult("Home");
    }
}
```

If you want to read more click [here](./docs/Readme.md).

