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
                .Where(s => s.OrganizationTypeId == organizationTypeId && s.IsActive == true)
                .Select(s => new StaffInfoDto
                {
                    StaffId = s.Id,
                    StaffName = $"{s.FirstName} {s.LastName}",
                    DepartmentName = s.Department.Name
                })
                .ToListAsync();
            if (staffInfo.Count == 0) throw new MessageNotFoundException("No staffs found");
            return staffInfo;
        }

        public async Task<List<StaffLeaveDto>> GetStaffInfoByStaffId(List<int> staffIds)
        {
            var organizationTypeIds = await _context.StaffCreations
                .Where(staff => staffIds.Contains(staff.Id) && staff.IsActive == true)
                .Select(staff => staff.OrganizationTypeId)
                .Distinct()
                .ToListAsync();
            if (organizationTypeIds.Count > 1)
            {
                throw new InvalidOperationException("The given employees belong to different organizations.");
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
            if (staffInfo.Count == 0) throw new MessageNotFoundException("Staff Information not found.");

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
            var result = await _context.AssignLeaveTypes
                .Where(l => l.IsActive)
                .Join(_context.LeaveTypes.Where(leave => leave.IsActive),
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
            if (result.Count == 0) throw new MessageNotFoundException("No assigned leave types found");
            return result;
        }

        public async Task<string> CreateAssignLeaveType(CreateAssignLeaveTypeDTO dto)
        {
            await LeaveNotFoundMethod(dto.LeaveTypeId, dto.OrganizationTypeId);
            var newAssignLeaveType = new AssignLeaveType
            {
                LeaveTypeId = dto.LeaveTypeId,
                OrganizationTypeId = dto.OrganizationTypeId,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.AssignLeaveTypes.AddAsync(newAssignLeaveType);
            await _context.SaveChangesAsync();

            return "LeaveType assigned successfully";
        }

        private async Task LeaveNotFoundMethod(int leaveTypeId, int organizationTypeId)
        {
            var leave = await _context.LeaveTypes.AnyAsync(p => p.Id == leaveTypeId && p.IsActive == true);
            if (!leave) throw new MessageNotFoundException("Leave type not found");
            var org = await _context.OrganizationTypes.AnyAsync(p => p.Id == organizationTypeId && p.IsActive == true);
            if (!org) throw new MessageNotFoundException("Organization type not found");
        }
        public async Task<string> UpdateAssignLeaveType(UpdateAssignLeaveTypeDTO dto)
        {
            var assignLeaveType = await _context.AssignLeaveTypes.FindAsync(dto.Id);
            if (assignLeaveType == null) throw new MessageNotFoundException("Assigned leave type not found");
            await LeaveNotFoundMethod(dto.LeaveTypeId, dto.OrganizationTypeId);
            assignLeaveType.LeaveTypeId = dto.LeaveTypeId;
            assignLeaveType.OrganizationTypeId = dto.OrganizationTypeId;
            assignLeaveType.IsActive = true;
            assignLeaveType.UpdatedBy = dto.UpdatedBy;
            assignLeaveType.UpdatedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return "Assigned leave type updated successfully";
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
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == item.StaffId && s.IsActive == true);
                var leaveTypeExists = await _context.LeaveTypes.AnyAsync(lt => lt.Id == leaveCreditDebitRequest.LeaveTypeId && lt.IsActive);
                if (staff == null || !leaveTypeExists)
                {
                    continue;
                }
                if (lastRecord != null)
                {
                    lastRecord.IsActive = false;
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

        public async Task<string> AddReaderConfigurationAsync(ReaderConfigurationRequest request)
        {
            var leave = await _context.ReaderTypes.AnyAsync(p => p.Id == request.ReaderTypeId && p.IsActive == true);
            if (!leave) throw new MessageNotFoundException("Reader type not found");
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
                .Include(r => r.ReaderType)
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
            var hasUnfreezed = await _context.AttendanceRecords.AnyAsync(f => f.IsFreezed == null || f.IsFreezed == false);
            if (!hasUnfreezed) throw new InvalidOperationException("Attendance records are frozen. Attendance regularization is not allowed.");
            var newRecords = new List<AttendanceStatus>();
            foreach (var staffId in request.StaffIds)
            {
                var attendanceRecords = await _context.AttendanceRecords
                    .Where(a => a.StaffId == staffId && (a.AttendanceDate >= request.FromDate && a.AttendanceDate <= request.ToDate))
                    .ToListAsync();
                if (attendanceRecords.Any())
                {
                    foreach (var record in attendanceRecords)
                    {
                        record.StatusId = request.StatusId;
                        record.IsRegularized = true;
                        record.UpdatedBy = request.CreatedBy;
                        record.UpdatedUtc = DateTime.UtcNow;
                    }
                }
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
            await _context.SaveChangesAsync();
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
            await _context.AttendanceStatusColors.AddAsync(attendanceStatus);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<AttendanceStatusColorResponse>> GetAttendanceStatusColor()
        {
            var response = await _context.AttendanceStatusColors
                .Select(color => new AttendanceStatusColorResponse
                {
                    Id = color.Id,
                    StatusName = color.StatusName,
                    ShortName = color.ShortName,
                    ColourCode = color.ColourCode,
                    IsActive = color.IsActive
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