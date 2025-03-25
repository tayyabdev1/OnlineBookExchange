using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using OnlineBookExchange.BLL;
using OnlineBookExchange.ViewModels;
using OnlineBookExchange.Services;
using System.IO;
using OnlineBookExchange.DAL;


namespace OnlineBookExchange.Controllers.Api
{
    public class BooksController : ApiController
    {
        OnlineBookExchangeEntities db;

        private readonly HttpContext _httpContext;
        private readonly BooksDto _booksDto;
        public BooksController()
        {
            db = new OnlineBookExchangeEntities();
            _httpContext = System.Web.HttpContext.Current;
            _booksDto = new BooksDto();
        }

        // Book List
        [HttpGet]
        public IHttpActionResult GetBooks()
        {
            var list = new BooksDto().GetBooks();
            return Ok(list);
        }
        

        [HttpGet]
        [Route ("api/books/adminbooks")]
        public IHttpActionResult AdminGetBooks()
        {
            var list = new BooksDto().GetBooks();
            return Ok(list);
        }

        // Book Details
        [HttpGet]
        public IHttpActionResult GetBook(int bookID)
        {
            var book = new BooksDto().GetBook(bookID);
            if (book != null)
            {
                return Ok(book);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "Book not found with ID: " + bookID);
            }
        }


        // Book Create
        [HttpPost]
        [Route("api/books")]
        public IHttpActionResult CreateBook()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                var bookDto = new BooksDto
                {
                    UserID = Convert.ToInt32(httpRequest.Form["UserID"]),
                    Title = httpRequest.Form["Title"],
                    Author = httpRequest.Form["Author"],
                    Genre = httpRequest.Form["Genre"],
                    Condition = httpRequest.Form["Condition"],
                    Status = httpRequest.Form["Status"],
                };
                if (httpRequest.Files.Count > 0)
                {
                    var imageFile = httpRequest.Files["bookImage"];
                    bookDto.BookPicture = UploadImage(imageFile);
                }

                var newBookId = new BooksDto().UploadBook(bookDto);

                return Created(new Uri(Request.RequestUri + "/" + newBookId), bookDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private string UploadImage(HttpPostedFile imageFile)
        {
            if (imageFile?.ContentLength > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    throw new Exception("Invalid File Type");

                var fileName = Guid.NewGuid() + extension;
                var path = Path.Combine(HttpContext.Current.Server.MapPath("/Images"), fileName);
                imageFile.SaveAs(path);
                return "/Images/" + fileName;
            }
            return null;
        }


        [HttpDelete]
        public IHttpActionResult DeleteBook(int id)
        {
            var result = _booksDto.DeleteBook(id);

            if (result)
                return Ok(new { success = true, message = "Book deleted successfully." });
            else
                return BadRequest("Failed to delete the book. Please try again.");
        }


        [HttpPut]
        public IHttpActionResult UpdateBook(ViewModels.BooksViewModel booksViewModel)
        {
            if (booksViewModel?.Books == null)
            {
                return BadRequest("Invalid data.");
            }

            var updateBook = new BooksDto().UpdateBook
            (
                new BooksDto
                {
                    BookID = booksViewModel.Books.BookID,
                    Title = booksViewModel.Books.Title,
                    Author = booksViewModel.Books.Author,
                    Genre = booksViewModel.Books.Genre,
                    Condition = booksViewModel.Books.Condition,
                    Status = booksViewModel.Books.Status,
                }
            );

            if (updateBook) return Ok();
            else return BadRequest();
        }
    }
}
