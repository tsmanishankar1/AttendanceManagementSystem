using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;

namespace AttendanceManagement.Services
{
    public class ApproveApplicationService
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public ApproveApplicationService(AttendanceManagementSystemContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<string> ApproveApplicationRequisition(ApproveLeaveRequest approveLeaveRequest)
        {
            var message = "";
            var selectedRows = approveLeaveRequest.SelectedRows;
            var approverName = _context.StaffCreations.Where(s => s.Id == approveLeaveRequest.ApprovedBy && s.IsActive == true).Select(s => $"{s.FirstName}{s.LastName}").FirstOrDefault();
            //string approvedDateTime = DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
            var applicationType = await _context.ApplicationTypes.Where(a => a.Id == approveLeaveRequest.ApplicationTypeId && a.IsActive).Select(a => a.Name).FirstOrDefaultAsync();
            foreach (var item in selectedRows)
            {
                if (approveLeaveRequest.ApplicationTypeId == 1)
                {
                    var leave = await _context.LeaveRequisitions.FirstOrDefaultAsync(l => l.Id == item.Id);
                    //if (leave == null) throw new MessageNotFoundException("Leave request not found");
                    /*                var leave1 = await _context.LeaveRequisitions.Where(l => l.Id == item.Id && (l.Status1 == false || l.Status2 == false) && l.IsActive == true).FirstOrDefaultAsync();
                                    if (leave1 != null) throw new InvalidOperationException("Leave request already rejected");
                    */
                    var leaveType = await _context.LeaveTypes.Where(l => l.Id == leave.LeaveTypeId && l.IsActive).Select(l => l.Name).FirstOrDefaultAsync();
                    var staffOrCreatorId = leave.StaffId ?? leave.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);
                    if (approveLeaveRequest.IsApproved)
                    {
                        if (staff.ApprovalLevel2 == null)
                        {
                            if (leave.Status1 == null)
                            {
                                var individualLeave = await _context.IndividualLeaveCreditDebits
                                    .Where(l => l.StaffCreationId == staffOrCreatorId
                                                && l.LeaveTypeId == leave.LeaveTypeId
                                                && l.IsActive == true)
                                    .OrderByDescending(l => l.Id)
                                    .FirstOrDefaultAsync();

                                if (individualLeave != null && individualLeave.AvailableBalance > 0 && individualLeave.AvailableBalance >= leave.TotalDays)
                                {
                                    individualLeave.AvailableBalance = decimal.Subtract(individualLeave.AvailableBalance, leave.TotalDays);
                                    individualLeave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                    individualLeave.UpdatedUtc = DateTime.UtcNow;
                                }
                                else
                                {
                                    if (leave.StaffId != null)
                                    {
                                        throw new MessageNotFoundException($"Insufficient leave balance found for Staff {staffName}");
                                    }
                                    else
                                    {
                                        throw new MessageNotFoundException("Insufficient leave balance found");
                                    }
                                }
                                leave.Status1 = true;
                                leave.IsActive = false;
                                leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                leave.UpdatedUtc = DateTime.UtcNow;
                            }
                            else if (leave.Status1 != null && leave.Status1 == false && leave.UpdatedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been rejected");
                            }
                            else if (leave.Status1 != null && leave.Status1 == true && leave.UpdatedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been approved");
                            }
                        }
                        else if (staff.ApprovalLevel2 != null)
                        {
                            if (leave.Status1 == null)
                            {
                                var individualLeave = await _context.IndividualLeaveCreditDebits
                                    .Where(l => l.StaffCreationId == staffOrCreatorId
                                                && l.LeaveTypeId == leave.LeaveTypeId
                                                && l.IsActive == true)
                                    .OrderByDescending(l => l.Id)
                                    .FirstOrDefaultAsync();

                                if (individualLeave != null && individualLeave.AvailableBalance > 0 && individualLeave.AvailableBalance >= leave.TotalDays)
                                {
                                    //individualLeave.AvailableBalance = decimal.Subtract(individualLeave.AvailableBalance, leave.TotalDays);
                                    individualLeave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                    individualLeave.UpdatedUtc = DateTime.UtcNow;
                                }
                                else
                                {
                                    if (leave.StaffId != null)
                                    {
                                        throw new MessageNotFoundException($"Insufficient leave balance found for Staff {staffName}");
                                    }
                                    else
                                    {
                                        throw new MessageNotFoundException("Insufficient leave balance found");
                                    }
                                }
                                leave.Status1 = true;
                                leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                leave.UpdatedUtc = DateTime.UtcNow;
                            }
                            else if (leave.Status1 != null && leave.Status1 == false && leave.UpdatedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been rejected");
                            }
                            else if (leave.Status1 != null && leave.Status1 == true && leave.UpdatedBy == approveLeaveRequest.ApprovedBy && leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been approved");
                            }
                            else if (leave.Status1 == true && leave.Status2 == null)
                            {
                                var individualLeave = await _context.IndividualLeaveCreditDebits
                                    .Where(l => l.StaffCreationId == staffOrCreatorId
                                                && l.LeaveTypeId == leave.LeaveTypeId
                                                && l.IsActive == true)
                                    .OrderByDescending(l => l.Id)
                                    .FirstOrDefaultAsync();

                                if (individualLeave != null && individualLeave.AvailableBalance > 0 && individualLeave.AvailableBalance >= leave.TotalDays)
                                {
                                    individualLeave.AvailableBalance = decimal.Subtract(individualLeave.AvailableBalance, leave.TotalDays);
                                    individualLeave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                    individualLeave.UpdatedUtc = DateTime.UtcNow;
                                }
                                else
                                {
                                    if (leave.StaffId != null)
                                    {
                                        throw new MessageNotFoundException($"Insufficient leave balance found for Staff {staffName}");
                                    }
                                    else
                                    {
                                        throw new MessageNotFoundException("Insufficient leave balance found");
                                    }
                                }
                                leave.Status2 = true;
                                leave.IsActive = false;
                                leave.ApprovedBy = approveLeaveRequest.ApprovedBy;
                                leave.ApprovedOn = DateTime.UtcNow;
                            }
                            else if (leave.Status2 != null && leave.Status2 == false && leave.ApprovedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been rejected");
                            }
                            else if (leave.Status2 != null && leave.Status2 == true && leave.ApprovedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been approved");
                            }
                        }
                        await _context.SaveChangesAsync();

                        string requestedTime = leave.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                        string approvedTime = leave.UpdatedUtc.HasValue ? leave.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                        var notification = new ApprovalNotification
                        {
                            StaffId = leave.CreatedBy,
                            Message = $"Your leave request has been approved. Approved by - {approverName} on {approvedTime}",
                            IsActive = true,
                            CreatedBy = approveLeaveRequest.ApprovedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        _context.ApprovalNotifications.Add(notification);
                        await _context.SaveChangesAsync();
                        leave.ApprovalNotificationId = notification.Id;
                        await _context.SaveChangesAsync();
                        if (approver1 != null)
                        {
                            await _emailService.SendLeaveApprovalEmail(
                                recipientEmail: staff.OfficialEmail,
                                recipientId: staff.Id,
                                recipientName: staffName,
                                isApproved: approveLeaveRequest.IsApproved,
                                leaveType: leaveType,
                                fromDate: leave.FromDate,
                                toDate: leave.ToDate,
                                totalDays: leave.TotalDays,
                                reason: leave.Reason,
                                approvedBy: approveLeaveRequest.ApprovedBy,
                                approverName: approverName,
                                approvedTime: approvedTime
                            );
                            if (approver2 != null && (!leave.IsEmailSent.HasValue || leave.IsEmailSent == false))
                            {
                                await _emailService.SendLeaveRequestEmail(
                                    recipientEmail: approver2.OfficialEmail,
                                    recipientId: approver2.Id,
                                    recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                    applicationTypeId: leave.ApplicationTypeId,
                                    id: leave.Id,
                                    leaveType: leaveType,
                                    fromDate: leave.FromDate,
                                    toDate: leave.ToDate,
                                    fromDuration: leave.StartDuration,
                                    toDuration: leave.EndDuration,
                                    totalDays: leave.TotalDays,
                                    reason: leave.Reason,
                                    createdBy: staffOrCreatorId,
                                    creatorName: staffName,
                                    requestDate: requestedTime
                                );
                                leave.IsEmailSent = true;
                                await _context.SaveChangesAsync();
                            }
                        }
                        message = "Leave request approved successfully";
                    }
                    else if (!approveLeaveRequest.IsApproved)
                    {
                        if (staff.ApprovalLevel2 == null)
                        {
                            if (leave.Status1 != null && leave.Status1 == true && leave.UpdatedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been approved");
                            }

                            if (leave.Status1 != null && leave.Status1 == false && leave.UpdatedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been rejected");
                            }
                            if (leave.Status1 == null)
                            {
                                leave.Status1 = false;
                                leave.IsActive = false;
                                leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                leave.UpdatedUtc = DateTime.UtcNow;
                            }
                        }
                        else if (staff.ApprovalLevel2 != null)
                        {
                            if (leave.Status1 == null)
                            {
                                leave.Status1 = false;
                                leave.IsActive = false;
                                leave.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                leave.UpdatedUtc = DateTime.UtcNow;
                            }
                            else if (leave.Status1 != null && leave.Status1 == true && leave.UpdatedBy == approveLeaveRequest.ApprovedBy && leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been approved");
                            }
                            else if (leave.Status1 != null && leave.Status1 == false && leave.UpdatedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been rejected");
                            }
                            else if (leave.Status1 == true && leave.Status2 == null)
                            {
                                leave.Status2 = false;
                                leave.IsActive = false;
                                leave.ApprovedBy = approveLeaveRequest.ApprovedBy;
                                leave.ApprovedOn = DateTime.UtcNow;
                            }
                            else if (leave.Status2 != null && leave.Status2 == true && leave.ApprovedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been approved");
                            }
                            else if (leave.Status2 != null && leave.Status2 == false && leave.ApprovedBy == approveLeaveRequest.ApprovedBy && !leave.IsActive)
                            {
                                throw new InvalidOperationException("Leave request has already been rejected");
                            }
                        }
                        await _context.SaveChangesAsync();

                        string requestedTime = leave.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                        string approvedTime = leave.UpdatedUtc.HasValue ? leave.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                        var notification = new ApprovalNotification
                        {
                            StaffId = leave.CreatedBy,
                            Message = $"Your leave request has been rejected. Rejected by - {approverName} on {approvedTime}",
                            IsActive = true,
                            CreatedBy = approveLeaveRequest.ApprovedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        _context.ApprovalNotifications.Add(notification);
                        await _context.SaveChangesAsync();
                        leave.ApprovalNotificationId = notification.Id;
                        await _context.SaveChangesAsync();

                        if (approver1 != null)
                        {
                            await _emailService.SendLeaveApprovalEmail(
                                recipientEmail: staff.OfficialEmail,
                                recipientId: staff.Id,
                                recipientName: staffName,
                                isApproved: approveLeaveRequest.IsApproved,
                                leaveType: leaveType,
                                totalDays: leave.TotalDays,
                                fromDate: leave.FromDate,
                                toDate: leave.ToDate,
                                reason: leave.Reason,
                                approvedBy: approveLeaveRequest.ApprovedBy,
                                approverName: approverName,
                                approvedTime: approvedTime
                            );

                            /*                        if (approver2 != null && (!leave.IsEmailSent.HasValue || leave.IsEmailSent == false))
                                                    {
                                                        await _emailService.SendLeaveRequestEmail(
                                                            recipientEmail: approver2.OfficialEmail,
                                                            recipientId: approver2.Id,
                                                            recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                                            applicationTypeId: leave.ApplicationTypeId,
                                                            id: leave.Id,
                                                            leaveType: leaveType,
                                                            fromDate: leave.FromDate,
                                                            toDate: leave.ToDate,
                                                            totalDays: leave.TotalDays,
                                                            reason: leave.Reason,
                                                            createdBy: staffOrCreatorId,
                                                            creatorName: staffName,
                                                            requestDate: requestedTime
                                                        );
                                                        leave.IsEmailSent = true;
                                                        await _context.SaveChangesAsync();
                                                    }
                            */
                        }
                        message = "Leave request rejected successfully";
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 2)
                {
                    var permissionRequest = await _context.CommonPermissions.Where(l => l.Id == item.Id).FirstOrDefaultAsync();
                    //if (permission == null) throw new MessageNotFoundException("Permission request not found");
                    var permissionType = await _context.PermissionTypes.Where(l => l.Name == permissionRequest.PermissionType && l.IsActive).Select(l => l.Name).FirstOrDefaultAsync();
                    var staffOrCreatorId = permissionRequest.StaffId ?? permissionRequest.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);
                    if (approver2 == null)
                    {
                        var permissionRequest1 = await _context.CommonPermissions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (permissionRequest1 != null)
                        {
                            if (permissionRequest1.Status1.HasValue && permissionRequest1.Status1 != null)
                            {
                                string statusMessage = permissionRequest.Status1 == true ? "Common Permission request has already been approved." : "Common Permission request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (permissionRequest != null && permissionRequest.Status1 == null)
                        {
                            permissionRequest.Status1 = approveLeaveRequest.IsApproved ? true : false;
                            permissionRequest.IsActive = false;
                            permissionRequest.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            permissionRequest.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var permissionRequest1 = await _context.CommonPermissions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (permissionRequest1 != null)
                        {
                            if (permissionRequest1.Status1.HasValue && permissionRequest1.Status1 != null)
                            {
                                string statusMessage = permissionRequest.Status1 == true ? "Common Permission request has already been approved." : "Common Permission request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var permissionRequest2 = await _context.CommonPermissions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (permissionRequest2 != null)
                        {
                            if (permissionRequest2.Status2.HasValue && permissionRequest2.Status2 != null)
                            {
                                string statusMessage = permissionRequest.Status2 == true ? "Common Permission request has already been approved." : "Common Permission request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (permissionRequest != null && permissionRequest.Status1 == true && permissionRequest.Status2 == null)
                        {
                            permissionRequest.Status2 = approveLeaveRequest.IsApproved ? true : false;
                            permissionRequest.IsActive = false;
                            permissionRequest.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            permissionRequest.ApprovedOn = DateTime.UtcNow;
                        }
                        if (permissionRequest != null && permissionRequest.Status1 == null)
                        {
                            permissionRequest.Status1 = approveLeaveRequest.IsApproved ? true : false;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                permissionRequest.IsActive = false;
                            }
                            permissionRequest.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            permissionRequest.UpdatedUtc = DateTime.UtcNow;
                        }

                    }
                    await _context.SaveChangesAsync();

                    string approvedTime = permissionRequest.UpdatedUtc.HasValue ? permissionRequest.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    message = approveLeaveRequest.IsApproved ? "Common Permission request approved successfully" : "Common Permission request rejected successfully";
                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your common permission request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your common permission request has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = permissionRequest.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    permissionRequest.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    var duration = permissionRequest.EndTime - permissionRequest.StartTime;
                    var formattedDuration = $"{duration.Hours:D2}:{duration.Minutes:D2}";
                    string requestDateTime = permissionRequest.CreatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    if (approver1 != null)
                    {
                        await _emailService.SendCommonPermissionApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            permissionType: permissionType,
                            permissionDate: permissionRequest.PermissionDate,
                            startTime: permissionRequest.StartTime,
                            endTime: permissionRequest.EndTime,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!permissionRequest.IsEmailSent.HasValue || permissionRequest.IsEmailSent == false))
                        {
                            await _emailService.SendCommonPermissionRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                applicationTypeId: permissionRequest.ApplicationTypeId,
                                id: permissionRequest.Id,
                                permissionType: permissionRequest.PermissionType,
                                permissionDate: permissionRequest.PermissionDate,
                                startTime: permissionRequest.StartTime,
                                endTime: permissionRequest.EndTime,
                                duration: formattedDuration,
                                remarks: permissionRequest.Remarks,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestDateTime
                            );
                            permissionRequest.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 3)
                {
                    var manualPunch = await _context.ManualPunchRequistions.FirstOrDefaultAsync(m => m.Id == item.Id);
                    /*                if (manualPunch == null) throw new MessageNotFoundException("Manual punch request not found");
                                    var punchType = manualPunch.SelectPunch;
                    */
                    var staffOrCreatorId = manualPunch.StaffId ?? manualPunch.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                    if (approver2 == null)
                    {
                        var manualPunchRequest1 = await _context.ManualPunchRequistions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (manualPunchRequest1 != null)
                        {
                            if (manualPunchRequest1.Status1.HasValue && manualPunchRequest1.Status1 != null)
                            {
                                string statusMessage = manualPunch.Status1 == true ? "Manual Punch request has already been approved." : "Manual Punch request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (manualPunch != null && manualPunch.Status1 == null)
                        {
                            manualPunch.Status1 = approveLeaveRequest.IsApproved;
                            manualPunch.IsActive = false;
                            manualPunch.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            manualPunch.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var manualPunchRequest1 = await _context.ManualPunchRequistions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (manualPunchRequest1 != null)
                        {
                            if (manualPunchRequest1.Status1.HasValue && manualPunchRequest1.Status1 != null)
                            {
                                string statusMessage = manualPunch.Status1 == true ? "Manual Punch request has already been approved." : "Manual Punch request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var manualPunchRequest2 = await _context.ManualPunchRequistions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (manualPunchRequest2 != null)
                        {
                            if (manualPunchRequest2.Status2.HasValue && manualPunchRequest2.Status2 != null)
                            {
                                string statusMessage = manualPunch.Status2 == true ? "Manual Punch request has already been approved." : "Manual Punch request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (manualPunch != null && manualPunch.Status1 == true && manualPunch.Status2 == null)
                        {
                            manualPunch.Status2 = approveLeaveRequest.IsApproved;
                            manualPunch.IsActive = false;
                            manualPunch.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            manualPunch.ApprovedOn = DateTime.UtcNow;
                        }
                        if (manualPunch != null && manualPunch.Status1 == null)
                        {
                            manualPunch.Status1 = approveLeaveRequest.IsApproved;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                manualPunch.IsActive = false;
                            }
                            manualPunch.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            manualPunch.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    await _context.SaveChangesAsync();

                    string requestedTime = manualPunch.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = manualPunch.UpdatedUtc.HasValue ? manualPunch.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Manual punch request approved successfully" : "Manual punch request rejected successfully";
                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your manual punch request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your manual punch request has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = manualPunch.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    manualPunch.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendManualPunchApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            punchType: manualPunch.SelectPunch,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!manualPunch.IsEmailSent.HasValue || manualPunch.IsEmailSent == false))
                        {
                            await _emailService.SendManualPunchRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                staffName: staffName,
                                id: manualPunch.Id,
                                applicationTypeId: manualPunch.ApplicationTypeId,
                                selectPunch: manualPunch.SelectPunch,
                                inPunch: manualPunch.InPunch,
                                outPunch: manualPunch.OutPunch,
                                remarks: manualPunch.Remarks,
                                createdBy: staffOrCreatorId,
                                requestDate: requestedTime
                            );
                            manualPunch.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 4)
                {
                    var onDuty = await _context.OnDutyRequisitions.FirstOrDefaultAsync(o => o.Id == item.Id);
                    /*                if (onDuty == null) throw new MessageNotFoundException("On duty request not found");
                    */
                    var staffOrCreatorId = onDuty.StaffId ?? onDuty.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                    if (approver2 == null)
                    {
                        var onDutyRequest1 = await _context.OnDutyRequisitions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (onDutyRequest1 != null)
                        {
                            if (onDutyRequest1.Status1.HasValue && onDutyRequest1.Status1 != null)
                            {
                                string statusMessage = onDuty.Status1 == true ? "On Duty request has already been approved." : "On Duty request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (onDuty != null && onDuty.Status1 == null)
                        {
                            onDuty.Status1 = approveLeaveRequest.IsApproved;
                            onDuty.IsActive = false;
                            onDuty.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            onDuty.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var onDutyRequest1 = await _context.OnDutyRequisitions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (onDutyRequest1 != null)
                        {
                            if (onDutyRequest1.Status1.HasValue && onDutyRequest1.Status1 != null)
                            {
                                string statusMessage = onDuty.Status1 == true ? "On Duty request has already been approved." : "On Duty request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var onDutyRequest2 = await _context.OnDutyRequisitions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (onDutyRequest2 != null)
                        {
                            if (onDutyRequest2.Status2.HasValue && onDutyRequest2.Status2 != null)
                            {
                                string statusMessage = onDuty.Status2 == true ? "On Duty request has already been approved." : "On Duty request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (onDuty != null && onDuty.Status1 == true && onDuty.Status2 == null)
                        {
                            onDuty.Status2 = approveLeaveRequest.IsApproved;
                            onDuty.IsActive = false;
                            onDuty.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            onDuty.ApprovedOn = DateTime.UtcNow;
                        }
                        if (onDuty != null && onDuty.Status1 == null)
                        {
                            onDuty.Status1 = approveLeaveRequest.IsApproved;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                onDuty.IsActive = false;
                            }
                            onDuty.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            onDuty.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    await _context.SaveChangesAsync();

                    string requestedTime = onDuty.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = onDuty.UpdatedUtc.HasValue ? onDuty.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "On-duty request approved successfully" : "On-duty request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your on-duty request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your on-duty request has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = onDuty.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    onDuty.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendOnDutyApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            startDate: onDuty.StartDate,
                            endDate: onDuty.EndDate,
                            startTime: onDuty.StartTime,
                            endTime: onDuty.EndTime,
                            totalDays: onDuty.TotalDays,
                            totalHours: onDuty.TotalHours,
                            reason: onDuty.Reason,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!onDuty.IsEmailSent.HasValue || onDuty.IsEmailSent == false))
                        {
                            await _emailService.SendOnDutyRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                id: onDuty.Id,
                                applicationTypeId: onDuty.ApplicationTypeId,
                                startDate: onDuty.StartDate,
                                endDate: onDuty.EndDate,
                                startTime: onDuty.StartTime,
                                endTime: onDuty.EndTime,
                                totalDays: onDuty.TotalDays,
                                totalHours: onDuty.TotalHours,
                                reason: onDuty.Reason,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            onDuty.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 5)
                {
                    var businessTravel = await _context.BusinessTravels.FirstOrDefaultAsync(l => l.Id == item.Id);
                    /*                if (businessTravel == null) throw new MessageNotFoundException("Business travel request not found");
                    */
                    var staffOrCreatorId = businessTravel.StaffId ?? businessTravel.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                    if (approver2 == null)
                    {
                        var businessTravelRequest1 = await _context.BusinessTravels
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (businessTravelRequest1 != null)
                        {
                            if (businessTravelRequest1.Status1.HasValue && businessTravelRequest1.Status1 != null)
                            {
                                string statusMessage = businessTravel.Status1 == true ? "Business Travel request has already been approved." : "Business Travel request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (businessTravel != null && businessTravel.Status1 == null)
                        {
                            businessTravel.Status1 = approveLeaveRequest.IsApproved;
                            businessTravel.IsActive = false;
                            businessTravel.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            businessTravel.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var businessTravelRequest1 = await _context.BusinessTravels
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (businessTravelRequest1 != null)
                        {
                            if (businessTravelRequest1.Status1.HasValue && businessTravelRequest1.Status1 != null)
                            {
                                string statusMessage = businessTravel.Status1 == true ? "Business Travel request has already been approved." : "Business Travel request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var businessTravelRequest2 = await _context.BusinessTravels
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (businessTravelRequest2 != null)
                        {
                            if (businessTravelRequest2.Status2.HasValue && businessTravelRequest2.Status2 != null)
                            {
                                string statusMessage = businessTravel.Status2 == true ? "Business Travel request has already been approved." : "Business Travel request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (businessTravel != null && businessTravel.Status1 == true && businessTravel.Status2 == null)
                        {
                            businessTravel.Status2 = approveLeaveRequest.IsApproved;
                            businessTravel.IsActive = false;
                            businessTravel.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            businessTravel.ApprovedOn = DateTime.UtcNow;
                        }
                        if (businessTravel != null && businessTravel.Status1 == null)
                        {
                            businessTravel.Status1 = approveLeaveRequest.IsApproved;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                businessTravel.IsActive = false;
                            }
                            businessTravel.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            businessTravel.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    await _context.SaveChangesAsync();

                    string requestedTime = businessTravel.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = businessTravel.UpdatedUtc.HasValue ? businessTravel.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Business travel request approved successfully" : "Business travel request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your business travel request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your business travel request has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = businessTravel.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    businessTravel.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendBusinessTravelApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            fromDate: businessTravel.FromDate,
                            toDate: businessTravel.ToDate,
                            fromTime: businessTravel.FromTime,
                            toTime: businessTravel.ToTime,
                            reason: businessTravel.Reason,
                            totalDays: businessTravel.TotalDays,
                            totalHours: businessTravel.TotalHours,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!businessTravel.IsEmailSent.HasValue || businessTravel.IsEmailSent == false))
                        {
                            await _emailService.SendBusinessTravelRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                id: businessTravel.Id,
                                applicationTypeId: businessTravel.ApplicationTypeId,
                                fromDate: businessTravel.FromDate,
                                toDate: businessTravel.ToDate,
                                fromTime: businessTravel.FromTime,
                                toTime: businessTravel.ToTime,
                                totalDays: businessTravel.TotalDays,
                                totalHours: businessTravel.TotalHours,
                                reason: businessTravel.Reason,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            businessTravel.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 6)
                {
                    var workFromHome = await _context.WorkFromHomes.FirstOrDefaultAsync(l => l.Id == item.Id);
                    /*                if (workFromHome == null) throw new MessageNotFoundException("Work from home request not found");
                    */
                    var staffOrCreatorId = workFromHome.StaffId ?? workFromHome.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                    if (approver2 == null)
                    {
                        var workFromHomeRequest1 = await _context.WorkFromHomes
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (workFromHomeRequest1 != null)
                        {
                            if (workFromHomeRequest1.Status1.HasValue && workFromHomeRequest1.Status1 != null)
                            {
                                string statusMessage = workFromHome.Status1 == true ? "Work From Home request has already been approved." : "Work From Home request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (workFromHome != null && workFromHome.Status1 == null)
                        {
                            workFromHome.Status1 = approveLeaveRequest.IsApproved;
                            workFromHome.IsActive = false;
                            workFromHome.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            workFromHome.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var workFromHomeRequest1 = await _context.WorkFromHomes
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (workFromHomeRequest1 != null)
                        {
                            if (workFromHomeRequest1.Status1.HasValue && workFromHomeRequest1.Status1 != null)
                            {
                                string statusMessage = workFromHome.Status1 == true ? "Work From Home request has already been approved." : "Work From Home request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }

                        var workFromHomeRequest2 = await _context.WorkFromHomes
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (workFromHomeRequest2 != null)
                        {
                            if (workFromHomeRequest2.Status2.HasValue && workFromHomeRequest2.Status2 != null)
                            {
                                string statusMessage = workFromHome.Status2 == true ? "Work From Home request has already been approved." : "Work From Home request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (workFromHome != null && workFromHome.Status1 == true && workFromHome.Status2 == null)
                        {
                            workFromHome.Status2 = approveLeaveRequest.IsApproved;
                            workFromHome.IsActive = false;
                            workFromHome.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            workFromHome.ApprovedOn = DateTime.UtcNow;
                        }
                        if (workFromHome != null && workFromHome.Status1 == null)
                        {
                            workFromHome.Status1 = approveLeaveRequest.IsApproved;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                workFromHome.IsActive = false;
                            }
                            workFromHome.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            workFromHome.UpdatedUtc = DateTime.UtcNow;
                        }
                    }

                    await _context.SaveChangesAsync();

                    string requestedTime = workFromHome.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = workFromHome.UpdatedUtc.HasValue ? workFromHome.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Work from home request approved successfully" : "Work from home request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your work from home request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your work from home request has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = workFromHome.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    workFromHome.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendWorkFromHomeApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            fromDate: workFromHome.FromDate,
                            toDate: workFromHome.ToDate,
                            fromTime: workFromHome.FromTime,
                            toTime: workFromHome.ToTime,
                            reason: workFromHome.Reason,
                            totalDays: workFromHome.TotalDays,
                            totalHours: workFromHome.TotalHours,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!workFromHome.IsEmailSent.HasValue || workFromHome.IsEmailSent == false))
                        {
                            await _emailService.SendWorkFromHomeRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                id: workFromHome.Id,
                                applicationTypeId: workFromHome.ApplicationTypeId,
                                fromDate: workFromHome.FromDate,
                                toDate: workFromHome.ToDate,
                                fromTime: workFromHome.FromTime,
                                toTime: workFromHome.ToTime,
                                totalDays: workFromHome.TotalDays,
                                totalHours: workFromHome.TotalHours,
                                reason: workFromHome.Reason,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            workFromHome.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 7)
                {
                    var shiftChange = await _context.ShiftChanges.FirstOrDefaultAsync(l => l.Id == item.Id);
                    /*                if (shiftChange == null) throw new MessageNotFoundException("Shift change request not found");
                    */
                    var staffOrCreatorId = shiftChange.StaffId ?? shiftChange.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);
                    var shiftName = await _context.Shifts.Where(s => s.Id == shiftChange.ShiftId && s.IsActive).Select(s => s.Name).FirstOrDefaultAsync();

                    if (approver2 == null)
                    {
                        var shiftChangeRequest1 = await _context.ShiftChanges
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (shiftChangeRequest1 != null)
                        {
                            if (shiftChangeRequest1.Status1.HasValue && shiftChangeRequest1.Status1 != null)
                            {
                                string statusMessage = shiftChange.Status1 == true ? "Shift Change request has already been approved." : "Shift Change request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (shiftChange != null && shiftChange.Status1 == null)
                        {
                            shiftChange.Status1 = approveLeaveRequest.IsApproved;
                            shiftChange.IsActive = false;
                            shiftChange.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            shiftChange.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var shiftChangeRequest1 = await _context.ShiftChanges
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (shiftChangeRequest1 != null)
                        {
                            if (shiftChangeRequest1.Status1.HasValue && shiftChangeRequest1.Status1 != null)
                            {
                                string statusMessage = shiftChange.Status1 == true ? "Shift Change request has already been approved." : "Shift Change request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var shiftChangeRequest2 = await _context.ShiftChanges
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (shiftChangeRequest2 != null)
                        {
                            if (shiftChangeRequest2.Status2.HasValue && shiftChangeRequest2.Status2 != null)
                            {
                                string statusMessage = shiftChange.Status2 == true ? "Shift Change request has already been approved." : "Shift Change request has already been rejected.";

                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (shiftChange != null && shiftChange.Status1 == true && shiftChange.Status2 == null)
                        {
                            shiftChange.Status2 = approveLeaveRequest.IsApproved;
                            shiftChange.IsActive = false;
                            shiftChange.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            shiftChange.ApprovedOn = DateTime.UtcNow;
                        }
                        if (shiftChange != null && shiftChange.Status1 == null)
                        {
                            shiftChange.Status1 = approveLeaveRequest.IsApproved;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                shiftChange.IsActive = false;
                            }
                            shiftChange.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            shiftChange.UpdatedUtc = DateTime.UtcNow;
                        }
                    }

                    await _context.SaveChangesAsync();

                    string requestedTime = shiftChange.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = shiftChange.UpdatedUtc.HasValue ? shiftChange.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Shift change request approved successfully" : "Shift change request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your shift change request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your shift change request has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = shiftChange.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    shiftChange.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendShiftChangeApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            shiftName: shiftName,
                            fromDate: shiftChange.FromDate,
                            toDate: shiftChange.ToDate,
                            reason: shiftChange.Reason,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!shiftChange.IsEmailSent.HasValue || shiftChange.IsEmailSent == false))
                        {
                            await _emailService.SendShiftChangeRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                id: shiftChange.Id,
                                applicationTypeId: shiftChange.ApplicationTypeId,
                                shiftName: shiftName,
                                fromDate: shiftChange.FromDate,
                                toDate: shiftChange.ToDate,
                                reason: shiftChange.Reason,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            shiftChange.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 8)
                {
                    var shiftExtension = await _context.ShiftExtensions.FirstOrDefaultAsync(s => s.Id == item.Id);
                    /*                if (shiftExtensionRequest == null) throw new MessageNotFoundException("Shift extension request not found");
                    */
                    var staffOrCreatorId = shiftExtension.StaffId ?? shiftExtension.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                    if (approver2 == null)
                    {
                        var shiftExtensionReques1 = await _context.ShiftExtensions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (shiftExtensionReques1 != null)
                        {
                            if (shiftExtensionReques1.Status1.HasValue && shiftExtensionReques1.Status1 != null)
                            {
                                string statusMessage = shiftExtension.Status1 == true ? "Shift Extension request has already been approved." : "Shift Extension request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (shiftExtension != null && shiftExtension.Status1 == null)
                        {
                            shiftExtension.Status1 = approveLeaveRequest.IsApproved;
                            shiftExtension.IsActive = false;
                            shiftExtension.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            shiftExtension.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var shiftExtensionReques1 = await _context.ShiftExtensions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (shiftExtensionReques1 != null)
                        {
                            if (shiftExtensionReques1.Status1.HasValue && shiftExtensionReques1.Status1 != null)
                            {
                                string statusMessage = shiftExtension.Status1 == true ? "Shift Extension request has already been approved." : "Shift Extension request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var shiftExtensionReques2 = await _context.ShiftExtensions
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (shiftExtensionReques2 != null)
                        {
                            if (shiftExtensionReques2.Status2.HasValue && shiftExtensionReques2.Status2 != null)
                            {
                                string statusMessage = shiftExtension.Status2 == true ? "Shift Extension request has already been approved." : "Shift Extension request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (shiftExtension != null && shiftExtension.Status1 == true && shiftExtension.Status2 == null)
                        {
                            shiftExtension.Status2 = approveLeaveRequest.IsApproved;
                            shiftExtension.IsActive = false;
                            shiftExtension.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            shiftExtension.ApprovedOn = DateTime.UtcNow;
                        }
                        if (shiftExtension != null && shiftExtension.Status1 == null)
                        {
                            shiftExtension.Status1 = approveLeaveRequest.IsApproved;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                shiftExtension.IsActive = false;
                            }
                            shiftExtension.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            shiftExtension.UpdatedUtc = DateTime.UtcNow;
                        }
                    }

                    await _context.SaveChangesAsync();

                    string requestedTime = shiftExtension.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = shiftExtension.UpdatedUtc.HasValue ? shiftExtension.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Shift extension request approved successfully" : "Shift extension request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your shift extension request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your shift extension request has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = shiftExtension.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    shiftExtension.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendShiftExtensionApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            transactionDate: shiftExtension.TransactionDate,
                            durationHours: shiftExtension.DurationHours,
                            beforeShiftHours: shiftExtension.BeforeShiftHours,
                            afterShiftHours: shiftExtension.AfterShiftHours,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!shiftExtension.IsEmailSent.HasValue || shiftExtension.IsEmailSent == false))
                        {
                            await _emailService.SendShiftExtensionRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                id: shiftExtension.Id,
                                applicationTypeId: shiftExtension.ApplicationTypeId,
                                transactionDate: shiftExtension.TransactionDate,
                                durationHours: shiftExtension.DurationHours,
                                beforeShiftHours: shiftExtension.BeforeShiftHours,
                                afterShiftHours: shiftExtension.AfterShiftHours,
                                remarks: shiftExtension.Remarks,
                                createdBy: staffOrCreatorId,
                                creatorName: staffName,
                                requestDate: requestedTime
                            );
                            shiftExtension.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 9)
                {
                    var weeklyOffHoliday = await _context.WeeklyOffHolidayWorkings.FirstOrDefaultAsync(w => w.Id == item.Id);
                    /*                if (weeklyOffHolidayRequest == null) throw new MessageNotFoundException("Weekly off holiday working request not found");
                    */
                    var staffOrCreatorId = weeklyOffHoliday.StaffId ?? weeklyOffHoliday.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var shiftName = await _context.Shifts.Where(s => s.Id == weeklyOffHoliday.ShiftId && s.IsActive).Select(s => s.Name).FirstOrDefaultAsync();
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                    if (approver2 == null)
                    {
                        var weeklyOffHolidayRequest1 = await _context.WeeklyOffHolidayWorkings
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (weeklyOffHolidayRequest1 != null)
                        {
                            if (weeklyOffHolidayRequest1.Status1.HasValue && weeklyOffHolidayRequest1.Status1 != null)
                            {
                                string statusMessage = weeklyOffHoliday.Status1 == true ? "Weekly Off Holiday Working request has already been approved." : "Weekly Off Holiday Working request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (weeklyOffHoliday != null && weeklyOffHoliday.Status1 == null)
                        {
                            weeklyOffHoliday.Status1 = approveLeaveRequest.IsApproved;
                            weeklyOffHoliday.IsActive = false;
                            weeklyOffHoliday.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            weeklyOffHoliday.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var weeklyOffHolidayRequest1 = await _context.WeeklyOffHolidayWorkings
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (weeklyOffHolidayRequest1 != null)
                        {
                            if (weeklyOffHolidayRequest1.Status1.HasValue && weeklyOffHolidayRequest1.Status1 != null)
                            {
                                string statusMessage = weeklyOffHoliday.Status1 == true ? "Weekly Off Holiday Working request has already been approved." : "Weekly Off Holiday Working request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var weeklyOffHolidayRequest2 = await _context.WeeklyOffHolidayWorkings
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (weeklyOffHolidayRequest2 != null)
                        {
                            if (weeklyOffHolidayRequest2.Status2.HasValue && weeklyOffHolidayRequest2.Status2 != null)
                            {
                                string statusMessage = weeklyOffHoliday.Status2 == true ? "Weekly Off Holiday Working request has already been approved." : "Weekly Off Holiday Working request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (weeklyOffHoliday != null && weeklyOffHoliday.Status1 == true && weeklyOffHoliday.Status2 == null)
                        {
                            weeklyOffHoliday.Status2 = approveLeaveRequest.IsApproved;
                            weeklyOffHoliday.IsActive = false;
                            weeklyOffHoliday.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            weeklyOffHoliday.ApprovedOn = DateTime.UtcNow;
                        }
                        if (weeklyOffHoliday != null && weeklyOffHoliday.Status1 == null)
                        {
                            weeklyOffHoliday.Status1 = approveLeaveRequest.IsApproved;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                weeklyOffHoliday.IsActive = false;
                            }
                            weeklyOffHoliday.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            weeklyOffHoliday.UpdatedUtc = DateTime.UtcNow;
                        }
                    }

                    await _context.SaveChangesAsync();

                    string requestedTime = weeklyOffHoliday.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = weeklyOffHoliday.UpdatedUtc.HasValue ? weeklyOffHoliday.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Weekly off/holiday working request approved successfully" : "Weekly off/holiday working request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your weekly off/holiday working request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your weekly off/holiday working request has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = weeklyOffHoliday.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    weeklyOffHoliday.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendWeeklyOffHolidayApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            shiftSelectType: weeklyOffHoliday.SelectShiftType,
                            shiftName: shiftName,
                            shiftInTime: weeklyOffHoliday.ShiftInTime,
                            shiftOutTime: weeklyOffHoliday.ShiftOutTime,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!weeklyOffHoliday.IsEmailSent.HasValue || weeklyOffHoliday.IsEmailSent == false))
                        {
                            await _emailService.SendWeeklyOffHolidayWorkingRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                staffName: staffName,
                                selectShiftType: weeklyOffHoliday.SelectShiftType,
                                id: weeklyOffHoliday.Id,
                                applicationTypeId: weeklyOffHoliday.ApplicationTypeId,
                                txnDate: weeklyOffHoliday.TxnDate,
                                shiftName: shiftName,
                                shiftInTime: weeklyOffHoliday.ShiftInTime,
                                shiftOutTime: weeklyOffHoliday.ShiftOutTime,
                                requestDate: requestedTime,
                                createdBy: staffOrCreatorId
                            );
                            weeklyOffHoliday.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 10)
                {
                    var compOffAvail = await _context.CompOffAvails.FirstOrDefaultAsync(c => c.Id == item.Id && c.IsActive);
                    /*                if (compOffAvailRequest == null) throw new MessageNotFoundException("Compoff avail request not found");
                    */
                    var staffOrCreatorId = compOffAvail.StaffId ?? compOffAvail.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                    if (approver2 == null)
                    {
                        var compOffAvailRequest1 = await _context.CompOffAvails
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (compOffAvailRequest1 != null)
                        {
                            if (compOffAvailRequest1.Status1.HasValue && compOffAvailRequest1.Status1 != null)
                            {
                                string statusMessage = compOffAvail.Status1 == true ? "CompOff Avail request has already been approved." : "CompOff Avail request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (compOffAvail != null && compOffAvail.Status1 == null)
                        {
                            compOffAvail.Status1 = approveLeaveRequest.IsApproved;
                            compOffAvail.IsActive = false;
                            compOffAvail.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            compOffAvail.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var compOffAvailRequest1 = await _context.CompOffAvails
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (compOffAvailRequest1 != null)
                        {
                            if (compOffAvailRequest1.Status1.HasValue && compOffAvailRequest1.Status1 != null)
                            {
                                string statusMessage = compOffAvail.Status1 == true ? "CompOff Avail request has already been approved." : "CompOff Avail request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var compOffAvailRequest2 = await _context.CompOffAvails
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (compOffAvailRequest2 != null)
                        {
                            if (compOffAvailRequest2.Status2.HasValue && compOffAvailRequest2.Status2 != null)
                            {
                                string statusMessage = compOffAvail.Status2 == true ? "CompOff Avail request has already been approved." : "CompOff Avail request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (compOffAvail != null && compOffAvail.Status1 == true && compOffAvail.Status2 == null)
                        {
                            compOffAvail.Status2 = approveLeaveRequest.IsApproved;
                            compOffAvail.IsActive = false;
                            compOffAvail.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            compOffAvail.ApprovedOn = DateTime.UtcNow;
                        }
                        if (compOffAvail != null && compOffAvail.Status1 == null)
                        {
                            compOffAvail.Status1 = approveLeaveRequest.IsApproved;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                compOffAvail.IsActive = false;
                            }
                            compOffAvail.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            compOffAvail.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approveLeaveRequest.IsApproved)
                    {
                        if (approver2 == null)
                        {
                            if (compOffAvail != null && compOffAvail.Status1 == true)
                            {
                                var lastCompOffCredit = await _context.CompOffCredits
                                    .Where(c => c.CreatedBy == compOffAvail.CreatedBy)
                                    .OrderByDescending(c => c.CreatedUtc)
                                    .FirstOrDefaultAsync();
                                if (lastCompOffCredit == null) throw new MessageNotFoundException("Insufficient CompOff balance found");
                                if (lastCompOffCredit != null && lastCompOffCredit.Balance > 0)
                                {
                                    lastCompOffCredit.Balance = (lastCompOffCredit.Balance ?? 0) - (int)compOffAvail.TotalDays;
                                    lastCompOffCredit.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                    lastCompOffCredit.UpdatedUtc = DateTime.UtcNow;
                                }
                            }
                        }
                        if (approver2 != null)
                        {
                            if (compOffAvail != null && compOffAvail.Status1 == true && compOffAvail.Status2 == true && approveLeaveRequest.IsApproved)
                            {
                                var lastCompOffCredit = await _context.CompOffCredits
                                    .Where(c => c.CreatedBy == compOffAvail.CreatedBy)
                                    .OrderByDescending(c => c.CreatedUtc)
                                    .FirstOrDefaultAsync();
                                if (lastCompOffCredit == null) throw new MessageNotFoundException("Insufficient CompOff balance found");
                                if (lastCompOffCredit != null && lastCompOffCredit.Balance > 0)
                                {
                                    lastCompOffCredit.Balance = (lastCompOffCredit.Balance ?? 0) - (int)compOffAvail.TotalDays;
                                    lastCompOffCredit.UpdatedBy = approveLeaveRequest.ApprovedBy;
                                    lastCompOffCredit.UpdatedUtc = DateTime.UtcNow;
                                }
                            }
                        }
                    }
                    await _context.SaveChangesAsync();

                    string requestedTime = compOffAvail.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = compOffAvail.UpdatedUtc.HasValue ? compOffAvail.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Comp-Off Avail request approved successfully" : "Comp-Off Avail request rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your Comp-Off avail request has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your Comp-Off avail request has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = compOffAvail.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    compOffAvail.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendCompOffAvailApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            workedDate: compOffAvail.WorkedDate,
                            fromDate: compOffAvail.FromDate,
                            toDate: compOffAvail.ToDate,
                            totalDays: compOffAvail.TotalDays,
                            reason: compOffAvail.Reason,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!compOffAvail.IsEmailSent.HasValue || compOffAvail.IsEmailSent == false))
                        {
                            await _emailService.SendCompOffApprovalRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                staffName: staffName,
                                id: compOffAvail.Id,
                                applicationTypeId: compOffAvail.ApplicationTypeId,
                                workedDate: compOffAvail.WorkedDate,
                                fromDate: compOffAvail.FromDate,
                                toDate: compOffAvail.ToDate,
                                totalDays: compOffAvail.TotalDays,
                                reason: compOffAvail.Reason,
                                requestDate: requestedTime,
                                createdBy: staffOrCreatorId
                            );
                            compOffAvail.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 11)
                {
                    var compOffCredit = await _context.CompOffCredits.FirstOrDefaultAsync(c => c.Id == item.Id);
                    /*                if (compOffCreditRequest == null) throw new MessageNotFoundException("Compoff credit request not found");
                    */
                    var staffOrCreatorId = compOffCredit.StaffId ?? compOffCredit.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");
                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                    if (approver2 == null)
                    {
                        var compOffCreditRequest1 = await _context.CompOffCredits
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (compOffCreditRequest1 != null)
                        {
                            if (compOffCreditRequest1.Status1.HasValue && compOffCreditRequest1.Status1 != null)
                            {
                                string statusMessage = compOffCredit.Status1 == true ? "CompOff Credit request has already been approved." : "CompOff Credit request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (compOffCredit != null && compOffCredit.Status1 == null)
                        {
                            compOffCredit.Status1 = approveLeaveRequest.IsApproved;
                            compOffCredit.IsActive = false;
                            compOffCredit.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            compOffCredit.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var compOffCreditRequest1 = await _context.CompOffCredits
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (compOffCreditRequest1 != null)
                        {
                            if (compOffCreditRequest1.Status1.HasValue && compOffCreditRequest1.Status1 != null)
                            {
                                string statusMessage = compOffCredit.Status1 == true ? "CompOff Credit request has already been approved." : "CompOff Credit request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var compOffCreditRequest2 = await _context.CompOffCredits
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (compOffCreditRequest2 != null)
                        {
                            if (compOffCreditRequest2.Status2.HasValue && compOffCreditRequest2.Status2 != null)
                            {
                                string statusMessage = compOffCredit.Status2 == true ? "CompOff Credit request has already been approved." : "CompOff Credit request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (compOffCredit != null && compOffCredit.Status1 == true && compOffCredit.Status2 == null)
                        {
                            compOffCredit.Status2 = approveLeaveRequest.IsApproved;
                            compOffCredit.IsActive = false;
                            compOffCredit.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            compOffCredit.ApprovedOn = DateTime.UtcNow;
                        }
                        if (compOffCredit != null && compOffCredit.Status1 == null)
                        {
                            compOffCredit.Status1 = approveLeaveRequest.IsApproved;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                compOffCredit.IsActive = false;
                            }
                            compOffCredit.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            compOffCredit.UpdatedUtc = DateTime.UtcNow;
                        }
                    }

                    if (approveLeaveRequest.IsApproved)
                    {
                        if (approver2 == null)
                        {
                            if (compOffCredit != null && compOffCredit.Status1 == true)
                            {
                                compOffCredit.Balance = (compOffCredit.Balance ?? 0) + (int)compOffCredit.TotalDays;
                            }
                        }
                        if (approver2 != null)
                        {
                            if (compOffCredit != null && compOffCredit.Status1 == true && compOffCredit.Status2 == true)
                            {
                                compOffCredit.Balance = (compOffCredit.Balance ?? 0) + (int)compOffCredit.TotalDays;
                            }
                        }
                    }
                    await _context.SaveChangesAsync();

                    string requestedTime = compOffCredit.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = compOffCredit.UpdatedUtc.HasValue ? compOffCredit.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Comp-Off Credit request approved successfully" : "Comp-Off request Credit rejected successfully";

                    var notificationMessage = approveLeaveRequest.IsApproved
                        ? $"Your Comp-Off credit has been approved. Approved by - {approverName} on {approvedTime}"
                        : $"Your Comp-Off credit has been rejected. Rejected by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = compOffCredit.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();
                    compOffCredit.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendCompOffCreditApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: approveLeaveRequest.IsApproved,
                            workedDate: compOffCredit.WorkedDate,
                            balance: compOffCredit.Balance,
                            totalDays: compOffCredit.TotalDays,
                            reason: compOffCredit.Reason,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );
                        if (approveLeaveRequest.IsApproved && approver2 != null && (!compOffCredit.IsEmailSent.HasValue || compOffCredit.IsEmailSent == false))
                        {
                            await _emailService.SendCompOffCreditRequestEmail(
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                staffName: staffName,
                                id: compOffCredit.Id,
                                applicationTypeId: compOffCredit.ApplicationTypeId,
                                workedDate: compOffCredit.WorkedDate,
                                totalDays: compOffCredit.TotalDays,
                                balance: compOffCredit.Balance,
                                reason: compOffCredit.Reason,
                                requestDate: requestedTime,
                                createdBy: staffOrCreatorId
                            );
                            compOffCredit.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (approveLeaveRequest.ApplicationTypeId == 18)
                {
                    var reimbursementRequest = await _context.Reimbursements.FirstOrDefaultAsync(r => r.Id == item.Id);

                    /*                if (reimbursementRequest == null)
                                        throw new MessageNotFoundException("Reimbursement request not found");
                    */
                    var staffOrCreatorId = reimbursementRequest.StaffId ?? reimbursementRequest.CreatedBy;
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffOrCreatorId && s.IsActive == true);
                    if (staff == null) throw new MessageNotFoundException("Staff not found");

                    var staffName = $"{staff.FirstName} {staff.LastName}";
                    var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel1 && s.IsActive == true);
                    var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staff.ApprovalLevel2 && s.IsActive == true);

                    if (approver2 == null)
                    {
                        var reimbursementRequest1 = await _context.Reimbursements
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.UpdatedBy == approveLeaveRequest.ApprovedBy && l.IsActive == false);
                        if (reimbursementRequest1 != null)
                        {
                            if (reimbursementRequest1.Status1.HasValue && reimbursementRequest1.Status1 != null)
                            {
                                string statusMessage = reimbursementRequest.Status1 == true ? "Reimbursement request has already been approved." : "Reimbursement request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (reimbursementRequest != null && reimbursementRequest.Status1 == null)
                        {
                            reimbursementRequest.Status1 = approveLeaveRequest.IsApproved ? true : false;
                            reimbursementRequest.IsActive = false;
                            reimbursementRequest.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            reimbursementRequest.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    if (approver2 != null)
                    {
                        var reimbursementRequest1 = await _context.Reimbursements
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 == null && l.UpdatedBy == approveLeaveRequest.ApprovedBy);
                        if (reimbursementRequest1 != null)
                        {
                            if (reimbursementRequest1.Status1.HasValue && reimbursementRequest1.Status1 != null)
                            {
                                string statusMessage = reimbursementRequest.Status1 == true ? "Reimbursement request has already been approved." : "Reimbursement request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        var reimbursementRequest2 = await _context.Reimbursements
                            .FirstOrDefaultAsync(l => l.Id == item.Id && l.Status1 != null && l.Status2 != null && l.ApprovedBy == approveLeaveRequest.ApprovedBy);
                        if (reimbursementRequest2 != null)
                        {
                            if (reimbursementRequest2.Status2.HasValue && reimbursementRequest2.Status2 != null)
                            {
                                string statusMessage = reimbursementRequest.Status2 == true ? "Reimbursement request has already been approved." : "Reimbursement request has already been rejected.";
                                throw new InvalidOperationException(statusMessage);
                            }
                        }
                        if (reimbursementRequest != null && reimbursementRequest.Status1 == true && reimbursementRequest.Status2 == null)
                        {
                            reimbursementRequest.Status2 = approveLeaveRequest.IsApproved ? true : false;
                            reimbursementRequest.IsActive = false;
                            reimbursementRequest.ApprovedBy = approveLeaveRequest.ApprovedBy;
                            reimbursementRequest.ApprovedOn = DateTime.UtcNow;
                        }
                        if (reimbursementRequest != null && reimbursementRequest.Status1 == null)
                        {
                            reimbursementRequest.Status1 = approveLeaveRequest.IsApproved ? true : false;
                            if (!approveLeaveRequest.IsApproved)
                            {
                                reimbursementRequest.IsActive = false;
                            }
                            reimbursementRequest.UpdatedBy = approveLeaveRequest.ApprovedBy;
                            reimbursementRequest.UpdatedUtc = DateTime.UtcNow;
                        }
                    }

                    await _context.SaveChangesAsync();

                    string requestedTime = reimbursementRequest.CreatedUtc.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                    string approvedTime = reimbursementRequest.UpdatedUtc.HasValue ? reimbursementRequest.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");

                    message = approveLeaveRequest.IsApproved ? "Reimbursement request approved successfully" : "Reimbursement request rejected successfully";
                    var notificationMessage = $"Your reimbursement request has been approved. Approved by - {approverName} on {approvedTime}";

                    var notification = new ApprovalNotification
                    {
                        StaffId = reimbursementRequest.CreatedBy,
                        Message = notificationMessage,
                        IsActive = true,
                        CreatedBy = approveLeaveRequest.ApprovedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    _context.ApprovalNotifications.Add(notification);
                    await _context.SaveChangesAsync();

                    reimbursementRequest.ApprovalNotificationId = notification.Id;
                    await _context.SaveChangesAsync();

                    if (approver1 != null)
                    {
                        await _emailService.SendReimbursementApprovalEmail(
                            recipientEmail: staff.OfficialEmail,
                            recipientId: staff.Id,
                            recipientName: staffName,
                            isApproved: true,
                            billDate: reimbursementRequest.BillDate,
                            billNo: reimbursementRequest.BillNo,
                            description: reimbursementRequest.Description,
                            billPeriod: reimbursementRequest.BillPeriod,
                            amount: reimbursementRequest.Amount,
                            approvedBy: approveLeaveRequest.ApprovedBy,
                            approverName: approverName,
                            approvedTime: approvedTime
                        );

                        if (approveLeaveRequest.IsApproved && approver2 != null && (!reimbursementRequest.IsEmailSent.HasValue || reimbursementRequest.IsEmailSent == false))
                        {
                            await _emailService.SendReimbursementRequestEmail(
                                id: reimbursementRequest.Id,
                                applicationTypeId: reimbursementRequest.ApplicationTypeId,
                                recipientEmail: approver2.OfficialEmail,
                                recipientId: approver2.Id,
                                recipientName: $"{approver2.FirstName} {approver2.LastName}",
                                staffName: staffName,
                                requestDate: requestedTime,
                                billDate: reimbursementRequest.BillDate,
                                billNo: reimbursementRequest.BillNo,
                                description: reimbursementRequest.Description,
                                billPeriod: reimbursementRequest.BillPeriod,
                                amount: reimbursementRequest.Amount,
                                createdBy: staffOrCreatorId
                            );
                            reimbursementRequest.IsEmailSent = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            return message;
        }
    }
}
