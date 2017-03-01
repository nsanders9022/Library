using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class Copy
    {
        private int _id;
        private int _bookId;

        public Copy(int bookId, int id = 0)
        {
            _id = id;
            _bookId = bookId;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetBookId()
        {
            return _bookId;
        }

        //delete all rows from copies db table
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM copies;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //return list of all copies in the database
        public static List<Copy> GetAll()
        {
            List<Copy> allCopies = new List<Copy>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM copies;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                int bookId = rdr.GetInt32(1);
                Copy newCopy = new Copy(bookId, id);
                allCopies.Add(newCopy);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }
            return allCopies;
        }

        public override bool Equals(System.Object otherCopy)
        {
            if (!(otherCopy is Copy))
            {
                return false;
            }
            else
            {
                Copy newCopy = (Copy) otherCopy;
                bool idEquality = (this.GetId() == newCopy.GetId());
                bool bookIdEquality = (this.GetBookId() == newCopy.GetBookId());

                return (idEquality && bookIdEquality);
            }
        }

        //Save instance into database
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO copies (books_id) OUTPUT INSERTED.id VALUES(@BookId);", conn);

            cmd.Parameters.Add(new SqlParameter("@BookId", this.GetBookId()));


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

        //return client information as per id argument
        public static Copy Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE id = @CopyId;", conn);
            cmd.Parameters.Add(new SqlParameter("@CopyId", id.ToString()));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            int foundBookId = 0;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundBookId = rdr.GetInt32(1);
            }

            Copy foundCopy = new Copy(foundBookId, foundId);

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return foundCopy;
        }

        //delete copy from db
        public void DeleteCopy()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM copies WHERE id = @CopyId;", conn);
            cmd.Parameters.Add(new SqlParameter("@CopyId", this.GetId()));
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
