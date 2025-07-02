using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Services
{
    public class WeeklyOffService : IWeeklyOffService
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
            var isDuplicate = await _context.WeeklyOffMasters.AnyAsync(w => w.WeeklyOffName.ToLower() == weeklyOffRequest.WeeklyOffName.ToLower());
            if (isDuplicate) throw new ValidationException("Weekly off name already exists");
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
                    IsActive = true,
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
            var existingWeeklyOffMaster = await _context.WeeklyOffMasters.FirstOrDefaultAsync(w => w.Id == updatedWeeklyOff.WeeklyOffId);
            if (existingWeeklyOffMaster == null) throw new MessageNotFoundException("Weekly off not found");
            if (!string.IsNullOrWhiteSpace(updatedWeeklyOff.WeeklyOffName))
            {
                var isDuplicate = await _context.WeeklyOffMasters.AnyAsync(w => w.Id != updatedWeeklyOff.WeeklyOffId && w.WeeklyOffName.ToLower() == updatedWeeklyOff.WeeklyOffName.ToLower());
                if (isDuplicate) throw new ValidationException("Weekly off name already exists");
            }
            existingWeeklyOffMaster.WeeklyOffName = updatedWeeklyOff.WeeklyOffName;
            existingWeeklyOffMaster.IsActive = updatedWeeklyOff.IsActive;
            existingWeeklyOffMaster.UpdatedBy = updatedWeeklyOff.UpdatedBy;
            existingWeeklyOffMaster.UpdatedUtc = DateTime.UtcNow;

            var existingDetails = await _context.WeeklyOffDetails
                .Where(d => d.WeeklyOffMasterId == updatedWeeklyOff.WeeklyOffId)
                .ToListAsync();

            foreach (var detail in existingDetails)
            {
                detail.IsActive = false;
                detail.UpdatedBy = updatedWeeklyOff.UpdatedBy;
                detail.UpdatedUtc = DateTime.UtcNow;
            }
            if (updatedWeeklyOff.MarkWeeklyOff != null && updatedWeeklyOff.MarkWeeklyOff.Any())
            {
                var newDetails = updatedWeeklyOff.MarkWeeklyOff
                    .Distinct()
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
            await _context.SaveChangesAsync();

            return message;
        }
    }
}