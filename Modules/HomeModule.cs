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

            Post["/add_book"] =  _ => {
                Book newBook = new Book(Request.Form["title"]);
                newBook.Save();

                Author newAuthor = new Author(Request.Form["author-first"], Request.Form["author-last"]);
                newAuthor.Save();
                newBook.AddAuthor(newAuthor);

                Copy newCopy = new Copy(newBook.GetId());
                newCopy.Save();

                newBook.AddCopy(Request.Form["num-copies"]);

                List<Book> allBooks = Book.GetAll();

                return View["books.cshtml", allBooks];
            };

            Post["/add_patron"] =  _ => {
                Patron newPatron = new Patron(Request.Form["patron-first"], Request.Form["patron-last"]);
                newPatron.Save();

                List<Patron> allPatrons = Patron.GetAll();
                return View["patrons.cshtml", allPatrons];
            };

            Get["/book/{id}"] = parameters => {
                Book newBook = Book.Find(parameters.id);
                return View["book.cshtml", newBook];
            };

            Get["/author/{id}"] = parameters => {
                Author newAuthor = Author.Find(parameters.id);
                return View["author.cshtml", newAuthor];
            };

            Get["/patron/{id}"] = parameters => {
                Patron newPatron = Patron.Find(parameters.id);
                return View["patron.cshtml", newPatron];
            };

            Post["/books/delete"] = _ => {
                Book.DeleteAll();
                List<Book> allBooks = Book.GetAll();
                return View["books.cshtml", allBooks];
            };

            Post["/book/delete/{id}"] = parameters => {
                Book newBook = Book.Find(parameters.id);
                newBook.DeleteBook();
                List<Book> allBooks = Book.GetAll();
                return View["books.cshtml", allBooks];
            };

            Post["book/search_title"] = _ => {
                Book searchedBook = Book.SearchTitle(Request.Form["search-book-title"]);
                return View["book.cshtml", searchedBook];
            };



        }

    }

}
