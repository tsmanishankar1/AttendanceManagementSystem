using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using System.Net.Mail;

namespace AttendanceManagement.Application.App
{
    public class EmailApp : IEmailApp
    {
        private readonly IEmailInfra _emailInfra;
        public EmailApp(IEmailInfra emailInfra)
        {
            _emailInfra = emailInfra;
        }

        public async Task AssignManager(string recipientEmail, int recipientId, string recipientName, string probationerName, DateOnly startDate, DateOnly endDate, int createdBy)
            => await _emailInfra.AssignManager(recipientEmail, recipientId, recipientName, probationerName, startDate, endDate, createdBy);

        public async Task SendAgmApprovalNotification(int createdBy, string? recipientEmail, string name)
            => await _emailInfra.SendAgmApprovalNotification(createdBy, recipientEmail, name);

        public async Task SendAppraisalEmailAsync(string toEmail, string staffName, byte[] attachmentBytes, string attachmentName, int createdBy)
            => await _emailInfra.SendAppraisalEmailAsync(toEmail, staffName, attachmentBytes, attachmentName, createdBy);

        public async Task SendApprovalEmail(string recipientEmail, string subject, string emailBody, int approvedBy)
            => await _emailInfra.SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);

        public async Task SendBusinessTravelRequestEmail(string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate, DateTime? fromTime, DateTime? toTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate)
            => await _emailInfra.SendBusinessTravelRequestEmail(recipientEmail, recipientId, recipientName, id, applicationTypeId, fromDate, toDate, fromTime, toTime, totalDays, totalHours, reason, createdBy, creatorName, requestDate);

        public async Task SendCommonPermissionRequestEmail(string recipientEmail, int recipientId, string recipientName, int? applicationTypeId, int id, string permissionType, DateOnly permissionDate, DateTime startTime, DateTime endTime, string duration, string remarks, int createdBy, string creatorName, string requestDate)
            => await _emailInfra.SendCommonPermissionRequestEmail(recipientEmail, recipientId, recipientName, applicationTypeId, id, permissionType, permissionDate, startTime, endTime, duration, remarks, createdBy, creatorName, requestDate);

        public async Task SendCompOffApprovalRequestEmail(string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId, DateOnly workedDate, DateOnly fromDate, DateOnly toDate, decimal totalDays, string reason, string requestDate, int createdBy)
            => await _emailInfra.SendCompOffApprovalRequestEmail(recipientEmail, recipientId, recipientName, staffName, id, applicationTypeId, workedDate, fromDate, toDate, totalDays, reason, requestDate, createdBy);

        public async Task SendCompOffCreditRequestEmail(string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId, DateOnly workedDate, decimal totalDays, int? balance, string reason, string requestDate, int createdBy)
            => await _emailInfra.SendCompOffCreditRequestEmail(recipientEmail, recipientId, recipientName, staffName, id, applicationTypeId, workedDate, totalDays, balance, reason, requestDate, createdBy);

        public async Task SendEmailWithAttachment(string recipientEmail, string subject, string emailBody, List<Attachment> attachments, int approvedBy)
            => await _emailInfra.SendEmailWithAttachment(recipientEmail, subject, emailBody, attachments, approvedBy);

        public async Task SendHrApprovalNotification(int approvedBy, string approver)
            => await _emailInfra.SendHrApprovalNotification(approvedBy, approver);

        public async Task SendLeaveRequestEmail(string recipientEmail, int recipientId, string recipientName, int applicationTypeId, int id, string leaveType, DateOnly fromDate, DateOnly toDate, string fromDuration, string? toDuration, decimal totalDays, string reason, int createdBy, string creatorName, string requestDate)
            => await _emailInfra.SendLeaveRequestEmail(recipientEmail, recipientId, recipientName, applicationTypeId, id, leaveType, fromDate, toDate, fromDuration, toDuration, totalDays, reason, createdBy, creatorName, requestDate);

        public async Task SendMailToMis(string dropDown, int createdBy, List<SelectedEmployeesForAppraisal> staffList)
            => await _emailInfra.SendMailToMis(dropDown, createdBy, staffList);

