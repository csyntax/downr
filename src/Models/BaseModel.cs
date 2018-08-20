namespace downr.Models
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public abstract class BaseModel : PageModel
    {
        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            this.HandleHttpContext(context.HttpContext);

            base.OnPageHandlerExecuted(context);
        }

        private void HandleHttpContext(HttpContext httpContext)
        {
           if (httpContext.Request.Path.Value == "/")
           {
               httpContext.Response.Redirect("/posts");
           }
        }
    }
}