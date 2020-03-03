namespace downr.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Diagnostics;

    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public void OnGet()
            => RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}