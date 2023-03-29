using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using System.Data.SqlClient;

namespace Duke_Queue.Pages.LiveOffice
{
    public class LiveOffice1Model : PageModel
    {
        public IActionResult OnGet()
        {
            var isInstructor = HttpContext.Session.GetString("isInstructor");
            switch (isInstructor)
            {
                //If user is an instructor
                case "true":
                    //Write code for professor meeting listings
                    //Instructor facing
                    return RedirectToPage("/LiveOffice/InstructorLiveOffice1");
                //If user is a student
                case "false":
                    //Student facing
                    return Page();
                //If user is not logged in
                default:
                    return RedirectToPage("/Login");
            }
        }
    }
}
