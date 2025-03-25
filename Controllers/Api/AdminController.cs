using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using OnlineBookExchange.BLL;
using OnlineBookExchange.ViewModels;
using OnlineBookExchange.Services;

namespace OnlineBookExchange.Controllers.Api
{
    [JwtAuthentication]
    public class AdminController : ApiController
    {
        // Users List
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            var list = new UserDto().GetUsers();
            return Ok(list);
        }


        [HttpPut]
        [Route("api/admin/deactivate/{userID}")]
        public IHttpActionResult DeactivateUser(int userID)
        {
            var user = new UserDto
            {
                UserID = userID,
                IsActive = false,
            };

            var deActivate = new UserDto().DeactivateUser(user);
            if (deActivate)
            {
                return Ok();
            }
            else
                return BadRequest("Fail to update Status");
        }
        
        [HttpPut]
        [Route("api/admin/activate/{userID}")]
        public IHttpActionResult ActivateUser(int userID)
        {
            var user = new UserDto
            {
                UserID = userID,
                IsActive = true,
            };

            var deActivate = new UserDto().ActivateUser(user);
            if (deActivate)
            {
                return Ok();
            }
            else
                return BadRequest("Fail to update Status");
        }
    }
}
