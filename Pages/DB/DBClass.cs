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
            cmdHoursRead.CommandText = "SELECT O.timeSlot, O.officeHoursDate, O.officeHoursID, L.locationName, Q.queueCount " +
                "FROM OfficeHours O " +
                "JOIN Instructor I ON O.instructorID = I.instructorID " +
                "JOIN Location L ON O.locationID = L.locationID " +
                "LEFT JOIN ( " +
                " SELECT officeHoursID, COUNT(*) AS queueCount " +
                "FROM OfficeHoursQueue " +
                " GROUP BY officeHoursID " +
                ") Q ON O.officeHoursID = Q.officeHoursID " +
                "WHERE I.instructorID = " + professorID + " ";

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

        public static void InsertOffice(int locationid, string date, string time, int instructorid)
        {

            string loginQuery =
                "INSERT INTO OfficeHours (locationID, officeHoursDate, timeSlot, instructorID) values (@locationID, @OfficeHoursDate, @timeSlot, @instructorID)";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = OfficeHoursDBConnection;
            cmdLogin.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@locationID", locationid);
            cmdLogin.Parameters.AddWithValue("@officeHoursDate", date);
            cmdLogin.Parameters.AddWithValue("@timeSlot", time);
            cmdLogin.Parameters.AddWithValue("@instructorID", instructorid);


            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            cmdLogin.ExecuteNonQuery();

        }

        public static SqlDataReader QueueReader(int hourID)
        {
            SqlCommand cmdQueueRead = new SqlCommand();
            cmdQueueRead.Connection = OfficeHoursDBConnection;
            cmdQueueRead.Connection.ConnectionString = OfficeHoursDBConnString;
            string loginQuery =
                    "SELECT S.studentFirstName, S.studentLastName, OQ.officeHoursQueuePurpose " +
                    "FROM OfficeHoursQueue OQ, Student S, OfficeHours O " +
                    "WHERE OQ.studentID = S.studentID and OQ.officeHoursID = O.officeHoursID and O.officeHoursID =" + hourID;
            cmdQueueRead.CommandText = loginQuery;
            cmdQueueRead.Connection.Open();
            SqlDataReader tempReader = cmdQueueRead.ExecuteReader();

            return tempReader;


        }
        public static int IDFinder(string username)
        {
            // This method expects to receive an SQL SELECT
            // query that uses the COUNT command.

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = OfficeHoursDBConnection;
            cmdLogin.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdLogin.CommandText = "SELECT instructorID FROM Instructor WHERE username = '" + username + "'";
            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            int id = (int)cmdLogin.ExecuteScalar();

            return id;

        }
        public static int OfficeIDFinder(string username)
        {
            // This method expects to receive an SQL SELECT
            // query that uses the COUNT command.

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = OfficeHoursDBConnection;
            cmdLogin.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdLogin.CommandText = "SELECT officeID FROM Instructor WHERE username = '" + username + "'";
            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            int id = (int)cmdLogin.ExecuteScalar();

            return id;

        }

        public static void InsertQueue(string officeHoursQueuePurpose, int studentID, int officeHoursID)
        {

            string insertQuery = "INSERT INTO OfficeHoursQueue (officeHoursID, officeHoursQueuePurpose, studentID) VALUES (@officeHoursID, @officeHoursQueuePurpose, @studentID)";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = OfficeHoursDBConnection;
            cmdLogin.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdLogin.CommandText = insertQuery;
            cmdLogin.Parameters.AddWithValue("@officeHoursQueuePurpose", officeHoursQueuePurpose);
            cmdLogin.Parameters.AddWithValue("@studentID", studentID);
            cmdLogin.Parameters.AddWithValue("@officeHoursID", officeHoursID);


            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            cmdLogin.ExecuteNonQuery();

        }

        public static int StudentIDFinder(string username)
        {
            // This method expects to receive an SQL SELECT
            // query that uses the COUNT command.

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = OfficeHoursDBConnection;
            cmdLogin.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdLogin.CommandText = "SELECT studentID FROM Student WHERE username = '" + username + "'";
            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            int id = (int)cmdLogin.ExecuteScalar();

            return id;

        }

        public static Boolean isSignedUp(int officeHourID, int studentID)
        {
            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = OfficeHoursDBConnection;
            cmdLogin.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdLogin.CommandText = "SELECT COUNT(*) FROM OfficeHoursQueue WHERE studentID = " + studentID + "and officeHoursID = " + officeHourID;
            cmdLogin.Connection.Open();
            int count = (int)cmdLogin.ExecuteScalar();

            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }
        public static SqlDataReader ArchiveRecord(int studentID, int hourID)
        {

            SqlCommand cmdGeneralRead = new SqlCommand();
            cmdGeneralRead.Connection = OfficeHoursDBConnection;
            cmdGeneralRead.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdGeneralRead.CommandText = "INSERT INTO ArchiveQueue SELECT * FROM OfficeHoursQueue WHERE studentID = " + studentID + "and officeHoursID =" + hourID;
            cmdGeneralRead.Connection.Open();
            SqlDataReader tempReader = cmdGeneralRead.ExecuteReader();

            return tempReader;

        }
        public static SqlDataReader DeleteRecord(int studentID, int hourID)
        {

            SqlCommand cmdGeneralRead = new SqlCommand();
            cmdGeneralRead.Connection = OfficeHoursDBConnection;
            cmdGeneralRead.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdGeneralRead.CommandText = "DELETE FROM OfficeHourQueue WHERE studentID = " + studentID + "and officeHoursID =" + hourID;
            cmdGeneralRead.Connection.Open();
            SqlDataReader tempReader = cmdGeneralRead.ExecuteReader();

            return tempReader;

        }

        public static SqlDataReader SingleStudentReader(int StudentID)
        {
            SqlCommand cmdStudentRead = new SqlCommand();
            cmdStudentRead.Connection = OfficeHoursDBConnection;
            cmdStudentRead.Connection.ConnectionString =
            OfficeHoursDBConnString;
            cmdStudentRead.CommandText = "SELECT * FROM Student WHERE studentID = " + StudentID;
            cmdStudentRead.Connection.Open();
            SqlDataReader tempReader = cmdStudentRead.ExecuteReader();
            return tempReader;
        }

        public static void UpdateStudent(Student s)
        {
            String sqlQuery = "UPDATE Student SET ";

            static SqlDataReader SingleStudentReader(int studentid)
            {
                throw new NotImplementedException();
            }

            static void UpdateProduct(Student studentview)
            {
                throw new NotImplementedException();
            }
            sqlQuery = "UPDATE Student SET studentFirstName='" + s.StudentFirstName + "', studentLastName='" + s.StudentLastName + "', studentEmail='" + s.StudentEmail + "', studentImage='" + s.Image + "' WHERE studentID=" + s.StudentID;
            SqlCommand cmdStudentRead = new SqlCommand();
            cmdStudentRead.Connection = OfficeHoursDBConnection;
            cmdStudentRead.Connection.ConnectionString =
            OfficeHoursDBConnString;
            cmdStudentRead.CommandText = sqlQuery;
            cmdStudentRead.Connection.Open();
            cmdStudentRead.ExecuteNonQuery();
        }

        public static SqlDataReader SingleInstructorReader(int InstructorID)
        {
            SqlCommand cmdStudentRead = new SqlCommand();
            cmdStudentRead.Connection = OfficeHoursDBConnection;
            cmdStudentRead.Connection.ConnectionString =
            OfficeHoursDBConnString;
            cmdStudentRead.CommandText = "SELECT * FROM Instructor WHERE instructorID = " + InstructorID;
            cmdStudentRead.Connection.Open();
            SqlDataReader tempReader = cmdStudentRead.ExecuteReader();
            return tempReader;
        }

        public static void UpdateInstructor(Instructor i)
        {
            String sqlQuery = "UPDATE Instructor SET ";

            static SqlDataReader SingleInstructorReader(int instructorid)
            {
                throw new NotImplementedException();
            }

            static void UpdateProduct(Instructor instructorview)
            {
                throw new NotImplementedException();
            }
            sqlQuery = "UPDATE Instructor SET instructorFirstName='" + i.InstructorFirstName + "', instructorLastName='" + i.InstructorLastName + "', instructorEmail='" + i.InstructorEmail + "', instructorImage='" + i.Image + "' WHERE instructorID=" + i.InstructorID;
            SqlCommand cmdStudentRead = new SqlCommand();
            cmdStudentRead.Connection = OfficeHoursDBConnection;
            cmdStudentRead.Connection.ConnectionString =
            OfficeHoursDBConnString;
            cmdStudentRead.CommandText = sqlQuery;
            cmdStudentRead.Connection.Open();
            cmdStudentRead.ExecuteNonQuery();
        }

        public static void EditOffice(int locationid, string date, string time, int instructorid,int officeID)
        {

            string loginQuery =
                "UPDATE OfficeHours SET officeHoursDate = @officeHoursDate, timeSlot = @timeSlot, instructorID = @instructorID, locationID = @locationID" +
                " WHERE officeHoursID = @officeID";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = OfficeHoursDBConnection;
            cmdLogin.Connection.ConnectionString = OfficeHoursDBConnString;
            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@officeID", officeID);
            cmdLogin.Parameters.AddWithValue("@locationID", locationid);
            cmdLogin.Parameters.AddWithValue("@officeHoursDate", date);
            cmdLogin.Parameters.AddWithValue("@timeSlot", time);
            cmdLogin.Parameters.AddWithValue("@instructorID", instructorid);



            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.    
            cmdLogin.ExecuteNonQuery();

        }

    }
}

