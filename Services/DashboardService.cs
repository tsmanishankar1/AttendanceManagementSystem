using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceManagement.AtrakModels;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class DashboardService
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly AtrakContext _atrakContext;

        public DashboardService(AttendanceManagementSystemContext context, AtrakContext atrakContext)
        {
            _context = context;
            _atrakContext = atrakContext;
        }

        public async Task<List<object>> GetTodaysAnniversaries(int eventTypeId)
        {
            var eventType = await _context.EventTypes.AnyAsync(e => e.Id == eventTypeId && e.IsActive);
            if (!eventType) throw new MessageNotFoundException("Event type not found");
            var today = DateOnly.FromDateTime(DateTime.Now);
            var tenDaysAgo = today.AddDays(-10);
            var currentYear = today.Year;
            var result = new List<object>();
            var staffWithAnniversaries = await _context.StaffCreations
                .Where(e =>
                    e.IsActive == true && (
                        (eventTypeId == 1 && e.Dob.Month == today.Month && e.Dob.Day == today.Day) ||
                        (eventTypeId == 2 && e.MarriageDate != null && e.MarriageDate.Value.Month == today.Month && e.MarriageDate.Value.Day == today.Day) ||
                        (eventTypeId == 3 && e.JoiningDate >= tenDaysAgo && e.JoiningDate <= today) ||
                        (eventTypeId == 4 && e.JoiningDate.Month == today.Month && e.JoiningDate.Day == today.Day) ||
                        (eventTypeId == 5 && (today.Year - e.JoiningDate.Year) == 3 && e.JoiningDate.Month == today.Month && e.JoiningDate.Day == today.Day)
                    )
                )
                .ToListAsync();
            if (!staffWithAnniversaries.Any())
            {
                throw new MessageNotFoundException("No records found for the selected event type");
            }
            if (eventTypeId == 1) 
            {
                var birthday = staffWithAnniversaries.Select(staff => new StaffBirthDayDto
                {
                    StaffId = staff.Id,
                    StaffCreationId = staff.StaffId,
                    StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                    Location =  _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId)?.Name ?? string.Empty,
                    Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId)?.Name ?? string.Empty,
                    BirthDate = staff.Dob.ToString("MMMM dd"),
                    ProfilePhoto = staff.ProfilePhoto
                }).ToList<object>();
                if (birthday.Count == 0)
                {
                    throw new MessageNotFoundException("No birthdays today");
                }
                result = birthday;
            }
            else if (eventTypeId == 2) 
            {
                var weddingAnniversary = staffWithAnniversaries.Select(staff => new StaffAnniversaryDto
                {
                    StaffId = staff.Id,
                    StaffCreationId = staff.StaffId,
                    StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                    Location = _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId)?.Name ?? string.Empty,
                    Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId)?.Name ?? string.Empty,
                    MarriageDate = staff.MarriageDate?.ToString("MMMM dd") ?? string.Empty,
                    ProfilePhoto = staff.ProfilePhoto
                }).ToList<object>();
                if (weddingAnniversary.Count == 0)
                {
                    throw new MessageNotFoundException("No wedding anniversaries today");
                }
                result = weddingAnniversary;
            }
            else if (eventTypeId == 3) 
            {
                var newJoinees = staffWithAnniversaries.Select(staff => new NewJoinee
                {
                    StaffId = staff.Id,
                    StaffCreationId = staff.StaffId,
                    StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                    Location = _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId)?.Name ?? string.Empty,
                    Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId)?.Name ?? string.Empty,
                    JoiningDate = staff.JoiningDate.ToString("MMMM dd"),
                    ProfilePhoto = staff.ProfilePhoto
                }).ToList<object>();
                if (newJoinees.Count == 0)
                {
                    throw new MessageNotFoundException("No New Joiners recently");
                }
                result = newJoinees;
            }
            else if (eventTypeId == 4) 
            {
                var joiningAnniversaries = staffWithAnniversaries
                    .Where(staff => (today.Year - staff.JoiningDate.Year) > 0)
                    .Select(staff => new JoiningAnniversary
                    {
                        StaffId = staff.Id,
                        StaffCreationId = staff.StaffId,
                        StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                        Location = _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId)?.Name ?? string.Empty,
                        Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId)?.Name ?? string.Empty,
                        JoiningDate = staff.JoiningDate.ToString("MMMM dd"),
                        JoiningAnniversaryYear = $"{(today.Year - staff.JoiningDate.Year + 1)}{GetOrdinalSuffix(today.Year - staff.JoiningDate.Year + 1)} Year",
                        ProfilePhoto = staff.ProfilePhoto
                    }).ToList<object>();
                if (joiningAnniversaries.Count == 0)
                {
                    throw new MessageNotFoundException("No joining anniversaries today");
                }
                result = joiningAnniversaries;
            }
            else if (eventTypeId == 5) 
            {
                var threeYearStaff = staffWithAnniversaries.Select(staff => new ThreeYearsOfService
                {
                    StaffId = staff.Id,
                    StaffCreationId = staff.StaffId,
                    StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                    Location = _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId && loc.IsActive)?.Name ?? string.Empty,
                    Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId && des.IsActive)?.Name ?? string.Empty,
                    ProfilePhoto = staff.ProfilePhoto
                }).ToList<object>();
                if (threeYearStaff.Count == 0)
                {
                    throw new MessageNotFoundException("No staff completed 3 years of service today");
                }
                result = threeYearStaff;
            }
            return result;
        }

        private static string GetOrdinalSuffix(int number)
        {
            if (number <= 0) return number.ToString();

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }
            switch (number % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        public async Task<List<NewJoinee>> GetNewJoinee()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var tenDaysAgo = today.AddDays(-10);

            var newJoinees = await (from staff in _context.StaffCreations
                                    join org in _context.OrganizationTypes
                                    on staff.OrganizationTypeId equals org.Id
                                    join loc in _context.LocationMasters
                                    on staff.LocationMasterId equals loc.Id
                                    join des in _context.DesignationMasters
                                    on staff.DesignationId equals des.Id
                                    where staff.JoiningDate >= tenDaysAgo && staff.JoiningDate <= today && staff.IsActive == true
                                    select new NewJoinee
                                    {
                                        StaffId = staff.Id,
                                        StaffCreationId = staff.StaffId,
                                        StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                                        Location = loc.Name,
                                        Designation = des.Name,
                                        JoiningDate = staff.JoiningDate.ToString("MMMM dd")
                                    })
                                    .ToListAsync();
            if (newJoinees.Count == 0) throw new MessageNotFoundException("No New Joiners recently");
            return newJoinees;
        }

        public async Task<List<object>> GetAllHolidaysAsync(int staffId, int shiftTypeId)
        {
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive == true);
            if (staff == null) throw new MessageNotFoundException("Staff not found");
            int targetHolidayCalendarId;
            if (shiftTypeId == 1)
            {
                targetHolidayCalendarId = await _context.HolidayCalendarConfigurations
                    .Where(x => x.IsActive && x.CalendarYear == DateTime.UtcNow.Year && x.ShiftTypeId == shiftTypeId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
            }
            else if (shiftTypeId == 2)
            {
                targetHolidayCalendarId = await _context.HolidayCalendarConfigurations
                    .Where(x => x.IsActive && x.CalendarYear == DateTime.UtcNow.Year && x.ShiftTypeId == shiftTypeId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
            }
            else
            {
                targetHolidayCalendarId = staff.HolidayCalendarId;
            }

            var holiday = await (
                from hc in _context.HolidayCalendarTransactions
                join hm in _context.HolidayMasters on hc.HolidayMasterId equals hm.Id
                join hcc in _context.HolidayCalendarConfigurations on hc.HolidayCalendarId equals hcc.Id
                where hc.IsActive && hm.IsActive && hcc.IsActive && hcc.CalendarYear == DateTime.UtcNow.Year && hcc.Id == targetHolidayCalendarId
                select new
                {
                    Id = hc.Id,
                    HolidayName = hm.Name,
                    FromDate = hc.FromDate,
                    ToDate = hc.ToDate
                })
                .ToListAsync<object>();

            if (holiday.Count == 0)
                throw new MessageNotFoundException("No holidays found");

            return holiday;
        }

        public async Task<List<object>> GetLeaveDetailsWithDefaultsAsync(int StaffCreationId)
        {
            var allLeaveTypes = await _context.LeaveTypes
                .Where(lt => lt.IsActive)
                .Select(lt => new { lt.Id, lt.Name })
                .ToListAsync();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var userLeaveRecords = await _context.IndividualLeaveCreditDebits
                .Where(l => l.StaffCreationId == StaffCreationId && l.IsActive)
                .GroupBy(l => l.LeaveTypeId)
                .Select(g => new
                {
                    LeaveTypeId = g.Key,
                    AvailableBalance = g.OrderByDescending(l => l.Id).FirstOrDefault().AvailableBalance
                })
                .ToListAsync();
            var leaveDetails = allLeaveTypes.Select(lt =>
            {
                var record = userLeaveRecords.FirstOrDefault(r => r.LeaveTypeId == lt.Id);
                return new
                {
                    LeaveTypeId = lt.Id, 
                    LeaveTypeName = lt.Name,
                    AvailableBalance = record?.AvailableBalance ?? 0
                };
            }).ToList();
            return leaveDetails.Cast<object>().ToList();
        }

        public async Task<List<object>> GetHeadCountByDepartmentAsync()
        {
            var today = DateTime.Today;
            var attendanceData = await _atrakContext.SmaxTransactions.Where(st => st.TrDate == today).ToListAsync();
            var result = await (from sc in _context.StaffCreations
                                join dm in _context.DepartmentMasters on sc.DepartmentId equals dm.Id
                                where sc.IsActive == true && dm.IsActive
                                select new
                                {
                                    sc.StaffId,
                                    DepartmentName = dm.Name
                                }).ToListAsync();
            var groupedData = result.GroupBy(r => r.DepartmentName)
                .Select(g => {
                    int headCount = g.Count();
                    int presentCount = g.Count(x => attendanceData.Any(att => att.TrChId == x.StaffId));
                    int absentCount = headCount - presentCount;
                    double presentPercentage = headCount > 0 ? Math.Round(presentCount * 100.0 / headCount, 2) : 0;
                    double absentPercentage = headCount > 0 ? Math.Round(absentCount * 100.0 / headCount, 2) : 0;
                    return new
                    {
                        DepartmentName = g.Key,
                        HeadCount = headCount,
                        PresentCount = presentCount,
                        AbsentCount = absentCount,
                        PresentPercentage = presentPercentage,
                        AbsentPercentage = absentPercentage
                    };
                }).ToList();
            return groupedData.Cast<object>().ToList();
        }

        public async Task<List<object>> GetUpcomingShiftsForStaffAsync(int staffId)
        {
            var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var upcomingShifts = await _context.AssignShifts
                .Where(asg => asg.IsActive &&
                              asg.StaffId == staffId &&
                              asg.FromDate == tomorrow)
                .OrderBy(asg => asg.FromDate)
                .Select(asg => new
                {
                   ShiftId = asg.Shift.Id,
                   ShiftName = asg.Shift.Name,
                   StartTime = asg.Shift.StartTime,
                   EndTime = asg.Shift.EndTime,
                   Date = asg.FromDate
                })
                .ToListAsync();
            if (upcomingShifts.Count == 0) throw new MessageNotFoundException("No Shift is Assigned");
            return upcomingShifts.Cast<object>().ToList();
        }

        public async Task<string> CreateAnnouncement(AnnouncementDto announcementDto)
        {
            var announcement = new Announcement
            {
                Title = announcementDto.Title,
                Description = announcementDto.Description,
                IsActive = announcementDto.IsActive,
                CreatedBy = announcementDto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.Announcements.AddAsync(announcement);
            await _context.SaveChangesAsync();
            return "Announcement created successfully";
        }

        public async Task<List<AnnouncementResponse>> GetAnnouncement()
        {
            var getAnnouncement = await (from an in _context.Announcements
                                         select new AnnouncementResponse
                                         {
                                             Id = an.Id,
                                             Title = an.Title,
                                             Description = an.Description,
                                             IsActive = an.IsActive,
                                             CreatedBy = an.CreatedBy
                                         })
                                         .ToListAsync();
            if (getAnnouncement.Count == 0) throw new MessageNotFoundException("Announcement not found");
            return getAnnouncement;
        }

        public async Task<string> UpdateAnnouncement(AnnouncementResponse announcementResponse)
        {
            var existingAnnouncement = await _context.Announcements.FirstOrDefaultAsync(a => a.Id == announcementResponse.Id);
            if (existingAnnouncement == null) throw new MessageNotFoundException("Announcement not found");
            existingAnnouncement.Title = announcementResponse.Title;
            existingAnnouncement.Description = announcementResponse.Description;
            existingAnnouncement.IsActive = announcementResponse.IsActive;
            existingAnnouncement.UpdatedBy = announcementResponse.CreatedBy;
            existingAnnouncement.UpdatedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return "Announcement updated successfully";
        }
    }
}