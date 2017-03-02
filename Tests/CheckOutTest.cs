using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryApp.Objects
{
  public class CheckoutTest: IDisposable
  {
    public CheckoutTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    //Test that if there are no cipies, GetAll returns empty
    [Fact]
    public void TestGetAll_NoCheckouts_ReturnsEmptyList()
    {
      //Arrange, Act
      Checkout.DeleteAll();
      List<Checkout> allCheckouts = Checkout.GetAll();

      //Assert
      List<Checkout> actualResult = allCheckouts;
      List<Checkout> expectedResult = new List<Checkout>{};
      Assert.Equal(expectedResult, actualResult);
    }

    //test if equals override works
    [Fact]
    public void TestEqualOverride_TrueIfCheckoutIsSame()
    {
      //Arrange, Act
      DateTime dueDate = new DateTime(2017, 3, 15);
      DateTime returnDate = new DateTime(2017, 3, 7);
      Checkout firstCheckout = new Checkout(dueDate, 1, 1);
      Checkout secondCheckout = new Checkout(dueDate, 1, 1);

      //Assert
      Assert.Equal(firstCheckout, secondCheckout);
    }

    //tests if instances are saved to db
    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      DateTime dueDate = new DateTime(2017, 3, 15);
      DateTime returnDate = new DateTime(2017, 3, 7);
      Checkout newCheckout = new Checkout(dueDate, 1, 1);

      //Act
      newCheckout.Save();

      //Assert
      List<Checkout> actualResult = Checkout.GetAll();
      List<Checkout> expectedResult = new List<Checkout>{newCheckout};

      Assert.Equal(expectedResult, actualResult);
    }

    //tests that each instance is assigned corresponding db id
    [Fact]
    public void TestSave_AssignIdtoObject()
    {
      //Arrange
      DateTime dueDate = new DateTime(2017, 3, 15);
      DateTime returnDate = new DateTime(2017, 3, 7);
      Checkout testCheckout = new Checkout(dueDate, 1, 1);

      //Act
      testCheckout.Save();
      Checkout savedCheckout = Checkout.GetAll()[0];

      //Assert
      int actualResult = savedCheckout.GetId();
      int expectedResult = testCheckout.GetId();

      Assert.Equal(expectedResult, actualResult);
    }

    //Tests that GetAll method pulls all items from db
    [Fact]
    public void TestGetAll_Checkouts_ReturnsListOfCheckouts()
    {
      //Arrange
      DateTime dueDate = new DateTime(2017, 3, 15);
      DateTime returnDate = new DateTime(2017, 3, 7);
      Checkout firstCheckout = new Checkout(dueDate, 1, 1);
      Checkout secondCheckout = new Checkout(dueDate, 2, 2);

      //Act
      firstCheckout.Save();
      secondCheckout.Save();

      //Assert
      List<Checkout> actualResult = Checkout.GetAll();
      List<Checkout> expectedResult = new List<Checkout>{firstCheckout, secondCheckout};

      Assert.Equal(expectedResult, actualResult);
    }

    //Tests db-fetching specific item
    [Fact]
    public void TestFind_FindsCheckoutInDatabase()
    {
      //Arrange
      DateTime dueDate = new DateTime(2017, 3, 15);
      DateTime returnDate = new DateTime(2017, 3, 7);
      Checkout testCheckout = new Checkout(dueDate, 1, 1);
      testCheckout.Save();

      //Act
      Checkout foundCheckout = Checkout.Find(testCheckout.GetId());

      //Assert
      Assert.Equal(testCheckout, foundCheckout);
    }

    [Fact]
    public void Test_DeleteThisCheckout_OneCheckoutDeletedFromDatabase()
    {
      //Arrange
      DateTime dueDate = new DateTime(2017, 3, 15);
      DateTime returnDate = new DateTime(2017, 3, 7);
      Checkout firstCheckout = new Checkout(dueDate, 1, 1);
      Checkout secondCheckout = new Checkout(dueDate, 2, 2);
      firstCheckout.Save();
      secondCheckout.Save();

      //Act
      firstCheckout.DeleteCheckout();
      List<Checkout> result = Checkout.GetAll();
      List<Checkout> verify = new List<Checkout>{secondCheckout};

      //Assert
      Assert.Equal(verify, result);
    }

    [Fact]
    public void Test_UpdateReturnDate_UpdatesReturnDateinDB()
    {
      //Arrange
      DateTime dueDate = new DateTime(2017, 3, 15);
      DateTime returnDate = new DateTime(2017, 3, 7);
      Checkout testCheckout = new Checkout(dueDate, 1, 1);
      testCheckout.Save();

      DateTime newReturnDate = new DateTime(2017, 4, 7);

      //Act
      testCheckout.UpdateReturnDate(newReturnDate);


      //Assert
      DateTime actualResult = testCheckout.GetReturnDate();
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
      Checkout testCheckout = new Checkout(dueDate, 1, 1);
      testCheckout.Save();

      //Act
      List<Checkout> allDue = Checkout.SearchDueDate(dueDate);
      List<Checkout> testDue = new List<Checkout> {testCheckout};
      //Assert
      Assert.Equal(testDue, allDue);
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
