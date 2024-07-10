using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IReportServices
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(int id);
        Task<Report> CreateAsync(ReportDTO Rid);
        Task UpdateAsync(int id, ReportDTO Rid);
        Task DeleteAsync(int id);
        Task<int> GetReportCountAsync();
        Task<IEnumerable<Report>> GetReportByUserIdAsync(int userId);
    }
}
