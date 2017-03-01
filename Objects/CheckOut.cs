using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class CheckOut
    {
        private int _id;
        private DateTime _dueDate;
        private DateTime _returnDate;
        private int _patronsId;
        private int _copiesId;

        public CheckOut(DateTime dueDate, DateTime returnDate, int patronsId, int copiesId, int id = 0)
        {
            _id = id;
            _dueDate = dueDate;
            _returnDate = returnDate;
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

        //delete all rows from checkouts db table
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM checkouts;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //return list of all checkouts in the database
        public static List<CheckOut> GetAll()
        {
            List<CheckOut> allCheckOuts = new List<CheckOut>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                DateTime dueDate = rdr.GetDateTime(1);
                DateTime returnDate = rdr.GetDateTime(2);
                int patronsId = rdr.GetInt32(3);
                int copiesId = rdr.GetInt32(4);
                CheckOut newCheckOut = new CheckOut(dueDate, returnDate, patronsId, copiesId, id);
                allCheckOuts.Add(newCheckOut);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }
            return allCheckOuts;
        }

        public override bool Equals(System.Object otherCheckOut)
        {
            if (!(otherCheckOut is CheckOut))
            {
                return false;
            }
            else
            {
                CheckOut newCheckOut = (CheckOut) otherCheckOut;
                bool idEquality = (this.GetId() == newCheckOut.GetId());
                bool dueDateEquality = (this.GetDueDate() == newCheckOut.GetDueDate());
                bool returnDateEquality = (this.GetReturnDate() == newCheckOut.GetReturnDate());
                bool patronsIdEquality = (this.GetPatronsId() == newCheckOut.GetPatronsId());
                bool copiesIdEquality = (this.GetCopiesId() == newCheckOut.GetCopiesId());

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

        //return checkout information as per id argument
        public static CheckOut Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts WHERE id = @CheckOutId;", conn);
            cmd.Parameters.Add(new SqlParameter("@CheckOutId", id.ToString()));
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
                foundReturnDate = rdr.GetDateTime(2);
                foundPatronsId = rdr.GetInt32(3);
                foundCopiesId = rdr.GetInt32(4);
            }

            CheckOut foundCheckOut = new CheckOut(foundDueDate, foundReturnDate, foundPatronsId, foundCopiesId, foundId);

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return foundCheckOut;
        }

        //delete copy from db
        public void DeleteCheckOut()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM checkouts WHERE id = @CheckOutId;", conn);
            cmd.Parameters.Add(new SqlParameter("@CheckOutId", this.GetId()));
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
    }
}
