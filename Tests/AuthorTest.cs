using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class AuthorTest: IDisposable
    {
        public AuthorTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }
        //Test that if there are no authors, GetAll returns empty
        [Fact]
        public void TestGetAll_NoAuthors_ReturnsEmptyList()
        {
            //Arrange, Act
            Author.DeleteAll();
            List<Author> allAuthors = Author.GetAll();

            //Assert
            List<Author> actualResult = allAuthors;
            List<Author> expectedResult = new List<Author>{};
            Assert.Equal(expectedResult, actualResult);
        }

        //test if equals override works
        [Fact]
        public void TestEqualOverride_TrueIfAuthorNameIsSame()
        {
            //Arrange, Act
            Author firstAuthor = new Author("Herman", "Melville");
            Author secondAuthor = new Author("Herman", "Melville");

            //Assert
            Assert.Equal(firstAuthor, secondAuthor);
        }

        //tests if instances are saved to db
        [Fact]
        public void Test_Save_SavesToDatabase()
        {
            //Arrange
            Author newAuthor = new Author("Herman", "Melville");

            //Act
            newAuthor.Save();

            //Assert
            List<Author> actualResult = Author.GetAll();
            List<Author> expectedResult = new List<Author>{newAuthor};

            Assert.Equal(expectedResult, actualResult);
        }

        //tests that each instance is assigned corresponding db id
        [Fact]
        public void TestSave_AssignIdtoObject()
        {
            //Arrange
            Author testAuthor = new Author("Herman", "Melville");

            //Act
            testAuthor.Save();
            Author savedAuthor = Author.GetAll()[0];

            //Assert
            int actualResult = savedAuthor.GetId();
            int expectedResult = testAuthor.GetId();

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests that GetAll method pulls all items from db
        [Fact]
        public void TestGetAll_Authors_ReturnsListOfAuthors()
        {
            //Arrange
            Author firstAuthor = new Author("Herman", "Melville");
            Author secondAuthor = new Author("J. K", "Rowling");

            //Act
            firstAuthor.Save();
            secondAuthor.Save();

            //Assert
            List<Author> actualResult = Author.GetAll();
            List<Author> expectedResult = new List<Author>{firstAuthor, secondAuthor};

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests db-fetching specific item
        [Fact]
        public void TestFind_FindsAuthorInDatabase()
        {
            //Arrange
            Author testAuthor = new Author("Herman", "Melville");
            testAuthor.Save();

            //Act
            Author foundAuthor = Author.Find(testAuthor.GetId());

            //Assert
            Assert.Equal(testAuthor, foundAuthor);
        }

        [Fact]
        public void Test_Update_UpdatesAuthorNameinDB()
        {
            //Arrange
            Author testAuthor = new Author("Herman", "Melville");
            testAuthor.Save();

            string newFirstName = "J. K.";
            string newLastName = "Rowling";

            //Act
            testAuthor.UpdateName(newFirstName, newLastName);


            //Assert
            string actualResultFirstName = testAuthor.GetFirstName();
            string expectedResultFirstName = newFirstName;

            string actualResultLastName = testAuthor.GetLastName();
            string expectedResultLastName = newLastName;

            Assert.Equal(expectedResultFirstName, actualResultFirstName);
            Assert.Equal(expectedResultLastName, actualResultLastName);
        }

        [Fact]
        public void Test_DeleteThisAuthor_OneAuthorDeletedFromDatabase()
        {
            //Arrange
            Author firstAuthor = new Author("Herman", "Melville");
            Author secondAuthor = new Author("J. K", "Rowling");
            firstAuthor.Save();
            secondAuthor.Save();

            //Act
            firstAuthor.DeleteAuthor();
            List<Author> result = Author.GetAll();
            List<Author> verify = new List<Author>{secondAuthor};

            //Assert
            Assert.Equal(verify, result);
        }

        //Tests db-fetching specific item
        [Fact]
        public void TestSearchAuthor_SearchAuthorLastNameInDatabase()
        {
            //Arrange
            Author testAuthor = new Author("J. K", "Rowling");
            testAuthor.Save();

            //Act
            Author foundAuthor = Author.SearchAuthor("Rowling");

            //Assert
            Assert.Equal(testAuthor, foundAuthor);
        }

        //Delete everything between tests
        public void Dispose()
        {
            Author.DeleteAll();
        }
    }
}
