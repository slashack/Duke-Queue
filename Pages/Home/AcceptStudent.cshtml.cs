using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Duke_Queue.Pages.Home
{
    public class AcceptStudentModel : PageModel
    {
        
        public int OfficeHoursQueueID;
        public void OnGet(int OfficeHoursQueueID)
        {
        }
    }
}
