using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class WeeklyOffService
    {
        private readonly AttendanceManagementSystemContext _context;

        public WeeklyOffService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<string> CreateWeeklyOffAsync(WeeklyOffRequest weeklyOffRequest)
        {
            var message = "Weekly offs added successfully";
            if (weeklyOffRequest.MarkWeeklyOff == null || !weeklyOffRequest.MarkWeeklyOff.Any())
            {
                throw new MessageNotFoundException("No weekly off selected.");
            }
            var weeklyOffMaster = new WeeklyOffMaster
            {
                WeeklyOffName = weeklyOffRequest.WeeklyOffName,
                IsActive = weeklyOffRequest.IsActive,
                CreatedBy = weeklyOffRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.WeeklyOffMasters.AddAsync(weeklyOffMaster);
            await _context.SaveChangesAsync();

            var weeklyOffDetails = weeklyOffRequest.MarkWeeklyOff.Select(mark =>
                new WeeklyOffDetail
                {
                    WeeklyOffMasterId = weeklyOffMaster.Id,
                    MarkWeeklyOff = mark,
                    IsActive = weeklyOffRequest.IsActive,
                    CreatedBy = weeklyOffRequest.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                }).ToList();
            await _context.WeeklyOffDetails.AddRangeAsync(weeklyOffDetails);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<WeeklyOffResponse>> GetAllWeeklyOffsAsync()
        {
            var weeklyOffs = await _context.WeeklyOffMasters
                .Select(w => new WeeklyOffResponse
                {
                    WeeklyOffId = w.Id,
                    WeeklyOffName = w.WeeklyOffName,
                    IsActive = w.IsActive,
                    CreatedBy = w.CreatedBy,
                    MarkWeeklyOffs = _context.WeeklyOffDetails
                        .Where(d => d.WeeklyOffMasterId == w.Id && d.IsActive)
                        .Select(d => new WeeklyOffDetailResponse
                        {
                            MarkWeeklyOffId = d.Id,
                            MarkWeeklyOff = ((WeekdaysEnum)d.MarkWeeklyOff).ToString()
                        }).ToList()
                })
                .ToListAsync();
            if (!weeklyOffs.Any())
            {
                throw new MessageNotFoundException("Weekly off not found");
            }
            return weeklyOffs;
        }

        public async Task<string> UpdateWeeklyOffAsync(UpdateWeeklyOff updatedWeeklyOff)
        {
            var message = "Weekly off updated successfully";

            // Step 1: Fetch and validate the master record
            var existingWeeklyOffMaster = await _context.WeeklyOffMasters
                .FirstOrDefaultAsync(w => w.Id == updatedWeeklyOff.WeeklyOffId);

            if (existingWeeklyOffMaster == null)
                throw new MessageNotFoundException("Weekly off not found");

            // Step 2: Update master fields
            existingWeeklyOffMaster.WeeklyOffName = updatedWeeklyOff.WeeklyOffName;
            existingWeeklyOffMaster.IsActive = updatedWeeklyOff.IsActive;
            existingWeeklyOffMaster.UpdatedBy = updatedWeeklyOff.UpdatedBy;
            existingWeeklyOffMaster.UpdatedUtc = DateTime.UtcNow;

            // Step 3: Deactivate all existing details (clean slate)
            var existingDetails = await _context.WeeklyOffDetails
                .Where(d => d.WeeklyOffMasterId == updatedWeeklyOff.WeeklyOffId)
                .ToListAsync();

            foreach (var detail in existingDetails)
            {
                detail.IsActive = false;
                detail.UpdatedBy = updatedWeeklyOff.UpdatedBy;
                detail.UpdatedUtc = DateTime.UtcNow;
            }

            // Step 4: Insert new weekly offs
            if (updatedWeeklyOff.MarkWeeklyOff != null && updatedWeeklyOff.MarkWeeklyOff.Any())
            {
                var newDetails = updatedWeeklyOff.MarkWeeklyOff
                    .Distinct() // Avoid duplicates
                    .Select(mark => new WeeklyOffDetail
                    {
                        WeeklyOffMasterId = updatedWeeklyOff.WeeklyOffId,
                        MarkWeeklyOff = mark,
                        IsActive = true,
                        CreatedBy = updatedWeeklyOff.UpdatedBy,
                        CreatedUtc = DateTime.UtcNow
                    }).ToList();

                await _context.WeeklyOffDetails.AddRangeAsync(newDetails);
            }

            // Step 5: Save all changes
            await _context.SaveChangesAsync();

            return message;
        }
    }
}