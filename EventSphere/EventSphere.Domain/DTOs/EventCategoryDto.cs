using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.DTOs
{
    namespace EventSphere.API.DTOs
    {
        public class EventCategoryDto
        {
            public int Id { get; set; }
            public string CategoryName { get; set; }
        }
    }

}
