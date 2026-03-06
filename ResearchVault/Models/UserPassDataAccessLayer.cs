using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserPassDataAccessLayer> _logger;

        public UserPassDataAccessLayer(IConfiguration configuration, ILogger<UserPassDataAccessLayer> logger = null)
        {
            _configuration = configuration;
            _logger = logger;
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
                    comm.Parameters.AddWithValue("@Password", ValidationLibrary.hashPassword(rUser.Password));

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
            catch (SqlException sqlEx)
            {
                //IMPORTANT: Do not expose login-specific errors
                _logger?.LogError(sqlEx, "Database error in GetUserLogin for Email: {Email}", rUser.Email);
                rUser.Feedback = "Login failed. Please check your credentials and try again.";
            }
            catch (InvalidOperationException ioEx)
            {
                _logger?.LogError(ioEx, "Connection error in GetUserLogin");
                rUser.Feedback = "A connection error occurred. Please try again.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in GetUserLogin");
                rUser.Feedback = "An unexpected error occurred. Please try again.";
            }

            return lstUserModel;
        }

    }
}