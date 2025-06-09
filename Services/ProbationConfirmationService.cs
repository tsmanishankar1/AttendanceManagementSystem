using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Core;

namespace AttendanceManagement.Services
{
    public class ProbationConfirmationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ProbationConfirmationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AttendanceManagementSystemContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                var today = DateOnly.FromDateTime(DateTime.Today);
                var normalProbations = await dbContext.Probations
                    .Where(p => p.IsActive &&
                                p.ProbationEndDate == today &&
                                (p.IsNotificationSent == null || p.IsNotificationSent == false) &&
                                !dbContext.Feedbacks.Any(f => f.ProbationId == p.Id && f.ExtensionPeriod != null))
                    .ToListAsync(stoppingToken);
                var extendedFeedbacks = await dbContext.Feedbacks
                    .Include(f => f.Probation)
                    .Where(f => f.IsActive &&
                                f.ExtensionPeriod == today &&
                                (f.IsNotificationSent == null || f.IsNotificationSent == false) &&
                                f.Probation.IsActive)
                    .ToListAsync(stoppingToken);
                foreach (var p in normalProbations)
                {
                    var staff = await dbContext.StaffCreations
                        .Where(s => s.Id == p.StaffCreationId && s.IsActive == true)
                        .Select(s => new { s.FirstName, s.LastName })
                        .FirstOrDefaultAsync(stoppingToken);

                    if (staff != null)
                    {
                        await emailService.SendProbationNotificationToHrAsync(
                            $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                            p.ProbationStartDate,
                            p.ProbationEndDate);

                        p.IsNotificationSent = true;
                    }
                }
                foreach (var f in extendedFeedbacks)
                {
                    var staff = await dbContext.StaffCreations
                        .Where(s => s.Id == f.Probation.StaffCreationId && s.IsActive == true)
                        .Select(s => new { s.FirstName, s.LastName })
                        .FirstOrDefaultAsync(stoppingToken);
                    if (staff != null && f.ExtensionPeriod.HasValue)
                    {
                        await emailService.SendProbationNotificationToHrAsync(
                            $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                            f.Probation.ProbationStartDate,
                            f.ExtensionPeriod.Value);

                        f.IsNotificationSent = true;
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}