        public async Task SendManualPunchRequestEmail(string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId, string selectPunch, DateTime? inPunch, DateTime? outPunch, string remarks, int createdBy, string requestDate)
            => await _emailInfra.SendManualPunchRequestEmail(recipientEmail, recipientId, recipientName, staffName, id, applicationTypeId, selectPunch, inPunch, outPunch, remarks, createdBy, requestDate);

        public async Task SendMisUploadNotificationToHr(int createdBy, string name, string dropDown)
            => await _emailInfra.SendMisUploadNotificationToHr(createdBy, name, dropDown);

        public async Task SendOnDutyRequestEmail(string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? startDate, DateOnly? endDate, DateTime? startTime, DateTime? endTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate)
            => await _emailInfra.SendOnDutyRequestEmail(recipientEmail, recipientId, recipientName, id, applicationTypeId, startDate, endDate, startTime, endTime, totalDays, totalHours, reason, createdBy, creatorName, requestDate);

        public async Task SendPendingStaffApprovalEmail(StaffCreation staff, StaffCreation createdByUser, ApprovePendingStaff approvePendingStaff)
            => await _emailInfra.SendPendingStaffApprovalEmail(staff, createdByUser, approvePendingStaff);

        public async Task SendPendingStaffRequestEmail(string managerEmail, StaffCreation staff)
            => await _emailInfra.SendPendingStaffRequestEmail(managerEmail, staff);

        public async Task SendProbationConfirmationNotificationToHrAsync(int recipientId, string probationerName, DateOnly startDate, DateOnly endDate, DateOnly? extensionPeriod, bool isApproved, string approverName, string approvedTime, int approvedBy)
            => await _emailInfra.SendProbationConfirmationNotificationToHrAsync(recipientId, probationerName, startDate, endDate, extensionPeriod, isApproved, approverName, approvedTime, approvedBy);

        public async Task SendProbationNotificationToHrAsync(string probationerName, DateOnly probationStartDate, DateOnly probationEndDate)
            => await _emailInfra.SendProbationNotificationToHrAsync(probationerName, probationStartDate, probationEndDate);

        public async Task SendReimbursementRequestEmail(int id, int? applicationTypeId, string recipientEmail, int recipientId, string recipientName, string staffName, string requestDate, DateOnly billDate, string billNo, string? description, string billPeriod, decimal amount, int createdBy)
            => await _emailInfra.SendReimbursementRequestEmail(id, applicationTypeId, recipientEmail, recipientId, recipientName, staffName, requestDate, billDate, billNo, description, billPeriod, amount, createdBy);

        public async Task SendShiftChangeRequestEmail(string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, string shiftName, DateOnly? fromDate, DateOnly? toDate, string reason, int createdBy, string creatorName, string requestDate)
            => await _emailInfra.SendShiftChangeRequestEmail(recipientEmail, recipientId, recipientName, id, applicationTypeId, shiftName, fromDate, toDate, reason, createdBy, creatorName, requestDate);

        public async Task SendShiftExtensionRequestEmail(string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? transactionDate, string? durationHours, DateTime? beforeShiftHours, DateTime? afterShiftHours, string? remarks, int createdBy, string creatorName, string requestDate)
            => await _emailInfra.SendShiftExtensionRequestEmail(recipientEmail, recipientId, recipientName, id, applicationTypeId, transactionDate, durationHours, beforeShiftHours, afterShiftHours, remarks, createdBy, creatorName, requestDate);

        public async Task SendWeeklyOffHolidayWorkingRequestEmail(string recipientEmail, int recipientId, string recipientName, string staffName, string selectShiftType, int id, int applicationTypeId, DateOnly txnDate, string shiftName, DateTime? shiftInTime, DateTime? shiftOutTime, string requestDate, int createdBy)
            => await _emailInfra.SendWeeklyOffHolidayWorkingRequestEmail(recipientEmail, recipientId, recipientName, staffName, selectShiftType, id, applicationTypeId, txnDate, shiftName, shiftInTime, shiftOutTime, requestDate, createdBy);

        public async Task SendWorkFromHomeRequestEmail(string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate, DateTime? fromTime, DateTime? toTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate)
            => await _emailInfra.SendWorkFromHomeRequestEmail(recipientEmail, recipientId, recipientName, id, applicationTypeId, fromDate, toDate, fromTime, toTime, totalDays, totalHours, reason, createdBy, creatorName, requestDate);
    }
}