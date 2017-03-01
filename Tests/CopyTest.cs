using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class CopyTest: IDisposable
    {
        public CopyTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }
        //Test that if there are no cipies, GetAll returns empty
        [Fact]
        public void TestGetAll_NoCopies_ReturnsEmptyList()
        {
            //Arrange, Act
            Copy.DeleteAll();
            List<Copy> allCopys = Copy.GetAll();

            //Assert
            List<Copy> actualResult = allCopys;
            List<Copy> expectedResult = new List<Copy>{};
            Assert.Equal(expectedResult, actualResult);
        }

        //test if equals override works
        [Fact]
        public void TestEqualOverride_TrueIfCopyIsSame()
        {
            //Arrange, Act
            Copy firstCopy = new Copy(4);
            Copy secondCopy = new Copy(4);

            //Assert
            Assert.Equal(firstCopy, secondCopy);
        }

        //tests if instances are saved to db
        [Fact]
        public void Test_Save_SavesToDatabase()
        {
            //Arrange
            Copy newCopy = new Copy(5);

            //Act
            newCopy.Save();

            //Assert
            List<Copy> actualResult = Copy.GetAll();
            List<Copy> expectedResult = new List<Copy>{newCopy};

            Assert.Equal(expectedResult, actualResult);
        }

        //tests that each instance is assigned corresponding db id
        [Fact]
        public void TestSave_AssignIdtoObject()
        {
            //Arrange
            Copy testCopy = new Copy(4);

            //Act
            testCopy.Save();
            Copy savedCopy = Copy.GetAll()[0];

            //Assert
            int actualResult = savedCopy.GetId();
            int expectedResult = testCopy.GetId();

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests that GetAll method pulls all items from db
        [Fact]
        public void TestGetAll_Copies_ReturnsListOfCopies()
        {
            //Arrange
            Copy firstCopy = new Copy(4);
            Copy secondCopy = new Copy(5);

            //Act
            firstCopy.Save();
            secondCopy.Save();

            //Assert
            List<Copy> actualResult = Copy.GetAll();
            List<Copy> expectedResult = new List<Copy>{firstCopy, secondCopy};

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests db-fetching specific item
        [Fact]
        public void TestFind_FindsCopyInDatabase()
        {
            //Arrange
            Copy testCopy = new Copy(4);
            testCopy.Save();

            //Act
            Copy foundCopy = Copy.Find(testCopy.GetId());

            //Assert
            Assert.Equal(testCopy, foundCopy);
        }

        [Fact]
        public void Test_DeleteThisCopy_OneCopyDeletedFromDatabase()
        {
            //Arrange
            Copy firstCopy = new Copy(4);
            Copy secondCopy = new Copy(5);
            firstCopy.Save();
            secondCopy.Save();

            //Act
            firstCopy.DeleteCopy();
            List<Copy> result = Copy.GetAll();
            List<Copy> verify = new List<Copy>{secondCopy};

            //Assert
            Assert.Equal(verify, result);
        }

        //Delete everything between tests
        public void Dispose()
        {
            Copy.DeleteAll();
        }
    }
}
