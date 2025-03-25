using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookExchange.BLL
{
    public class ReportedIssuesDto
    {
        OnlineBookExchangeEntities db;

        public ReportedIssuesDto()
        {
            db = new OnlineBookExchangeEntities();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public int IssueID { get; set; }
        public Nullable<int> ReporterID { get; set; }
        public string Subject { get; set; }
        public Nullable<int> BookID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        public virtual Users Users { get; set; }


        public int CreateReport(ReportedIssuesDto reportedDto)
        {
            var newReport = new ReportedIssues
            {
                ReporterID = reportedDto.ReporterID,
                Subject = reportedDto.Subject,
                BookID = reportedDto.BookID,
                UserID = reportedDto.UserID,
                Description = reportedDto.Description,
                Status = "Pending",
                CreatedAt = DateTime.Now,
            };
            db.ReportedIssues.Add(newReport);
            var savedChanges = db.SaveChanges();
            return 1;
        }

        public List<ReportedIssuesDto> GetReports()
        {
            using (var context = new OnlineBookExchangeEntities())
            {
                return context.ReportedIssues.Select(r => new ReportedIssuesDto
                {
                    IssueID = r.IssueID,
                    ReporterID = r.ReporterID,
                    Subject = r.Subject,
                    BookID = r.BookID,
                    UserID = r.UserID,
                    Description = r.Description,
                    Status = r.Status,
                }).ToList();
            }
        }

        public bool ResolvedIssue(ReportedIssuesDto issues)
        {
            var findissue = db.ReportedIssues.Find(issues.IssueID);
            if (findissue == null)
            {
                return false;
            }
            findissue.Status = issues.Status;
            db.SaveChanges();

            return true;
        }
    }
}
