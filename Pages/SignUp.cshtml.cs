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
            DBClass.InsertHashedPasswordQuery("INSERT INTO HashedCredentials(Username, Password) " +
                "VALUES ('" + Username + "', '" + PasswordHash.HashPassword(Password) + "') ");

            DBClass.OfficeHoursDBConnection.Close();

            //Student Insert
            DBClass.InsertQuery(
                "INSERT INTO Student (studentID, username, studentFirstName, studentLastName, studentEmail) " +
                "VALUES ('" + studentID + "', '" + Username + "', '" + FirstName + "', '" + LastName + "', '" + Email + "')"
            );



            DBClass.OfficeHoursDBConnection.Close();

            return RedirectToPage("/Login");
        }
    }
}
