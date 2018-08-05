using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
//using Models.GolbalParmeters;

namespace LoginWebApi.Models
{
    public class DAL
    {
        // sql data    
        public static readonly string CONNECTION_STRING = "Data Source=DESKTOP-GT17LEF\\SQLEXPRESS;Initial Catalog=UserAccounts;Integrated Security=True";
        public static readonly string PARAMETER_USERNAME = "@UserName";
        public static readonly string PARAMETER_FIRSTNAME = "@FirstName";
        public static readonly string PARAMETER_LASTNAEM = "@LastName";
        public static readonly string PARAMETER_EMAIL = "@Email";
        public static readonly string PARAMETER_USERPASSWORD = "@UserPassword";

        public static void Register(string userName, string firstName, string lastName, string email, string userPassword)
        {
            // Instantiating connection to db
            SqlConnection conn = new SqlConnection(CONNECTION_STRING);
            string storedProcName = "spAccount_Add";
            try
            {
                // Open the connection
                conn.Open();

                // Pass the connection to a command object
                SqlCommand cmd = new SqlCommand(storedProcName, conn);

                //set the command object so it knows to execute a stored procedure
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter(PARAMETER_USERNAME, userName));
                cmd.Parameters.Add(new SqlParameter(PARAMETER_FIRSTNAME, firstName));
                cmd.Parameters.Add(new SqlParameter(PARAMETER_LASTNAEM, lastName));
                cmd.Parameters.Add(new SqlParameter(PARAMETER_EMAIL, email));
                cmd.Parameters.Add(new SqlParameter(PARAMETER_USERPASSWORD, userPassword));

                //// execute the bastard
                cmd.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("My Error: sql connection problem at Register() ...");
            }
            finally
            {
                // 4. Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
