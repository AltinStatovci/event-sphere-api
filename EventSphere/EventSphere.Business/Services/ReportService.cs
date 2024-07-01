using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<Report> _reportRepository;
        public ReportService(IGenericRepository<Report> reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<Report> CreateAsync(ReportDTO Rid)
        {
            var report = new Report
            {
                UserId = Rid.UserId,
                UserName = Rid.UserName,
                UserLastName = Rid.UserLastName,
                UserEmail = Rid.UserEmail,
                ReportName = Rid.ReportName,
                ReportDesc = Rid.ReportDesc,
                ReportAnsw=Rid.ReportAnsw
            };
            await _reportRepository.AddAsync(report);
            return report;
        }

        public async Task DeleteAsync(int id)
        {
            await _reportRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _reportRepository.GetAllAsync();
        }

        public async Task<Report> GetReportByIdAsync(int id)
        {
            return await _reportRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Report>> GetReportByUserIdAsync(int userId) // Return a collection
        {
            return await _reportRepository.FindAsync(r => r.UserId == userId);
        }

        public async Task UpdateAsync(int id, ReportDTO Rid)
        {
            var report = await _reportRepository.GetByIdAsync(id);

            report.UserId = Rid.UserId;
            report.UserName = Rid.UserName;
            report.UserLastName = Rid.UserLastName;
            report.UserEmail = Rid.UserEmail;
            report.ReportName = Rid.ReportName;
            report.ReportDesc = Rid.ReportDesc;
            report.ReportAnsw = Rid.ReportAnsw;



            await _reportRepository.UpdateAsync(report);
        }

        public async Task<int> GetReportCountAsync()
        {
            return await _reportRepository.CountAsync();
        }

       
    }
}
