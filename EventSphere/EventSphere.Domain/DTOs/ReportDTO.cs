using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.DTOs
{
    public class ReportDTO
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string ReportName { get; set; }
        public string ReportDesc { get; set; }
        public string ReportAnsw { get; set; } = "Waiting for Respond";
    }
}
