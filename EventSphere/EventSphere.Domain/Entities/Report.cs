using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.Entities
{
    public class Report
    {
        public int reportId { get; set; }
        public int userId {  get; set; }
        public string userName { get; set; }
        public string userlastName { get; set; }
        public string userEmail { get; set; }
        public string reportName { get; set; }
        public string reportDesc { get; set; }
        public string reportAnsw { get; set; } = "Waiting for Respond";
    }
}
