﻿using System.IO;
using System.Threading.Tasks;

namespace Fiona.IDE.Components.Layout.Menu;

internal class MenuService
{

    // Idea for a generate project, fiona IDE will be have own "csproj" and will be generated "source code" which Fiona server will be compiled to normal code
    public Task CreateProjectFile(string path, string name)
    {
        File.Create($"{path}/{name}.fsln");
        return Task.CompletedTask;
    }

}