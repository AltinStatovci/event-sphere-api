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
            [Required(ErrorMessage = "ID is required.")]
            public int ID { get; set; }
            [Required(ErrorMessage = "Category Name is required.")]
            public string CategoryName { get; set; }
        }
    }

}
