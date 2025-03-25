using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using OnlineBookExchange.BLL;
using OnlineBookExchange.DAL;
using OnlineBookExchange.ViewModels;
using OnlineBookExchange.Services;

namespace OnlineBookExchange.Controllers.Api
{

    [RoutePrefix("api/ratings")]
    public class RatingsController : ApiController
    {
        private readonly OnlineBookExchangeEntities dbContext;

        public RatingsController()
        {
            dbContext = new OnlineBookExchangeEntities();
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddRating(RatingsViewModel ratingsViewModel)
        {
            if (ratingsViewModel.BookConditionRating < 1 || ratingsViewModel.BookConditionRating > 10 ||
                ratingsViewModel.ExchangePartnerRating < 1 || ratingsViewModel.ExchangePartnerRating > 10)
                return BadRequest("Ratings must be between 1 and 10.");

            var exchange = dbContext.Exchanges.FirstOrDefault(e => e.ExchangeID == ratingsViewModel.Exchange_id && e.Status == "Returned");
            if (exchange == null)
                return BadRequest("Invalid exchange or status.");

            var rating = new RatingsDto().AddRating
                (
                    new RatingsDto
                    {
                        Exchange_id = ratingsViewModel.Exchange_id,
                        RaterID = ratingsViewModel.RaterID,
                        RateeID = ratingsViewModel.RateeID,
                        BookID = ratingsViewModel.BookID,
                        BookConditionRating = ratingsViewModel.BookConditionRating,
                        ExchangePartnerRating = ratingsViewModel.ExchangePartnerRating,
                        Rview = ratingsViewModel.Rview,
                        CreatedAt = DateTime.Now
                    }
                );
            if (rating) return Created("", rating);
            else return BadRequest();
        }

        [HttpGet]
        [Route("user/{userId}")]
        public IHttpActionResult GetUserReviews(int userId)
        {
            var reviews = new RatingsDto().GetUserReviews(userId);
            if (reviews == null || !reviews.Any())
                return BadRequest();

            return Ok(reviews);
        }

        [HttpGet]
        [Route("user/{bookId}")]
        public IHttpActionResult GetBookReviews(int bookId)
        {
            var reviews = new RatingsDto().GetBookReviews(bookId);
            if (reviews != null || !reviews.Any())
                return BadRequest();

            return Ok(reviews);
        }
    }
}
