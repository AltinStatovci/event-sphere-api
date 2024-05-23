using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly EventSphereDbContext _context;

        public UserRepository(EventSphereDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
