using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Duke_Queue.Pages.Home
{
    public class ViewQueue3Model : PageModel
    {
        public void OnGet(DateTime End)
        {
            ViewData["End"] = End.ToString();

        }
    }
}
