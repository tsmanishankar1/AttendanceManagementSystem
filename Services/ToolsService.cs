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
                    DepartmentName = s.Department.FullName
                    // Assuming Department has a 'Name' property
                })
                .ToListAsync();

            return staffInfo;
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
                    .Where(s => s.Id == item.StaffId && s.IsActive==true)
                    .FirstOrDefaultAsync();

                var leaveTypeExists = await _context.LeaveTypes
                    .AnyAsync(lt => lt.Id == leaveCreditDebitRequest.LeaveTypeId && lt.IsActive);

                if (staff == null || !leaveTypeExists)
                {
                    continue; // Skip processing if staff or leave type doesn't exist
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

        public async Task<List<ApplicationTypeDto>> GetAllApplicationTypesAsync()
        {
            var applicationType = await (from application in _context.ApplicationTypes
                                         select new ApplicationTypeDto
                                         {
                                             ApplicationTypeId = application.Id,
                                             ApplicationTypeName = application.ApplicationTypeName
                                         })
                                        .ToListAsync();
            if(applicationType.Count == 0)
            {
                throw new MessageNotFoundException("No application types found");
            }
            return applicationType;
        }
        public async Task<string> AddApplicationTypeAsync(ApplicationTypeRequest applicationType)
        {
            var message = "Application type added successfully";
            var application = new ApplicationType
            {
                ApplicationTypeName = applicationType.ApplicationTypeName
            };
            _context.ApplicationTypes.Add(application);
            await _context.SaveChangesAsync();
            return message;
        }
       

    }
}

