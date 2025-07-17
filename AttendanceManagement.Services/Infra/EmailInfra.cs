using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Infrastructure.Data;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class EmailInfra : IEmailInfra
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly IConfiguration _configuration;
        public EmailInfra(AttendanceManagementSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SendApprovalEmail(string recipientEmail, string subject, string emailBody, int approvedBy)
        {
            var host = _configuration["Smtp:host"];
            var port = _configuration.GetValue<int>("Smtp:port");
            var userName = _configuration["Smtp:userName"];
            var password = _configuration["Smtp:password"];
            var from = _configuration["Smtp:from"];
            var ssl = _configuration.GetValue<bool>("Smtp:SSL");
            var defaultCredential = _configuration.GetValue<bool>("Smtp:defaultCredential");

            if (from != null)
            {
                try
                {
                    var message = new MailMessage
                    {
                        Subject = subject,
                        Body = emailBody,
                        IsBodyHtml = true,
                        From = new MailAddress(from, "HR Team")
                    };
                    message.To.Add(new MailAddress(recipientEmail));
                    using var smtp = new SmtpClient(host, port)
                    {
                        UseDefaultCredentials = defaultCredential,
                        Credentials = new NetworkCredential(userName, password),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = ssl
                    };
                    await smtp.SendMailAsync(message);
                    var log = new EmailLog
                    {
                        From = from,
                        To = recipientEmail,
                        EmailSubject = subject,
                        EmailBody = emailBody,
                        IsSent = true,
                        IsError = false,
                        CreatedBy = approvedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _context.EmailLogs.AddAsync(log);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var log = new EmailLog
                    {
                        From = from,
                        To = recipientEmail,
                        EmailSubject = subject,
                        EmailBody = emailBody,
                        IsSent = false,
                        IsError = true,
                        ErrorDescription = ex.Message,
                        ErrorStackTrace = ex.StackTrace ?? string.Empty,
                        ErrorInnerException = ex.InnerException?.ToString() ?? string.Empty,
                        CreatedBy = approvedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _context.EmailLogs.AddAsync(log);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task SendEmailWithAttachment(string recipientEmail, string subject, string emailBody, List<Attachment> attachments, int approvedBy)
        {
            var host = _configuration["Smtp:host"];
            var port = _configuration.GetValue<int>("Smtp:port");
            var userName = _configuration["Smtp:userName"];
            var password = _configuration["Smtp:password"];
            var from = _configuration["Smtp:from"];
            var ssl = _configuration.GetValue<bool>("Smtp:SSL");
            var defaultCredential = _configuration.GetValue<bool>("Smtp:defaultCredential");

            if (from != null)
            {
                try
                {
                    var message = new MailMessage
                    {
                        Subject = subject,
                        Body = emailBody,
                        IsBodyHtml = true,
                        From = new MailAddress(from, "HR Team")
                    };
                    message.To.Add(new MailAddress(recipientEmail));
                    using var smtp = new SmtpClient(host, port)
                    {
                        UseDefaultCredentials = defaultCredential,
                        Credentials = new NetworkCredential(userName, password),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = ssl
                    };
                    if (attachments != null)
                    {
                        foreach (var attachment in attachments)
                            message.Attachments.Add(attachment);
                    }
                    await smtp.SendMailAsync(message);
                    var log = new EmailLog
                    {
                        From = from,
                        To = recipientEmail,
                        EmailSubject = subject,
                        EmailBody = emailBody,
                        IsSent = true,
                        IsError = false,
                        CreatedBy = approvedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _context.EmailLogs.AddAsync(log);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var log = new EmailLog
                    {
                        From = from,
                        To = recipientEmail,
                        EmailSubject = subject,
                        EmailBody = emailBody,
                        IsSent = false,
                        IsError = true,
                        ErrorDescription = ex.Message,
                        ErrorStackTrace = ex.StackTrace ?? string.Empty,
                        ErrorInnerException = ex.InnerException?.ToString() ?? string.Empty,
                        CreatedBy = approvedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _context.EmailLogs.AddAsync(log);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task SendPendingStaffRequestEmail(string managerEmail, StaffCreation staff)
        {
            if (managerEmail != null && staff != null)
            {
                var reportingManager = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == staff.ApprovalLevel1 && u.OfficialEmail == managerEmail && u.IsActive == true);
                if (reportingManager != null)
                {
                    var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                    string Base64UrlEncode(string input)
                    {
                        return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                            .TrimEnd('=')
                            .Replace('+', '-')
                            .Replace('/', '_');
                    }
                    var staffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
                    var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                    var approvalJson = JsonSerializer.Serialize(new
                    {
                        staffId = staff.Id,
                        StaffCreationId = staff.StaffId,
                        StaffName = staffName,
                        Department = department,
                        IsApproved = true,
                        ApprovedBy = staff.CreatedBy
                    });
                    var rejectJson = JsonSerializer.Serialize(new
                    {
                        staffId = staff.Id,
                        StaffCreationId = staff.StaffId,
                        StaffName = staffName,
                        Department = department,
                        IsApproved = false,
                        ApprovedBy = staff.CreatedBy
                    });

                    string approvalEncoded = Base64UrlEncode(approvalJson);
                    string rejectEncoded = Base64UrlEncode(rejectJson);
                    string approvalLink = $"{frontEndUrl}/#/main/Employee/Approve?data={approvalEncoded}";
                    string rejectLink = $"{frontEndUrl}/#/main/Employee/Approve?data={rejectEncoded}";
                    string reportingManagerFullName = $"{reportingManager.FirstName}{(string.IsNullOrWhiteSpace(reportingManager.LastName) ? "" : " " + reportingManager.LastName)}";
                    string staffFullName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
                    string subject = "Staff Approval Request";

                    string emailBody = $@"
                        <p>Dear {reportingManagerFullName},</p>
                        <p>A new staff member <strong>{staffFullName}</strong> has been added and requires your approval.</p>       
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                    await SendApprovalEmail(managerEmail, subject, emailBody, staff.CreatedBy);
                }
            }
        }

        public async Task SendPendingStaffApprovalEmail(StaffCreation staff, StaffCreation createdByUser, ApprovePendingStaff approvePendingStaff)
        {
            var createdByUserName = $"{createdByUser.FirstName}{(string.IsNullOrWhiteSpace(createdByUser.LastName) ? "" : " " + createdByUser.LastName)}";
            var staffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
            var approver = await _context.StaffCreations.FirstOrDefaultAsync(a => a.Id == approvePendingStaff.ApprovedBy && a.IsActive == true);
            if (approver != null)
            {
                var approverName = $"{approver.FirstName}{(string.IsNullOrWhiteSpace(approver.LastName) ? "" : " " + approver.LastName)}";
                string approvedTime = staff.UpdatedUtc.HasValue ? staff.UpdatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss") : "N/A";

                string subject = approvePendingStaff.IsApproved ? "Staff Profile Approved" : "Staff Profile Rejected";
                string emailBody = $@"
                        <p>Dear {createdByUserName},</p>
                        <p>The staff profile for {staffName} has been {(approvePendingStaff.IsApproved ? "approved" : "rejected")} by {approverName} on {approvedTime}.</p>       
                        <br>Best Regards,<br>
                        HR Team";

                if (!string.IsNullOrEmpty(createdByUser.OfficialEmail))
                {
                    await SendApprovalEmail(createdByUser.OfficialEmail, subject, emailBody, approvePendingStaff.ApprovedBy);
                }
            }
        }

        public async Task SendLeaveRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int applicationTypeId, int id,  string leaveType, DateOnly fromDate, DateOnly toDate,
        string fromDuration, string? toDuration, decimal totalDays, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver1 = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver1 != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if (staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "New Leave Requisition Submitted";
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department,
                            LeaveType = leaveType
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department,
                            LeaveType = leaveType
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";
                        string fromDateFormatted = fromDate.ToString("dd-MMM-yyyy");
                        string toDateFormatted = toDate.ToString("dd-MMM-yyyy");

                        string emailBody = $@"
                            <p>Dear {recipientName},</p>
                            <p>A new Leave requisition has been submitted.</p>
                            <p><strong>Leave Type:</strong> {leaveType}</p>
                            <p><strong>From Date:</strong> {fromDateFormatted}</p>
                            <p><strong>To Date:</strong> {toDateFormatted}</p>
                            <p><strong>From Duration:</strong> {fromDuration}</p>
                            <p><strong>To Duration:</strong> {toDuration}</p>
                            <p><strong>Total Days:</strong> {totalDays}</p>
                            <p><strong>Reason:</strong> {reason}</p>
                            <p><strong>Requested By:</strong> {creatorName}</p>
                            <p><strong>Requested Time:</strong> {requestDate}</p>
                            <p>Please review this request and take appropriate action:</p>
                            <p>
                                <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                                <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                            </p>
                            <br>Best Regards,<br>
                            HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendCommonPermissionRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int? applicationTypeId, int id, string permissionType, DateOnly permissionDate,
        DateTime startTime, DateTime endTime, string duration, string remarks, int createdBy, string creatorName, string requestDate)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "New Permission Request Submitted";
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department,
                            PermissionType = permissionType
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department,
                            PermissionType = permissionType
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";
                        string permissionDateFormatted = permissionDate.ToString("dd-MMM-yyyy");
                        string startTimeFormatted = startTime.ToString("dd-MMM-yyyy hh:mm tt");
                        string endTimeFormatted = endTime.ToString("dd-MMM-yyyy hh:mm tt");

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new Permission request has been submitted.</p>
                        <p><strong>Permission Date:</strong> {permissionDateFormatted}</p>
                        <p><strong>Start Time:</strong> {startTimeFormatted}</p>
                        <p><strong>End Time:</strong> {endTimeFormatted}</p>
                        <p><strong>Permission Type:</strong> {permissionType}</p>
                        <p><strong>Duration:</strong> {duration}</p>
                        <p><strong>Reason:</strong> {remarks}</p>
                        <p><strong>Requested By:</strong> {creatorName}</p>
                        <p><strong>Request Date:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendManualPunchRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId, 
        string selectPunch, DateTime? inPunch, DateTime? outPunch, string remarks, int createdBy, string requestDate)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "Manual Punch Requisition Submitted";
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department,
                            SelectPunch = selectPunch
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department,
                            SelectPunch = selectPunch
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";
                        string inPunchFormatted = inPunch.HasValue ? inPunch.Value.ToString("hh:mm tt") : "N/A";
                        string outPunchFormatted = outPunch.HasValue ? outPunch.Value.ToString("hh:mm tt") : "N/A";

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new Manual Punch requisition has been submitted.</p>
                        <p><strong>Select Punch:</strong> {selectPunch}</p>";

                        if (inPunch.HasValue)
                            emailBody += $"<p><strong>In Punch:</strong> {inPunch.Value.ToString("hh:mm tt")}</p>";

                        if (outPunch.HasValue)
                            emailBody += $"<p><strong>Out Punch:</strong> {outPunch.Value.ToString("hh:mm tt")}</p>";

                        emailBody += $@"
                        <p><strong>Remarks:</strong> {remarks}</p>
                        <p><strong>Requested By:</strong> {staffName}</p>
                        <p><strong>Requested Time:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendOnDutyRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? startDate, DateOnly? endDate, DateTime? startTime,
        DateTime? endTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "New On Duty Requisition Submitted";
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();

                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";
                        string details;
                        if (startDate.HasValue && endDate.HasValue)
                        {
                            details = $@"
                            <p><strong>Start Date:</strong> {startDate.Value:dd-MMM-yyyy}</p>
                            <p><strong>End Date:</strong> {endDate.Value:dd-MMM-yyyy}</p>
                            <p><strong>Total Days:</strong> {totalDays}</p>";
                        }
                        else
                        {
                            details = $@"
                            <p><strong>Start Time:</strong> {startTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
                            <p><strong>End Time:</strong> {endTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
                            <p><strong>Total Hours:</strong> {totalHours}</p>";
                        }

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new On Duty requisition has been submitted.</p>
                        {details}
                        <p><strong>Reason:</strong> {reason}</p>
                        <p><strong>Requested By:</strong> {creatorName}</p>
                        <p><strong>Requested Time:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendBusinessTravelRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate,
        DateTime? fromTime, DateTime? toTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "New Business Travel Requisition Submitted";
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";
                        string details;
                        if (fromDate.HasValue && toDate.HasValue)
                        {
                            details = $@"
                            <p><strong>From Date:</strong> {fromDate.Value:dd-MMM-yyyy}</p>
                            <p><strong>To Date:</strong> {toDate.Value:dd-MMM-yyyy}</p>
                            <p><strong>Total Days:</strong> {totalDays}</p>";
                        }
                        else
                        {
                            details = $@"
                            <p><strong>From Time:</strong> {fromTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
                            <p><strong>To Time:</strong> {toTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
                            <p><strong>Total Hours:</strong> {totalHours}</p>";
                        }

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new Business Travel requisition has been submitted.</p>
                        {details}
                        <p><strong>Reason:</strong> {reason}</p>
                        <p><strong>Requested By:</strong> {creatorName}</p>
                        <p><strong>Requested Time:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendWorkFromHomeRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate,
        DateTime? fromTime, DateTime? toTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "New Work From Home Requisition Submitted";
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";
                        string details;
                        if (fromDate.HasValue && toDate.HasValue)
                        {
                            details = $@"
                            <p><strong>From Date:</strong> {fromDate.Value:dd-MMM-yyyy}</p>
                            <p><strong>To Date:</strong> {toDate.Value:dd-MMM-yyyy}</p>
                            <p><strong>Total Days:</strong> {totalDays}</p>";
                        }
                        else
                        {
                            details = $@"
                            <p><strong>From Time:</strong> {fromTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
                            <p><strong>To Time:</strong> {toTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
                            <p><strong>Total Hours:</strong> {totalHours}</p>";
                        }

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new Work From Home requisition has been submitted.</p>
                        {details}
                        <p><strong>Reason:</strong> {reason}</p>
                        <p><strong>Requested By:</strong> {creatorName}</p>
                        <p><strong>Requested Time:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendShiftChangeRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, string shiftName,
        DateOnly? fromDate, DateOnly? toDate, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "New Shift Change Requisition Submitted";
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";

                        string details = $@"
                        <p><strong>Shift Name:</strong> {shiftName}</p>
                        <p><strong>From Date:</strong> {fromDate?.ToString("dd-MMM-yyyy") ?? "N/A"}</p>
                        <p><strong>To Date:</strong> {toDate?.ToString("dd-MMM-yyyy") ?? "N/A"}</p>";

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new Shift Change requisition has been submitted.</p>
                        {details}
                        <p><strong>Reason:</strong> {reason}</p>
                        <p><strong>Requested By:</strong> {creatorName}</p>
                        <p><strong>Requested Time:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendShiftExtensionRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? transactionDate, string? durationHours,
        DateTime? beforeShiftHours, DateTime? afterShiftHours, string? remarks, int createdBy, string creatorName, string requestDate)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "New Shift Extension Requisition Submitted";
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = creatorName,
                            Department = department
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";
                        string details = $@"
                        <p><strong>Transaction Date:</strong> {transactionDate:dd-MMM-yyyy}</p>
                        <p><strong>Duration Hours:</strong> {durationHours}</p>";

                        if (beforeShiftHours.HasValue)
                            details += $"<p><strong>Before Shift Hours:</strong> {beforeShiftHours}</p>";

                        if (afterShiftHours.HasValue)
                            details += $"<p><strong>After Shift Hours:</strong> {afterShiftHours}</p>";

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new Shift Extension requisition has been submitted.</p>
                        {details}
                        <p><strong>Remarks:</strong> {remarks}</p>
                        <p><strong>Requested By:</strong> {creatorName}</p>
                        <p><strong>Requested Time:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p> 
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendWeeklyOffHolidayWorkingRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, string selectShiftType, int id, int applicationTypeId,
        DateOnly txnDate, string shiftName, DateTime? shiftInTime, DateTime? shiftOutTime, string requestDate, int createdBy)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "Weekly Off/ Holiday Working Request Submitted";
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";
                        string shiftInTimeFormatted = shiftInTime.HasValue ? shiftInTime.Value.ToString("dd-MMM-yyyy hh:mm tt") : "N/A";
                        string shiftOutTimeFormatted = shiftOutTime.HasValue ? shiftOutTime.Value.ToString("dd-MMM-yyyy hh:mm tt") : "N/A";

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new Weekly Off/ Holiday Working request has been submitted.</p>
                        <p><strong>Shift:</strong> {shiftName}</p>
                        <p><strong>Transaction Date:</strong> {txnDate:dd-MMM-yyyy}</p>";

                        if (shiftInTime.HasValue)
                            emailBody += $"<p><strong>Shift In Time:</strong> {shiftInTimeFormatted}</p>";

                        if (shiftOutTime.HasValue)
                            emailBody += $"<p><strong>Shift Out Time:</strong> {shiftOutTimeFormatted}</p>";

                        emailBody += $@"
                        <p><strong>Requested By:</strong> {staffName}</p>
                        <p><strong>Requested Time:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendCompOffCreditRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId,
        DateOnly workedDate, decimal totalDays, int? balance, string reason, string requestDate, int createdBy)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                    string subject = "CompOff Credit Request Submitted";
                    string Base64UrlEncode(string input)
                    {
                        return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                            .TrimEnd('=')
                            .Replace('+', '-')
                            .Replace('/', '_');
                    }
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";
                        string balanceText = balance.HasValue ? balance.Value.ToString() : "0";

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new CompOff Credit request has been submitted.</p>
                        <p><strong>Worked Date:</strong> {workedDate:dd-MMM-yyyy}</p>
                        <p><strong>Total Days Credited:</strong> {totalDays}</p>
                        <p><strong>Updated Balance:</strong> {balance}</p>
                        <p><strong>Reason:</strong> {reason}</p>
                        <p><strong>Requested By:</strong> {staffName}</p>
                        <p><strong>Requested Time:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendCompOffApprovalRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId,
        DateOnly workedDate, DateOnly fromDate, DateOnly toDate, decimal totalDays, string reason, string requestDate, int createdBy)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if(staff != null)
                    {
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "CompOff Avail Request Submitted";
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new CompOff Avail request has been submitted.</p>
                        <p><strong>Worked Date:</strong> {workedDate:dd-MMM-yyyy}</p>
                        <p><strong>CompOff Period:</strong> {fromDate:dd-MMM-yyyy} to {toDate:dd-MMM-yyyy}</p>
                        <p><strong>Total Days:</strong> {totalDays}</p>
                        <p><strong>Reason:</strong> {reason}</p>
                        <p><strong>Requested By:</strong> {staffName}</p>
                        <p><strong>Requested Time:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendReimbursementRequestEmail(
        int id, int? applicationTypeId, string recipientEmail, int recipientId, string recipientName, string staffName,
        string requestDate, DateOnly billDate, string billNo, string? description, string billPeriod, decimal amount, int createdBy)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver != null)
                {
                    var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
                    if (staff != null)
                    {
                        var department = await _context.DepartmentMasters.Where(d => d.Id == staff.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefaultAsync();
                        var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                        string subject = "Reimbursement Request Submitted";
                        string Base64UrlEncode(string input)
                        {
                            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                        }
                        var approvalJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = true,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department
                        });
                        var rejectJson = JsonSerializer.Serialize(new
                        {
                            Id = id,
                            IsApproved = false,
                            ApplicationTypeId = applicationTypeId,
                            ApprovedBy = recipientId,
                            StaffCreationId = staff.StaffId,
                            StaffName = staffName,
                            Department = department
                        });
                        string approvalEncoded = Base64UrlEncode(approvalJson);
                        string rejectEncoded = Base64UrlEncode(rejectJson);
                        string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={approvalEncoded}";
                        string rejectLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?data={rejectEncoded}";

                        string emailBody = $@"
                        <p>Dear {recipientName},</p>
                        <p>A new Reimbursement request has been submitted.</p>
                        <p><strong>Requested By:</strong> {staffName}</p>
                        <p><strong>Department:</strong> {department}</p>
                        <p><strong>Bill Date:</strong> {billDate:dd-MMM-yyyy}</p>
                        <p><strong>Bill No:</strong> {billNo}</p>
                        <p><strong>Description:</strong> {description}</p>
                        <p><strong>Bill Period:</strong> {billPeriod}</p>
                        <p><strong>Amount:</strong> ₹{amount:F2}</p>
                        <p><strong>Requested On:</strong> {requestDate}</p>
                        <p>Please review this request and take appropriate action:</p>
                        <p>
                            <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
                            <a href='{rejectLink}' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none;'>Reject</a>
                        </p>
                        <br>Best Regards,<br>
                        HR Team";

                        await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                    }
                }
            }
        }

        public async Task SendProbationNotificationToHrAsync(string probationerName, DateOnly probationStartDate, DateOnly probationEndDate)
        {
            var toEmail = _configuration["Smtp:to"];
            if (toEmail != null)
            {
                var createdBy = int.TryParse(_configuration["Smtp:mailTriggerId"], out var id) ? id : 4;
                var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                //string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?staffId={createdBy}";
                var subject = $"Probation Confirmation Required: {probationerName}";
                var body = $@"
                <p>Dear HR Team,</p>

                <p>This is to notify you that the probation period for <strong>{probationerName}</strong> has ended on <strong>{probationEndDate:dd-MMM-yyyy}</strong>.</p>

                <p>Details:</p>
                <ul>
                    <li><strong>Probationer:</strong> {probationerName}</li>
                    <li><strong>Probation Start Date:</strong> {probationStartDate:dd-MMM-yyyy}</li>
                    <li><strong>Probation End Date:</strong> {probationEndDate:dd-MMM-yyyy}</li>
                </ul>

                <p>Please initiate the confirmation process at your earliest convenience.</p>
                <p>Regards,<br/>Attendance Management System</p>";

                await SendApprovalEmail(toEmail, subject, body, createdBy);
            }
        }

        public async Task AssignManager(string recipientEmail, int recipientId, string recipientName, string probationerName, DateOnly startDate, DateOnly endDate, int createdBy)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var receiver1 = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (receiver1 != null)
                {
                    var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                    //string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?staffId={recipientId}";
                    string subject = "Probation Confirmation Review Assignment";

                    string fromDateFormatted = startDate.ToString("dd-MMM-yyyy");
                    string toDateFormatted = endDate.ToString("dd-MMM-yyyy");

                    string emailBody = $@"
                    <p>Dear {recipientName},</p>

                    <p>You have been assigned as the manager to review a probation confirmation request.</p>

                    <p><strong>Probationer's Name:</strong> {probationerName}<br/>
                    <strong>Probation Start Date:</strong> {fromDateFormatted}<br/>
                    <strong>Probation End Date:</strong> {toDateFormatted}</p>

                    <p>Please log in to the system to review and take the necessary action.</p>

                    <br>Best Regards,<br>
                    HR Team";

                    await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
                }
            }
        }

        public async Task SendProbationConfirmationNotificationToHrAsync(int recipientId, string probationerName, DateOnly startDate, DateOnly endDate, DateOnly? extensionPeriod, bool isApproved, string approverName, string approvedTime, int approvedBy)
        {
            var recipientEmail = _configuration["Smtp:to"];
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var recipient = await _context.StaffCreations.FirstOrDefaultAsync(u => u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (recipient != null)
                {
                    string subject = isApproved ? "Probation Confirmation Approved" : "Probation Confirmation Extended";
                    string statusMessage = isApproved ? "approved" : "extended";
                    string actionBy = isApproved ? "Approved by" : "Extended by";
                    string startDateFormatted = startDate.ToString("dd-MMM-yyyy");
                    string endDateFormatted = endDate.ToString("dd-MMM-yyyy");
                    var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
                    //string approvalLink = $"{frontEndUrl}/#/main/Tools/MyApprovalsTools?staffId={recipientId}";
                    //string actionText = $"You can view the details <a href='{approvalLink}'>here</a>.";
                    string emailBody = $@"
                    <p>Dear HR Team,</p>

                    <p>The probation confirmation request for the following employee has been <strong>{statusMessage}</strong>:</p>

                    <p><strong>Probationer's Name:</strong> {probationerName}</p>
                    <p><strong>Probation Start Date:</strong> {startDateFormatted}</p>
                    <p><strong>Probation End Date:</strong> {endDateFormatted}</p>";

                    if (!isApproved && extensionPeriod.HasValue)
                    {
                        string extensionPeriodFormatted = extensionPeriod.Value.ToString("dd-MMM-yyyy");
                        emailBody += $@"
                        <p><strong>Extension Period:</strong> {extensionPeriodFormatted}</p>";
                    }

                    emailBody += $@"
                    <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
                    <br>Best Regards,<br>
                    Attendance Management System";

                    await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
                }
            }
        }

        public async Task SendMailToMis(string dropDown, int createdBy, List<SelectedEmployeesForAppraisal> staffList)
        {
            var recipientEmail = _configuration["Smtp:mis"];
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var recipient = await _context.StaffCreations.FirstOrDefaultAsync(u => u.OfficialEmail == recipientEmail && u.IsActive == true);
                if (recipient != null)
                {
                    var subject = $"Selected Employees Moved to MIS - {dropDown}";
                    var bodyBuilder = new StringBuilder();
                    bodyBuilder.AppendLine("The following employees have been moved to MIS:<br/><ul>");
                    foreach (var staff in staffList)
                    {
                        bodyBuilder.AppendLine($"<li>{staff.EmployeeId} - {staff.EmployeeName} ({staff.Department})</li>");
                    }
                    bodyBuilder.AppendLine("</ul>");
                    await SendApprovalEmail(recipientEmail, subject, bodyBuilder.ToString(), createdBy);
                }
            }
        }

        public async Task SendMisUploadNotificationToHr(int createdBy, string name, string dropDown)
        {
            var hrEmail = _configuration["Smtp:hr"];
            if (!string.IsNullOrEmpty(hrEmail))
            {
                var subject = $"MIS Sheet Uploaded for {dropDown}";
                var body = $@"
                Dear HR Team,<br/><br/>
                The MIS sheet has been successfully uploaded.<br/><br/>
                <b>Uploaded By:</b> {name}<br/>
                Please review and proceed with the necessary steps.<br/><br/>
                Regards,<br/>
                Attendance Management System";

                await SendApprovalEmail(hrEmail, subject, body, createdBy);
            }
        }

        public async Task SendAgmApprovalNotification(int createdBy, string? recipientEmail, string name)
        {
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var subject = $"Employees Moved to AGM Approval";
                var body = $@"
                Dear {name},<br/><br/>
                The following staff member's appraisal records have been moved for AGM approval.<br/><br/>
                Kindly proceed with the next level of approval.<br/><br/>
                Regards,<br/>
                HR Team";

                await SendApprovalEmail(recipientEmail, subject, body, createdBy);
            }
        }

        public async Task SendHrApprovalNotification(int approvedBy, string approver)
        {
            var recipientEmail = _configuration["Smtp:hr"];
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var subject = $"AGM Approved - Employee Appraisal";
                var body = $@"
                Dear HR Team,<br/><br/>
                The following employee's performance review has been approved by AGM and is ready for final HR processing<br/><br/>
                <b>Approved By:</b> {approver}<br/><br/>
                Please proceed with the final HR actions.<br/><br/>
                Regards,<br/>
                Attendance Management System";

                await SendApprovalEmail(recipientEmail, subject, body, approvedBy);
            }
        }

        public async Task SendAppraisalEmailAsync(string toEmail, string staffName, byte[] attachmentBytes, string attachmentName, int createdBy)
        {
            if (!string.IsNullOrEmpty(toEmail))
            {
                var subject = "Appraisal Letter";
                var body = $@"
                Dear {staffName},<br/><br/>
                Please find attached your appraisal letter for the current year.<br/><br/>
                Regards,<br/>
                HR Team";

                var stream = new MemoryStream(attachmentBytes);
                using (var attachment = new Attachment(stream, attachmentName, MediaTypeNames.Application.Pdf))
                {
                    await SendEmailWithAttachment(recipientEmail: toEmail, subject: subject, emailBody: body,
                        attachments: new List<Attachment> { attachment }, approvedBy: createdBy);
                }
            }
        }
    }
}