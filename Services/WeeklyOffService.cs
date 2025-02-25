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
            var message = "Weekly off added successfully";
            var weeklyOff = new WeeklyOffMaster
            {
                WeeklyOffName = weeklyOffRequest.WeeklyOffName,
                MarkWeeklyOff = weeklyOffRequest.MarkWeeklyOff,
                IsActive = weeklyOffRequest.IsActive,
                CreatedBy = weeklyOffRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.WeeklyOffMasters.AddAsync(weeklyOff);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<WeeklyOffResponse> GetWeeklyOffByIdAsync(int weeklyOffId)
        {
            var weekly = await (from week in _context.WeeklyOffMasters
                                where week.Id == weeklyOffId
                                select new WeeklyOffResponse
                                {
                                    WeeklyOffId = week.Id,
                                    WeeklyOffName = week.WeeklyOffName,
                                    MarkWeeklyOff = ((WeekdaysEnum)week.MarkWeeklyOff).ToString(),
                                    IsActive = week.IsActive,
                                    CreatedBy = week.CreatedBy
                                }).FirstOrDefaultAsync();
            if (weekly == null)
            {
                throw new MessageNotFoundException("Weekly off not found");
            }
            return weekly;
        }

        public async Task<IEnumerable<WeeklyOffResponse>> GetAllWeeklyOffsAsync()
        {
            var weekly = await (from week in _context.WeeklyOffMasters
                                select new WeeklyOffResponse
                                {
                                    WeeklyOffId = week.Id,
                                    WeeklyOffName = week.WeeklyOffName,
                                    MarkWeeklyOff = ((WeekdaysEnum)week.MarkWeeklyOff).ToString(),
                                    IsActive = week.IsActive,
                                    CreatedBy = week.CreatedBy
                                }).ToListAsync();
            if (weekly.Count == 0)
            {
                throw new MessageNotFoundException("Weekly off not found");
            }
            return weekly;
        }

        public async Task<string> UpdateWeeklyOffAsync(UpdateWeeklyOff updatedWeeklyOff)
        {
            var message = "Weekly off updated successfully";
            var existingWeeklyOff = await _context.WeeklyOffMasters.FirstOrDefaultAsync(w => w.Id == updatedWeeklyOff.WeeklyOffId);
            if (existingWeeklyOff == null)
                throw new MessageNotFoundException("Weekly off not found");

            existingWeeklyOff.WeeklyOffName = updatedWeeklyOff.WeeklyOffName;
            existingWeeklyOff.MarkWeeklyOff = updatedWeeklyOff.MarkWeeklyOff;
            existingWeeklyOff.UpdatedBy = updatedWeeklyOff.UpdatedBy;
            existingWeeklyOff.IsActive = updatedWeeklyOff.IsActive;
            existingWeeklyOff.UpdatedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return message;
        }
    }

}

