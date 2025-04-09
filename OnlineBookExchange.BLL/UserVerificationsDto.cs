using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookExchange.BLL
{
    public class UserVerificationsDto
    {
        public int ID { get; set; }
        public Nullable<int> UserId { get; set; }
        public string CNICNumber { get; set; }
        public string CNICFrontImage { get; set; }
        public string CNICBackImage { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        public virtual Users Users { get; set; }
    }
}
