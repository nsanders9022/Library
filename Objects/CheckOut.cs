using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class Checkout
    {
        private int _id;
        private DateTime _dueDate;
        private DateTime _returnDate;
        private int _patronsId;
        private int _copiesId;

        public DateTime defaultReturnDate = new DateTime(1900, 1, 1);

        public Checkout(DateTime dueDate, int patronsId, int copiesId, int id = 0)
        {
            _id = id;
            _dueDate = dueDate;
            _returnDate = defaultReturnDate;
            _patronsId = patronsId;
            _copiesId = copiesId;
        }

        public int GetId()
        {
            return _id;
        }

        public DateTime GetDueDate()
        {
            return _dueDate;
        }

        public DateTime GetReturnDate()
        {
            return _returnDate;
        }

        public int GetPatronsId()
        {
            return _patronsId;
        }

        public int GetCopiesId()
        {
            return _copiesId;
        }

        public void SetReturnDate(DateTime newReturnDate)
        {
          _returnDate = newReturnDate;
        }

        //delete all rows from Checkouts db table
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM checkouts;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //return list of all Checkouts in the database
        public static List<Checkout> GetAll()
        {
            List<Checkout> allCheckouts = new List<Checkout>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                DateTime dueDate = rdr.GetDateTime(1);
                int patronsId = rdr.GetInt32(3);
                int copiesId = rdr.GetInt32(4);
                Checkout newCheckout = new Checkout(dueDate, patronsId, copiesId, id);
                allCheckouts.Add(newCheckout);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }
            return allCheckouts;
        }

        public override bool Equals(System.Object otherCheckout)
        {
            if (!(otherCheckout is Checkout))
            {
                return false;
            }
            else
            {
                Checkout newCheckout = (Checkout) otherCheckout;
                bool idEquality = (this.GetId() == newCheckout.GetId());
                bool dueDateEquality = (this.GetDueDate() == newCheckout.GetDueDate());
                bool returnDateEquality = (this.GetReturnDate() == newCheckout.GetReturnDate());
                bool patronsIdEquality = (this.GetPatronsId() == newCheckout.GetPatronsId());
                bool copiesIdEquality = (this.GetCopiesId() == newCheckout.GetCopiesId());

                return (idEquality && dueDateEquality && returnDateEquality && patronsIdEquality && copiesIdEquality);
            }
        }

        //Save instance into database
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (due_date, date_returned, patrons_id, copies_id) OUTPUT INSERTED.id VALUES(@DueDate, @ReturnDate, @PatronsId, @CopiesId);", conn);

            cmd.Parameters.Add(new SqlParameter("@DueDate", this.GetDueDate()));

            cmd.Parameters.Add(new SqlParameter("@ReturnDate", this.GetReturnDate()));

            cmd.Parameters.Add(new SqlParameter("@PatronsId", this.GetPatronsId()));

            cmd.Parameters.Add(new SqlParameter("@CopiesId", this.GetCopiesId()));


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

        //return Checkout information as per id argument
        public static Checkout Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts WHERE id = @CheckoutId;", conn);
            cmd.Parameters.Add(new SqlParameter("@CheckoutId", id.ToString()));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            DateTime foundDueDate = new DateTime(1900, 1, 1);
            DateTime foundReturnDate = new DateTime(1900, 1, 1);
            int foundPatronsId = 0;
            int foundCopiesId = 0;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundDueDate = rdr.GetDateTime(1);
                // foundReturnDate = rdr.GetDateTime(2);
                foundPatronsId = rdr.GetInt32(3);
                foundCopiesId = rdr.GetInt32(4);
            }

            Checkout foundCheckout = new Checkout(foundDueDate, foundPatronsId, foundCopiesId, foundId);

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return foundCheckout;
        }

        //delete copy from db
        public void DeleteCheckout()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM checkouts WHERE id = @CheckoutId;", conn);
            cmd.Parameters.Add(new SqlParameter("@CheckoutId", this.GetId()));
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void UpdateReturnDate(DateTime newReturnDate)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            //new command to change any changed fields
            SqlCommand cmd = new SqlCommand("UPDATE checkouts SET date_returned = @ReturnDate OUTPUT INSERTED.date_returned WHERE id = @CheckoutId;", conn);

            //Get id of author to use in command
            cmd.Parameters.Add(new SqlParameter("@CheckoutId", this.GetId()));
            cmd.Parameters.Add(new SqlParameter("@ReturnDate", newReturnDate));

            //execute reader
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                this._returnDate = rdr.GetDateTime(0);
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

        //search for a book based on inputted title
        public static List<Checkout> SearchDueDate(DateTime dueDate)
        {
            List<Checkout> allDue = new List<Checkout>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts WHERE due_date = @DueDate;", conn);
            cmd.Parameters.Add(new SqlParameter("@DueDate", dueDate));
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                DateTime foundDueDate = rdr.GetDateTime(1);
                // DateTime foundReturnDate = rdr.GetDateTime(2);
                int foundPatronsId = rdr.GetInt32(3);
                int foundCopiesId = rdr.GetInt32(4);

                Checkout foundCheckout = new Checkout(foundDueDate, foundPatronsId, foundCopiesId, foundId);
                allDue.Add(foundCheckout);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return allDue;
        }

        public string GetBookTitle()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("SELECT books.title FROM checkouts JOIN copies ON (checkouts.copies_id = copies.id) JOIN books ON (copies.books_id = books.id) WHERE checkouts.id = @CheckoutId;", conn);

          cmd.Parameters.Add(new SqlParameter("@CheckoutId", this.GetId()));
          SqlDataReader rdr = cmd.ExecuteReader();

          string bookTitle = "";

          while (rdr.Read())
          {
            bookTitle = rdr.GetString(0);
          }

          if(rdr != null)
          {
              rdr.Close();
          }

          if(conn != null)
          {
              conn.Close();
          }

          return bookTitle;
        }
    }
}
