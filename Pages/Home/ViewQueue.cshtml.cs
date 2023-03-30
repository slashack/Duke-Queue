using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Duke_Queue.Pages.Home
{
    public class ViewQueueModel : PageModel
    {
        public List<OfficeHoursQueue> OfficeHoursQueue { get; set; }

        public ViewQueueModel()
        {
            // Initialize the OfficeHoursQueue list
            OfficeHoursQueue = new List<OfficeHoursQueue>();
        }


        public IActionResult OnGet(int OfficeHourID)
        {
            //Checks if user is an instructor
            if (HttpContext.Session.GetString("isInstructor") != "true")
            {
                return RedirectToPage("/Login");
            }
            else
            {
                if (TempData.ContainsKey("ErrorMessage"))
                {
                    string errorMessage = TempData["ErrorMessage"].ToString();
                    ViewData["ErrorMessage"] = errorMessage;
                }
                SqlDataReader OfficeHoursQueueReader = DBClass.QueueReader(OfficeHourID);
                while (OfficeHoursQueueReader.Read())
                {
                    OfficeHoursQueue.Add(new OfficeHoursQueue
                    {
                        StudentFirstName = OfficeHoursQueueReader["studentFirstName"].ToString(),
                        StudentLastName = OfficeHoursQueueReader["studentLastName"].ToString(),
                        officeHoursQueuePurpose = OfficeHoursQueueReader["officeHoursQueuePurpose"].ToString(),
                    });
                }
                // Close your connection in DBClass
                DBClass.OfficeHoursDBConnection.Close();
                return Page();
            }
        }

        public IActionResult OnPostArchiveStudent()
        {
     

            //DBClass.ArchiveRecord(studentID, OfficeHourID);
            //DBClass.DeleteRecord(studentID, OfficeHourID);
            return Page();
        }
    }
}
