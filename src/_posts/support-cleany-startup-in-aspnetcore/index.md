---
title: Подържане на изчистен Startup в ASP.NET Core
slug: support-cleany-startup-in-aspnetcore
date: 16-07-2017
---

Когато използваме *ConfigureServices* за да конфигурираме нашето приложение e
доста възможно да се достигне до дълги и разхвърляни методи за конфигурация.

Това не се получава от сложна програмна логика, но с всички *middlewares* и *services*,
както и други опции за конфигуриране, методите тогава стават дълги и разхвърляни.

Аз правя нещо подобно, извиквам всеки service метод по-отделно.

```csharp
public void ConfigureServices(IServiceCollection services)
{ 
    services.AddCustomizedMvc();
    services.AddCustomizedIdentity();
    services.AddDataStores();
}
```

Всички подробности за тези извиквания на методи изполват *IApplicationBuilder* или *IServiceCollection*.
Ето един пример.

```csharp
public static class ServiceCollectionExtensions 
{ 
    public static IServiceCollection AddCustomizedMvc(this IServiceCollection services) 
    {
        var locationFormat = @"Features\Shared\{0}.cshtml";
        var expander = new ViewWithControllerViewLocationExpander();

        services
            .AddMvc()
            .AddRazorOptions(options => { 
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add(locationFormat); 
                options.ViewLocationExpanders.Add(expander); 
            });

        return services;
    }
}
```