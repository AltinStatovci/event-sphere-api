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
    
    public class ReportServices : IReportServices
    {
        private readonly IGenericRepository<Report> _reportRepository;
        public ReportServices(IGenericRepository<Report> reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<Report> CreateAsync(ReportDTO Rid)
        {
            var report = new Report
            {
                userId = Rid.userId,
                userName = Rid.userName,
                userlastName = Rid.userlastName,
                userEmail = Rid.userEmail,
                reportName = Rid.reportName,
                reportDesc = Rid.reportDesc,
                reportAnsw=Rid.reportAnsw
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
            return await _reportRepository.FindAsync(r => r.userId == userId);
        }

        public async Task UpdateAsync(int id, ReportDTO Rid)
        {
            var report = await _reportRepository.GetByIdAsync(id);

            report.userId = Rid.userId;
            report.userName = Rid.userName;
            report.userlastName = Rid.userlastName;
            report.userEmail = Rid.userEmail;
            report.reportName = Rid.reportName;
            report.reportDesc = Rid.reportDesc;
            report.reportAnsw = Rid.reportAnsw;



            await _reportRepository.UpdateAsync(report);
        }

        public async Task<int> GetReportCountAsync()
        {
            return await _reportRepository.CountAsync();
        }

       
    }
}
