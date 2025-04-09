using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBookExchange.ViewModels
{
    public class UserAddressesViewModel
    {
        public int ID { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string VerificationStatus { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        public virtual Users Users { get; set; }
    }
}