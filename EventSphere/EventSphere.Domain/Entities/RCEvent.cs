using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.Entities
{
    public class RCEvent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int Ecount { get; set; }
    }
}
