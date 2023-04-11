using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Duke_Queue.Pages.Scheduling
{
    public class JoinQueueModel : PageModel
    {
        [BindProperty]
        public OfficeHoursQueue JoinQueue { get; set; }

        public void OnGet(int OfficeHourID)
        {
            HttpContext.Session.SetInt32("OfficeHourID", OfficeHourID);

        }

        public IActionResult OnPost()
        {

            JoinQueue.OfficeHourID = (int)HttpContext.Session.GetInt32("OfficeHourID");

            JoinQueue.StudentID = DBClass.StudentIDFinder(HttpContext.Session.GetString("username"));

            DBClass.OfficeHoursDBConnection.Close();


            DBClass.InsertQueue(JoinQueue.officeHoursQueuePurpose, JoinQueue.StudentID, JoinQueue.OfficeHourID);

            DBClass.OfficeHoursDBConnection.Close();

            return RedirectToPage("/Scheduling/Scheduling2", new { OfficeHourID = JoinQueue.OfficeHourID });
        }
    }
}