using OnlineBookExchange.BLL;
using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBookExchange.ViewModels
{
    public class ReportedIssuesViewModel
    {
        public int IssueID { get; set; }
        public Nullable<int> ReporterID { get; set; }
        public string Subject { get; set; }
        public Nullable<int> BookID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public IEnumerable<ReportedIssuesDto> Reports { get; set; }

        public virtual Users Users { get; set; }
    }
}