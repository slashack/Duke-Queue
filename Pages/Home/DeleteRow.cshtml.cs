using Duke_Queue.Pages.DataClasses;
using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Duke_Queue.Pages.Home
{
    public class DeleteRowModel : PageModel
    {
        [BindProperty]
        public int SelectedOfficeHourID { get; set; }

        public void OnGet(int SelectedOfficeHourID)
        {
            string queryOne = "DELETE FROM OfficeHoursQueue WHERE officeHoursID = " + SelectedOfficeHourID;
            string queryTwo = "DELETE FROM OfficeHours WHERE officeHoursID = " + SelectedOfficeHourID;
            DBClass.InsertQuery(queryOne);
            DBClass.OfficeHoursDBConnection.Close();
            DBClass.InsertQuery(queryTwo);
            DBClass.OfficeHoursDBConnection.Close();
        }
    }
}
