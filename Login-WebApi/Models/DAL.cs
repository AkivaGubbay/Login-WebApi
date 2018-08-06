using LoginWebApi.Contracts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
//using Models.GlobalParmeters;

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

         

        public static UserRegisterResponse Register(string userName, string firstName, string lastName, string email, string userPassword)
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
                return new UserRegisterResponse(userName, firstName, lastName, email);
            }
            catch
            {
                Console.WriteLine("My Error: sql connection problem at Register() ...");
                return null;
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

        public static int PasswordExist(string password)
        {
            // Instantiating connection to db
            SqlConnection conn = new SqlConnection(CONNECTION_STRING);
            string storedProcName = "spCountUserPassword";

            try
            {
                // Open the connection
                conn.Open();

                // Pass the connection to a command object
                SqlCommand cmd = new SqlCommand(storedProcName, conn);

                //set the command object so it knows to execute a stored procedure
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //add parameters to command, which will be passed to the stored procedure

                cmd.Parameters.Add(new SqlParameter(PARAMETER_USERPASSWORD, password));

                //// execute the bastard
                int numOfUserName = int.Parse(cmd.ExecuteScalar().ToString());

                if (numOfUserName == 0) return GlobalParmeters.VALID;
                else return GlobalParmeters.NOT_VAID;
            }
            catch
            {
                Console.WriteLine("My Error: sql connection problem at Register() ...");
                return GlobalParmeters.ENCOUNTERED_EXCEPTION;
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


        public static int EmailExist(string email)
        {
            // Instantiating connection to db
            SqlConnection conn = new SqlConnection(CONNECTION_STRING);
            string storedProcName = "spCountEmail";

            try
            {
                // Open the connection
                conn.Open();

                // Pass the connection to a command object
                SqlCommand cmd = new SqlCommand(storedProcName, conn);

                //set the command object so it knows to execute a stored procedure
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //add parameters to command, which will be passed to the stored procedure
                
                cmd.Parameters.Add(new SqlParameter(PARAMETER_EMAIL, email));

                //// execute the bastard
                int numOfUserName = int.Parse(cmd.ExecuteScalar().ToString());

                if (numOfUserName == 0) return GlobalParmeters.VALID;
                else return GlobalParmeters.NOT_VAID;
            }
            catch
            {
                Console.WriteLine("My Error: sql connection problem at Register() ...");
                return GlobalParmeters.ENCOUNTERED_EXCEPTION;
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

        public static int UserNameExist(string userName)
        {
            // Instantiating connection to db
            SqlConnection conn = new SqlConnection(CONNECTION_STRING);
            string storedProcName = "spCountUserName";
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

                // execute the bastard
                int numOfUserName = int.Parse(cmd.ExecuteScalar().ToString());

                if (numOfUserName == 0) return GlobalParmeters.VALID;
                else return GlobalParmeters.NOT_VAID;
            }
            catch(Exception ex)
            {
                Console.WriteLine("My Error: sql connection problem at Register() ...");
                return GlobalParmeters.ENCOUNTERED_EXCEPTION;
            }
            finally
            {
                // 4. Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
            //int countExist = 0;
            //var result = cme.ExecuteScalar(); //returns object that needs casting to get proper value

            //if (result != null && int.TryParse(result.ToString(), out countExist))
            return GlobalParmeters.ENCOUNTERED_EXCEPTION;
        }
    }
}
