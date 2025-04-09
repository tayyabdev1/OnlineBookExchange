using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBookExchange.ViewModels
{
    public class UserVerificationsViewModel
    {
        public Nullable<int> UserId { get; set; }

        // CNIC Verification
        public string CNICNumber { get; set; }
        public string CNICFrontImage { get; set; }
        public string CNICBackImage { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        public UserViewModel Users { get; set; }


        // User Addresses
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string VerificationStatus { get; set; }
        public Nullable<System.DateTime> AddressCreatedAt { get; set; }
    }
}