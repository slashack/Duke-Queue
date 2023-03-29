using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duke_Queue.Pages.DB;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Duke_Queue.Pages
{
    public class Scheduling3Model : PageModel
    {

        public int? selectedHourID;
        public int? selectedInstructorID;
        //User Input Properties
        [BindProperty]
        [Required]
        public String? MeetingStart { get; set; }
        [BindProperty]
        [Required]
        public int MeetingDuration { get; set; }
        [BindProperty]
        [Required]
        public String? MeetingPurpose { get; set; }

        //Populates form
        public IActionResult OnPostPopulateHandler()
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
                MeetingStart = "11:30AM";
                MeetingDuration = 30;
                MeetingPurpose = "I am lost on chapter 1";

            }
            return Page();
        }

        //Clears form values
        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            MeetingStart = "";
            MeetingDuration = 0;
            MeetingPurpose = "";
            return Page();
        }
        //Checks to see if required fields are met
        public IActionResult OnPostSubmit()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                Random random = new Random();
                int newMeetingID = random.Next(1, 10000);
                //Insert Query for purpose, ProfessorID and OfficeHoursID
                DBClass.InsertQuery("INSERT INTO Meeting(meetingID, meetingStart, meetingDuration, meetingPurpose, studentID, officeHoursID) " +
                    "VALUES (" + newMeetingID + ", '" + MeetingStart + "', " + MeetingDuration + ", '" + MeetingPurpose + "' , " + HttpContext.Session.GetInt32("userID") + ", " + HttpContext.Session.GetInt32("selectedHourID") + ") ");
                DBClass.OfficeHoursDBConnection.Close();

                return RedirectToPage("/Home/Home1");

            }
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                //Pulls session state variables
                selectedHourID = HttpContext.Session.GetInt32("selectedHourID");
                selectedInstructorID = HttpContext.Session.GetInt32("selectedInstructorID");
                return Page();
            }
        }
    }
}
