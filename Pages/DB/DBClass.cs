using Duke_Queue.Pages.DB;
using Microsoft.AspNetCore.Identity;
using Duke_Queue.Pages.DataClasses;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Duke_Queue.Pages.DB;

namespace Duke_Queue.Pages.DB
{
    public class DBClass
    {
        // Connection at Data Field Level
        public static SqlConnection OfficeHoursDBConnection = new SqlConnection
        ();
        // Connection String
        // www.connectionstrings.com
        // Connection Methods:
        private static readonly String? OfficeHoursDBConnString =
        "Server=Localhost;Database=Lab3;Trusted_Connection=True";
        private static readonly String? AuthConnString = "Server=Localhost;Database=AUTH;Trusted_Connection=True";

        //Authentication DB String And Hashed Login
        public static bool HashedParameterLogin(string Username, string Password)
        {
            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = OfficeHoursDBConnection;
            cmdLogin.Connection.ConnectionString = AuthConnString;

            cmdLogin.CommandType = System.Data.CommandType.StoredProcedure;
            cmdLogin.CommandText = "sp_Lab3Login";
            cmdLogin.Parameters.AddWithValue("@Username", Username);
            cmdLogin.Parameters.AddWithValue("@Password", Password);
            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            SqlDataReader hashReader = cmdLogin.ExecuteReader();
            if (hashReader.Read())
            {
                string correctHash = hashReader["Password"].ToString();

                if (PasswordHash.ValidatePassword(Password, correctHash))
                {
                    return true;
                }
            }

            return false;
        }


        public static void InsertHashedPasswordQuery(string sqlQuery)
        {

            SqlCommand cmdGeneralInsert = new SqlCommand();
            cmdGeneralInsert.Connection = OfficeHoursDBConnection;
            cmdGeneralInsert.Connection.ConnectionString = AuthConnString;
            cmdGeneralInsert.CommandText = sqlQuery;
            cmdGeneralInsert.Connection.Open();
            cmdGeneralInsert.ExecuteNonQuery();

        }
        public static void CreateHashedUser(string Username, string Password, int credentialsID)
        {
            string loginQuery = "INSERT INTO HashedCredentials (username,password,credentialsID) values (@Username, @Password, @CredentialsID)";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = OfficeHoursDBConnection;
            cmdLogin.Connection.ConnectionString = OfficeHoursDBConnString;

            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@Username", Username);
            cmdLogin.Parameters.AddWithValue("@Password", PasswordHash.HashPassword(Password));
            cmdLogin.Parameters.AddWithValue("@CredentialsID", credentialsID);

            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            cmdLogin.ExecuteNonQuery();

        }

        //Instructor SQL Reader
        public static SqlDataReader InstructorReader()
        {
            SqlCommand cmdInstructorRead = new SqlCommand();
            cmdInstructorRead.Connection = OfficeHoursDBConnection;
            cmdInstructorRead.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdInstructorRead.CommandText = "SELECT * FROM Instructor";
            cmdInstructorRead.Connection.Open();
            SqlDataReader tempReader = cmdInstructorRead.ExecuteReader();
            return tempReader;
        }
        //Student SQL Reader
        public static SqlDataReader StudentReader()
        {
            SqlCommand cmdStudentRead = new SqlCommand();
            cmdStudentRead.Connection = OfficeHoursDBConnection;
            cmdStudentRead.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdStudentRead.CommandText = "SELECT * FROM Student";
            cmdStudentRead.Connection.Open();
            SqlDataReader tempReader = cmdStudentRead.ExecuteReader();
            return tempReader;
        }
        //User Selected Hours Reader 
        public static SqlDataReader HoursReader(int professorID)
        {
            SqlCommand cmdHoursRead = new SqlCommand();
            cmdHoursRead.Connection = OfficeHoursDBConnection;
            cmdHoursRead.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdHoursRead.CommandText = "SELECT O.timeSlot, O.officeHoursDate ,O.officeHoursID, L.locationName " +
                "FROM OfficeHours O, Instructor I, Location L " +
                "WHERE  I.instructorID = O.instructorID and O.locationID = L.locationID and I.instructorID = " + professorID;

            cmdHoursRead.Connection.Open();
            SqlDataReader tempReader = cmdHoursRead.ExecuteReader();
            return tempReader;

        }

        // Query is passed from the invoking code.
        public static SqlDataReader GeneralReaderQuery(string sqlQuery)
        {

            SqlCommand cmdGeneralRead = new SqlCommand();
            cmdGeneralRead.Connection = OfficeHoursDBConnection;
            cmdGeneralRead.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdGeneralRead.CommandText = sqlQuery;
            cmdGeneralRead.Connection.Open();
            SqlDataReader tempReader = cmdGeneralRead.ExecuteReader();

            return tempReader;

        }

        // Can run and return results for any query, if results exist.
        // Query is passed from the invoking code.
        public static void InsertQuery(string sqlQuery)
        {

            SqlCommand cmdGeneralInsert = new SqlCommand();
            cmdGeneralInsert.Connection = OfficeHoursDBConnection;
            cmdGeneralInsert.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdGeneralInsert.CommandText = sqlQuery;
            cmdGeneralInsert.Connection.Open();
            cmdGeneralInsert.ExecuteNonQuery();

        }
       
        //public static void InsertOffice(int locationName, string date, string time, int instructorID)
        //{

        //    string loginQuery =
        //        "INSERT INTO OfficeHours (locationID, officeHoursDate, timeSlot, instructorID) values (@office#, @dateID, @timeID, @facultyID)";

        //    SqlCommand cmdLogin = new SqlCommand();
        //    cmdLogin.Connection = Lab3DBConnection;
        //    cmdLogin.Connection.ConnectionString = Lab3DBConnString;
        //    cmdLogin.CommandText = loginQuery;
        //    cmdLogin.Parameters.AddWithValue("@office#", officenumber);
        //    cmdLogin.Parameters.AddWithValue("@dateID", date);
        //    cmdLogin.Parameters.AddWithValue("@timeID", time);
        //    cmdLogin.Parameters.AddWithValue("@facultyID", facultyID);


        //    cmdLogin.Connection.Open();

        //    // ExecuteScalar() returns back data type Object
        //    // Use a typecast to convert this to an int.
        //    // Method returns first column of first row.
        //    cmdLogin.ExecuteNonQuery();

        //}
    }
}

