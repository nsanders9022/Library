using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using LibraryApp.Objects;

namespace LibraryApp
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        Dictionary<object, object> model = new Dictionary<object, object>();
        List<Checkout> allCheckouts = Checkout.GetAll();
        foreach (Checkout checkout in allCheckouts)
        {
          Patron thisPatron = Patron.Find(checkout.GetId());
          model.Add(checkout, thisPatron);
        }
        return View["index.cshtml", model];
      };

      Get["/books"] = _ => {
        List<Book> allBooks = Book.GetAll();
        return View["books.cshtml", allBooks];
      };

      Get["/patrons"] = _ => {
        List<Patron> allPatrons = Patron.GetAll();
        return View["patrons.cshtml", allPatrons];
      };

      Get["/authors"] = _ => {
        List<Author> allAuthors = Author.GetAll();
        return View["authors.cshtml", allAuthors];
      };



    }

  }

}
