# Fiona command line

## How to install tool

Currently, the tool is not published on nuget, so you have to install it locally

1. Download nuget package from realise (in the future it will be available on the nuget)
2. Install dotnet tool ```dotnet tool install --global --add-source {pathToFolderWithNupkg}```

This tool will develop equally with ui, for user which prefers a work from console instead of UI.

In each command which has {PathToFolderWithFsln}, the default value is the current folder.
So If you run a command where you have fsln file you don't have to.

## Create project

### About command

Create project create a new project. It's create a new

1. fsln file
2. sln file
3. console app with installed Fiona.Hosting

### How to use it

``` dotnet Fiona Create {pathToDestinationFolder} {ProjectName}```
eg. `dotnet Fiona Create E:\100Commitow\ConsoleApp TestFromConsole`

## Compile one file

### About command

This command compiles only one file to csharp. It's created or override exists file `{filename}. Fn.cs`

### How to use it

```dotnet Fiona CompileFile {PathToFile} {PathToFolderWithFsln}```
eg. `dotnet Fiona `

## Compile all fn files

### About command

This command will be, compile all fn files from solution to c# file

### How to use it

```dotnet Fiona Compile {PathToSolution}``` or just ```dotnet Fiona Compile```
