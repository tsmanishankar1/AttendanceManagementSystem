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
                var now = DateTime.Now;
                var nextRunTime = DateTime.Today.AddHours(10);
                if (now > nextRunTime)
                {
                    nextRunTime = nextRunTime.AddDays(1);
                }
                var delay = nextRunTime - now;
                await Task.Delay(delay, stoppingToken);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AttendanceManagementSystemContext>();
                    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                    var today = DateOnly.FromDateTime(DateTime.Today);
                    var probations = await (from p in dbContext.Probations
                                            join f in dbContext.Feedbacks on p.Id equals f.ProbationId into feedbackGroup
                                            from f in feedbackGroup.DefaultIfEmpty()
                                            where p.IsActive &&
                                                  (
                                                      (f.ExtensionPeriod == null && p.ProbationEndDate == today) ||
                                                      (f != null && f.ExtensionPeriod != null && f.ExtensionPeriod == today)
                                                  )
                                            select p).ToListAsync(stoppingToken);
                    foreach (var employee in probations)
                    {
                        var probation = await dbContext.Probations.FirstOrDefaultAsync(p => p.Id == employee.Id && p.IsActive, stoppingToken);
                        var probationer = await dbContext.StaffCreations.FirstOrDefaultAsync(s => s.Id == probation.StaffCreationId && s.IsActive == true, stoppingToken);
                        await emailService.SendProbationNotificationToHrAsync($"{probationer.FirstName} {probationer.LastName}", probation.ProbationStartDate, probation.ProbationEndDate);
                    }
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
