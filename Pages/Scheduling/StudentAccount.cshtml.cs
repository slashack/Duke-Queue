using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Duke_Queue.Pages.Scheduling
{
    public class StudentAccountModel : PageModel
    {
        [BindProperty]
        public Student EditView { get; set; }

        [BindProperty]
        public IFormFile Picture { get; set; }      

        public StudentAccountModel()
        {
            EditView = new Student();
        }


        public void OnGet(int studentid)
        {

            SqlDataReader SingleReader = DBClass.SingleStudentReader(studentid);

                while (SingleReader.Read())
                {
                    EditView.StudentID = studentid;
                    EditView.StudentFirstName = SingleReader["studentFirstName"].ToString();
                    EditView.StudentLastName = SingleReader["studentLastName"].ToString();
                    EditView.StudentEmail = SingleReader["studentEmail"].ToString();
                    //Add image to SQL DATABASE Student table 
                    EditView.Image = SingleReader["studentImage"].ToString();
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
                SqlDataReader SingleReader = DBClass.SingleStudentReader(EditView.StudentID);
                while (SingleReader.Read())
                {
                    EditView.Image = SingleReader["studentImage"].ToString();
                }
                DBClass.OfficeHoursDBConnection.Close();
            }

            DBClass.UpdateStudent(EditView);
            DBClass.OfficeHoursDBConnection.Close();


            return RedirectToPage("/Scheduling/StudentAccount", new { studentid = EditView.StudentID });
        }
    }


}


