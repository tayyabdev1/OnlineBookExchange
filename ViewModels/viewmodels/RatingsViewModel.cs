using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBookExchange.ViewModels
{
    public class RatingsViewModel
    {
        public int RatingID { get; set; }
        public Nullable<int> Exchange_id { get; set; }
        public Nullable<int> RaterID { get; set; }
        public string Rview { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> RateeID { get; set; }
        public Nullable<int> BookID { get; set; }
        public Nullable<int> ExchangePartnerRating { get; set; }
        public Nullable<int> BookConditionRating { get; set; }

        public virtual Exchanges Exchanges { get; set; }
        public virtual Users Users { get; set; }
        public virtual Books Books { get; set; }
        public virtual Users Users1 { get; set; }
    }
}