using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AttendanceManagement.Services
{
    public class ToolsService
    {
        private readonly AttendanceManagementSystemContext _context;

        public ToolsService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<List<StaffInfoDto>> GetStaffInfoByOrganizationTypeAsync(int organizationTypeId)
        {
            var staffInfo = await _context.StaffCreations
                .Where(s => s.OrganizationTypeId == organizationTypeId)
                .Include(s => s.Department)
                .Select(s => new StaffInfoDto
                {
                    StaffId = s.Id,
                    StaffName = $"{s.FirstName} {s.LastName}",
                    DepartmentName = s.Department.Name
                })
                .ToListAsync();

            return staffInfo;
        }
        public async Task<List<StaffLeaveDto>> GetStaffInfoByStaffId(List<int> staffIds)
        {
            var organizationTypeIds = await _context.StaffCreations
                .Where(staff => staffIds.Contains(staff.Id))
                .Select(staff => staff.OrganizationTypeId)
                .Distinct()
                .ToListAsync();

            if (organizationTypeIds.Count > 1)
            {
                throw new Exception("The given employees belong to different organizations.");
            }

            var staffInfo = await (
                from staff in _context.StaffCreations
                join assignLeaveGroup in _context.AssignLeaveTypes
                    on staff.OrganizationTypeId equals assignLeaveGroup.OrganizationTypeId into assignLeaveLeftJoin
                from assignLeave in assignLeaveLeftJoin.DefaultIfEmpty()
                join leaveType in _context.LeaveTypes
                    on assignLeave.LeaveTypeId equals leaveType.Id into leaveTypeLeftJoin
                from leave in leaveTypeLeftJoin.DefaultIfEmpty()
                join org in _context.OrganizationTypes on staff.OrganizationTypeId equals org.Id
                where staffIds.Contains(staff.Id)
                let latestCredit = (
                    from credit in _context.IndividualLeaveCreditDebits
                    where assignLeave != null
                        && credit.LeaveTypeId == assignLeave.LeaveTypeId
                        && credit.StaffCreationId == staff.Id
                    orderby credit.Id descending
                    select (decimal?)credit.AvailableBalance
                ).FirstOrDefault()
                select new
                {
                    StaffId = staff.Id,
                    StaffName = $"{staff.FirstName} {staff.LastName}",
                    OrganizationTypeId = staff.OrganizationTypeId,
                    LeaveTypeId = leave != null ? leave.Id : (int?)null,
                    LeaveTypeName = leave != null ? leave.Name : null,
                    AvailableBalance = latestCredit ?? 0
                }
            ).ToListAsync();
            if (staffInfo.Count == 0)
                throw new MessageNotFoundException("Staff Information not found.");

#pragma warning disable CS8629 // Nullable value type may be null.
            var result = staffInfo
                .GroupBy(s => new { s.StaffId, s.StaffName, s.OrganizationTypeId })
                .Select(g => new StaffLeaveDto
                {
                    StaffId = g.Key.StaffId,
                    StaffName = g.Key.StaffName,
                    OrganizationTypeId = g.Key.OrganizationTypeId,
                    LeaveDetails = g
                        .Where(l => l.LeaveTypeId.HasValue)
                        .Select(l => new LeaveDetailDto
                        {
                            LeaveTypeId = l.LeaveTypeId.Value,
                            LeaveTypeName = l.LeaveTypeName,
                            AvailableBalance = l.AvailableBalance
                        })
                        .ToList()
                })
                .ToList();
            return result;
        }
        public async Task<List<AssignLeaveTypeDTO>> GetAllAssignLeaveTypes()
        {
            return await _context.AssignLeaveTypes
                .Join(_context.LeaveTypes,
                    assign => assign.LeaveTypeId,
                    leave => leave.Id,
                    (assign, leave) => new AssignLeaveTypeDTO
                    {
                        Id = assign.Id,
                        LeaveTypeId = assign.LeaveTypeId,
                        LeaveTypeName = leave.Name,
                        OrganizationTypeId = assign.OrganizationTypeId
                    })
                .ToListAsync();
        }

        public async Task<string> CreateAssignLeaveType(CreateAssignLeaveTypeDTO dto)
        {
            var newAssignLeaveType = new AssignLeaveType
            {
                LeaveTypeId = dto.LeaveTypeId,
                OrganizationTypeId = dto.OrganizationTypeId,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AssignLeaveTypes.Add(newAssignLeaveType);
            await _context.SaveChangesAsync();

            return "Assign LeaveType Created Successfully";
        }

        public async Task<string> UpdateAssignLeaveType(UpdateAssignLeaveTypeDTO dto)
        {
            var assignLeaveType = await _context.AssignLeaveTypes.FindAsync(dto.Id);
            if (assignLeaveType == null)
                throw new MessageNotFoundException("AssignLeaveType not found.");

            assignLeaveType.LeaveTypeId = dto.LeaveTypeId;
            assignLeaveType.OrganizationTypeId = dto.OrganizationTypeId;
            assignLeaveType.IsActive = true;
            assignLeaveType.UpdatedBy = dto.UpdatedBy;
            assignLeaveType.UpdatedUtc = DateTime.UtcNow;

            _context.AssignLeaveTypes.Update(assignLeaveType);
            await _context.SaveChangesAsync();

            return "Assign LeaveType Updated Successfully";
        }
        public async Task<string> AddLeaveCreditDebitForMultipleStaffAsync(LeaveCreditDebitRequest leaveCreditDebitRequest)
        {
            var message = "";
            var selectedRows = leaveCreditDebitRequest.SelectedRows;

            foreach (var item in selectedRows)
            {
                var leaveCreditDebit = new IndividualLeaveCreditDebit();

                var lastRecord = await _context.IndividualLeaveCreditDebits
                    .Where(r => r.StaffCreationId == item.StaffId &&
                                r.LeaveTypeId == leaveCreditDebitRequest.LeaveTypeId && r.IsActive)
                    .OrderByDescending(r => r.Id)
                    .FirstOrDefaultAsync();

                decimal lastActualBalance = lastRecord?.ActualBalance ?? 0;
                decimal lastAvailableBalance = lastRecord?.AvailableBalance ?? 0;

                var staff = await _context.StaffCreations
                    .Where(s => s.Id == item.StaffId && s.IsActive == true)
                    .FirstOrDefaultAsync();

                var leaveTypeExists = await _context.LeaveTypes
                    .AnyAsync(lt => lt.Id == leaveCreditDebitRequest.LeaveTypeId && lt.IsActive);

                if (staff == null || !leaveTypeExists)
                {
                    continue;
                }

                if (leaveCreditDebitRequest.TransactionFlag)
                {
                    leaveCreditDebit.ActualBalance = lastActualBalance + leaveCreditDebitRequest.LeaveCount;
                    leaveCreditDebit.AvailableBalance = lastAvailableBalance + leaveCreditDebitRequest.LeaveCount;
                    message = "Leave credited successfully for selected staff.";
                }
                else
                {
                    leaveCreditDebit.ActualBalance = lastActualBalance;
                    if (lastAvailableBalance >= leaveCreditDebitRequest.LeaveCount)
                    {
                        leaveCreditDebit.AvailableBalance = lastAvailableBalance - leaveCreditDebitRequest.LeaveCount;
                        message = "Leave debited successfully for selected staff.";
                    }
                    else
                    {
                        continue;
                    }
                }

                leaveCreditDebit.LeaveTypeId = leaveCreditDebitRequest.LeaveTypeId;
                leaveCreditDebit.StaffCreationId = item.StaffId;
                leaveCreditDebit.TransactionFlag = leaveCreditDebitRequest.TransactionFlag;
                leaveCreditDebit.LeaveReason = leaveCreditDebitRequest.LeaveReason;
                leaveCreditDebit.Month = leaveCreditDebitRequest.Month;
                leaveCreditDebit.Year = leaveCreditDebitRequest.Year;
                leaveCreditDebit.LeaveCount = leaveCreditDebitRequest.LeaveCount;
                leaveCreditDebit.Remarks = leaveCreditDebitRequest.Remarks;
                leaveCreditDebit.CreatedBy = leaveCreditDebitRequest.CreatedBy;
                leaveCreditDebit.CreatedUtc = DateTime.UtcNow;
                leaveCreditDebit.IsActive = true;
                await _context.IndividualLeaveCreditDebits.AddAsync(leaveCreditDebit);
            }
            await _context.SaveChangesAsync();
            return message;
        }
        //public async Task<string> AddLeaveCreditDebitAsync(IndividualLeaveRequest leaveCreditDebitRequest)
        //{
        //    try
        //    {
        //        var message = "";
        //        var leaveCreditDebit = new IndividualLeaveCreditDebit();
        //        // Fetch the last record for the specified StaffCreationId and LeaveTypeId
        //        var lastRecord = await _context.IndividualLeaveCreditDebits
        //            .Where(r => r.StaffCreationId == leaveCreditDebitRequest.StaffCreationId &&
        //                        r.LeaveTypeId == leaveCreditDebitRequest.LeaveTypeId && r.IsActive)
        //            .OrderByDescending(r => r.Id)
        //            .FirstOrDefaultAsync();

        //        decimal lastActualBalance = lastRecord?.ActualBalance ?? 0;
        //        decimal lastAvailableBalance = lastRecord?.AvailableBalance ?? 0;

        //        // Check if the StaffCreation and LeaveType are active (assuming IsActive exists)
        //        var staffCreationActive = await _context.StaffCreations
        //            .Where(sc => sc.Id == leaveCreditDebitRequest.StaffCreationId && sc.IsActive==true)
        //            .AnyAsync();

        //        var leaveTypeActive = await _context.LeaveTypes
        //            .Where(lt => lt.Id == leaveCreditDebitRequest.LeaveTypeId && lt.IsActive)
        //            .AnyAsync();

        //        if (!staffCreationActive || !leaveTypeActive)
        //        {
        //            throw new MessageNotFoundException("Staff or Leave type does not exist");
        //        }

        //        // If TransactionFlag is true, it's a credit (leave addition)
        //        if (leaveCreditDebitRequest.TransactionFlag)
        //        {
        //            leaveCreditDebit.ActualBalance = lastActualBalance + leaveCreditDebitRequest.LeaveCount;
        //            leaveCreditDebit.AvailableBalance = lastAvailableBalance + leaveCreditDebitRequest.LeaveCount;
        //            message = "Leave credited successfully";
        //        }
        //        else
        //        {
        //            // If it's a debit (leave deduction), update the available balance
        //            leaveCreditDebit.ActualBalance = lastActualBalance;
        //            if (lastAvailableBalance >= leaveCreditDebitRequest.LeaveCount)
        //            {
        //                leaveCreditDebit.AvailableBalance = lastAvailableBalance - leaveCreditDebitRequest.LeaveCount;
        //                message = "Leave debited successfully";
        //            }
        //            else
        //            {
        //                throw new MessageNotFoundException("No available balance");
        //            }
        //        }

        //        // Set IsActive to true for the new credit/debit record
        //        leaveCreditDebit.LeaveTypeId = leaveCreditDebitRequest.LeaveTypeId;
        //        leaveCreditDebit.StaffCreationId = leaveCreditDebitRequest.StaffCreationId;
        //        leaveCreditDebit.TransactionFlag = leaveCreditDebitRequest.TransactionFlag;
        //        leaveCreditDebit.LeaveReason = leaveCreditDebitRequest.LeaveReason;
        //        leaveCreditDebit.Month = leaveCreditDebitRequest.Month;
        //        leaveCreditDebit.Year = leaveCreditDebitRequest.Year;
        //        leaveCreditDebit.LeaveCount = leaveCreditDebitRequest.LeaveCount;
        //        leaveCreditDebit.Remarks = leaveCreditDebitRequest.Remarks;
        //        leaveCreditDebit.CreatedBy = leaveCreditDebitRequest.CreatedBy;
        //        leaveCreditDebit.CreatedUtc = DateTime.UtcNow;
        //        leaveCreditDebit.IsActive = leaveCreditDebitRequest.Isactive;

        //        // Add the record to the database and save changes
        //        await _context.IndividualLeaveCreditDebits.AddAsync(leaveCreditDebit);
        //        await _context.SaveChangesAsync();

        //        return message;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<string> AddReaderConfigurationAsync(ReaderConfigurationRequest request)
        {
            var readerConfiguration = new ReaderConfiguration
            {
                ReaderName = request.ReaderName,
                ReaderIpAddress = request.ReaderIpAddress,
                IsAttendanceReader = request.IsAttendanceReader,
                IsAccessReader = request.IsAccessReader,
                IsActive = request.IsActive,
                CreatedBy = request.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                ReaderTypeId = request.ReaderTypeId
            };

            await _context.ReaderConfigurations.AddAsync(readerConfiguration);
            await _context.SaveChangesAsync();

            return "Reader Configuration added successfully";
        }
        public async Task<List<ReaderConfigurationResponse>> GetReaderConfigurationsAsync()
        {
            var result = await _context.ReaderConfigurations
                .Include(r => r.ReaderType) // Join with ReaderType table
                .Select(r => new ReaderConfigurationResponse
                {
                    Id = r.Id,
                    ReaderName = r.ReaderName,
                    ReaderIpAddress = r.ReaderIpAddress,
                    IsAttendanceReader = r.IsAttendanceReader,
                    IsAccessReader = r.IsAccessReader,
                    IsActive = r.IsActive,
                    CreatedBy = r.CreatedBy,
                    CreatedUtc = r.CreatedUtc,
                    ReaderTypeId = r.ReaderTypeId,
                    ReaderTypeName = r.ReaderType.Name 
                })
                .ToListAsync();

            return result;
        }
        public async Task<string> UpdateAttendanceStatusAsync(UpdateAttendanceStatusRequest request)
        {
            if (request.StaffIds == null || !request.StaffIds.Any())
            {
                return "StaffIds cannot be empty.";
            }

            var newRecords = new List<AttendanceStatus>();

            foreach (var staffId in request.StaffIds)
            {
                var attendanceRecords = await _context.AttendanceRecords
            .Where(a => a.StaffId == staffId && a.FirstIn.HasValue && a.FirstIn.Value.Date == request.FromDate.Date)
            .ToListAsync();

                if (attendanceRecords.Any())
                {
                    foreach (var record in attendanceRecords)
                    {
                        record.StatusId = request.StatusId; // Update status
                        record.IsRegularized = true; // Set as regularized
                        record.UpdatedBy = request.CreatedBy;
                        record.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                // Add new attendance status record
                newRecords.Add(new AttendanceStatus
                {
                    StaffId = staffId,
                    StatusId = request.StatusId,
                    FromDate = request.FromDate,
                    DurationId = request.DurationId,
                    ToDate = request.ToDate,
                    Remarks = request.Remarks,
                    IsActive = true,
                    CreatedBy = request.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                });
            }

            // Update attendance records
            await _context.SaveChangesAsync();

            // Insert new attendance status records
            await _context.AttendanceStatuses.AddRangeAsync(newRecords);
            await _context.SaveChangesAsync();

            return "Attendance status updated successfully.";
        }



        public async Task<string> CreateAttendanceStatusColorAsync(AttendanceStatusColorDto dto)
        {
            var message = "Attendance status created successfully.";
            var attendanceStatus = new AttendanceStatusColor
            {
                StatusName = dto.StatusName,
                ShortName = dto.ShortName,
                ColourCode = dto.ColourCode,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AttendanceStatusColors.Add(attendanceStatus);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<AttendanceStatusColorResponse>> GetAttendanceStatusColor()
        {
            var response = await _context.AttendanceStatusColors
                .Where(color => color.IsActive)
                .Select(color => new AttendanceStatusColorResponse
                {
                    Id = color.Id,
                    StatusName = color.StatusName,
                    ShortName = color.ShortName,
                    ColourCode = color.ColourCode
                })
                .ToListAsync();

            if (response.Count == 0)
            {
                throw new MessageNotFoundException("No attendance status color found");
            }

            return response;
        }

        public async Task<string> UpdateAttendanceStatusColor(UpdateAttendanceStatusColor updateAttendanceStatusColor)
        {
            var message = "Attendance status Updated successfully.";
            var existingStatusColor = await _context.AttendanceStatusColors.FirstOrDefaultAsync(a => a.Id == updateAttendanceStatusColor.Id);
            if(existingStatusColor == null)
            {
                throw new MessageNotFoundException("No attendance status color found");
            }
            existingStatusColor.StatusName = updateAttendanceStatusColor.StatusName;
            existingStatusColor.ShortName = updateAttendanceStatusColor.ShortName;
            existingStatusColor.ColourCode = updateAttendanceStatusColor.ColourCode;
            existingStatusColor.IsActive = updateAttendanceStatusColor.IsActive;
            existingStatusColor.UpdatedBy = updateAttendanceStatusColor.UpdatedBy;
            existingStatusColor.UpdatedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return message;
        }
    }

}

