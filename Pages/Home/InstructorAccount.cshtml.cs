using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Duke_Queue.Pages.Home
{
    public class InstructorAccountModel : PageModel
    {
        [BindProperty]
        public Instructor EditView { get; set; }

        [BindProperty]
        public IFormFile Picture { get; set; }

        public InstructorAccountModel()
        {
            EditView = new Instructor();
        }


        public void OnGet(int instructorid)
        {

            SqlDataReader SingleReader = DBClass.SingleInstructorReader(instructorid);
            while (SingleReader.Read())
            {
                EditView.InstructorID = instructorid;
                EditView.InstructorFirstName = SingleReader["instructorFirstName"].ToString();
                EditView.InstructorLastName = SingleReader["instructorLastName"].ToString();
                EditView.InstructorEmail = SingleReader["instructorEmail"].ToString();
                //Add image to SQL DATABASE Student table 
                EditView.Image = SingleReader["instructorImage"].ToString();
            }
            DBClass.OfficeHoursDBConnection.Close();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (Picture != null && Picture.Length > 0)
            {
                // Save the uploaded picture
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Picture.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await Picture.CopyToAsync(stream);
                }
                EditView.Image = fileName;
            }

            if (Picture == null || Picture.Length == 0)
            {
                // Get the current image from the database
                SqlDataReader SingleReader = DBClass.SingleInstructorReader(EditView.InstructorID);
                while (SingleReader.Read())
                {
                    EditView.Image = SingleReader["instructorImage"].ToString();
                }
                DBClass.OfficeHoursDBConnection.Close();
            }

            DBClass.UpdateInstructor(EditView);
            DBClass.OfficeHoursDBConnection.Close();


            return RedirectToPage("/Home/InstructorAccount", new { instructorid = EditView.InstructorID });
        }
    }
}
