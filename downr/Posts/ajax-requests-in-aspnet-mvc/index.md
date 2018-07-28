---
title: Ajax заявки в ASP.NET MVC
slug: ajax-requests-in-aspnet-mvc
date: 15-06-2017
---

Наскоро ми се наложи да правя ASP.NET MVC приложение
където *ActionResult*-те да се изпълняват само при Ajax заявки.
Решението на проблема реших да го споделя и може да го намерите по долу.

```csharp
public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
{
    public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
    {
        return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
    }
}
```