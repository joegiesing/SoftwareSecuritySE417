using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using ResearchVault.Models;

using Microsoft.Extensions.Configuration;
using System.Reflection.PortableExecutable;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace ResearchVault.Models
{
    public class SourceDataAccessLayer
    {

        readonly string? ConnectionString;

        private readonly IConfiguration _configuration;


        public SourceDataAccessLayer(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //title, author, link, date created, date added, type, category, tags, notes

        //add source to the database
        public void AddSource(SourceModel rSource, Int32? Uid)
        {

            //creating sql connection as variable 'connection'
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                //string for sql command to add source to the database
                string strSQL = "INSERT INTO Source (Title, Author, Publisher, Link, DateCreated, DateAdded, Type, Category, Tags, Favorite, Notes, UserID) VALUES (@Title, @Author, @publisher, @Link, @DateCreated, @DateAdded, @Type, @Category, @Tags, @Favorite, @Notes, @UserID);";

                rSource.Feedback = "";
                rSource.DateAdded = DateTime.Now;

                //rSource.Title = null;
                //rSource.Author = null;
                //rSource.Publisher = null;
                //rSource.Link = null;

                //rSource.Type = null;
                //rSource.Category = null;
                //rSource.Tags = null;
                //rSource.Favorite = false;
                //rSource.Notes = null;

                try     //test for connection to database and return error if cannot connect
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
                        // Check if Category is null and inject DBNull.Value if true
                        if (rSource.Category == null)
                        {
                            command.Parameters.AddWithValue("@Category", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Category", rSource.Category);
                        }

                        // Check if Tags is null and inject DBNull.Value if true
                        if (rSource.Tags == null)
                        {
                            command.Parameters.AddWithValue("@Tags", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Tags", rSource.Tags);
                        }

                        command.Parameters.AddWithValue("@Favorite", rSource.Favorite);

                        // Check if Notes is null and inject DBNull.Value if true
                        if (rSource.Notes == null)
                        {
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Notes", rSource.Notes);
                        }

                        command.Parameters.AddWithValue("@UserID", Uid);


                        conn.Open();

                        rSource.Feedback = command.ExecuteNonQuery().ToString() + " Record Added";

                        conn.Close();
                    }
                }
                catch (Exception err)   //Error message
                {
                    rSource.Feedback = "tERROR: " + err.Message;
                }
            }
        }




        //update source
        public void UpdateSource(SourceModel rSource)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    //create sql command variable
                    SqlCommand command = new SqlCommand();

                    //rSource.DateAdded = DateTime.Now;

                    //create sql update string 
                    string strSQL = "UPDATE Source SET Title = @Title, Author = @Author, Publisher = @Publisher, Link = @Link, DateCreated = @DateCreated, Type = @Type, Category = @Category, Tags = @Tags, Favorite = @Favorite, Notes = @Notes WHERE SourceID = @SourceID;";

                    command.CommandText = strSQL;
                    command.Connection = conn;
                    command.CommandType = CommandType.Text;

                    //parameters to fill placeholders with values and send to database
                    command.Parameters.AddWithValue("@SourceID", rSource.SourceID);
                    command.Parameters.AddWithValue("@Title", rSource.Title);
                    command.Parameters.AddWithValue("@Author", rSource.Author);
                    command.Parameters.AddWithValue("@Publisher", rSource.Publisher);
                    command.Parameters.AddWithValue("@Link", rSource.Link);
                    command.Parameters.AddWithValue("@DateCreated", rSource.DateCreated);
                    command.Parameters.AddWithValue("@Type", rSource.Type);

                    // Check if Category is null and inject DBNull.Value if true
                    if (rSource.Category == null)
                    {
                        command.Parameters.AddWithValue("@Category", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Category", rSource.Category);
                    }

                    // Check if Tags is null and inject DBNull.Value if true
                    if (rSource.Tags == null)
                    {
                        command.Parameters.AddWithValue("@Tags", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Tags", rSource.Tags);
                    }

                    command.Parameters.AddWithValue("@Favorite", rSource.Favorite);

                    // Check if Notes is null and inject DBNull.Value if true
                    if (rSource.Notes == null)
                    {
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Notes", rSource.Notes);
                    }

                    //command.Parameters.AddWithValue("@UserID", rSource.UserID);

                    conn.Open();
                    rSource.Feedback = command.ExecuteNonQuery().ToString() + " Record Added";
                    //command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                rSource.Feedback = "ERROR: " + err.Message;
            }
            

        }


        //string strSQL = "UPDATE Source SET Title = @Title, Author = @Author, Publisher = @Publisher, Link = @Link, DateCreated = @DateCreated, DateAdded = @DateAdded, Type = @Type, Category = @Category, Tags = @Tags, Favorite = @Favorite, Notes = @Notes WHERE SourceID = @SourceID;";
        //command.Parameters.AddWithValue("@DateAdded", rSource.DateAdded);
        //command.Parameters.AddWithValue("@Category", rSource.Category ?? DBNull.Value);
        //command.Parameters.AddWithValue("@Tags", rSource.Tags ?? DBNull.Value);
        //command.Parameters.AddWithValue("@Favorite", rSource.Favorite);
        //command.Parameters.AddWithValue("@Notes", rSource.Notes ?? DBNull.Value);

        //delete source from database
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
            catch (Exception err)
            {
                rSource.Feedback = "ERROR: " + err.Message;
            }
            return rSource;

        }





        //list Sources
        public IEnumerable<SourceModel> ListSources(Int32? uID, string? sqlStr)
        {
            List<SourceModel> sourceList = new List<SourceModel>();
            string strSQL = "";


            if (sqlStr == null)
            {
                strSQL = "SELECT * FROM Source WHERE Source.UserID =" + uID + " ORDER BY DateAdded DESC;";
            }
            else
            {
                //strSQL = sqlStr.Trim();
                strSQL = sqlStr;
            }

            try
            {
                using SqlConnection con = new(ConnectionString);

                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.CommandType = CommandType.Text;
                

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    SourceModel rSource = new()
                    {
                        SourceID = Convert.ToInt32(dr["SourceID"]),
                        Title = dr["Title"].ToString(),
                        Author = dr["Author"].ToString(),
                        Publisher = dr["Publisher"].ToString(),
                        Link = dr["Link"].ToString(),
                        //DateAdded = DateTime.Parse(dr["DateAdded"].ToString()),
                        DateCreated = DateTime.Parse(dr["DateCreated"].ToString()),
                        Type = dr["Type"].ToString(),
                        Category = dr["Category"].ToString(),
                        Tags = dr["Tags"].ToString(),
                        Favorite = Boolean.Parse(dr["Favorite"].ToString()),
                        Notes = dr["Notes"].ToString(),
                        UserID = Convert.ToInt32(dr["UserID"]),
                        Feedback = ""

                    };

                    sourceList.Add(rSource);
                }
                con.Close();
            }
            catch (Exception err)
            {
                sourceList.Add(new SourceModel { Feedback = "ERROR: " + err.Message });
            }

            return sourceList;
        }



        //edit Source
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
                        rSource.DateCreated = DateTime.Parse(dr["DateCreated"].ToString());
                        rSource.DateAdded = DateTime.Parse(dr["DateAdded"].ToString());
                        //rSource.DateCreated = dr.IsDBNull("DateCreated") ? (DateTime?)null : dr.GetDateTime("DateCreated");
                        rSource.Type = dr["Type"].ToString();
                        rSource.Category = dr["Category"].ToString();
                        rSource.Favorite = Boolean.Parse(dr["Favorite"].ToString());
                        rSource.Notes = dr["Notes"].ToString();
                        rSource.UserID = Convert.ToInt32(dr["UserID"]);

                        
                        //rSource.Feedback += dr["DateCreated"].ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                rSource.Feedback += err.Message;
            }

            return rSource;
        }


    }
}