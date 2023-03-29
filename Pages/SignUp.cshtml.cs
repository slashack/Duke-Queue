using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Duke_Queue.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        [Required]
        public String? FirstName { get; set; }
        [BindProperty]
        [Required]
        public String? LastName { get; set; }
        [BindProperty]
        [Required]
        public String? Email { get; set; }
        [BindProperty]
        [Required]
        public String? Username { get; set; }
        [BindProperty]
        [Required]
        public String? Password { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            Random random = new Random();
            int studentID = random.Next(0, 100000);
            int credentialsID = random.Next(100001, 200000);
            //Credentials Insert
            DBClass.InsertQuery("INSERT INTO HashedCredentials(credentialsID, username, password) " +
                "VALUES (" + credentialsID + ", '" + Username + "', '" + Password + "') ");

            DBClass.OfficeHoursDBConnection.Close();

            //Student Insert
            DBClass.InsertQuery("INSERT INTO Student(studentID, studentFirstName, studentLastName, studentEmail, credentialsID) " +
                "VALUES (" + studentID + ", '" + FirstName + "', '" + LastName + "', '" + Email + "', " + credentialsID + ") ");


            DBClass.OfficeHoursDBConnection.Close();

            return RedirectToPage("/Login");
        }
    }
}
