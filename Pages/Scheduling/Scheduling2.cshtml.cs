using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using System.Data.SqlClient;

namespace Duke_Queue.Pages
{
    public class Scheduling2Model : PageModel
    {
        public int SelectedHourID { get; set; }
        //Pass session state instantiation
        public int? selectedInstructorID;
        //Instantiation of Hour List
        public List<OfficeHour> OfficeHours { get; set; }
        public Scheduling2Model()
        {
            //Passed to view for user
            OfficeHours = new List<OfficeHour>();
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                selectedInstructorID = HttpContext.Session.GetInt32("selectedInstructorID");
                //Gathers queue length


                //Populates hours for user selection based on professor selected
                SqlDataReader HoursReader = DBClass.HoursReader((int)selectedInstructorID);
                while (HoursReader.Read())
                {
                    OfficeHours.Add(new OfficeHour
                    {
                        OfficeHourID = Int32.Parse(HoursReader["officeHoursID"].ToString()),
                        OfficeHoursDate = HoursReader["officeHoursDate"].ToString(),
                        TimeSlot = HoursReader["timeSlot"].ToString(),
                        Location = HoursReader["locationName"].ToString(),
                        QueueLength = (HoursReader["queueCount"]).ToString(),
                    });
                }
                DBClass.OfficeHoursDBConnection.Close();
                return Page();
            }

            //student finder
           
        }
        //Page Handler for buttons
        public IActionResult OnPostHourSelect(int selectedHourID)
        {
            //Session selected hour setter
            SqlDataReader GeneralReader = DBClass.GeneralReaderQuery("SELECT * " +
                "FROM Meeting M, OfficeHours O " +
                "WHERE O.officeHoursID = M.officeHoursID AND O.officeHoursID = " + selectedHourID + " AND M.studentID = " + HttpContext.Session.GetInt32("userID") + " ");
            if (GeneralReader.HasRows)
            {
                DBClass.OfficeHoursDBConnection.Close();
                TempData["ErrorMessage"] = "An appointment already exists for the selected professor and time";
                return RedirectToPage("Scheduling1");
            }
            else
            {
                DBClass.OfficeHoursDBConnection.Close();
                HttpContext.Session.SetInt32("selectedHourID", selectedHourID);
                return RedirectToPage("Scheduling3");
            }
        }
    }
}
