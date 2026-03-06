using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using ResearchVault.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection.PortableExecutable;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace ResearchVault.Models
{
    public class SourceDataAccessLayer
    {

        #nullable enable
        readonly string? ConnectionString;

        private readonly IConfiguration _configuration;
        private readonly ILogger<SourceDataAccessLayer> _logger;


        public SourceDataAccessLayer(IConfiguration configuration, ILogger<SourceDataAccessLayer> logger = null)
        {
            _configuration = configuration;
            _logger = logger;
            ConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        // add source to the database
        public void AddSource(SourceModel rSource, Int32? Uid)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string strSQL = "INSERT INTO Source (Title, Author, Publisher, Link, DateCreated, DateAdded, Type, Category, Tags, Favorite, Notes, UserID) VALUES (@Title, @Author, @publisher, @Link, @DateCreated, @DateAdded, @Type, @Category, @Tags, @Favorite, @Notes, @UserID);";

                rSource.Feedback = "";
                rSource.DateAdded = DateTime.Now;

                try
                {
                    using (SqlCommand command = new SqlCommand(strSQL, conn))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddWithValue("@Title", rSource.Title);
                        command.Parameters.AddWithValue("@Author", rSource.Author);
                        command.Parameters.AddWithValue("@Publisher", rSource.Publisher);
                        command.Parameters.AddWithValue("@Link", rSource.Link);
                        command.Parameters.AddWithValue("@DateCreated", rSource.DateCreated);
                        command.Parameters.AddWithValue("@DateAdded", rSource.DateAdded);
                        command.Parameters.AddWithValue("@Type", rSource.Type);

                        if (rSource.Category == null)
                            command.Parameters.AddWithValue("@Category", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Category", rSource.Category);

                        if (rSource.Tags == null)
                            command.Parameters.AddWithValue("@Tags", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Tags", rSource.Tags);

                        command.Parameters.AddWithValue("@Favorite", rSource.Favorite);

                        if (rSource.Notes == null)
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", rSource.Notes);

                        command.Parameters.AddWithValue("@UserID", Uid);

                        conn.Open();
                        rSource.Feedback = command.ExecuteNonQuery().ToString() + " Record Added";
                        conn.Close();
                    }
                }
                catch (SqlException sqlEx)
                {
                    // Log detailed error server-side — never expose to user
                    _logger?.LogError(sqlEx, "Database error in AddSource for UserID: {Uid}", Uid);
                    rSource.Feedback = "Unable to add source. Please try again.";
                }
                catch (InvalidOperationException ioEx)
                {
                    _logger?.LogError(ioEx, "Connection error in AddSource for UserID: {Uid}", Uid);
                    rSource.Feedback = "A connection error occurred. Please try again.";
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Unexpected error in AddSource for UserID: {Uid}", Uid);
                    rSource.Feedback = "An unexpected error occurred. Please try again.";
                }
            }
        }


        // update source
        public void UpdateSource(SourceModel rSource)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand();

                    string strSQL = "UPDATE Source SET Title = @Title, Author = @Author, Publisher = @Publisher, Link = @Link, DateCreated = @DateCreated, Type = @Type, Category = @Category, Tags = @Tags, Favorite = @Favorite, Notes = @Notes WHERE SourceID = @SourceID;";

                    command.CommandText = strSQL;
                    command.Connection = conn;
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("@SourceID", rSource.SourceID);
                    command.Parameters.AddWithValue("@Title", rSource.Title);
                    command.Parameters.AddWithValue("@Author", rSource.Author);
                    command.Parameters.AddWithValue("@Publisher", rSource.Publisher);
                    command.Parameters.AddWithValue("@Link", rSource.Link);
                    command.Parameters.AddWithValue("@DateCreated", rSource.DateCreated);
                    command.Parameters.AddWithValue("@Type", rSource.Type);

                    if (rSource.Category == null)
                        command.Parameters.AddWithValue("@Category", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Category", rSource.Category);

                    if (rSource.Tags == null)
                        command.Parameters.AddWithValue("@Tags", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Tags", rSource.Tags);

                    command.Parameters.AddWithValue("@Favorite", rSource.Favorite);

                    if (rSource.Notes == null)
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Notes", rSource.Notes);

                    conn.Open();
                    rSource.Feedback = command.ExecuteNonQuery().ToString() + " Record Updated";
                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                _logger?.LogError(sqlEx, "Database error in UpdateSource for SourceID: {SourceID}", rSource.SourceID);
                rSource.Feedback = "Unable to update source. Please try again.";
            }
            catch (InvalidOperationException ioEx)
            {
                _logger?.LogError(ioEx, "Connection error in UpdateSource for SourceID: {SourceID}", rSource.SourceID);
                rSource.Feedback = "A connection error occurred. Please try again.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in UpdateSource for SourceID: {SourceID}", rSource.SourceID);
                rSource.Feedback = "An unexpected error occurred. Please try again.";
            }
        }


        // delete source from database
        public SourceModel DeleteSource(int? id)
        {
            SourceModel rSource = new SourceModel();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string strSQL = "DELETE FROM Source WHERE SourceID = @SourceID;";
                    SqlCommand comm = new SqlCommand(strSQL, conn);

                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@SourceID", id);

                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                _logger?.LogError(sqlEx, "Database error in DeleteSource for SourceID: {id}", id);
                rSource.Feedback = "Unable to delete source. Please try again.";
            }
            catch (InvalidOperationException ioEx)
            {
                _logger?.LogError(ioEx, "Connection error in DeleteSource for SourceID: {id}", id);
                rSource.Feedback = "A connection error occurred. Please try again.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in DeleteSource for SourceID: {id}", id);
                rSource.Feedback = "An unexpected error occurred. Please try again.";
            }

            return rSource;
        }


        // list Sources
        public IEnumerable<SourceModel> ListSources(Int32? uID, string? sqlStr)
        {
            List<SourceModel> sourceList = new List<SourceModel>();
            string strSQL = "";

            if (sqlStr == null)
            {
                //Safe User Id
                strSQL = "SELECT * FROM Source WHERE Source.UserID = @uID ORDER BY DateAdded DESC;";
            }
            else
            {
                strSQL = sqlStr;
            }

            try
            {
                using SqlConnection con = new(ConnectionString);
                using SqlCommand cmd = new SqlCommand(strSQL, con);

                cmd.CommandType = CommandType.Text;

                con.Open();
                using SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    SourceModel rSource = new()
                    {
                        SourceID = Convert.ToInt32(dr["SourceID"]),
                        Title = dr["Title"]?.ToString(),
                        Author = dr["Author"]?.ToString(),
                        Publisher = dr["Publisher"]?.ToString(),
                        Link = dr["Link"]?.ToString(),
                        DateCreated = dr.GetDateTime(dr.GetOrdinal("DateCreated")),
                        DateAdded = dr.GetDateTime(dr.GetOrdinal("DateAdded")),
                        Type = dr["Type"]?.ToString(),
                        Category = dr["Category"] == DBNull.Value ? null : dr["Category"].ToString(),
                        Tags = dr["Tags"] == DBNull.Value ? null : dr["Tags"].ToString(),
                        Favorite = Convert.ToBoolean(dr["Favorite"]),
                        Notes = dr["Notes"] == DBNull.Value ? null : dr["Notes"].ToString(),
                        UserID = Convert.ToInt32(dr["UserID"]),
                        Feedback = ""
                    };

                    sourceList.Add(rSource);
                }
            }
            catch (SqlException sqlEx)
            {
                // Do NOT add the raw error to sourceList — that leaks DB details to the UI
                _logger?.LogError(sqlEx, "Database error in ListSources for UserID: {uID}", uID);
                sourceList.Add(new SourceModel { Feedback = "Unable to retrieve sources. Please try again." });
            }
            catch (InvalidOperationException ioEx)
            {
                _logger?.LogError(ioEx, "Connection error in ListSources for UserID: {uID}", uID);
                sourceList.Add(new SourceModel { Feedback = "A connection error occurred. Please try again." });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in ListSources for UserID: {uID}", uID);
                sourceList.Add(new SourceModel { Feedback = "An unexpected error occurred. Please try again." });
            }

            return sourceList;
        }


        // get single source for edit
        public SourceModel GetOneSource(int? id)
        {
            SourceModel rSource = new SourceModel();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string strSQL = "SELECT * FROM Source WHERE SourceID = @SourceID;";
                    SqlCommand comm = new SqlCommand(strSQL, conn);

                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@SourceID", id);

                    conn.Open();
                    SqlDataReader dr = comm.ExecuteReader();

                    while (dr.Read())
                    {
                        rSource.SourceID = Convert.ToInt32(dr["SourceID"]);
                        rSource.Title = dr["Title"].ToString();
                        rSource.Author = dr["Author"].ToString();
                        rSource.Publisher = dr["Publisher"].ToString();
                        rSource.Link = dr["Link"].ToString();

                        //DateTime tempDate;
                        //if (dr["DateCreated"] != null && DateTime.TryParse(dr["DateCreated"].ToString(), out tempDate))
                        //{
                        //    rSource.DateCreated = tempDate;

                        //}

                        //Changed to prevent null warnings and check for null
                        rSource.DateCreated = dr["DateCreated"] != DBNull.Value && DateTime.TryParse(dr["DateCreated"].ToString(), out var dtCreated) ? dtCreated: DateTime.MinValue;
                        rSource.DateAdded = dr["DateAdded"] != DBNull.Value && DateTime.TryParse(dr["DateAdded"].ToString(), out var dtAdded) ? dtAdded : DateTime.MinValue;

                        //rSource.DateCreated = dr.IsDBNull("DateCreated") ? (DateTime?)null : dr.GetDateTime("DateCreated");
                        rSource.Type = dr["Type"].ToString();
                        rSource.Category = dr["Category"].ToString();

                        rSource.Favorite = dr["Favorite"] != DBNull.Value && Boolean.TryParse(dr["Favorite"].ToString(), out var favorite) ? favorite : false;

                        rSource.Notes = dr["Notes"].ToString();
                        rSource.UserID = Convert.ToInt32(dr["UserID"]);
                    }
                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                _logger?.LogError(sqlEx, "Database error in GetOneSource for SourceID: {id}", id);
                rSource.Feedback = "Unable to retrieve source. Please try again.";
            }
            catch (FormatException fEx)
            {
                _logger?.LogError(fEx, "Data format error in GetOneSource for SourceID: {id}", id);
                rSource.Feedback = "Source data could not be loaded. Please try again.";
            }
            catch (InvalidOperationException ioEx)
            {
                _logger?.LogError(ioEx, "Connection error in GetOneSource for SourceID: {id}", id);
                rSource.Feedback = "A connection error occurred. Please try again.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in GetOneSource for SourceID: {id}", id);
                rSource.Feedback = "An unexpected error occurred. Please try again.";
            }

            return rSource;
        }

    }
}