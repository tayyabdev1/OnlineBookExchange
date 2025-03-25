using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineBookExchange.DAL;
using OnlineBookExchange.BLL;
using OnlineBookExchange.ViewModels;
using System.Net.Mail;
using OnlineBookExchange.Helpers;
using OnlineBookExchange.Services;

namespace OnlineBookExchange.Controllers.Api
{

    [RoutePrefix("api/exchanges")]
    public class ExchangesController : ApiController
    {
        private readonly OnlineBookExchangeEntities _context;

        public ExchangesController()
        {
            _context = new OnlineBookExchangeEntities();
        }


        [HttpPost]
        [Route("delivered")]
        public IHttpActionResult MarkAsDelivered(ExchangesViewModel exchangesViewModel)
        {
            if (exchangesViewModel?.ExchangeID == null)
            {
                return BadRequest("ExchangeID is required");
            }
            var exchangeDto = new ExchangesDto
            {
                ExchangeID = exchangesViewModel.ExchangeID,
                Status = "Delivered",
            };

            var delivered = new ExchangesDto().MarkAsDelivered(exchangeDto);

            if (delivered)
            {
                return Ok();
            }
            else
                return BadRequest("Fail to update Status");
        }


        [HttpPost]
        [Route("outForReturn")]
        public IHttpActionResult MarkAsOutForReturn(ExchangesViewModel exchangesViewModel)
        {
            if (exchangesViewModel?.ExchangeID == null)
            {
                return BadRequest("Exchange ID is required");
            }
            var exchangeDto = new ExchangesDto
            {
                ExchangeID = exchangesViewModel.ExchangeID,
                Status = "Out for Return",
                ReturnDate = DateTime.Now,
            };
            var returned = new ExchangesDto().MarkAsOutForReturn(exchangeDto);
            if (returned)
            {
                return Ok();
            }
            else
                return BadRequest("Fail to update Status");
        }

        [HttpPost]
        [Route("returned")]
        public IHttpActionResult MarkAsReturned(ExchangesViewModel exchangesViewModel)
        {
            if (exchangesViewModel?.ExchangeID == null)
            {
                return BadRequest("Exchange ID required");
            }
            var exchangeDto = new ExchangesDto
            {
                ExchangeID = exchangesViewModel.ExchangeID,
                Status = "Returned",
            };
            var returned = new ExchangesDto().MarkAsReturned(exchangeDto);
            if (returned)
            {
                return Ok();
            }
            else
                return BadRequest("Fail to update Status");
        }
    }
}
