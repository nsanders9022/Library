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

        //Tests db-fetching specific item
        [Fact]
        public void TestFind_FindsBookInDatabase()
        {
            //Arrange
            Book testBook = new Book("War and Peace");
            testBook.Save();

            //Act
            Book foundBook = Book.Find(testBook.GetId());

            //Assert
            Assert.Equal(testBook, foundBook);
        }

        [Fact]
        public void Test_Update_UpdatesBookTitleinDB()
        {
            //Arrange
            Book testBook = new Book("War and Peace");
            testBook.Save();

            string newTitle = "War of the Worlds";

            //Act
            testBook.UpdateTitle(newTitle);


            //Assert
            string actualResult = testBook.GetTitle();
            string expectedResult = newTitle;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Test_DeleteThisBook_OneBookDeletedFromDatabase()
        {
            //Arrange
            Book firstBook = new Book("War and Peace");
            Book secondBook = new Book("War of the Worlds");
            firstBook.Save();
            secondBook.Save();

            //Act
            firstBook.DeleteBook();
            List<Book> result = Book.GetAll();
            List<Book> verify = new List<Book>{secondBook};

            //Assert
            Assert.Equal(verify, result);
        }

        //Tests db-fetching specific item
        [Fact]
        public void TestSearchTitle_SearchTitlesBookInDatabase()
        {
            //Arrange
            Book testBook = new Book("War and Peace");
            testBook.Save();

            //Act
            Book foundBook = Book.SearchTitle("War and Peace");

            //Assert
            Assert.Equal(testBook, foundBook);
        }

        //Test that if there are no books, GetAll returns empty
        [Fact]
        public void TestGetAuthors_NoAuthors_ReturnsEmptyList()
        {
            //Arrange, Act
            Book testBook = new Book("War and Peace");
            List<Author> allAuthors= testBook.GetAuthors();

            //Assert
            List<Author> actualResult = allAuthors;
            List<Author> expectedResult = new List<Author>{};
            Assert.Equal(expectedResult, actualResult);
        }

        //Tests if row is added to books_authors
        [Fact]
        public void AddAuthor_ForRowAddedToJoinTable_Row()
        {
            //Arrange
            Book testBook = new Book("War and Peace");
            testBook.Save();
            Author testAuthor = new Author("Leo", "Tolstoy");
            testAuthor.Save();

            //Act
            testBook.AddAuthor(testAuthor);

            //Assert
            List<Author> actualResult = testBook.GetAuthors();
            List<Author> expectedResult = new List<Author>{testAuthor};

            Assert.Equal(expectedResult, actualResult);
        }

        //Test that if there are no copies, GetAll returns empty
        [Fact]
        public void TestCountCopies_NoCopies_ReturnsEmptyList()
        {
            //Arrange, Act
            Book testBook = new Book("War and Peace");
            int allCopies= testBook.CountCopies();

            //Assert
            int actualResult = allCopies;
            int expectedResult = 0;
            Assert.Equal(expectedResult, actualResult);
        }

        //Add a copy of a book
        [Fact]
        public void AddCopy_ForRowAddedToJoinTable_Row()
        {
            //Arrange
            Book testBook = new Book("War and Peace");
            testBook.Save();

            //Act
            testBook.AddCopy(4);

            //Assert
            int actualResult = testBook.CountCopies();
            int expectedResult = 4;

            Assert.Equal(expectedResult, actualResult);
        }

        //Get all copies of individual book
        [Fact]
        public void GetCopies_ForBook_ListOfCopies()
        {
          //Arrange, Act
          Book testBook = new Book("War and Peace");
          testBook.Save();
          int bookId = testBook.GetId();
          testBook.AddCopy(4);

          List<Copy> copyList = testBook.GetCopies();

          //Assert
          List<int> actualResult = new List<int>{};

          foreach (Copy copy in copyList)
          {
            int thisCopy = copy.GetBookId();
            actualResult.Add(thisCopy);
          }

          List<int> expectedResult = new List<int> {bookId, bookId, bookId, bookId};

          Assert.Equal(expectedResult, actualResult);
        }

        //Find first available copy of book to checkout
        [Fact]
        public void CheckoutBook_GetFirstAvailableCopyOfBookNoCurrentCheckouts_NewRow()
        {
          //Arrange
          Book testBook = new Book("War and Peace");
          testBook.Save();
          testBook.AddCopy(4);
          List<Copy> allCopies = testBook.GetCopies();

          testBook.CheckoutBook(1);
          int expectedResult = allCopies[0].GetId();
          List<Checkout> allCheckouts = Checkout.GetAll();
          int actualResult = allCheckouts[0].GetCopiesId();

          Assert.Equal(expectedResult, actualResult);
        }

        //Find first available copy of book to checkout
        [Fact]
        public void CheckoutBook_GetFirstAvailableCopyOfBookOtherCheckouts_NewRow()
        {
          //Arrange
          Book testBook = new Book("War and Peace");
          testBook.Save();
          testBook.AddCopy(4);
          List<Copy> allCopies = testBook.GetCopies();

          testBook.CheckoutBook(1);
          testBook.CheckoutBook(1);

          int expectedResult = allCopies[1].GetId();
          List<Checkout> allCheckouts = Checkout.GetAll();
          int actualResult = allCopies[1].GetId();

          Assert.Equal(expectedResult, actualResult);
        }

        //See the number of available copies for a specific Book
        [Fact]
        public void AvailableCopies_GetNumberOfCopies_int()
        {
          Book testBook = new Book("War and Peace");
          testBook.Save();
          testBook.AddCopy(4);
          List<Copy> allCopies = testBook.GetCopies();

          testBook.CheckoutBook(1);
          testBook.CheckoutBook(1);

          int expectedResult = 2;
          int actualResult = testBook.AvailableCopies();

          Assert.Equal(expectedResult, actualResult);
        }

        //Returns a book to the library and sets the return date in Checkout object
        [Fact]
        public void ReturnBook_SetBookReturnDate_ChangesValue()
        {
          Book testBook = new Book ("War and Peace");
          testBook.Save();
          testBook.AddCopy(1);
          testBook.CheckoutBook(1);

          List<Checkout> allCheckouts = Checkout.GetAll();
          Console.WriteLine("CHECKOUT COUNT: " + allCheckouts.Count);

          int patronId = 1;

          testBook.ReturnBook(patronId);

          DateTime expectedResult = DateTime.Today;
          DateTime actualResult = allCheckouts[0].GetReturnDate();

          Assert.Equal(expectedResult, actualResult);
        }


        //Delete everything between tests
        public void Dispose()
        {
            Book.DeleteAll();
            Author.DeleteAll();
            Copy.DeleteAll();
            Checkout.DeleteAll();
        }
    }
}
