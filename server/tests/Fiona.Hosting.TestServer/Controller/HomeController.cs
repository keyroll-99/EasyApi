using Fiona.Hosting.Controller;
using Fiona.Hosting.Controller.Attributes;

namespace Fiona.Hosting.TestServer.Controller;

[Controller]
public sealed class HomeController
{
    public Task<string> Index()
    {
        return Task.FromResult("Home");
    }
}