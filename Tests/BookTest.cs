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

        //tests if instances are saved to db
        [Fact]
        public void Test_Save_SavesToDatabase()
        {
            //Arrange
            Book newBook = new Book("War and Peace");

            //Act
            newBook.Save();

            //Assert
            List<Book> actualResult = Book.GetAll();
            List<Book> expectedResult = new List<Book>{newBook};

            Assert.Equal(expectedResult, actualResult);
        }

        //tests that each instance is assigned corresponding db id
        [Fact]
        public void TestSave_AssignIdtoObject()
        {
            //Arrange
            Book testBook = new Book("War and Peace");

            //Act
            testBook.Save();
            Book savedBook = Book.GetAll()[0];

            //Assert
            int actualResult = savedBook.GetId();
            int expectedResult = testBook.GetId();

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests that GetAll method pulls all items from db
        [Fact]
        public void TestGetAll_Books_ReturnsListOfBooks()
        {
            //Arrange
            Book firstBook = new Book("War and Peace");
            Book secondBook = new Book("War of the Worlds");

            //Act
            firstBook.Save();
            secondBook.Save();

            //Assert
            List<Book> actualResult = Book.GetAll();
            List<Book> expectedResult = new List<Book>{firstBook, secondBook};

            Assert.Equal(expectedResult, actualResult);
        }

        //Delete everything between tests
        public void Dispose()
        {
            Book.DeleteAll();
        }
    }
}
