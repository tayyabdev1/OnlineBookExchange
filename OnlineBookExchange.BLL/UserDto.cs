using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;


namespace OnlineBookExchange.BLL
{
    public class UserDto
    {
        OnlineBookExchangeEntities db;

        public UserDto()
        {
            db = new OnlineBookExchangeEntities();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string ProfilePicture { get; set; }


        // User's List, will use in Admin Controller
        public List<UserDto> GetUsers()
        {
            var users = new List<UserDto>();
            var obj = db.Users.ToList();
            var jsonstring = new JavaScriptSerializer().Serialize(obj);
            users = new JavaScriptSerializer().Deserialize<List<UserDto>>(jsonstring);
            return users;
        }

        public UserDto GetUser(int userID)
        {
            var objInDb = db.Users.FirstOrDefault(x => x.UserID == userID);
            if (objInDb != null)
            {
                var user = new UserDto
                {
                    UserID = objInDb.UserID,
                    Username = objInDb.Username,
                    Email = objInDb.Email,
                    Password = objInDb.Password,
                    ContactNumber = objInDb.ContactNumber,
                    Address = objInDb.Address,
                    IsActive = objInDb.IsActive,
                    ProfilePicture = objInDb.ProfilePicture
                };
                return user;
            }
            return null;
        }

        public bool DeactivateUser(UserDto users)
        {
            var finduser = db.Users.Find(users.UserID);
            if (finduser == null)
            {
                return false;
            }
            finduser.IsActive = users.IsActive;
            db.SaveChanges();

            return true;
        }
        public bool ActivateUser(UserDto users)
        {
            var finduser = db.Users.Find(users.UserID);
            if (finduser == null)
            {
                return false;
            }
            finduser.IsActive = users.IsActive;
            db.SaveChanges();

            return true;
        }

        public Users GetUserId(string username, string password)
        {
            using (var db = new OnlineBookExchangeEntities())
            {
                return db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
        }


        public UserDto GetProfile(int userId)
        {
            var Genre = new UserDto();
            Console.WriteLine("Looking up user with ID:", userId);
            var objInDb = db.Users.FirstOrDefault(x => x.UserID == userId);
            if (objInDb != null)
            {
                return new UserDto
                {
                    UserID = objInDb.UserID,
                    Username = objInDb.Username,
                    Email = objInDb.Email,
                    ContactNumber = objInDb.ContactNumber,
                    Address = objInDb.Address,
                    Role = objInDb.Role,
                    CreatedAt = objInDb.CreatedAt,
                    ProfilePicture = objInDb.ProfilePicture,
                };
            }
            return null;
        }


        public bool UpdateUser(UserDto userDto)
        {
            var objInDb = db.Users.FirstOrDefault(x => x.UserID == userDto.UserID);
            if (objInDb != null)
            {
                objInDb.UserID = userDto.UserID;
                objInDb.Username = userDto.Username;
                objInDb.Email = userDto.Email;
                objInDb.Password = userDto.Password;
                objInDb.ContactNumber = userDto.ContactNumber;
                objInDb.Address = userDto.Address;
                var savechanges = db.SaveChanges();
                return savechanges > 0;
            }
            return false;
        }

        //public bool Signup(UserDto userdto)
        //{
        //    var passwordHasher = new PasswordHasher();
        //    var newUser = new Users
        //    {
        //        Username = userdto.Username,
        //        Email = userdto.Email,
        //        Password = userdto.Password,
        //        ContactNumber = userdto.ContactNumber,
        //        Address = userdto.Address,
        //        Role = "User",
        //        CreatedAt = DateTime.Now,
        //    };
        //    db.Users.Add(newUser);
        //    var saveChanges = db.SaveChanges();
        //    return saveChanges > 0;
        //}

        public string Login(String username, String password)
        {
            using (var context = new OnlineBookExchangeEntities())
            {
                var user = context.Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
                //var matchingUser = user.SingleOrDefault(x => x.Password == password);
                return user?.Role;
            }
        }
    }
}
