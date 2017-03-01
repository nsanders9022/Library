using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
    public class PatronTest: IDisposable
    {
        public PatronTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
        }
        //Test that if there are no patrons, GetAll returns empty
        [Fact]
        public void TestGetAll_NoPatrons_ReturnsEmptyList()
        {
            //Arrange, Act
            Patron.DeleteAll();
            List<Patron> allPatrons = Patron.GetAll();

            //Assert
            List<Patron> actualResult = allPatrons;
            List<Patron> expectedResult = new List<Patron>{};
            Assert.Equal(expectedResult, actualResult);
        }

        //test if equals override works
        [Fact]
        public void TestEqualOverride_TrueIfPatronNameIsSame()
        {
            //Arrange, Act
            Patron firstPatron = new Patron("Allie", "Holcombe");
            Patron secondPatron = new Patron("Allie", "Holcombe");

            //Assert
            Assert.Equal(firstPatron, secondPatron);
        }

        //tests if instances are saved to db
        [Fact]
        public void Test_Save_SavesToDatabase()
        {
            //Arrange
            Patron newPatron = new Patron("Allie", "Holcombe");

            //Act
            newPatron.Save();

            //Assert
            List<Patron> actualResult = Patron.GetAll();
            List<Patron> expectedResult = new List<Patron>{newPatron};

            Assert.Equal(expectedResult, actualResult);
        }

        //tests that each instance is assigned corresponding db id
        [Fact]
        public void TestSave_AssignIdtoObject()
        {
            //Arrange
            Patron testPatron = new Patron("Allie", "Holcombe");

            //Act
            testPatron.Save();
            Patron savedPatron = Patron.GetAll()[0];

            //Assert
            int actualResult = savedPatron.GetId();
            int expectedResult = testPatron.GetId();

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests that GetAll method pulls all items from db
        [Fact]
        public void TestGetAll_Patrons_ReturnsListOfPatrons()
        {
            //Arrange
            Patron firstPatron = new Patron("Allie", "Holcombe");
            Patron secondPatron = new Patron("Nicole", "Sanders");

            //Act
            firstPatron.Save();
            secondPatron.Save();

            //Assert
            List<Patron> actualResult = Patron.GetAll();
            List<Patron> expectedResult = new List<Patron>{firstPatron, secondPatron};

            Assert.Equal(expectedResult, actualResult);
        }

        //Tests db-fetching specific item
        [Fact]
        public void TestFind_FindsPatronInDatabase()
        {
            //Arrange
            Patron testPatron = new Patron("Allie", "Holcombe");
            testPatron.Save();

            //Act
            Patron foundPatron = Patron.Find(testPatron.GetId());

            //Assert
            Assert.Equal(testPatron, foundPatron);
        }

        [Fact]
        public void Test_Update_UpdatesPatronNameinDB()
        {
            //Arrange
            Patron testPatron = new Patron("Allie", "Holcombe");
            testPatron.Save();

            string newFirstName = "Nicole";
            string newLastName = "Sanders";

            //Act
            testPatron.UpdateName(newFirstName, newLastName);


            //Assert
            string actualResultFirstName = testPatron.GetFirstName();
            string expectedResultFirstName = newFirstName;

            string actualResultLastName = testPatron.GetLastName();
            string expectedResultLastName = newLastName;

            Assert.Equal(expectedResultFirstName, actualResultFirstName);
            Assert.Equal(expectedResultLastName, actualResultLastName);
        }

        [Fact]
        public void Test_DeleteThisPatron_OnePatronDeletedFromDatabase()
        {
            //Arrange
            Patron firstPatron = new Patron("Allie", "Holcombe");
            Patron secondPatron = new Patron("Nicole", "Sanders");
            firstPatron.Save();
            secondPatron.Save();

            //Act
            firstPatron.DeletePatron();
            List<Patron> result = Patron.GetAll();
            List<Patron> verify = new List<Patron>{secondPatron};

            //Assert
            Assert.Equal(verify, result);
        }

        //Tests db-fetching specific item
        [Fact]
        public void TestSearchPatron_SearchPatronLastNameInDatabase()
        {
            //Arrange
            Patron testPatron = new Patron("Nicole", "Sanders");
            testPatron.Save();

            //Act
            Patron foundPatron = Patron.SearchPatron("Sanders");

            //Assert
            Assert.Equal(testPatron, foundPatron);
        }

        //Delete everything between tests
        public void Dispose()
        {
            Patron.DeleteAll();
        }
    }
}
