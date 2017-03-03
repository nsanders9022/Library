using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class Patron
    {
        private int _id;
        private string _firstName;
        private string _lastName;

        public Patron(string firstName, string lastName, int id = 0)
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

        //delete all rows from patrons db table
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //return list of all patrons in the database
        public static List<Patron> GetAll()
        {
            List<Patron> allPatrons = new List<Patron>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM patrons;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string firstName = rdr.GetString(1);
                string lastName = rdr.GetString(2);
                Patron newPatron = new Patron(firstName, lastName, id);
                allPatrons.Add(newPatron);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }
            return allPatrons;
        }

        public override bool Equals(System.Object otherPatron)
        {
            if (!(otherPatron is Patron))
            {
                return false;
            }
            else
            {
                Patron newPatron = (Patron) otherPatron;
                bool idEquality = (this.GetId() == newPatron.GetId());
                bool firstNameEquality = (this.GetFirstName() == newPatron.GetFirstName());
                bool lastNameEquality = (this.GetLastName() == newPatron.GetLastName());

                return (idEquality && firstNameEquality && lastNameEquality);
            }
        }

        //Save instance into database
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO patrons (first_name, last_name) OUTPUT INSERTED.id VALUES(@PatronFirstName, @PatronLastName);", conn);

            cmd.Parameters.Add(new SqlParameter("@PatronFirstName", this.GetFirstName()));
            cmd.Parameters.Add(new SqlParameter("@PatronLastName", this.GetLastName()));
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

        //return patron information as per id argument
        public static Patron Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = @PatronId;", conn);
            cmd.Parameters.Add(new SqlParameter("@PatronId", id.ToString()));
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

            Patron foundPatron = new Patron(foundFirstName, foundLastName, foundId);

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return foundPatron;
        }

        public void UpdateName(string newFirstName = null, string newLastName = null)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            //new command to change any changed fields
            SqlCommand cmd = new SqlCommand("UPDATE patrons SET first_name = @newFirstName, last_name = @newLastName OUTPUT INSERTED.first_name, INSERTED.last_name WHERE id = @PatronId;", conn);

            //Get id of patron to use in command
            cmd.Parameters.Add(new SqlParameter("@PatronId", this.GetId()));

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
        public void DeletePatron()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM patrons WHERE id = @PatronId;", conn);
            cmd.Parameters.Add(new SqlParameter("@PatronId", this.GetId()));
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        //search for a book based on inputted title
        public static Patron SearchPatron(string lastName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE last_name = @newLastName;", conn);
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

            Patron foundPatron = new Patron(foundFirstName, foundLastName, foundId);

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return foundPatron;
        }

        public List<Book> CheckedOutBooks()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT books.* FROM patrons JOIN checkouts ON (patrons.id = checkouts.patrons_id) JOIN copies ON (checkouts.copies_id = copies.id) JOIN books on (books.id = copies.books_id) WHERE patrons.id = @PatronsId;", conn);

            cmd.Parameters.Add(new SqlParameter("@PatronsId", this.GetId().ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Book> books = new List<Book>{};

            while(rdr.Read())
            {
                int bookId = rdr.GetInt32(0);
                string title = rdr.GetString(1);

                Book newBook = new Book(title, bookId);
                books.Add(newBook);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return books;
        }
    }
}
