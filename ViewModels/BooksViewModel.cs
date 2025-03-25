using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineBookExchange.BLL;
using OnlineBookExchange.DAL;


namespace OnlineBookExchange.ViewModels
{
    public class BooksViewModel
    {
        public BooksDto Books { get; set; }
        public IEnumerable<BooksDto> books { get; set; }
        public List<BooksDto> book { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Username { get; set; }

        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public string BookPicture { get; set; }

        public virtual Users Users { get; set; }
        public virtual ICollection<Exchanges> Exchanges { get; set; }

    }
}