---
title: ASP.NET Core Dependency Injection
slug: aspnet-core-dependency-injection-overview
date: 05-09-2018
tags: .NET, ASP.NET
---

![ASP.NET Core Dependency Injection](media/header.jpg)

In this article,
I will share my experiences and suggestions 
on using Dependency Injection in ASP.NET Core applications. 
The motivation behind these principles are:

- Effectively designing services and their dependencies.
- Preventing multi-threading issues.
- Preventing memory-leaks.
- Preventing potential bugs.

## Basics

This article assumes that you are already familiar 
with **Dependency Injection** and **ASP.NET Core** in a basic level. 
If not, please read the 
[Dependency injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1) 
and [Dependency injection into controllers in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-2.1).

### Constructor Injection

Constructor injection is used to declare and 
obtain dependencies of a service on the **service construction**.

```csharp
public class ProductService
{
    private readonly IRepository<Product> productRepository;
    
    public ProductService(IRepository<Product> productRepository)
    {
        this.productRepository = productRepository;
    }
    
    public void Delete(int id)
    {
        this.productRepository.Delete(id);
        this.productRepository.Save();
    }
}
```

__ProductService__ is injecting __IRepository<Product>__ 
as a dependency in its constructor then using it 
inside the __Delete__ and __Save__ method.

**Good Practices:**

- Define **required dependencies** explicitly in the service constructor. 
Thus, the service can not be constructed without its dependencies.
- Assign injected dependency to a **readonly** field or property (to prevent accidentally assigning another value to it inside a method).

### Property Injection

_ProductService_ is declaring a _Logger_ property with public setter. 
**Dependency injection** container can set 
the _Logger_ if it is available (registered to DI container before).

```csharp
public class ProductService
{
    private readonly IRepository<Product> productRepository;

    public ILogger<ProductService> Logger { get; set; }
    
    public ProductService(IRepository<Product> productRepository)
    {
        this.productRepository = productRepository;
        this.Logger = NullLogger<ProductService>.Instance;
    }
    
    public void Delete(int id)
    {
        this.productRepository.Delete(id);
        this.productRepository.Save();

        this.Logger.LogInformation($"Delete a product where id = {id}");
    }
}
```

**Good Practices:**

