using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class DashboardService
    {
        private readonly AttendanceManagementSystemContext _context;

        public DashboardService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<EventTypeResponse>> GetAllEventTypes()
        {
            var eventTypes = await _context.EventTypes
                .Where(e => e.IsActive)
                .Select(e => new EventTypeResponse
                {
                    EventTypeId = e.Id,
                    EventTypeName = e.EventName
                })
                .ToListAsync();
            if (eventTypes.Count == 0)
            {
                throw new MessageNotFoundException("No event types found");
            }
            return eventTypes;
        }
        public async Task<List<object>> GetTodaysAnniversaries(int eventTypeId)
        {
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
                throw new MessageNotFoundException("No records found for the selected event type.");
            }

            if (eventTypeId == 1) 
            {
                var birthday = staffWithAnniversaries.Select(staff => new StaffBirthDayDto
                {
                    StaffId = staff.Id,
                    StaffCreationId = staff.StaffId,
                    StaffName = $"{staff.FirstName} {staff.LastName}",
                    Location = _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId)?.FullName ?? string.Empty,
                    Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId)?.FullName ?? string.Empty,
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
                    StaffName = $"{staff.FirstName} {staff.LastName}",
                    Location = _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId)?.FullName ?? string.Empty,
                    Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId)?.FullName ?? string.Empty,
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
                    StaffName = $"{staff.FirstName} {staff.LastName}",
                    Location = _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId)?.FullName ?? string.Empty,
                    Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId)?.FullName ?? string.Empty,
                    JoiningDate = staff.JoiningDate.ToString("MMMM dd"),
                    ProfilePhoto = staff.ProfilePhoto
                }).ToList<object>();
                if (newJoinees.Count == 0)
                {
                    throw new MessageNotFoundException("No New Joiners recently!");
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
                        StaffName = $"{staff.FirstName} {staff.LastName}",
                        Location = _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId)?.FullName ?? string.Empty,
                        Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId)?.FullName ?? string.Empty,
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
                    StaffName = $"{staff.FirstName} {staff.LastName}",
                    Location = _context.LocationMasters.FirstOrDefault(loc => loc.Id == staff.LocationMasterId && loc.IsActive)?.FullName ?? string.Empty,
                    Designation = _context.DesignationMasters.FirstOrDefault(des => des.Id == staff.DesignationId && des.IsActive)?.FullName ?? string.Empty,
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
                                        StaffName = $"{staff.FirstName} {staff.LastName}",
                                        Location = loc.FullName,
                                        Designation = des.FullName,
                                        JoiningDate = staff.JoiningDate.ToString("MMMM dd")
                                    })
                                    .ToListAsync();

            if (newJoinees.Count == 0) throw new MessageNotFoundException("No New Joiners recently!");
            return newJoinees;
        }

        public async Task<List<object>> GetAllHolidaysAsync()
        {
            var holiday = await _context.HolidayCalendarTransactions
                .Where(h => h.IsActive)
                .Select(h => new
                {
                    Id = h.Id,
                    HolidayName = h.HolidayMaster.HolidayName,
                    FromDate = h.FromDate,
                    ToDate = h.ToDate
                })
                .ToListAsync<object>();
            if (holiday == null)
            {
                throw new MessageNotFoundException("No holidays found");
            }
            return holiday;
        }

        public async Task<List<object>> GetLeaveDetailsWithDefaultsAsync(int StaffCreationId)
        {
            var allLeaveTypes = await _context.LeaveTypes
                .Where(lt => lt.IsActive)
                .Select(lt => new { lt.Id, lt.Name })
                .ToListAsync();
            var userLeaveRecords = await _context.IndividualLeaveCreditDebits
                .Where(l => l.StaffCreationId == StaffCreationId && l.IsActive)
                .GroupBy(l => l.LeaveTypeId)
                .Select(g => new
                {
                    LeaveTypeId = g.Key,
                    AvailableBalance = g.OrderByDescending(l => l.UpdatedUtc).First().AvailableBalance
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
            var result = await (from sc in _context.StaffCreations
                                join dm in _context.DepartmentMasters on sc.DepartmentId equals dm.Id
                                group sc by new { dm.Id, dm.FullName } into g
                                select new
                                {
                                    DepartmentName = g.Key.FullName,
                                    HeadCount = g.Count()
                                }).ToListAsync();

            return result.Cast<object>().ToList();
        }

        public async Task<List<object>> GetUpcomingShiftsForStaffAsync(int staffId)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow); 
            var currentShift = await _context.AssignShifts
                .Where(asg => asg.StaffId == staffId && asg.IsActive &&
                              asg.FromDate <= today && asg.ToDate >= today)
                .OrderBy(asg => asg.FromDate)
                .Select(asg => new { asg.ToDate })
                .FirstOrDefaultAsync();

            if (currentShift == null)
            {
                return new List<object>(); 
            }

            DateOnly currentShiftEndDate = currentShift.ToDate;

            var upcomingShifts = await _context.AssignShifts
                .Where(asg => asg.IsActive &&
                              asg.FromDate > currentShiftEndDate &&  
                              (asg.StaffId == staffId || asg.CreatedBy == staffId)) 
                .OrderBy(asg => asg.FromDate)
                .Select(asg => new
                {
                    asg.Shift.Id,
                    asg.Shift.ShiftName,
                    asg.Shift.StartTime,
                    asg.Shift.EndTime,
                    asg.FromDate,
                    asg.ToDate
                })
                .ToListAsync();

            return upcomingShifts.Cast<object>().ToList();
        }
    }
}