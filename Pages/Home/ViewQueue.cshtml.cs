using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace Duke_Queue.Pages.Home
{
    public class ViewQueueModel : PageModel
    {

        public List<OfficeHoursQueue> OfficeHoursQueue { get; set; }



        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [BindProperty]
        public int OfficeHoursQueueID { get; set; }


        //this consturctor i think is causing the problem
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

                // Initialize the OfficeHoursQueue list
                OfficeHoursQueue = new List<OfficeHoursQueue>();


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

        public int GetOfficeHoursQueueID()
        {
            return OfficeHoursQueueID;
        }

        public IActionResult OnPostAcceptStudent()
        {

            Start = DateTime.Now;
            return RedirectToPage("ViewQueue2", new { Start });
        }


        public IActionResult OnPostRemoveStudent()
        {

            End = DateTime.Now;
            ViewData["End"] = End.ToString();
            return RedirectToPage("ViewQueue3", new { End });
        }
    }
}

