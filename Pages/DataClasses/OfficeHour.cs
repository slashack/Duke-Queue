namespace Duke_Queue.Pages.DataClasses
{
    public class OfficeHour
    {
        //Hours Properties
        public int OfficeHourID { get; set; }
        public String? OfficeHoursDate { get; set; }
        public String? TimeSlot { get; set; }
        
        public int locationID { get; set; }

        public int instructorID { get; set;}
        public String? Location { get; set; }

    }
}
