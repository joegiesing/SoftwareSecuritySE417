using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ResearchVault.Models;

namespace ResearchVault.Models
{
    public class UserDataAccessLayer
    {

        readonly string ConnectionString;

        private readonly IConfiguration _configuration;
        private readonly ILogger<UserDataAccessLayer> _logger;

        public UserDataAccessLayer(IConfiguration configuration, ILogger<UserDataAccessLayer> logger = null)
        {
            _configuration = configuration;
            _logger = logger;
            ConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        // add user to the database
        public void AddUser(UserModel rUser)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string strSQL = "INSERT Into Users (FName, LName, Email, Username, Password, Permissions) VALUES (@FName, @LName, @Email, @Username, @Password, @Permissions);";

                rUser.Feedback = "";

                try
                {
                    using (SqlCommand command = new SqlCommand(strSQL, conn))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddWithValue("@FName", rUser.FName);
                        command.Parameters.AddWithValue("@LName", rUser.LName);
                        command.Parameters.AddWithValue("@Email", rUser.Email);
                        command.Parameters.AddWithValue("@Username", rUser.Username);
                        command.Parameters.AddWithValue("@Password", rUser.Password);
                        command.Parameters.AddWithValue("@Permissions", rUser.Permissions);

                        conn.Open();
                        rUser.Feedback = command.ExecuteNonQuery().ToString() + " User Added";
                        conn.Close();
                    }
                }
                catch (SqlException sqlEx)
                {
                    _logger?.LogError(sqlEx, "Database error in AddUser for Username: {Username}", rUser.Username);
                    rUser.Feedback = "Unable to create account. Please try again.";
                }
                catch (InvalidOperationException ioEx)
                {
                    _logger?.LogError(ioEx, "Connection error in AddUser for Username: {Username}", rUser.Username);
                    rUser.Feedback = "A connection error occurred. Please try again.";
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Unexpected error in AddUser for Username: {Username}", rUser.Username);
                    rUser.Feedback = "An unexpected error occurred. Please try again.";
                }
            }
        }


        // update user
        public void UpdateUser(UserModel rUser)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand();

                    string strSQL = "UPDATE Users SET FName = @FName, LName = @LName, Email = @Email, Username = @Username, Password = @Password, Permissions = @Permissions WHERE UserID = @UserID;";

                    command.CommandText = strSQL;
                    command.Connection = conn;
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("@FName", rUser.FName);
                    command.Parameters.AddWithValue("@LName", rUser.LName);
                    command.Parameters.AddWithValue("@Email", rUser.Email);
                    command.Parameters.AddWithValue("@Username", rUser.Username);
                    command.Parameters.AddWithValue("@Password", rUser.Password);
                    command.Parameters.AddWithValue("@Permissions", rUser.Permissions);
                    command.Parameters.AddWithValue("@UserID", rUser.UserID);

                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                _logger?.LogError(sqlEx, "Database error in UpdateUser for UserID: {UserID}", rUser.UserID);
                rUser.Feedback = "Unable to update user. Please try again.";
            }
            catch (InvalidOperationException ioEx)
            {
                _logger?.LogError(ioEx, "Connection error in UpdateUser for UserID: {UserID}", rUser.UserID);
                rUser.Feedback = "A connection error occurred. Please try again.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in UpdateUser for UserID: {UserID}", rUser.UserID);
                rUser.Feedback = "An unexpected error occurred. Please try again.";
            }
        }


        // delete user from database
        public UserModel DeleteUser(Int32? id)
        {
            UserModel rUser = new UserModel();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string strSQL = "DELETE FROM Users WHERE UserID = @UserID;";
                    SqlCommand comm = new SqlCommand(strSQL, conn);

                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@UserID", id);

                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                _logger?.LogError(sqlEx, "Database error in DeleteUser for UserID: {id}", id);
                rUser.Feedback = "Unable to delete user. Please try again.";
            }
            catch (InvalidOperationException ioEx)
            {
                _logger?.LogError(ioEx, "Connection error in DeleteUser for UserID: {id}", id);
                rUser.Feedback = "A connection error occurred. Please try again.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in DeleteUser for UserID: {id}", id);
                rUser.Feedback = "An unexpected error occurred. Please try again.";
            }

            return rUser;
        }


        // list Users
        public IEnumerable<UserModel> ListUsers(string? sqlStr)
        {
            List<UserModel> userList = new List<UserModel>();
            string strSQL = "";

            if (sqlStr == null)
            {
                strSQL = "SELECT * FROM Users ORDER BY UserID DESC;";
            }
            else
            {
                strSQL = sqlStr.Trim();
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(strSQL, conn);
                    command.CommandType = CommandType.Text;

                    conn.Open();
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        UserModel user = new UserModel
                        {
                            UserID = Convert.ToInt32(dr["UserID"]),
                            FName = dr["FName"].ToString(),
                            LName = dr["LName"].ToString(),
                            Email = dr["Email"].ToString(),
                            Username = dr["Username"].ToString(),
                            Password = dr["Password"].ToString(),
                            Permissions = Convert.ToInt32(dr["Permissions"].ToString()),
                            Feedback = ""
                        };

                        userList.Add(user);
                    }

                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                _logger?.LogError(sqlEx, "Database error in ListUsers");
                userList.Add(new UserModel { Feedback = "Unable to retrieve users. Please try again." });
            }
            catch (InvalidOperationException ioEx)
            {
                _logger?.LogError(ioEx, "Connection error in ListUsers");
                userList.Add(new UserModel { Feedback = "A connection error occurred. Please try again." });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in ListUsers");
                userList.Add(new UserModel { Feedback = "An unexpected error occurred. Please try again." });
            }

            return userList;
        }


        // get single user for edit
        public UserModel GetOneUser(int? id)
        {
            UserModel rUser = new UserModel();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string strSQL = "SELECT * FROM Users WHERE UserID = @UserID;";
                    SqlCommand comm = new SqlCommand(strSQL, conn);

                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@UserID", id);

                    conn.Open();
                    SqlDataReader dr = comm.ExecuteReader();

                    while (dr.Read())
                    {
                        rUser.UserID = Convert.ToInt32(dr["UserID"]);
                        rUser.FName = dr["FName"].ToString();
                        rUser.LName = dr["LName"].ToString();
                        rUser.Email = dr["Email"].ToString();
                        rUser.Username = dr["Username"].ToString();
                        rUser.Password = dr["Password"].ToString();
                        rUser.Permissions = Convert.ToInt32(dr["Permissions"].ToString());
                    }
                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                _logger?.LogError(sqlEx, "Database error in GetOneUser for UserID: {id}", id);
                rUser.Feedback = "Unable to retrieve user. Please try again.";
            }
            catch (InvalidOperationException ioEx)
            {
                _logger?.LogError(ioEx, "Connection error in GetOneUser for UserID: {id}", id);
                rUser.Feedback = "A connection error occurred. Please try again.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in GetOneUser for UserID: {id}", id);
                rUser.Feedback = "An unexpected error occurred. Please try again.";
            }

            return rUser;
        }

    }
}