using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookExchange.BLL
{
    public class ExchangesDto
    {
        OnlineBookExchangeEntities db;
        public ExchangesDto()
        {
            db = new OnlineBookExchangeEntities();
            db.Configuration.ProxyCreationEnabled = false;
        }
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

        public bool MarkAsDelivered(ExchangesDto exchanges)
        {
            var findexchange = db.Exchanges.Find(exchanges.ExchangeID);
            if (findexchange == null)
            {
                return false;
            }
            findexchange.Status = exchanges.Status;
            db.SaveChanges();

            return true;
        }

        public bool MarkAsOutForReturn(ExchangesDto exchanges)
        {
            var findexchange = db.Exchanges.Find(exchanges.ExchangeID);
            if (findexchange == null)
            {
                return false;   
            }
            findexchange.Status = exchanges.Status;
            findexchange.ReturnDate = DateTime.Now;
            db.SaveChanges();

            return true;
        }
        
        public bool MarkAsReturned(ExchangesDto exchanges)
        {
            var findexchange = db.Exchanges.Find(exchanges.ExchangeID);
            if (findexchange == null)
            {
                return false;   
            }
            findexchange.Status = exchanges.Status;
            db.SaveChanges();

            return true;
        }
    }
}
