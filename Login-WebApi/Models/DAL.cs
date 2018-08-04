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
        public static void Registration()
        {
            //1. Instantiating connection to db.
            SqlConnection conn = new SqlConnection(GolbalParmeters.CONNECTION_STRING);
            try
            {
                //2. Open the connection
                conn.Open();

                // 3.Pass the connection to a command object
                SqlCommand cmd = new SqlCommand("select * from Customers", conn);
            }
            finally { 
            //close db connection.
            if (conn != null)
            {
                conn.Close();
            }
            }

        }
    }
}
