using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class CheckOutTest: IDisposable
    {
        public CheckOutTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }
        //Test that if there are no cipies, GetAll returns empty
        [Fact]
        public void TestGetAll_NoCheckOuts_ReturnsEmptyList()
        {
            //Arrange, Act
            CheckOut.DeleteAll();
            List<CheckOut> allCheckOuts = CheckOut.GetAll();

            //Assert
            List<CheckOut> actualResult = allCheckOuts;
            List<CheckOut> expectedResult = new List<CheckOut>{};
            Assert.Equal(expectedResult, actualResult);
        }

        //test if equals override works
        [Fact]
        public void TestEqualOverride_TrueIfCheckOutIsSame()
        {
            //Arrange, Act
            DateTime dueDate = new DateTime(2017, 3, 15);
            DateTime returnDate = new DateTime(2017, 3, 7);
            CheckOut firstCheckOut = new CheckOut(dueDate, 1, 1);
            CheckOut secondCheckOut = new CheckOut(dueDate, 1, 1);

            //Assert
            Assert.Equal(firstCheckOut, secondCheckOut);
        }

        //tests if instances are saved to db
        [Fact]
        public void Test_Save_SavesToDatabase()
        {
            //Arrange
            DateTime dueDate = new DateTime(2017, 3, 15);
            DateTime returnDate = new DateTime(2017, 3, 7);
            CheckOut newCheckOut = new CheckOut(dueDate, 1, 1);

            //Act
            newCheckOut.Save();

            //Assert
            List<CheckOut> actualResult = CheckOut.GetAll();
            List<CheckOut> expectedResult = new List<CheckOut>{newCheckOut};

            Assert.Equal(expectedResult, actualResult);
        }

        //tests that each instance is assigned corresponding db id
        [Fact]
        public void TestSave_AssignIdtoObject()
        {
            //Arrange
            DateTime dueDate = new DateTime(2017, 3, 15);
            DateTime returnDate = new DateTime(2017, 3, 7);
            CheckOut testCheckOut = new CheckOut(dueDate, 1, 1);

            //Act
            testCheckOut.Save();
            CheckOut savedCheckOut = CheckOut.GetAll()[0];

            //Assert
            int actualResult = savedCheckOut.GetId();
            int expectedResult = testCheckOut.GetId();

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests that GetAll method pulls all items from db
        [Fact]
        public void TestGetAll_CheckOuts_ReturnsListOfCheckOuts()
        {
            //Arrange
            DateTime dueDate = new DateTime(2017, 3, 15);
            DateTime returnDate = new DateTime(2017, 3, 7);
            CheckOut firstCheckOut = new CheckOut(dueDate, 1, 1);
            CheckOut secondCheckOut = new CheckOut(dueDate, 2, 2);

            //Act
            firstCheckOut.Save();
            secondCheckOut.Save();

            //Assert
            List<CheckOut> actualResult = CheckOut.GetAll();
            List<CheckOut> expectedResult = new List<CheckOut>{firstCheckOut, secondCheckOut};

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests db-fetching specific item
        [Fact]
        public void TestFind_FindsCheckOutInDatabase()
        {
            //Arrange
            DateTime dueDate = new DateTime(2017, 3, 15);
            DateTime returnDate = new DateTime(2017, 3, 7);
            CheckOut testCheckOut = new CheckOut(dueDate, 1, 1);
            testCheckOut.Save();

            //Act
            CheckOut foundCheckOut = CheckOut.Find(testCheckOut.GetId());

            //Assert
            Assert.Equal(testCheckOut, foundCheckOut);
        }

        [Fact]
        public void Test_DeleteThisCheckOut_OneCheckOutDeletedFromDatabase()
        {
            //Arrange
            DateTime dueDate = new DateTime(2017, 3, 15);
            DateTime returnDate = new DateTime(2017, 3, 7);
            CheckOut firstCheckOut = new CheckOut(dueDate, 1, 1);
            CheckOut secondCheckOut = new CheckOut(dueDate, 2, 2);
            firstCheckOut.Save();
            secondCheckOut.Save();

            //Act
            firstCheckOut.DeleteCheckOut();
            List<CheckOut> result = CheckOut.GetAll();
            List<CheckOut> verify = new List<CheckOut>{secondCheckOut};

            //Assert
            Assert.Equal(verify, result);
        }

        [Fact]
        public void Test_UpdateReturnDate_UpdatesReturnDateinDB()
        {
            //Arrange
            DateTime dueDate = new DateTime(2017, 3, 15);
            DateTime returnDate = new DateTime(2017, 3, 7);
            CheckOut testCheckOut = new CheckOut(dueDate, 1, 1);
            testCheckOut.Save();

            DateTime newReturnDate = new DateTime(2017, 4, 7);

            //Act
            testCheckOut.UpdateReturnDate(newReturnDate);


            //Assert
            DateTime actualResult = testCheckOut.GetReturnDate();
            DateTime expectedResult = newReturnDate;

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests db-fetching specific item
        [Fact]
        public void TestSearchDueDate_SearchSearchDueDateInDatabase()
        {
            //Arrange
            DateTime dueDate = new DateTime(2017, 3, 15);
            DateTime returnDate = new DateTime(2017, 3, 7);
            CheckOut testCheckOut = new CheckOut(dueDate, 1, 1);
            testCheckOut.Save();

            //Act
            List<CheckOut> allDue = CheckOut.SearchDueDate(dueDate);
            List<CheckOut> testDue = new List<CheckOut> {testCheckOut};
            //Assert
            Assert.Equal(testDue, allDue);
        }

        //Delete everything between tests
        public void Dispose()
        {
            CheckOut.DeleteAll();
        }
    }
}
