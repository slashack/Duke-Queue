using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Duke_Queue.Pages.Scheduling
{
    public class JoinQueueModel : PageModel
    {
        [BindProperty]
        public OfficeHoursQueue joinQueue { get; set; }
        public void OnGet(int OfficeHourID)
        {
        }

        public IActionResult OnPost(int OfficeHourID) 
        {
            joinQueue.OfficeHourID = OfficeHourID;

            joinQueue.StudentID = DBClass.StudentIDFinder(HttpContext.Session.GetString("username"));
            DBClass.OfficeHoursDBConnection.Close();

           

            // Call the InsertOffice method to update the data
            DBClass.InsertQueue(joinQueue.officeHoursQueuePurpose, joinQueue.StudentID, joinQueue.OfficeHourID);

            // Close the database connection
            DBClass.OfficeHoursDBConnection.Close();

            // Redirect to the appropriate page based on the faculty ID in the URL
            return RedirectToPage("/Scheduling/Scheduling2");
        }
    }
}
