using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class BookTest: IDisposable
    {
        public BookTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }
        //Test that if there are no books, GetAll returns empty
        [Fact]
        public void TestGetAll_NoBooks_ReturnsEmptyList()
        {
            //Arrange, Act
            Book.DeleteAll();
            List<Book> allBooks = Book.GetAll();

            //Assert
            List<Book> actualResult = allBooks;
            List<Book> expectedResult = new List<Book>{};
            Assert.Equal(expectedResult, actualResult);
        }

        //test if equals override works
       [Fact]
       public void TestEqualOverride_TrueIfBookTitleIsSame()
       {
           //Arrange, Act
           Book firstBook = new Book("War and Peace");
           Book secondBook = new Book("War and Peace");

           //Assert
           Assert.Equal(firstBook, secondBook);
       }

        //Delete everything between tests
        public void Dispose()
        {
            Book.DeleteAll();
        }
    }
}
