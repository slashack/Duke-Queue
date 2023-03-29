using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using System.Data.SqlClient;

namespace Duke_Queue.Pages.Home
{
    public class InstructorHome2Model : PageModel
    {
        public List<OfficeHoursQueue> Meetings { get; set; }

        public InstructorHome2Model()
        {
            Meetings = new List<OfficeHoursQueue>();
        }

        public IActionResult OnGet()
        {
            var isInstructor = HttpContext.Session.GetString("isInstructor");
            switch (isInstructor)
            {
                //If user is an instructor
                case "false":
                    //Write code for professor meeting listings
                    //Instructor facing
                    return RedirectToPage("/Home/Home1");
                //If user is a student
                case "true":
                    //Instructor facing
                    SqlDataReader GeneralReader = DBClass.GeneralReaderQuery("SELECT meetingStart, studentFirstName, studentLastName, studentEmail, meetingPurpose, meetingDuration " +
                        "FROM Student S, Meeting M, OfficeHours O " +
                        "WHERE S.studentID = M.studentID AND M.officeHoursID = O.officeHoursID AND M.officeHoursID = " + HttpContext.Session.GetInt32("selectedOfficeHourID") + " " +
                        "ORDER BY meetingStart ");
                    while (GeneralReader.Read())
                    {
                        Meetings.Add(new OfficeHoursQueue
                        {
                            MeetingStart = (String)GeneralReader["meetingStart"],
                            StudentFirstName = (String)GeneralReader["studentFirstName"],
                            StudentLastName = (String)GeneralReader["studentLastName"],
                            StudentEmail = (String)GeneralReader["studentEmail"],
                            MeetingPurpose = (String)GeneralReader["meetingPurpose"],
                            MeetingDuration = (int)GeneralReader["meetingDuration"],
                        });
                    }

                    DBClass.OfficeHoursDBConnection.Close();
                    return Page();

                //If user is not logged in
                default:
                    return RedirectToPage("/Login");
            }
        }

    }
}
