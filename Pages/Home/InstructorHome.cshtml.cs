using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using System.Data.SqlClient;

namespace Duke_Queue.Pages.Home
{
    public class InstructorHomeModel : PageModel
    {
        //Instantiation of Office Hour List
        public List<OfficeHour> OfficeHours { get; set; }

        //Bound property to value of user selected Office Hour
        [BindProperty]
        public int SelectedOfficeHourID { get; set; }
        //List passed to the view for user
        public InstructorHomeModel()
        {
            OfficeHours = new List<OfficeHour>();
        }
        public IActionResult OnGet()
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
                SqlDataReader OfficeHoursReader = DBClass.HoursReader((int)HttpContext.Session.GetInt32("userID"));
                while (OfficeHoursReader.Read())
                {
                    OfficeHours.Add(new OfficeHour
                    {
                        OfficeHourID = Int32.Parse(OfficeHoursReader["officeHoursID"].ToString()),
                        OfficeHoursDate = OfficeHoursReader["officeHoursDate"].ToString(),
                        TimeSlot = OfficeHoursReader["timeSlot"].ToString(),
                        Location = OfficeHoursReader["locationName"].ToString(),
                    });
                }
                // Close your connection in DBClass
                DBClass.OfficeHoursDBConnection.Close();
                return Page();
            }
        }

        //public IActionResult OnPostAddOfficeHours(int selectedOfficeHourID)
        //{
        //    HttpContext.Session.SetInt32("selectedOfficeHourID", selectedOfficeHourID);
        //    return RedirectToPage("/Home/AddOfficeHour");
        //}



        public IActionResult OnPostInstructorHome2(int selectedOfficeHourID)
        {
            HttpContext.Session.SetInt32("selectedOfficeHourID", selectedOfficeHourID);
            return RedirectToPage("/Home/InstructorHome2");
        }
    }
}
