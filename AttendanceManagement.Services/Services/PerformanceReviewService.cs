using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AttendanceManagement.Services
{
    public class PerformanceReviewService : IPerformanceReview
    {
        private readonly AttendanceManagementSystemContext _context;
        public PerformanceReviewService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        /*        public async Task<List<PerformanceReviewDto>> GetPerformanceReviewCycle()
                {
                    var performances = await (from per in _context.PerformanceReviewCycles
                                              select new PerformanceReviewDto
                                              {
                                                  Id = per.Id,
                                                  From = per.From,
                                                  To = per.To,
                                                  IsActive = per.IsActive,
                                                  CreatedBy = per.CreatedBy
                                              })
                                              .ToListAsync();
                    if (performances.Count == 0) throw new MessageNotFoundException("No performance review cycles found");
                    return performances;
                }

                public async Task<string> CreatetPerformanceReviewCycle(PerformanceReviewRequest performanceReviewRequest)
                {
                    var message = "Performance review cycle created successfully";
                    var performance = new PerformanceReviewCycle
                    {
                        From = performanceReviewRequest.From,
                        To = performanceReviewRequest.To,
                        IsActive = performanceReviewRequest.IsActive,
                        CreatedBy = performanceReviewRequest.CreatedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _context.AddAsync(performance);
                    await _context.SaveChangesAsync();
                    return message;
                }

                public async Task<string> UpdatePerformanceReviewCycle (UpdatePerformanceReview updatePerformanceReview)
                {
                    var message = "Performance review cycle updated successfully";
                    var existingPerformanceReview = await _context.PerformanceReviewCycles.FirstOrDefaultAsync(p => p.Id == updatePerformanceReview.Id);
                    if (existingPerformanceReview == null) throw new MessageNotFoundException("Performance review cycle not found");
                    existingPerformanceReview.From = updatePerformanceReview.From;
                    existingPerformanceReview.To = updatePerformanceReview.To;
                    existingPerformanceReview.IsActive = updatePerformanceReview.IsActive;
                    existingPerformanceReview.UpdatedBy = updatePerformanceReview.UpdatedBy;
                    existingPerformanceReview.UpdatedUtc = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return message;
                }

                public async Task<List<EligibleEmployeeResponse>> GetEligibleEmployees()
                {
                    var performances = await (from per in _context.PerformanceReviewEmployees
                                              join staff in _context.StaffCreations on per.StaffId equals staff.Id
                                              join cycle in _context.PerformanceReviewCycles on per.PerformanceCycleId equals cycle.Id
                                              where per.IsActive == true && staff.IsActive == true && cycle.IsActive == true
                                              select new EligibleEmployeeResponse
                                              {
                                                  Id = per.Id,
                                                  StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                                                  PerformanceCycle = $"{cycle.From} - {cycle.To}",
                                                  IsActive = per.IsActive,
                                                  CreatedBy = per.CreatedBy
                                              })
                                  .ToListAsync();
                    if (performances.Count == 0) throw new MessageNotFoundException("No eligile staffs found");
                    return performances;
                }

                public async Task<string> CreatetEligibleEmployees(EligibleEmployeeRequest performanceReviewRequest)
                {
                    var message = "Eligible staffs submitted successfully";
                    var performance = new PerformanceReviewEmployee
                    {
                        StaffId = performanceReviewRequest.StaffId,
                        PerformanceCycleId = performanceReviewRequest.PerformanceCycleId,
                        IsActive = performanceReviewRequest.IsActive,
                        CreatedBy = performanceReviewRequest.CreatedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _context.AddAsync(performance);
                    await _context.SaveChangesAsync();
                    return message;
                }
        */

        public async Task<List<MonthlyPerformanceResponse>> GetMonthlyPerformance(int year, int month)
        {
            var performances = await (from per in _context.MonthlyPerformances
                                      join perty in _context.PerformanceUploadTypes on per.PerformanceTypeId equals perty.Id
                                      where per.Year == year && per.Month == month && per.IsActive == true && perty.IsActive
                                      select new MonthlyPerformanceResponse
                                      {
                                          EmployeeCode = per.EmployeeCode,
                                          EmployeeName = per.EmployeeName,
                                          Designation = per.Designation,
                                          ProductivityScore = per.ProductivityScore,
                                          QualityScore = per.QualityScore,
                                          PresentScore = per.PresentScore,
                                          TotalScore = per.TotalScore,
                                          ProductivityPercentage = per.ProductivityPercentage,
                                          QualityPercentage = per.QualityPercentage,
                                          PresentPercentage = per.PresentPercentage,
                                          FinalPercentage = per.FinalPercentage,
                                          Grade = per.Grade,
                                          TotalAbsents = per.TotalAbsents,
                                          ReportingHead = per.ReportingHead,
                                          TenureYears = per.TenureYears,
                                          HrComments = per.HrComments,
                                          PerformanceType = perty.Name,
                                          Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(per.Month),
                                          Year = per.Year
                                      }).ToListAsync();
            if(performances.Count == 0) throw new MessageNotFoundException("No monthly performance report found for the given year and month");
            return performances;
        }

        public async Task<List<QuarterlyPerformanceResponse>> GetQuarterlyPerformance(int year, string quarterType)
        {
            var performances = await (from per in _context.QuarterlyPerformances
                                      join perty in _context.PerformanceUploadTypes on per.PerformanceTypeId equals perty.Id
                                      where per.Year == year && per.Quarter == quarterType && per.IsActive && perty.IsActive
                                      select new QuarterlyPerformanceResponse
                                      {
                                          EmployeeCode = per.EmployeeCode,
                                          EmployeeName = per.EmployeeName,
                                          Designation = per.Designation,
                                          TenureYears = per.TenureYears,
                                          ProductivityPercentage = per.ProductivityPercentage,
                                          QualityPercentage = per.QualityPercentage,
                                          PresentPercentage = per.PresentPercentage,
                                          FinalPercentage = per.FinalPercentage,
                                          Grade = per.Grade,
                                          AbsentDays = per.AbsentDays,
                                          HrComments = per.HrComments,
                                          PerformanceType = perty.Name,
                                          Quarter = per.Quarter,
                                          Year = per.Year
                                      }).ToListAsync();
            if (performances.Count == 0) throw new MessageNotFoundException("No quarterly performance report found for the given year and month");
            return performances;
        }

        public async Task<List<YearlyPerformanceResponse>> GetYearlyPerformance(int year)
        {
            var performances = await (from per in _context.YearlyPerformances
                                      join perty in _context.PerformanceUploadTypes on per.PerformanceTypeId equals perty.Id
                                      where per.Year == year && per.IsActive == true
                                      select new YearlyPerformanceResponse
                                      {
                                          EmployeeCode = per.EmployeeCode,
                                          EmployeeName = per.EmployeeName,
                                          Designation = per.Designation,
                                          TenureYears = per.TenureYears,
                                          ProductivityPercentage = per.ProductivityPercentage,
                                          QualityPercentage = per.QualityPercentage,
                                          PresentPercentage = per.PresentPercentage,
                                          FinalPercentage = per.FinalPercentage,
                                          Grade = per.Grade,
                                          AbsentDays = per.AbsentDays,
                                          HrComments = per.HrComments,
                                          PerformanceType = perty.Name,
                                          Year = per.Year
                                      }).ToListAsync();
            if (performances.Count == 0) throw new MessageNotFoundException("No yearly performance report found for the given year and month");
            return performances;
        }
    }
}
