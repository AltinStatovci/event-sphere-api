using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
    }
}
