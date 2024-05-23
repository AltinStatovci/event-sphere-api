using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public interface IAccountService
    {
        Task<User> AddUserAsync(User user);
    }
}
