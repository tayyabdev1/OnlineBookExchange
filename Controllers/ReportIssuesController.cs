using OnlineBookExchange.BLL;
using OnlineBookExchange.DAL;
using OnlineBookExchange.Services;
using OnlineBookExchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBookExchange.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly OnlineBookExchangeEntities _context;

        public ReportIssuesController()
        {
            _context = new OnlineBookExchangeEntities();
        }
        // GET: ReportIssues
        public ActionResult Index()
        {
            var vm = new ReportedIssuesViewModel();
            vm.Reports = new ReportedIssuesDto().GetReports();
            return View(vm);
        }

        public ActionResult UserView()
        {
            int currentUser = Convert.ToInt32(Session["UserID"]);
            var reports = _context.ReportedIssues
                .Where(e => e.ReporterID == currentUser)
                .Select(e => new ReportedIssuesViewModel
                {
                    IssueID = e.IssueID,
                    ReporterID = e.ReporterID,
                    Subject = e.Subject,
                    BookID = e.BookID,
                    UserID = e.UserID,
                    Description = e.Description,
                    Status = e.Status,
                    CreatedAt = e.CreatedAt,
                }).ToList();
            return View(reports);
        }
    }
}