using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IEmailService
    {
        Task SendApprovalEmail(string recipientEmail, string subject, string emailBody, int approvedBy);
        Task SendEmailWithAttachment(string recipientEmail, string subject, string emailBody, List<Attachment> attachments, int approvedBy);
        Task SendPendingStaffRequestEmail(string managerEmail, StaffCreation staff);
        Task SendPendingStaffApprovalEmail(StaffCreation staff, StaffCreation createdByUser, ApprovePendingStaff approvePendingStaff);
        Task SendLeaveRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int applicationTypeId, int id, string leaveType, DateOnly fromDate, DateOnly toDate,
        string fromDuration, string? toDuration, decimal totalDays, string reason, int createdBy, string creatorName, string requestDate);
        Task SendCommonPermissionRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int? applicationTypeId, int id, string permissionType, DateOnly permissionDate,
        TimeOnly startTime, TimeOnly endTime, string duration, string remarks, int createdBy, string creatorName, string requestDate);
        Task SendManualPunchRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId,
        string selectPunch, DateTime? inPunch, DateTime? outPunch, string remarks, int createdBy, string requestDate);
        Task SendOnDutyRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? startDate, DateOnly? endDate, DateTime? startTime,
        DateTime? endTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate);
        Task SendBusinessTravelRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate,
        DateTime? fromTime, DateTime? toTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate);
        Task SendWorkFromHomeRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate,
        DateTime? fromTime, DateTime? toTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate);
        Task SendShiftChangeRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, string shiftName,
        DateOnly? fromDate, DateOnly? toDate, string reason, int createdBy, string creatorName, string requestDate);
        Task SendShiftExtensionRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? transactionDate, string? durationHours,
        DateTime? beforeShiftHours, DateTime? afterShiftHours, string? remarks, int createdBy, string creatorName, string requestDate);
        Task SendWeeklyOffHolidayWorkingRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, string selectShiftType, int id, int applicationTypeId,
        DateOnly txnDate, string shiftName, DateTime? shiftInTime, DateTime? shiftOutTime, string requestDate, int createdBy);
        Task SendCompOffCreditRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId,
        DateOnly workedDate, decimal totalDays, int? balance, string reason, string requestDate, int createdBy);
        Task SendCompOffApprovalRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId,
        DateOnly workedDate, DateOnly fromDate, DateOnly toDate, decimal totalDays, string reason, string requestDate, int createdBy);
        Task SendReimbursementRequestEmail(
        int id, int? applicationTypeId, string recipientEmail, int recipientId, string recipientName, string staffName,
        string requestDate, DateOnly billDate, string billNo, string? description, string billPeriod, decimal amount, int createdBy);
        Task SendProbationNotificationToHrAsync(string probationerName, DateOnly probationStartDate, DateOnly probationEndDate);
        Task AssignManager(string recipientEmail, int recipientId, string recipientName, string probationerName, DateOnly startDate, DateOnly endDate, int createdBy);
        Task SendProbationConfirmationNotificationToHrAsync(int recipientId, string probationerName, DateOnly startDate, DateOnly endDate, DateOnly? extensionPeriod, bool isApproved, string approverName, string approvedTime, int approvedBy);
        Task SendMailToMis(string dropDown, int createdBy, List<SelectedEmployeesForAppraisal> staffList);
        Task SendMisUploadNotificationToHr(int createdBy, string name, string dropDown);
        Task SendAgmApprovalNotification(int createdBy, string? recipientEmail, string name);
        Task SendHrApprovalNotification(int approvedBy, string approver);
        Task SendAppraisalEmailAsync(string toEmail, string staffName, byte[] attachmentBytes, string attachmentName, int createdBy);
    }
}
