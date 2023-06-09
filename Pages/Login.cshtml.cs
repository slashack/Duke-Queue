
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duke_Queue.Pages.DB;
using System.Data.SqlClient;
using Duke_Queue.Pages.DataClasses;

namespace Duke_Queue.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("isInstructor") != null)
            {
                return RedirectToPage("/Scheduling/Scheduling1");
            }
                return Page();

        }

        public IActionResult OnPost()
        {

            if (DBClass.HashedParameterLogin(Username, Password))
            {
                HttpContext.Session.SetString("username", Username);
                ViewData["LoginMessage"] = "Login Successful!";
                DBClass.OfficeHoursDBConnection.Close();

                //Queries for student ID and sets studentID upon login in session state
                SqlDataReader GeneralReader = DBClass.GeneralReaderQuery("SELECT instructorID " +
                    "FROM Instructor " +
                    "WHERE username = '" + Username + "' ");


                //Checks if instructorID is present and chooses whether to write instructorID or studentID into userID session state
                GeneralReader.Read();
                 if (GeneralReader.HasRows == false)
                    {
                        //Sets user as either an instructor or student using booleanish string
                        HttpContext.Session.SetString("isInstructor", "false");
                    //Queries for studentID and is only called in the first block of the if statement
                    DBClass.OfficeHoursDBConnection.Close();
                    SqlDataReader StudentReader = DBClass.GeneralReaderQuery("SELECT studentID " +
                    "FROM Student " +
                    "WHERE username ='" + Username + "' ");
                    StudentReader.Read();
                        HttpContext.Session.SetInt32("userID", (int)StudentReader["studentID"]);
                    DBClass.OfficeHoursDBConnection.Close();
                }
                    else
                    {
                        //Sets user as either an instructor or student using booleanish string
                        HttpContext.Session.SetString("isInstructor", "true");
                        HttpContext.Session.SetInt32("userID", (int)GeneralReader["instructorID"]);
                DBClass.OfficeHoursDBConnection.Close();
                    }

                return RedirectToPage("/Scheduling/Scheduling1");
            }
            else
            {
                ViewData["LoginMessage"] = "Username and/or Password Incorrect";
                DBClass.OfficeHoursDBConnection.Close();
                return Page();
            }

        }
    }
}