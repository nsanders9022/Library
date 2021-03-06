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
                    Patron thisPatron = Patron.Find(checkout.GetPatronsId());
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
                Dictionary<string, object> model = new Dictionary<string, object>();
                model.Add("book", newBook);
                model.Add("allAuthors", Author.GetAll());
                return View["book.cshtml", model];
            };

            Get["/author/{id}"] = parameters => {
                Author newAuthor = Author.Find(parameters.id);
                return View["author.cshtml", newAuthor];
            };

            Get["/patron/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Patron newPatron = Patron.Find(parameters.id);
                model.Add("patron", newPatron);
                model.Add("allBooks", Book.GetAll());
                return View["patron.cshtml", model];
            };

            Post["/patron/delete/{id}"] = parameters => {
                Patron newPatron = Patron.Find(parameters.id);
                newPatron.DeletePatron();
                List<Patron> allPatrons = Patron.GetAll();
                return View["patrons.cshtml", allPatrons];
            };

            Post["/patrons/delete"] = _ => {
                Patron.DeleteAll();
                List<Patron> allPatrons = Patron.GetAll();
                return View["patrons.cshtml", allPatrons];
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

            Post["/book/search_title"] = _ => {
                Book searchedBook = Book.SearchTitle(Request.Form["search-book-title"]);
                Dictionary<string, object> model = new Dictionary<string, object>();
                model.Add("book", searchedBook);
                model.Add("allAuthors", Author.GetAll());
                return View["book.cshtml", model];
            };

            Post["/patron/search_name"] = _ => {
                Patron searchedPatron = Patron.SearchPatron(Request.Form["search-patron-name"]);
                Dictionary<string, object> model = new Dictionary<string, object>();
                model.Add("patron", searchedPatron);
                model.Add("allBooks", Book.GetAll());
                return View["patron.cshtml", model];
            };

            Post["book/add_author/{id}"] = parameters => {
                Book searchedBook = Book.Find(parameters.id);
                searchedBook.AddAuthor(Author.Find(Request.Form["author-id"]));
                Dictionary<string, object> model = new Dictionary<string, object>();
                model.Add("book", searchedBook);
                model.Add("allAuthors", Author.GetAll());
                return View["book.cshtml", model];
            };

            Patch["/book/update_title"] = _ => {
                Book newBook = Book.Find(Request.Form["book-id"]);
                newBook.UpdateTitle(Request.Form["new-title"]);
                Dictionary<string, object> model = new Dictionary<string, object>();
                model.Add("book", newBook);
                model.Add("allAuthors", Author.GetAll());
                return View["book.cshtml", model];
            };

            Post["/book/add_copy"] = _ => {
                Book newBook = Book.Find(Request.Form["book-id"]);
                newBook.AddCopy(Request.Form["copy-amount"]);
                Dictionary<string, object> model = new Dictionary<string, object>();
                model.Add("book", newBook);
                model.Add("allAuthors", Author.GetAll());
                return View["book.cshtml", model];
            };

            Post["/book/checkout/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Patron foundPatron = Patron.Find(parameters.id);
                Book checkedoutBook = Book.Find(Request.Form["book-id"]);
                checkedoutBook.CheckoutBook(foundPatron.GetId());
                model.Add("patron", foundPatron);
                model.Add("allBooks", Book.GetAll());
                return View["patron.cshtml", model];
            };

            Post["/add_author"] = _ => {
                Author newAuthor = new Author(Request.Form["author-first"], Request.Form["author-last"]);
                newAuthor.Save();
                List<Author> allAuthors = Author.GetAll();
                return View["authors.cshtml", allAuthors];
            };

            Post["/authors/delete"] = _ => {
                Author.DeleteAll();
                List<Author> allAuthors = Author.GetAll();
                return View["authors.cshtml", allAuthors];
            };

            Post["/delete/author/{id}"] = parameters => {
                Author foundAuthor = Author.Find(parameters.id);
                foundAuthor.DeleteAuthor();
                List<Author> allAuthors = Author.GetAll();
                return View["authors.cshtml", allAuthors];
            };
        }
    }
}