- Use property injection only for optional dependencies. That means your service can properly work without these dependencies provided.
- Use [Null Object Pattern](https://en.wikipedia.org/wiki/Null_object_pattern) 
(as like in this example) if possible. 
Otherwise, always check for null while using the dependency.

## Service Locator

Service locator pattern is another way of obtaining dependencies.

```csharp
public class ProductService
{
    private readonly IRepository<Product> productRepository;
    private readonly ILogger<ProductService> logger;
    
    public ProductService(IServiceProvider serviceProvider)
    {
        this.productRepository = serviceProvider.GetRequiredService<IRepository<Product>>();
        this.logger = serviceProvider.GetService<ILogger<ProductService>>() ?? NullLogger<ProductService>.Instance;
    }
    
    public void Delete(int id)
    {
        this.productRepository.Delete(id);
        this.productRepository.Save();

        this.logger.LogInformation($"Delete a product where id = {id}");
    }
}
```

In _ProductService_, we injecting _IServiceProvider_ and resolving dependencies using it.
_GetRequiredService_ method throws exception if the requested dependency was not registered before. 
On the other hand, _GetService_ method just returns null in that case.

When you resolve services inside the constructor, 
they are released when the service is released. 
So, you don’t care about releasing/disposing services 
resolved inside the constructor (just like constructor and property injection).

**Good Practices:**

- Do not use the service locator pattern wherever possible (if the service type is known in the development time). Because it makes the dependencies implicit. That means it’s not possible to see the dependencies easily while creating an instance of the service. This is especially important for unit tests where you may want to mock some dependencies of a service.
- Resolve dependencies in the service constructor if possible. Resolving in a service method makes your application more complicated and error prone. I will cover the problems & solutions in the next sections.

## Service Life Times

There are three service lifetimes in ASP.NET Core Dependency Injection:

- **Singleton** services are created per DI container. That generally means that they are created only one time per application and then used for whole the application life time.
- **Scoped** services are created per scope. In a web application, every web request creates a new separated service scope. That means scoped services are generally created per web request.
- **Transient** services are created every time they are injected or requested.

DI container keeps track of all resolved services. Services are released and disposed when their lifetime ends:

- If the service has dependencies, they are also automatically released and disposed.
- If the service implements the _IDisposable_ interface, _Dispose_ method is automatically called on service release.

## Good Practices for register services

- Register your services as *transient* wherever possible. 
Because it’s simple to design transient services. 
You generally don’t care about multi-threading and memory leaks and you know the service has a short life.
- Use *scoped* service lifetime carefully since it can be tricky 
if you create child service scopes or use these services from a non-web application.
- Use *singleton* lifetime carefully since then you need to deal 
with multi-threading and potential memory leak problems.
- Do not depend on a *transient* or *scoped* service from a *singleton* service. 
Because the transient service becomes a singleton instance 
when a singleton service injects it and that may 
cause problems if the transient service is not designed to support such a scenario. 
ASP.NET Core’s default DI container already throws exceptions in such cases.

## Resolving Services in a Method Body

In some cases, you may need to resolve another service in a method of your service. 
In such cases, ensure that you release the service after usage. 
The best way of ensuring that is to create a service scope.

```csharp
public class PriceCalculator
{
    private readonly IServiceProvider serviceProvider;
    
    public PriceCalculator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public float Calculate(Product product, int count, Type taxStrategyServiceType)
    {
        using (var scope = this.serviceProvider.CreateScope())
        {
            var taxStrategy = (ITaxStrategy) scope.ServiceProvider.GetRequiredService(taxStrategyServiceType);

            var price = product.Price * count;
            
            return price + taxStrategy.CalculateTax(price);
        }
    }
}
```

_PriceCalculator_ injects the _IServiceProvider_ in its constructor and assigns it to a field. 
_PriceCalculator_ then uses it inside the _Calculate_ method to create a child service scope. 
It uses scope.ServiceProvider to resolve services, instead of the injected _serviceProvider_ instance. 
Thus, all services resolved from the scope is automatically _released/disposed_ at the end of the using statement.

- If you are resolving a service in a method body, always create a child service scope to ensure that the resolved services are properly released.
- If a method gets IServiceProvider as an argument, then you can directly resolve services from it without care about releasing/disposing. Creating/managing service scope is a responsibility of the code calling your method. Following this principle makes your code cleaner.
- Do not hold a reference to a resolved service! Otherwise, it may cause memory leaks and you will access to a disposed service when you use the object reference later (unless the resolved service is singleton).

## Singleton Services

Singleton services are generally designed to keep an application state. 
A cache is a good example of application states.

```csharp
public class FileService
{
    private readonly ConcurrentDictionary<string, byte[]> cache;
 
    public FileService()
    {
        this.cache = new ConcurrentDictionary<string, byte[]>();
    }
 
    public byte[] GetFileContent(string filePath)
    {
        return this.cache.GetOrAdd(filePath, _ =>
        {
            return File.ReadAllBytes(filePath);
        });
    }
}
```

FileService simply caches file contents to reduce disk reads. 
This service should be registered as singleton. 
Otherwise, caching will not work as expected.

- If the service holds a state, it should access to that state in a thread-safe manner. Because all requests concurrently uses the same instance of the service. I used ConcurrentDictionary instead of Dictionary to ensure thread safety.
- Do not use scoped or transient services from singleton services. Because, transient services might not be designed to be thread safe. If you have to use them, then take care of multi-threading while using these services (use lock for instance).
- Memory leaks are generally caused by singleton services. They are not released/disposed until the end of the application. So, if they instantiate classes (or inject) but not release/dispose them, they will also stay in the memory until the end of the application. Ensure that you release/dispose them at the right time. See the Resolving Services in a Method Body section above.
- If you cache data (file contents in this example), you should create a mechanism to update/invalidate the cached data when the original data source changes (when a cached file changes on the disk for this example).

## Scoped Services

Scoped lifetime *first seems* a good candidate to store per web request data.
Because ASP.NET Core creates a service scope per web request. 
So, if you register a service as scoped, it can be shared during a web request.

```csharp
public class RequestItemsService
{
    private readonly IDictionary<string, object> items;

    public RequestItemsService()
    {
        this.items = new Dictionary<string, object>();
    }

    public void Set(string name, object value)
    {
        this.items[name] = value;
    }

    public object Get(string name)
    {
        return this.items[name];
    }
}
```

If you register the RequestItemsService as scoped 
and inject it into two different services, 
then you can get an item that is added from another 
service because they will share the same RequestItemsService instance. 
That’s what we expect from scoped services.

If you create a child service scope and resolve the *RequestItemsService* 
from the child scope, then you will get a new instance 
of the *RequestItemsService* and it will not work as you expect. 
So, scoped service does not always means instance per web request.

You may think that you do not make such 
an obvious mistake (resolving a scoped inside a child scope). 
But, this is not a mistake (a very regular usage) 
and the case may not be such simple. 
If there is a big dependency graph between your services, 
you can not know if anybody created a child scope 
and resolved a service that injects another service… 
that finally injects a scoped service.

**Best practice:**

- A scoped service can be thought as an _optimization_ where 
it is injected by too many services in a web request. 
This, all these services will use a single instance 
of the service during the same web request.
- Scoped services don’t need to be designed as thread-safe. 
Because, they should be normally used by a single web-request/thread. 
But… in that case, you should *not share service scopes between different threads*!

Be careful if you design a scoped service to share data between 
other services in a web request (explained above). 

You can store per web request data inside the *HttpContext* 
(inject _IHttpContextAccessor_ to access it) which is the safer way of doing that. 

HttpContext’s lifetime is not scoped. 
Actually, it’s not registered to _DI_ at all (that’s why you don’t inject it, but inject IHttpContextAccessor instead). 
[HttpContextAccessor](https://github.com/aspnet/HttpAbstractions/blob/master/src/Microsoft.AspNetCore.Http/HttpContextAccessor.cs) implementation uses AsyncLocal 
to share the same HttpContext during a web request.

## Conclusion

Dependency injection seems simple to use at first, 
but there are potential multi-threading and memory leak problems 
if you don’t follow some strict principles.

I shared some good principles based on my own experiences during development of 
practical ASP.NET Core projects and I used for him these shared patterns in this article.