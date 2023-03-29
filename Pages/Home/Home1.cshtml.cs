using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using System.Runtime.CompilerServices;
using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;

namespace Duke_Queue.Pages.Home
{
    public class Home1Model : PageModel
    {
        public List<OfficeHoursQueue> OfficeHoursQueue { get; set; }

        public Home1Model()
        {
            OfficeHoursQueue = new List<OfficeHoursQueue>();
        }

        public IActionResult OnGet()
        {
            var isInstructor = HttpContext.Session.GetString("isInstructor");
            switch (isInstructor)
            {
                //If user is an instructor
                case "true":
                    //Write code for professor meeting listings
                    //Instructor facing
                    return RedirectToPage("/Home/InstructorHome");
                //If user is a student
                case "false":
                    //Student facing
/*                    SqlDataReader GeneralReader = DBClass.GeneralReaderQuery("SELECT officeHoursDate, locationName, instructorFirstName, instructorLastName " +
                        "FROM OfficeHours O, OfficeHoursQueue M, Location L, Instructor I " +
                        "WHERE M.officeHoursID = O.officeHoursID AND O.locationID = L.locationID AND O.instructorID = I.instructorID AND M.studentID = '" + HttpContext.Session.GetInt32("userID") +
                        "' ORDER BY officeHoursDate ");
                    while (GeneralReader.Read())
                    {
                        OfficeHoursQueue.Add(new OfficeHoursQueue
                        {
                            OfficeHoursDate = (DateTime)GeneralReader["officeHoursDate"],
                            LocationName = (String)GeneralReader["locationName"],
                            InstructorFirstName = (String)GeneralReader["instructorFirstName"],
                            InstructorLastName = (String)GeneralReader["instructorLastName"],
                            TimeSlot = (String)GeneralReader["timeSlot"],
                            
                            MeetingStart = (String)GeneralReader["meetingStart"],
                            MeetingDuration = (int)GeneralReader["meetingDuration"],
                            MeetingPurpose = (String)GeneralReader["meetingPurpose"]
                        });
                    }*/

                    DBClass.OfficeHoursDBConnection.Close();
                    return Page();

                //If user is not logged in
                default:
                    return RedirectToPage("/Login");
            }
        }

    }
}

