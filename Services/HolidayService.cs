using AttendanceManagement.DTOs;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class HolidayService
    {
        private readonly AttendanceManagementSystemContext _context;

        public HolidayService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<string> CreateHoliday(HolidayRequest holidayRequest)
        {
            var message = "Holiday added successfully";

            var holiday = new HolidayMaster
            {
                HolidayName = holidayRequest.HolidayName,
                HolidayTypeId = holidayRequest.HolidayTypeId,
                CreatedBy = holidayRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = holidayRequest.IsActive
            };
            _context.HolidayMasters.Add(holiday);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<HolidyTypeRequest>> GetAllHolidayType()
        {
            var holidayType = await (from holiday in _context.HolidayTypes
                                     select new HolidyTypeRequest
                                     {
                                         Id = holiday.Id,
                                         HolidayTypeName = holiday.HolidayName
                                     }).ToListAsync();
            if (holidayType.Count == 0) throw new MessageNotFoundException("No holiday type found");
            return holidayType;
        }

        public async Task<HolidayResponse> GetHolidayById(int holidayMasterId)
        {
            var allHoliday = await (from holiday in _context.HolidayMasters
                              join holidayType in _context.HolidayTypes
                              on holiday.HolidayTypeId equals holidayType.Id
                              where holiday.Id == holidayMasterId
                              select new HolidayResponse
                              {
                                  HolidayMasterId = holiday.Id,
                                  HolidayName = holiday.HolidayName,
                                  HolidayTypeId = holidayType.Id,
                                  HolidayTypeName = holidayType.HolidayName,
                                  IsActive = holiday.IsActive,
                                  CreatedBy = holiday.CreatedBy
                              })
                              .FirstOrDefaultAsync();
            if (allHoliday == null)
            {
                throw new MessageNotFoundException("Holiday not found");
            }
            return allHoliday;
        }

        public async Task<IEnumerable<HolidayResponse>> GetAllHolidaysAsync()
        {
            var allHoliday = await (from holiday in _context.HolidayMasters
                              join holidayType in _context.HolidayTypes
                              on holiday.HolidayTypeId equals holidayType.Id
                              select new HolidayResponse
                              {
                                  HolidayMasterId = holiday.Id,
                                  HolidayName = holiday.HolidayName,
                                  HolidayTypeId = holidayType.Id,
                                  HolidayTypeName = holidayType.HolidayName,
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

            var existingHoliday = _context.HolidayMasters.FirstOrDefault(h => h.Id == updatedHoliday.HolidayMasterId);
            if (existingHoliday == null)
                throw new MessageNotFoundException("Holiday not found");

            existingHoliday.HolidayName = updatedHoliday.HolidayName;
            existingHoliday.HolidayTypeId = updatedHoliday.HolidayTypeId;
            existingHoliday.IsActive = updatedHoliday.IsActive;
            existingHoliday.UpdatedBy = updatedHoliday.UpdatedBy;
            existingHoliday.UpdatedUtc = DateTime.UtcNow;

            _context.Update(existingHoliday);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<string> CreateHolidayCalendar(HolidayCalendarRequestDto request)
        {
            var message = "Holiday calendar credated successfully";
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var holidayCalendar = new HolidayCalendarConfiguration
            {
                GroupName = request.GroupName,
                CalendarYear = request.CalendarYear,
                Currents = request.Currents,
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
            _context.HolidayCalendarConfigurations.Add(holidayCalendar);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<HolidayConfigurationResponse>> GetAllHolidayCalendarsAsync()
        {
            var allHolidays = await (from holiday in _context.HolidayCalendarConfigurations
                                     select new
                                     {
                                         holiday.Id,
                                         holiday.GroupName,
                                         holiday.CalendarYear,
                                         holiday.Currents,
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
                GroupName = holiday.GroupName,
                CalendarYear = holiday.CalendarYear,
                Currents = holiday.Currents,
                IsActive = holiday.IsActive,
                CreatedBy = holiday.CreatedBy,
                Transactions = (from holidayTran in _context.HolidayCalendarTransactions
                                join holidayMaster in _context.HolidayMasters
                                on holidayTran.HolidayMasterId equals holidayMaster.Id
                                where holidayTran.HolidayCalendarId == holiday.Id && holidayTran.IsActive
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

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Fetch existing holiday calendar
            var existingCalendar = await _context.HolidayCalendarConfigurations
                .Include(h => h.HolidayCalendarTransactions)
                .FirstOrDefaultAsync(h => h.Id == request.Id);

            if (existingCalendar == null)
                throw new MessageNotFoundException("Holiday calendar not found.");

            // Update the HolidayCalendar properties
            existingCalendar.GroupName = request.GroupName ?? existingCalendar.GroupName;
            existingCalendar.CalendarYear = request.CalendarYear;
            existingCalendar.Currents = request.Currents;
            existingCalendar.IsActive = request.IsActive;
            existingCalendar.UpdatedBy = request.UpdatedBy;
            existingCalendar.UpdatedUtc = DateTime.UtcNow;

            if (request.Transactions != null)
            {
                var existingTransactions = existingCalendar.HolidayCalendarTransactions.ToList();

                foreach (var requestTransaction in request.Transactions)
                {
                    // Try to find existing transaction for this holiday master ID
                    var existingTransaction = existingTransactions
                        .FirstOrDefault(t => t.HolidayMasterId == requestTransaction.HolidayMasterId);

                    if (existingTransaction != null)
                    {
                        // Update existing transaction
                        existingTransaction.FromDate = requestTransaction.FromDate;
                        existingTransaction.ToDate = requestTransaction.ToDate;
                        existingTransaction.IsActive = true;
                        existingTransaction.UpdatedBy = request.UpdatedBy;
                        existingTransaction.UpdatedUtc = DateTime.UtcNow;
                    }
                    else
                    {
                        // Create new transaction if it doesn't exist
                        var newTransaction = new HolidayCalendarTransaction
                        {
                            HolidayMasterId = requestTransaction.HolidayMasterId,
                            FromDate = requestTransaction.FromDate,
                            ToDate = requestTransaction.ToDate,
                            IsActive = true,
                            CreatedBy = request.UpdatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        existingCalendar.HolidayCalendarTransactions.Add(newTransaction);
                    }
                }

                // Deactivate transactions that are not in the request
                var transactionsToDeactivate = existingTransactions
                    .Where(t => t.IsActive && !request.Transactions
                        .Any(rt => rt.HolidayMasterId == t.HolidayMasterId))
                    .ToList();

                foreach (var transaction in transactionsToDeactivate)
                {
                    transaction.IsActive = false;
                    transaction.UpdatedBy = request.UpdatedBy;
                    transaction.UpdatedUtc = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
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
                                        HolidayCalendarName = holidayCalendar.GroupName,
                                        IsActive = holiday.IsActive,
                                        CreatedBy = holiday.CreatedBy
                                    })
                              .ToListAsync();
            if (allHoliday.Count == 0)
            {
                throw new MessageNotFoundException("No holiday zone found");
            }
            return allHoliday;
        }

        public async Task<HolidayZoneResponse> GetHolidayZoneByIdAsync(int holidayZoneId)
        {
            var allHoliday = await (from holiday in _context.HolidayZoneConfigurations
                                    join holidayCalendar in _context.HolidayCalendarConfigurations
                                    on holiday.HolidayCalendarId equals holidayCalendar.Id
                                    where holiday.Id == holidayZoneId
                                    select new HolidayZoneResponse
                                    {
                                        HolidayZoneId = holiday.Id,
                                        HolidayZoneName = holiday.HolidayZoneName,
                                        HolidayCalanderId = holidayCalendar.Id,
                                        HolidayCalendarName = holidayCalendar.GroupName,
                                        IsActive = holiday.IsActive,
                                        CreatedBy = holiday.CreatedBy
                                    })
                              .FirstOrDefaultAsync();
            if (allHoliday == null)
            {
                throw new MessageNotFoundException("Holiday zone not found");
            }
            return allHoliday;
        }

        public async Task<string> CreateHolidayZoneAsync(HolidayZoneRequest holidayZoneRequest)
        {
            var message = "Holiday zone added successfully";

            var holidayZone = new HolidayZoneConfiguration
            {
                HolidayZoneName = holidayZoneRequest.HolidayZoneName,
                HolidayCalendarId = holidayZoneRequest.HolidayCalendarId,
                IsActive = holidayZoneRequest.IsActive,
                CreatedBy = holidayZoneRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.HolidayZoneConfigurations.Add(holidayZone);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<string> UpdateHolidayZoneAsync(UpdateHolidayZone holidayZone)
        {
            var message = "Holiday zone updated successfully";
            var existingHolidayZone = await _context.HolidayZoneConfigurations
                .FirstOrDefaultAsync(h => h.Id == holidayZone.HolidayZoneId);

            if (existingHolidayZone == null)
                throw new MessageNotFoundException("Holiday zone not found");

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