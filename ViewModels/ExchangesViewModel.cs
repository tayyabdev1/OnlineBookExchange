using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBookExchange.ViewModels
{
    public class ExchangesViewModel
    {
        public int ExchangeID { get; set; }
        public Nullable<int> BookID { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public Nullable<int> RequestorID { get; set; }
        public Nullable<System.DateTime> ExchangeDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string Status { get; set; }

        public virtual Books Books { get; set; }
        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }
        public virtual ICollection<Ratings> Ratings { get; set; }
    }
}