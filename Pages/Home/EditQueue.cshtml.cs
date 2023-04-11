using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Duke_Queue.Pages.Home
{
    public class EditQueueModel : PageModel
    {
        [BindProperty]
        public OfficeHour NewOfficeHours { get; set; }

        [BindProperty]
        public int SelectedOfficeHourID { get; set; }


        public void OnGet(int officeHourID)
        {
           
        }

        public IActionResult OnPost()
        {
            NewOfficeHours.instructorID = DBClass.IDFinder(HttpContext.Session.GetString("username"));
            DBClass.OfficeHoursDBConnection.Close();
            NewOfficeHours.locationID = DBClass.OfficeIDFinder(HttpContext.Session.GetString("username"));
            DBClass.OfficeHoursDBConnection.Close();

            // Call the InsertOffice method to update the data
            DBClass.EditOffice(NewOfficeHours.locationID, NewOfficeHours.OfficeHoursDate, NewOfficeHours.TimeSlotStart + "-" + NewOfficeHours.TimeSlotEnd, NewOfficeHours.instructorID, SelectedOfficeHourID);

            // Close the database connection
            DBClass.OfficeHoursDBConnection.Close();

            // Redirect to the appropriate page based on the faculty ID in the URL
            return RedirectToPage("/Home/Home1");
        }
    }
}
