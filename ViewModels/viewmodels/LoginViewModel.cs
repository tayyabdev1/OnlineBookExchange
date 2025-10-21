using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineBookExchange.BLL;

namespace OnlineBookExchange.ViewModels
{
    public class LoginViewModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        public string ProfilePicture { get; set; }
        public List<BooksDto> Books { get; set; }

        public string CurrentPassword { get; set; }  // Used for validation only
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}