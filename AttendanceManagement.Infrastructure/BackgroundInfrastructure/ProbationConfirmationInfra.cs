using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AttendanceManagement.Infrastructure
{
    public class ProbationConfirmationInfra : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ProbationConfirmationInfra(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AttendanceManagementSystemContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailInfra>();
                var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingInfra>();
                try
                {
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
                catch (Exception ex)
                {
                    await loggingService.LogError("Probation Confirmation Background Service", "GET", "/api/ProbationConfirmationService", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, 1, null);
                }
            }
        }
    }
}