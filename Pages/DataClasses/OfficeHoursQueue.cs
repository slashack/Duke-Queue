namespace Duke_Queue.Pages.DataClasses
{
    public class OfficeHoursQueue
    {
        public int OfficeHoursQueueID { get; set; }
        public int StudentID { get; set; }
        public int OfficeHourID { get; set; }
        //Instructor View
        public String StudentFirstName { get; set; }
        public String StudentLastName { get; set; }
        public String StudentEmail { get; set; }

        //Used in Home
        public DateTime OfficeHoursDate { get; set; }
        public String LocationName { get; set; }
        public String InstructorFirstName { get; set; }
        public String InstructorLastName { get; set; }
        public String TimeSlot { get; set; }
        public String MeetingStart { get; set; }
        public int MeetingDuration { get; set; }
        public String officeHoursQueuePurpose { get; set; }
    }
}
