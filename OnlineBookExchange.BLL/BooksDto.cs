using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using OnlineBookExchange.DAL;

namespace OnlineBookExchange.BLL
{
    public class BooksDto
    {
        OnlineBookExchangeEntities db;
        public BooksDto()
        {
            db = new OnlineBookExchangeEntities();
            db.Configuration.ProxyCreationEnabled = false;

        }

        public int BookID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }

        public string BookPicture { get; set; }
        public string Username { get; set; }

        public virtual Users Users { get; set; }
        public virtual ICollection<Exchanges> Exchanges { get; set; }



        // Book List
        public List<BooksDto> GetBooks()
        {
          var books = (from book in db.Books
            join user in db.Users on book.UserID equals user.UserID
            select new BooksDto
            {
                BookID = book.BookID,
                UserID = book.UserID,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                Condition = book.Condition,
                Status = book.Status,
                AddedDate = book.AddedDate,
                Username = user.Username, // Fetch username
                BookPicture = book.BookPicture
            }).ToList();
            return books;
        }


        // Book Details
        public BooksDto GetBook(int bookID)
        {
          var book = (from b in db.Books
           join u in db.Users on b.UserID equals u.UserID
           where b.BookID == bookID
           select new BooksDto
           {
               BookID = b.BookID,
               UserID = b.UserID,
               Title = b.Title,
               Author = b.Author,
               Genre = b.Genre,
               Condition = b.Condition,
               Status = b.Status,
               AddedDate = b.AddedDate,
               Username = u.Username,
               BookPicture = b.BookPicture
           }).FirstOrDefault();
            return book;
        }


        public int UploadBook(BooksDto booksDto)
        {
            using (var db = new OnlineBookExchangeEntities())
            {
                var newBook = new Books
                {
                    UserID = booksDto.UserID,
                    Title = booksDto.Title,
                    Author = booksDto.Author,
                    Genre = booksDto.Genre,
                    Condition = booksDto.Condition,
                    Status = booksDto.Status,
                    AddedDate = DateTime.Now,
                    BookPicture = booksDto.BookPicture,
                };
                db.Books.Add(newBook);
                db.SaveChanges();
                return newBook.BookID;
            }
        }


        public List<Books> GetBooksByUserId(int userId)
        {
            using (var context = new OnlineBookExchangeEntities())
            {
                return context.Books.Where(b => b.UserID == userId).ToList();
            }
        }

        public bool DeleteBook(int id)
        {
            var book = db.Books.Find(id);
            if (book == null) return false;

            db.Books.Remove(book);
            return db.SaveChanges() > 0;
        }

        public bool UpdateBook(BooksDto booksDto)
        {
            var objInDb = db.Books.FirstOrDefault(x => x.BookID == booksDto.BookID);
            if (objInDb != null)
            {
                objInDb.BookID = booksDto.BookID;
                objInDb.Title = booksDto.Title;
                objInDb.Author = booksDto.Author;
                objInDb.Genre = booksDto.Genre;
                objInDb.Condition = booksDto.Condition;
                objInDb.Status = booksDto.Status;
                var savechanges = db.SaveChanges();
                return savechanges > 0;
            }
            return false;
        }
    }
}
