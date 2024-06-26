using System.Net;
using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Configuration;
using Fiona.Hosting.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Hosting;

internal sealed class FionaHost : IFionaHost
{
    private readonly ServerConfig _config;
    private readonly HttpListener _httpListener = new();
    private readonly int _maxThreads;
    private readonly List<Task> _requestThreads;
    private readonly IServiceProvider _serviceProvider;

    public FionaHost(IServiceProvider serviceProvider, ServerConfig config)
    {
        _serviceProvider = serviceProvider;
        _config = config;

        ThreadPool.GetMaxThreads(out int maxThreads, out _);
        _maxThreads = maxThreads / 4; // 25% of max threads for requests
        _requestThreads = new List<Task>(maxThreads / 4);
    }

    public void Dispose()
    {
        ((IDisposable)_httpListener).Dispose();
    }

    public void Run()
    {
        ThrowErrorIfServerAlreadyRunning();
        ConfigureListener();
        RunHost().GetAwaiter().GetResult();
    }

    private void ConfigureListener()
    {
        _httpListener.Prefixes.Add($"{_config.Domain}:{_config.Port}/");
    }

    private async Task RunHost()
    {
        _httpListener.Start();

        while (_httpListener.IsListening)
        {
            HttpListenerContext context = await _httpListener.GetContextAsync();

            await CleanUpThreadsList();

            _requestThreads.Add(Task.Factory.StartNew(async () =>
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                MiddlewareCallStack callStack = scope.ServiceProvider.GetRequiredService<MiddlewareCallStack>();
                await callStack.Invoke(context);
            }));
        }

        _httpListener.Close();
    }

    private async Task CleanUpThreadsList()
    {
        if (_requestThreads.Count == _maxThreads)
        {
            _requestThreads.RemoveAll(x => x.IsCompleted);
            // I all request all still executing, wait for one to finish
            if (_requestThreads.Count == _maxThreads)
            {
                await Task.WhenAny(_requestThreads);
                _requestThreads.RemoveAll(x => x.IsCompleted);
            }
        }
    }

    private void ThrowErrorIfServerAlreadyRunning()
    {
        if (_httpListener.IsListening) throw new InvalidOperationException("Server is already running");
    }
}