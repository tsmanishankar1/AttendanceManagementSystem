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
                    var probations = await (
                        from p in dbContext.Probations
                        join f in dbContext.Feedbacks on p.Id equals f.ProbationId into feedbackGroup
                        from f in feedbackGroup.DefaultIfEmpty()
                        where p.IsActive && f.IsActive &&
                              (
                                  (f.ExtensionPeriod == null && p.ProbationEndDate == today) ||
                                  (f != null && f.ExtensionPeriod != null && f.ExtensionPeriod == today)
                              )
                        select new
                        {
                            p.Id,
                            p.StaffCreationId,
                            p.ProbationStartDate,
                            FinalEndDate = f != null && f.ExtensionPeriod != null ? f.ExtensionPeriod.Value : p.ProbationEndDate
                        }).ToListAsync(stoppingToken);
                    foreach (var probation in probations)
                    {
                        var probationer = await dbContext.StaffCreations.FirstOrDefaultAsync(s => s.Id == probation.StaffCreationId && s.IsActive == true, stoppingToken);
                        if(probationer  != null)
                        {
                            await emailService.SendProbationNotificationToHrAsync(probationer.Id, $"{probationer.FirstName} {probationer.LastName}", probation.ProbationStartDate, probation.FinalEndDate);

                        }
                    }
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
