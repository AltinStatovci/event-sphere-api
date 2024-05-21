using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{

    public class EventCategoryRepository : GenericRepository<EventCategory>, IGenericRepository<EventCategory>
    {
        public EventCategoryRepository(EventSphereDbContext context) : base(context)
        {
        }
    }
}
