using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.DTOs.User
{
    public class UpdateUserDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? RoleName { get; set; }
        public int RoleID { get; set; }
        
    }

}
