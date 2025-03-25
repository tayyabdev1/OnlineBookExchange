using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookExchange.BLL
{
    public class RatingsDto
    {
        OnlineBookExchangeEntities db;

        public RatingsDto()
        {
            db = new OnlineBookExchangeEntities();
            db.Configuration.ProxyCreationEnabled = false;
        }
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


        public bool AddRating(RatingsDto ratingDto)
        {
            var newRating = new Ratings
            {
                Exchange_id = ratingDto.Exchange_id,
                RaterID = ratingDto.RaterID,
                RateeID = ratingDto.RateeID,
                BookID = ratingDto.BookID,
                BookConditionRating = ratingDto.BookConditionRating,
                ExchangePartnerRating = ratingDto.ExchangePartnerRating,
                Rview = ratingDto.Rview,
                CreatedAt = DateTime.Now
            };
            db.Ratings.Add(newRating);
            var saveChanges = db.SaveChanges();
            return saveChanges > 0;
        }

        public List<RatingsDto> GetUserReviews(int userId)
        {
            return db.Ratings
                .Where(r => r.RateeID == userId)
                .Select(r => new RatingsDto
                {
                    Exchange_id = r.Exchange_id,
                    RaterID = r.RaterID,
                    RateeID = r.RateeID,
                    BookID = r.BookID,
                    BookConditionRating = r.BookConditionRating,
                    ExchangePartnerRating = r.ExchangePartnerRating,
                    Rview = r.Rview,
                    CreatedAt = DateTime.Now
                }).ToList();
        }

        public List<RatingsDto> GetBookReviews(int bookId)
        {
            return db.Ratings
                .Where(r => r.BookID == bookId)
                .Select(r => new RatingsDto
                {
                    Exchange_id = r.Exchange_id,
                    RaterID = r.RaterID,
                    RateeID = r.RateeID,
                    BookID = r.BookID,
                    BookConditionRating = r.BookConditionRating,
                    ExchangePartnerRating = r.ExchangePartnerRating,
                    Rview = r.Rview,
                    CreatedAt = r.CreatedAt
                }).ToList();
        }

    }
}
