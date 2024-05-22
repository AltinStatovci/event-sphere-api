using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public class RoleServices : IRoleServices
    {
        private readonly IGenericRepository<Role> _genericRepository;

        public RoleServices(IGenericRepository<Role> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<Role> AddRoleAsync(Role role)
        {
            return await _genericRepository.AddAsync(role);
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            await _genericRepository.DeleteAsync(roleId);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _genericRepository.GetByIdAsync(roleId);
        }

        public async Task UpdateRoleAsync(Role role)
        {
            await _genericRepository.UpdateAsync(role);
        }
    }
}
