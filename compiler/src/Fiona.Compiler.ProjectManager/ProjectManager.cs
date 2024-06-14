﻿using Fiona.Compiler.ProjectManager.Exceptions;
using Fiona.Compiler.ProjectManager.Models;
using Serilog;
using Serilog.Core;

namespace Fiona.Compiler.ProjectManager;

internal sealed class ProjectManager(ICommandRunner commandRunner, ILogger logger) : IProjectManager
{
    private FslnFile? Project { get; set; }

    public string GetPath()
    {
        if (Project is null)
        {
            throw new Exception("Project not loaded");
        }

        return Project.Path;
    }

    public async Task<string> CreateProject(string path, string name)
    {
        logger.Information("Create project {name}", name);
        string fullPath = $"{path}{Path.DirectorySeparatorChar}{name}.fsln";
        if (File.Exists(fullPath))// Todo: or exists any other files
        {
            throw new ProjectAlreadyExistsException(fullPath);
        }

        await CreateSln(path, name);
        await CreateFsln(name, path);
        
        logger.Information("Project {name} created", name);
        return fullPath;
    }

    public async Task LoadProject(string path)
    {

        Project = await FslnFile.LoadAsync(path);
    }

    public IEnumerable<ProjectFile> GetFiles()
        => Project?.ProjectFiles ?? Enumerable.Empty<ProjectFile>();
    public ProjectFile GetProjectFileByNamespaceAndName(string @namespace, string name)
        => Project?.ProjectFiles!.FirstOrDefault(x => x.Class.Namespace == @namespace && x.Class.Name == name) ?? throw new ProjectFileNotFoundException(@namespace);

    public Task CreateFileAsync(string name, string folderPath) => Project!.AddFile(name, folderPath);
    public async Task RemoveFile(ProjectFile projectFile)
    {
        await Project!.RemoveFile(projectFile);
    }

    public bool IsLoaded()
        => Project is not null;

    public string? GetName()
        => Project?.Name;

    private async Task CreateFsln(string name, string pathToFolder)
    {
        Project = await FslnFile.CreateAsync(name, pathToFolder);
    }

    private async Task CreateSln(string path, string name)
    {
        await commandRunner.RunCommandAsync($"dotnet new console -n {name}", path);
        await commandRunner.RunCommandAsync($"dotnet new sln --name {name}", path);
        await commandRunner.RunCommandAsync("dotnet add package Fiona.Hosting", $"{path}{Path.DirectorySeparatorChar}{name}");
        await commandRunner.RunCommandAsync($"dotnet sln add {path}{Path.DirectorySeparatorChar}{name}", path);
    }
}

public static class ProjectManagerFactory
{
    private static IProjectManager? _instace;

    private static object locker = new();
    
    public static IProjectManager GetOrCreate()
    {
        if (_instace is null)
        {
            lock (locker)
            {
                if (_instace is null)
                {
                    Logger logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
                    _instace = new ProjectManager(new CommandRunner(logger), logger);
                }
            }
        }

        return _instace;
    }
}