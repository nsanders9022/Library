using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
  public class Book
  {
    private int _id;
    private string _title;

    public Book(string title, int id = 0)
    {
      _id = id;
      _title = title;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetTitle()
    {
      return _title;
    }

    //delete all rows from clients db table
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM books;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    //return list of all books in the database
    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string title = rdr.GetString(1);
        Book newBook = new Book(title, id);
        allBooks.Add(newBook);
      }

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }
      return allBooks;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = (this.GetId() == newBook.GetId());
        bool titleEquality = (this.GetTitle() == newBook.GetTitle());

        return (idEquality && titleEquality);
      }
    }

    //Save instance into database
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (title) OUTPUT INSERTED.id VALUES(@BookTitle);", conn);

      cmd.Parameters.Add(new SqlParameter("@BookTitle", this.GetTitle()));


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
    public static Book Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = @BookId;", conn);
      cmd.Parameters.Add(new SqlParameter("@BookId", id.ToString()));
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string foundTitle = null;

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundTitle = rdr.GetString(1);
      }

      Book foundBook = new Book(foundTitle, foundId);

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return foundBook;
    }

    public void UpdateTitle(string newTitle)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE books SET title = @NewTitle OUTPUT INSERTED.title WHERE id = @BookId;", conn);
      cmd.Parameters.Add(new SqlParameter("@NewTitle", newTitle));
      cmd.Parameters.Add(new SqlParameter("@BookId", this.GetId()));

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._title = rdr.GetString(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
    }

    //delete book from db
    public void DeleteBook()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM books WHERE id = @BookId;", conn);
      cmd.Parameters.Add(new SqlParameter("@BookId", this.GetId()));
      cmd.ExecuteNonQuery();

      conn.Close();
    }

    //search for a book based on inputted title
    public static Book SearchTitle(string title)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE title = @BookTitle;", conn);
      cmd.Parameters.Add(new SqlParameter("@BookTitle", title));
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string foundTitle = null;

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundTitle = rdr.GetString(1);
      }

      Book foundBook = new Book(foundTitle, foundId);

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return foundBook;
    }

    //return authors linked to book
    public List<Author> GetAuthors()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT authors.* FROM books JOIN books_authors ON (books.id = books_authors.books_id) JOIN authors ON (books_authors.authors_id = authors.id) WHERE books.id = @BookId;", conn);
      cmd.Parameters.Add(new SqlParameter("@BookId", this.GetId().ToString()));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Author> authors = new List<Author>{};

      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string firstName = rdr.GetString(1);
        string lastName = rdr.GetString(2);
        Author newAuthor = new Author(firstName, lastName, authorId);
        authors.Add(newAuthor);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return authors;
    }

    //add row to books_authors
    public void AddAuthor(Author author)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books_authors (books_id, authors_id) VALUES (@BookId, @AuthorId);", conn);
      cmd.Parameters.Add(new SqlParameter("@BookId", this.GetId()));
      cmd.Parameters.Add(new SqlParameter("@AuthorId", author.GetId()));

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    //return copies of a book
    public List<Copy> GetCopies()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE books_id = @BookId;", conn);
      cmd.Parameters.Add(new SqlParameter("@BookId", this.GetId().ToString()));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Copy> copies = new List<Copy>{};

      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        int booksId = rdr.GetInt32(1);
        Copy newCopy = new Copy(booksId, copyId);
        copies.Add(newCopy);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return copies;
    }

    //returns number of copies of a book
    public int CountCopies()
    {
      return this.GetCopies().Count;
    }

    //Create new instances of copy with bookId
    public void AddCopy(int numberOfCopies)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO copies (books_id) VALUES (@BookId);", conn);
      cmd.Parameters.Add(new SqlParameter("@BookId", this.GetId()));

      for(int i = 0; i < numberOfCopies; i++)
      {
        cmd.ExecuteNonQuery();
      }

      if (conn != null)
      {
        conn.Close();
      }
    }

    //Create new row in checkout table
    public void CheckoutBook(int patronId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT TOP(1) id FROM copies WHERE books_id = @BookId AND copies.id NOT IN (SELECT copies_id FROM checkouts);", conn);
      cmd.Parameters.Add(new SqlParameter("@BookId", this.GetId()));
      SqlDataReader rdr = cmd.ExecuteReader();

      int copyId = 0;

      while(rdr.Read())
      {
        copyId = rdr.GetInt32(0);
      }

      DateTime dueDate = DateTime.Today.AddDays(14);

      CheckOut newCheckOut = new CheckOut(dueDate, patronId, copyId);

      newCheckOut.Save();

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public int AvailableCopies()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts WHERE copies_id IN (SELECT id FROM copies WHERE books_id = @BookId);", conn);
      cmd.Parameters.Add(new SqlParameter("@BookId", this.GetId()));
      SqlDataReader rdr = cmd.ExecuteReader();

      int count = 0;

      while(rdr.Read())
      {
        count += 1;
      }

      int totalCopies = this.GetCopies().Count;

      return totalCopies - count;
    }
  }
}
