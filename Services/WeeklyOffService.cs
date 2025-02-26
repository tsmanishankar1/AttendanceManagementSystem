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
                return "No weekly off selected.";
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
        public async Task<WeeklyOffResponse> GetWeeklyOffByIdAsync(int weeklyOffId)
        {
            var weeklyOff = await _context.WeeklyOffMasters
                .Where(w => w.Id == weeklyOffId)
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
                .FirstOrDefaultAsync();

            if (weeklyOff == null)
            {
                throw new MessageNotFoundException("Weekly off not found");
            }
            return weeklyOff;
        }
        public async Task<IEnumerable<WeeklyOffResponse>> GetAllWeeklyOffsAsync()
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
            var existingWeeklyOffMaster = await _context.WeeklyOffMasters
                .FirstOrDefaultAsync(w => w.Id == updatedWeeklyOff.WeeklyOffId);

            if (existingWeeklyOffMaster == null)
                throw new MessageNotFoundException("Weekly off not found");
            existingWeeklyOffMaster.WeeklyOffName = updatedWeeklyOff.WeeklyOffName;
            existingWeeklyOffMaster.IsActive = updatedWeeklyOff.IsActive;
            existingWeeklyOffMaster.UpdatedBy = updatedWeeklyOff.UpdatedBy;
            existingWeeklyOffMaster.UpdatedUtc = DateTime.UtcNow;
            var existingDetails = await _context.WeeklyOffDetails
                .Where(d => d.WeeklyOffMasterId == updatedWeeklyOff.WeeklyOffId)
                .ToListAsync();
            var existingMarkWeeklyOffs = existingDetails.Select(d => d.MarkWeeklyOff).ToList();
            var newMarkWeeklyOffs = updatedWeeklyOff.MarkWeeklyOff;

            var toDeactivate = existingDetails.Where(d => !newMarkWeeklyOffs.Contains(d.MarkWeeklyOff)).ToList();
            foreach (var detail in toDeactivate)
            {
                detail.IsActive = false;
                detail.UpdatedBy = updatedWeeklyOff.UpdatedBy;
                detail.UpdatedUtc = DateTime.UtcNow;
            }
            var toInsert = newMarkWeeklyOffs
                .Where(mark => !existingMarkWeeklyOffs.Contains(mark))
                .Select(mark => new WeeklyOffDetail
                {
                    WeeklyOffMasterId = updatedWeeklyOff.WeeklyOffId,
                    MarkWeeklyOff = mark,
                    IsActive = true,
                    CreatedBy = updatedWeeklyOff.UpdatedBy,
                    CreatedUtc = DateTime.UtcNow
                }).ToList();
            await _context.SaveChangesAsync();
            await _context.WeeklyOffDetails.AddRangeAsync(toInsert);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}