using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public interface IPromoCodeRepository : IGenericRepository<PromoCode>
    {
        Task<PromoCode> GetByCodeAsync(string code);
        Task<bool> IsPromoCodeValidAsync(string code);
    }
}
