using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class Author
    {
        private int _id;
        private string _firstName;
        private string _lastName;

        public Author(string firstName, string lastName, int id = 0)
        {
            _id = id;
            _firstName = firstName;
            _lastName = lastName;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetFirstName()
        {
            return _firstName;
        }

        public string GetLastName()
        {
            return _lastName;
        }

        //delete all rows from authors db table
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //return list of all authors in the database
        public static List<Author> GetAll()
        {
            List<Author> allAuthors = new List<Author>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM authors;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string firstName = rdr.GetString(1);
                string lastName = rdr.GetString(2);
                Author newAuthor = new Author(firstName, lastName, id);
                allAuthors.Add(newAuthor);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }
            return allAuthors;
        }

        public override bool Equals(System.Object otherAuthor)
        {
            if (!(otherAuthor is Author))
            {
                return false;
            }
            else
            {
                Author newAuthor = (Author) otherAuthor;
                bool idEquality = (this.GetId() == newAuthor.GetId());
                bool firstNameEquality = (this.GetFirstName() == newAuthor.GetFirstName());
                bool lastNameEquality = (this.GetLastName() == newAuthor.GetLastName());

                return (idEquality && firstNameEquality && lastNameEquality);
            }
        }

        //Save instance into database
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO authors (first_name, last_name) OUTPUT INSERTED.id VALUES(@AuthorFirstName, @AuthorLastName);", conn);

            cmd.Parameters.Add(new SqlParameter("@AuthorFirstName", this.GetFirstName()));
            cmd.Parameters.Add(new SqlParameter("@AuthorLastName", this.GetLastName()));
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
        }

        //return author information as per id argument
        public static Author Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = @AuthorId;", conn);
            cmd.Parameters.Add(new SqlParameter("@AuthorId", id.ToString()));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundFirstName = null;
            string foundLastName = null;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundFirstName = rdr.GetFirstName(1);
                foundLastName = rdr.GetLastName(2);
            }

            Author foundAuthor = new Author(foundFirstName, foundLastName, foundId);

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return foundAuthor;
        }

        public void Update(string newFirstName = null, string newLastName = null)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            //new command to change any changed fields
            SqlCommand cmd = new SqlCommand("UPDATE authors SET first_name = @newFirstName, last_name = @newLastName OUTPUT INSERTED.first_name, INSERTED.last_name WHERE id = @AuthorId;", conn);

            //Get id of author to use in command
            cmd.Parameters.Add(new SqlParameter("@AuthorId", this.GetId()));

            //CHANGE FIRST NAME

            //If there is a new first name, change it
            if (!String.IsNullOrEmpty(newFirstName))
            {
                cmd.Parameters.Add(new SqlParameter("@newFirstName", newFirstName));
            }
            //if there isn't a new first name, don't change the name
            else
            {
                cmd.Parameters.Add(new SqlParameter("@newFirstName", this.GetFirstName()));
            }

            //CHANGE LAST NAME

            //If there is a new last name, change it
            if (!String.IsNullOrEmpty(newLastName))
            {
                cmd.Parameters.Add(new SqlParameter("@newLastName", newLastName));
            }
            //if there isn't a new last name, don't change the name
            else
            {
                cmd.Parameters.Add(new SqlParameter("@newLastName", this.GetLastName()));
            }

            //execute reader
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                this._firstName = rdr.GetString(0);
                this._lastName = rdr.GetString(1);
            }

            if(rdr!= null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }

        //delete book from db
        public void DeleteAuthor()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM authors WHERE id = @AuthorId;", conn);
            cmd.Parameters.Add(new SqlParameter("@AuthorId", this.GetId()));
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        //search for a book based on inputted title
        public static Author SearchAuthor(string lastName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE last_name = @newLastName;", conn);
            cmd.Parameters.Add(new SqlParameter("@newLastName", lastName));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundFirstName = null;
            string foundLastName = null;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundFirstName = rdr.GetString(1);
                foundLastName = rdr.GetString(2);
            }

            Author foundAuthor = new Author(foundFirstName, foundLastName, foundId);

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return foundAuthor;
        }
    }
}
