using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace XmlBook2.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            IList<Models.Book> BookList = new List<Models.Book>();

            string path = Request.PathBase + "App_data/books.xml";

            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList Books = doc.GetElementsByTagName("book");


                foreach (XmlElement b in Books)
                {
                    Models.Book Book = new Models.Book();

                    
                    Book.BookTitle = b.GetElementsByTagName("title")[0].InnerText;
                    Book.FirstName = b.GetElementsByTagName("firstname")[0].InnerText;
                    Book.LastName = b.GetElementsByTagName("lastname")[0].InnerText;
                    Book.id = Int32.Parse(b.GetElementsByTagName("id")[0].InnerText);
                    var test = b.GetElementsByTagName("middlename")[0];
                    Book.MiddleName = "" ;
                    if (test != null) 
                    {
                      Book.MiddleName = test.InnerText;
                    } 

          



                   

                   
                    BookList.Add(Book);

                }
            }

            return View(BookList);
        }


        [HttpGet]
        //this will load when just loading the empty form
        public IActionResult Create()
        {
            var Book = new Models.Book();
            return View(Book);
        }
        //this will load when a form is submitted via post (form data passed as model)
        [HttpPost]
        public IActionResult Create(Models.Book b)
        {
            string path = Request.PathBase + "App_data/books.xml";

            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {

                doc.Load(path); 
                
                XmlElement book = _CreateBookElement(doc, b);
                doc.DocumentElement.AppendChild(book);

            }
            else
            {
                //doesn't exist so add stuff
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("books");

                XmlElement book = _CreateBookElement(doc, b);
                root.AppendChild(book);
                doc.AppendChild(root); 
            }
            doc.Save(path);

            return View();
        }

        private XmlElement _CreateBookElement(XmlDocument doc, Models.Book newBook)
        {
            XmlElement book = doc.CreateElement("book");


            XmlNode author = doc.CreateElement("author");
            XmlNode first = doc.CreateElement("firstname");
            first.InnerText = newBook.FirstName;
            XmlNode last = doc.CreateElement("lastname");
            last.InnerText = newBook.LastName;
            XmlNode middle = doc.CreateElement("middlename");
            middle.InnerText = newBook.MiddleName;
            XmlNode title = doc.CreateElement("title");
            title.InnerText = newBook.BookTitle;
            
            XmlNode id = doc.CreateElement("id");
            int id1 = doc.GetElementsByTagName("id").Count;
            id.InnerText = (id1 + 1).ToString();








            book.AppendChild(title);

            book.AppendChild(id);






            author.AppendChild(first);
            author.AppendChild(last);
            author.AppendChild(middle);

            book.AppendChild(author);
            return book;
        }

    }

}