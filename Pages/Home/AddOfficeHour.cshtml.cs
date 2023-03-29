//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Duke_Queue.Pages.DataClasses;
//using Duke_Queue.Pages.DB;

//namespace Duke_Queue.Pages.Home
//{
//    public class AddOfficeHourModel : PageModel
//    {
//        [BindProperty]
//        public OfficeHour NewOfficeHours { get; set; }

//        public void OnGet()
//        {
//        }

//        public IActionResult OnPost()
//        {
//            // Call the InsertOffice method to update the data
//            DBClass.InsertOffice(NewOfficeHours.Location, NewOfficeHours.OfficeHoursDate, NewOfficeHours.TimeSlot, NewOfficeHours.OfficeHourID); ;

//            // Close the database connection
//            DBClass.Lab3DBConnection.Close();

//            // Redirect to the appropriate page based on the faculty ID in the URL
//            return RedirectToPage("/OfficeHourSch/OfficeHourPageTeacher", new { facultyID = NewOfficeHours.instructorID });
//        }
//    }
//}
