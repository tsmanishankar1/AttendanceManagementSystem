using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class HolidayInfra : IHolidayInfra
    {
        private readonly AttendanceManagementSystemContext _context;

        public HolidayInfra(AttendanceManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<string> CreateHoliday(HolidayRequest holidayRequest)
        {
            var message = "Holiday added successfully";
            var duplicateHoliday = await _context.HolidayMasters
                .AnyAsync(h => h.Name.ToLower() == holidayRequest.HolidayName.ToLower()
                               && h.HolidayTypeId == holidayRequest.HolidayTypeId
                               && h.IsActive);

            if (duplicateHoliday)
                throw new ConflictException("Holiday name already exists for this holiday type");

            var holidayType = await _context.HolidayTypes.AnyAsync(h => h.Id == holidayRequest.HolidayTypeId && h.IsActive);
            if (!holidayType) throw new MessageNotFoundException("Holiday type not found");
            var holiday = new HolidayMaster
            {
                Name = holidayRequest.HolidayName,
                HolidayTypeId = holidayRequest.HolidayTypeId,
                CreatedBy = holidayRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = holidayRequest.IsActive
            };
            await _context.HolidayMasters.AddAsync(holiday);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<HolidyTypeRequest>> GetAllHolidayType()
        {
            var holidayType = await (from holiday in _context.HolidayTypes
                                     select new HolidyTypeRequest
                                     {
                                         Id = holiday.Id,
                                         HolidayTypeName = holiday.Name
                                     }).ToListAsync();
            if (holidayType.Count == 0) throw new MessageNotFoundException("No holiday type found");
            return holidayType;
        }

        public async Task<List<HolidayResponse>> GetAllHolidaysAsync()
        {
            var allHoliday = await (from holiday in _context.HolidayMasters
                              join holidayType in _context.HolidayTypes
                              on holiday.HolidayTypeId equals holidayType.Id
                              select new HolidayResponse
                              {
                                  HolidayMasterId = holiday.Id,
                                  HolidayName = holiday.Name,
                                  HolidayTypeId = holidayType.Id,
                                  HolidayTypeName = holidayType.Name,
                                  IsActive = holiday.IsActive,
                                  CreatedBy = holiday.CreatedBy
                              })
                              .ToListAsync();
            if (allHoliday.Count == 0)
            {
                throw new MessageNotFoundException("No holidays found");
            }
            return allHoliday;
        }

        public async Task<string> UpdateHoliday(UpdateHoliday updatedHoliday)
        {
            var message = "Holiday updated successfully";
            var duplicateHoliday = await _context.HolidayMasters
                .AnyAsync(h => h.Name.ToLower() == updatedHoliday.HolidayName.ToLower()
                               && h.HolidayTypeId == updatedHoliday.HolidayTypeId
                               && h.IsActive && h.Id != updatedHoliday.HolidayMasterId);

            if (duplicateHoliday)
                throw new ConflictException("Holiday name already exists for this holiday type");

            var holidayType = await _context.HolidayTypes.AnyAsync(h => h.Id == updatedHoliday.HolidayTypeId && h.IsActive);
            if (!holidayType) throw new MessageNotFoundException("Holiday type not found");
            var existingHoliday = await _context.HolidayMasters.FirstOrDefaultAsync(h => h.Id == updatedHoliday.HolidayMasterId);
            if (existingHoliday == null) throw new MessageNotFoundException("Holiday not found");

            existingHoliday.Name = updatedHoliday.HolidayName;
            existingHoliday.HolidayTypeId = updatedHoliday.HolidayTypeId;
            existingHoliday.IsActive = updatedHoliday.IsActive;
            existingHoliday.UpdatedBy = updatedHoliday.UpdatedBy;
            existingHoliday.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> CreateHolidayCalendar(HolidayCalendarRequestDto request)
        {
            var message = "Holiday calendar credated successfully";
            if (string.IsNullOrWhiteSpace(request.GroupName))
                throw new ArgumentException("Group name is required");

            var duplicateHolidayCalendar = await _context.HolidayCalendarConfigurations
                .AnyAsync(h => h.IsActive &&
                               h.Name.ToLower().Trim() == request.GroupName.ToLower().Trim() &&
                               h.CalendarYear == request.CalendarYear);

            if (duplicateHolidayCalendar)
                throw new ConflictException("Holiday calendar already exists for this year");

            var holidayCalendar = new HolidayCalendarConfiguration
            {
                Name = request.GroupName,
                CalendarYear = request.CalendarYear,
                Currents = request.Currents,
                ShiftTypeId = request.ShiftTypeId,
                IsActive = request.IsActive,
                CreatedBy = request.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            if(request.Transactions != null)
            {
                foreach (var transactionDto in request.Transactions)
                {
                    var transaction = new HolidayCalendarTransaction
                    {
                        HolidayMasterId = transactionDto.HolidayMasterId,
                        FromDate = transactionDto.FromDate,
                        ToDate = transactionDto.ToDate,
                        IsActive = true,
                        CreatedBy = request.CreatedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    holidayCalendar.HolidayCalendarTransactions.Add(transaction);
                }
            }
            await _context.HolidayCalendarConfigurations.AddAsync(holidayCalendar);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<HolidayConfigurationResponse>> GetAllHolidayCalendarsAsync()
        {
            var allHolidays = await (from holiday in _context.HolidayCalendarConfigurations
                                     select new
                                     {
                                         holiday.Id,
                                         holiday.Name,
                                         holiday.CalendarYear,
                                         holiday.Currents,
                                         holiday.ShiftTypeId,
                                         holiday.IsActive,
                                         holiday.CreatedBy
                                     })
                                   .ToListAsync();
            if (allHolidays.Count == 0)
            {
                throw new MessageNotFoundException("No holiday calendar found");
            }
            var holidayResponses = allHolidays.Select(holiday => new HolidayConfigurationResponse
            {
                HolidayCalendarId = holiday.Id,
                GroupName = holiday.Name,
                CalendarYear = holiday.CalendarYear,
                Currents = holiday.Currents,
                ShiftTypeId = holiday.ShiftTypeId,
                IsActive = holiday.IsActive,
                CreatedBy = holiday.CreatedBy,
                Transactions = (from holidayTran in _context.HolidayCalendarTransactions
                                join holidayMaster in _context.HolidayMasters
                                on holidayTran.HolidayMasterId equals holidayMaster.Id
                                where holidayTran.HolidayCalendarId == holiday.Id
                                select new HolidayCalendarTransactionDto
                                {
                                    HolidayMasterId = holidayTran.HolidayMasterId,
                                    FromDate = holidayTran.FromDate,
                                    ToDate = holidayTran.ToDate
                                })
                                .ToList()
            }).ToList();
            return holidayResponses;
        }

        public async Task<string> UpdateHolidayCalendar(UpdateHolidayCalanderDto request)
        {
            var message = "Holiday calendar updated successfully";
            var existingCalendar = await _context.HolidayCalendarConfigurations
                .Include(h => h.HolidayCalendarTransactions)
                .FirstOrDefaultAsync(h => h.Id == request.Id);
            if (existingCalendar == null)
            {
                throw new MessageNotFoundException("Holiday calendar not found");
            }
            if (string.IsNullOrWhiteSpace(request.GroupName))
                throw new ArgumentException("Group name is required");

            var duplicateHolidayCalendar = await _context.HolidayCalendarConfigurations
                .AnyAsync(h => h.IsActive &&
                               h.Name.ToLower().Trim() == request.GroupName.ToLower().Trim() &&
                               h.CalendarYear == request.CalendarYear && h.Id != request.Id);

            if (duplicateHolidayCalendar)
                throw new ConflictException("Holiday calendar already exists for this year");

            existingCalendar.Name = request.GroupName;
            existingCalendar.CalendarYear = request.CalendarYear;
            existingCalendar.Currents = request.Currents;
            existingCalendar.ShiftTypeId = request.ShiftTypeId;
            existingCalendar.IsActive = request.IsActive;
            existingCalendar.UpdatedBy = request.UpdatedBy;
            existingCalendar.UpdatedUtc = DateTime.UtcNow;
            if (!request.IsActive)
            {
                foreach (var transaction in existingCalendar.HolidayCalendarTransactions.Where(t => t.IsActive))
                {
                    transaction.IsActive = false;
                    transaction.UpdatedBy = request.UpdatedBy;
                    transaction.UpdatedUtc = DateTime.UtcNow;
                }
            }
            await _context.SaveChangesAsync();

            if (request.IsActive && request.Transactions != null && request.Transactions.Any())
            {
                var existingTransactions = await _context.HolidayCalendarTransactions
                    .Where(hct => hct.HolidayCalendarId == request.Id && hct.IsActive)
                    .ToListAsync();
                foreach (var transaction in existingTransactions)
                {
                    transaction.IsActive = false;
                    transaction.UpdatedBy = request.UpdatedBy;
                    transaction.UpdatedUtc = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
                var newTransactions = request.Transactions.Select(t => new HolidayCalendarTransaction
                {
                    HolidayCalendarId = request.Id,
                    HolidayMasterId = t.HolidayMasterId,
                    FromDate = t.FromDate,
                    ToDate = t.ToDate,
                    IsActive = true,
                    CreatedBy = request.UpdatedBy,
                    CreatedUtc = DateTime.UtcNow
                }).ToList();
                await _context.HolidayCalendarTransactions.AddRangeAsync(newTransactions);
                await _context.SaveChangesAsync();
            }
            return message;
        }

        public async Task<List<HolidayZoneResponse>> GetAllHolidayZonesAsync()
        {
            var allHoliday = await (from holiday in _context.HolidayZoneConfigurations
                                    join holidayCalendar in _context.HolidayCalendarConfigurations
                                    on holiday.HolidayCalendarId equals holidayCalendar.Id
                                    select new HolidayZoneResponse
                                    {
                                        HolidayZoneId = holiday.Id,
                                        HolidayZoneName = holiday.HolidayZoneName,
                                        HolidayCalanderId = holidayCalendar.Id,
                                        HolidayCalendarName = holidayCalendar.Name,
                                        IsActive = holiday.IsActive,
                                        CreatedBy = holiday.CreatedBy
                                    }).AsNoTracking()
                              .ToListAsync();
            if (allHoliday.Count == 0)
            {
                throw new MessageNotFoundException("No holiday zone found");
            }
            return allHoliday;
        }

        public async Task<string> CreateHolidayZoneAsync(HolidayZoneRequest holidayZoneRequest)
        {
            var message = "Holiday zone added successfully";
            var holidayCalander = await _context.HolidayCalendarConfigurations.AnyAsync(h => h.Id == holidayZoneRequest.HolidayCalendarId && h.IsActive);
            if (!holidayCalander) throw new MessageNotFoundException("Holiday calander not found");
            var duplicateZone = await _context.HolidayZoneConfigurations
            .AnyAsync(z => z.HolidayCalendarId == holidayZoneRequest.HolidayCalendarId &&
                       z.HolidayZoneName.ToLower() == holidayZoneRequest.HolidayZoneName.ToLower() &&
                       z.IsActive);

            if (duplicateZone)
                throw new ConflictException("Holiday zone name already exists in this calendar");
            var holidayZone = new HolidayZoneConfiguration
            {
                HolidayZoneName = holidayZoneRequest.HolidayZoneName,
                HolidayCalendarId = holidayZoneRequest.HolidayCalendarId,
                IsActive = holidayZoneRequest.IsActive,
                CreatedBy = holidayZoneRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.HolidayZoneConfigurations.AddAsync(holidayZone);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateHolidayZoneAsync(UpdateHolidayZone holidayZone)
        {
            var message = "Holiday zone updated successfully";
            var holidayCalander = await _context.HolidayCalendarConfigurations.AnyAsync(h => h.Id == holidayZone.HolidayCalendarId && h.IsActive);
            if (!holidayCalander) throw new MessageNotFoundException("Holiday calander not found");
            var existingHolidayZone = await _context.HolidayZoneConfigurations.FirstOrDefaultAsync(h => h.Id == holidayZone.HolidayZoneId);
            if (existingHolidayZone == null) throw new MessageNotFoundException("Holiday zone not found");
            var duplicateZone = await _context.HolidayZoneConfigurations
                .AnyAsync(z => z.HolidayCalendarId == holidayZone.HolidayCalendarId &&
                               z.HolidayZoneName.ToLower() == holidayZone.HolidayZoneName.ToLower() &&
                               z.IsActive &&
                               z.Id != holidayZone.HolidayZoneId);

            if (duplicateZone)
                throw new ConflictException("Holiday zone name already exists in this calendar");
            existingHolidayZone.HolidayZoneName = holidayZone.HolidayZoneName;
            existingHolidayZone.HolidayCalendarId = holidayZone.HolidayCalendarId;
            existingHolidayZone.IsActive = holidayZone.IsActive;
            existingHolidayZone.UpdatedBy = holidayZone.UpdatedBy;
            existingHolidayZone.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }
    }
}