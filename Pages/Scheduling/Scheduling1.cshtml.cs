using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Duke_Queue.Pages
{
    public class Scheduling1Model : PageModel
    {
        //Instantiation of Instructor List
        public List<Instructor> Instructors { get; set; }

       

        //Bound property to value of user selection professor
        [BindProperty]
        public int SelectedInstructorID { get; set; }



        //List passed to the view for user
        public Scheduling1Model()
        {
            Instructors = new List<Instructor>();
           
        }

        //Instructor creation on page load

       



        public IActionResult OnGet()
        {
            //No Instructors Allowed
            if (HttpContext.Session.GetString("isInstructor") == "true")
            {
                return RedirectToPage("/Home/InstructorHome");
            }
            //Checks if user is logged in
            else if (HttpContext.Session.GetString("username") == null)
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
                SqlDataReader InstructorReader = DBClass.InstructorReader();
                while (InstructorReader.Read())
                {
                    Instructors.Add(new Instructor
                    {
                        InstructorID = Int32.Parse(InstructorReader["instructorID"].ToString()),
                        InstructorFirstName = InstructorReader["instructorFirstName"].ToString(),
                        InstructorLastName = InstructorReader["instructorLastName"].ToString(),
                        OfficeID = InstructorReader["officeID"].ToString(),
                        InstructorEmail = InstructorReader["instructorEmail"].ToString(),
                    });
                }
                // Close your connection in DBClass
                DBClass.OfficeHoursDBConnection.Close();
                return Page();
            }

        }

        //Page Handler for button
        public IActionResult OnPostInstructorSelect(int SelectedInstructorID)
        {
            //Session selected instructor setter
            HttpContext.Session.SetInt32("selectedInstructorID", SelectedInstructorID);
            //Routes to second index page with the route parameter of user selected instructor
            return RedirectToPage("Scheduling2");

        }


        public IActionResult OnPostMyAccount()
        {
            int StudentID = DBClass.StudentIDFinder(HttpContext.Session.GetString("username"));

            DBClass.OfficeHoursDBConnection.Close();


            return RedirectToPage("/Scheduling/StudentAccount", new { studentid = StudentID });

           
        }
    }
}
