using OnlineBookExchange.BLL;
using OnlineBookExchange.Helpers;
using OnlineBookExchange.Services;
using OnlineBookExchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineBookExchange.Controllers.Api
{
    public class ReportIssuesController : ApiController
    {
        [HttpPost]
        [Route("api/reportIssue/create")]
        public IHttpActionResult CreateReport(ReportedIssuesDto reportDto)
        {
            var x = new ReportedIssuesDto().CreateReport(reportDto);
            if (x == 0)
            {
                return BadRequest("Not Intersted");
            }
            reportDto.IssueID = x;
            return Created(new Uri(Request.RequestUri + "/" + x), reportDto);
        }

        [HttpPost]
        [Route("api/reportIssue/solved")]
        public IHttpActionResult ResolvedIssue(ReportedIssuesViewModel reportedViewModel)
        {
            if (reportedViewModel?.IssueID == null)
            {
                return BadRequest("ID is required");
            }
            var reportDto = new ReportedIssuesDto
            {
                IssueID = reportedViewModel.IssueID,
                Status = "Solved",
            };

            var Solved = new ReportedIssuesDto().ResolvedIssue(reportDto);

            if (Solved)
            {
                return Ok();
            }
            else
                return BadRequest("Fail to update Status");
        }
    }
}
