using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Duke_Queue.Pages.Home
{
    public class ViewQueue2Model : PageModel
    {
        public void OnGet(DateTime Start)
        {
            ViewData["Start"] = Start.ToString();
        }


        
        
    }
}
