using System.Reflection;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Controller.Attributes;

namespace Fiona.Hosting.Routing.Attributes;

internal static class RouteAttributeUtils
{
    public static string GetBaseRoute(MemberInfo controller)
    {
        return GetRouteFromRouteAttribute(controller) ?? GetRouteFromControllerAttribute(controller);
    }

    public static (string? route, HttpMethodType methodType) GetMetadataFromRouteAttribute(MemberInfo method,
        string baseRoute)
    {
        RouteAttribute? controllerRouteAttribute = GetRouteAttribute(method);
        string? route = controllerRouteAttribute?.Route;
        HttpMethodType methodType = controllerRouteAttribute?.HttpMethodType ?? HttpMethodType.Get;

        if (route is null) return (route, methodType);

        return (
            string.IsNullOrWhiteSpace(route) || route.StartsWith('/') || string.IsNullOrWhiteSpace(baseRoute)
                ? route
                : $"/{route}", methodType);
    }

    private static RouteAttribute? GetRouteAttribute(MemberInfo memberInfo)
    {
        RouteAttribute? routeAttribute =
            (RouteAttribute?)memberInfo.GetCustomAttribute(typeof(RouteAttribute));

        return routeAttribute!;
    }

    private static string? GetRouteFromRouteAttribute(MemberInfo memberInfo)
    {
        RouteAttribute? routeAttribute = GetRouteAttribute(memberInfo);

        return routeAttribute?.Route;
    }

    private static string GetRouteFromControllerAttribute(MemberInfo controller)
    {
        ControllerAttribute? controllerAttribute =
            (ControllerAttribute?)controller.GetCustomAttribute(typeof(ControllerAttribute));

        MissingControllerAttributeException.ThrowIfNull(controllerAttribute);

        return controllerAttribute!.Route;
    }
}