using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ResearchVault.Models
{
    public class UserPassDataAccessLayer
    {

        string ConnectionString;

        private readonly IConfiguration _configuration;

        public UserPassDataAccessLayer(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }



        public IEnumerable<UserPassModel> GetUserLogin(UserPassModel rUser)
        {
            List<UserPassModel> lstUserModel = [];

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string strSQL = "SELECT TOP 1 * FROM Users WHERE Email = @Email AND Password = @Password;";
                    SqlCommand comm = new SqlCommand(strSQL, conn);
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.AddWithValue("@Email", rUser.Email);
                    comm.Parameters.AddWithValue("@Password", rUser.Password);

                    conn.Open();
                    SqlDataReader dr = comm.ExecuteReader();

                    while (dr.Read())
                    {
                        UserPassModel rMatch = new UserPassModel();

                        rMatch.UserID = Convert.ToInt32(dr["UserID"]);
                        rMatch.Email = dr["Email"].ToString();
                        rMatch.Password = dr["Password"].ToString();
                        rMatch.Permissions = Convert.ToInt32(dr["Permissions"]);

                        lstUserModel.Add(rMatch);
                    }

                    conn.Close();
                }
            }
            catch (Exception err)
            {
                rUser.Feedback = "ERROR: " + err.Message;
            }

            return lstUserModel;
        }

    }
}
