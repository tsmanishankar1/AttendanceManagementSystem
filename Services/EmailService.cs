using AttendanceManagement.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using AttendanceManagement.Input_Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Org.BouncyCastle.Cms;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AttendanceManagement.Services
{
    public class EmailService
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly IConfiguration _configuration;
        public EmailService(AttendanceManagementSystemContext context, IConfiguration configuration)
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

            if (from == null) throw new MessageNotFoundException("Sender not found");
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
                _context.EmailLogs.Add(log);
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
                    CreatedBy = approvedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                _context.EmailLogs.Add(log);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendApprovalEmail(string managerEmail, StaffCreation staff)
        {
            if (staff == null || string.IsNullOrEmpty(managerEmail)) throw new ArgumentNullException("Staff or Manager Email is null.");
            var reportingManager = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == staff.ApprovalLevel1 && u.OfficialEmail == managerEmail && u.IsActive == true);
            if (reportingManager == null) throw new Exception("Reporting manager not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string Base64UrlEncode(string input)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
            var staffName = $"{staff.FirstName} {staff.LastName}";
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
            string reportingManagerFullName = $"{reportingManager.FirstName} {reportingManager.LastName}";
            string staffFullName = $"{staff.FirstName} {staff.LastName}";
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

        public async Task SendLeaveRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int applicationTypeId, int id,  string leaveType, DateOnly fromDate, DateOnly toDate,
        string fromDuration, string toDuration, decimal totalDays, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Approver email not found");
            var receiver1 = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver1 == null) throw new Exception("Approver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";
            string fromDateFormatted = fromDate.ToString("dd-MMM-yyyy");
            string toDateFormatted = toDate.ToString("dd-MMM-yyyy");

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>A new leave requisition has been submitted.</p>
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

        public async Task SendCommonPermissionRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int? applicationTypeId, int id, string permissionType,
        DateOnly permissionDate, TimeOnly startTime, TimeOnly endTime, string duration, string remarks,
        int createdBy, string creatorName, string requestDate)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";
            string permissionDateFormatted = permissionDate.ToString("dd-MMM-yyyy");
            string startTimeFormatted = startTime.ToString("hh:mm tt");
            string endTimeFormatted = endTime.ToString("hh:mm tt");

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>A new permission request has been submitted.</p>
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

        public async Task SendManualPunchRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId, 
        string selectPunch, DateTime? inPunch, DateTime? outPunch, string remarks, int createdBy, string requestDate)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";
            string inPunchFormatted = inPunch.HasValue ? inPunch.Value.ToString("hh:mm tt") : "N/A";
            string outPunchFormatted = outPunch.HasValue ? outPunch.Value.ToString("hh:mm tt") : "N/A";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>A new manual punch requisition has been submitted.</p>
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

        public async Task SendOnDutyRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? startDate, DateOnly? endDate, DateTime? startTime,
        DateTime? endTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string subject = "New On-Duty Requisition Submitted";
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";
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
            <p>A new on-duty requisition has been submitted.</p>
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

        public async Task SendBusinessTravelRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate,
        DateTime? fromTime, DateTime? toTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";
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
            <p>A new business travel requisition has been submitted.</p>
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

        public async Task SendWorkFromHomeRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? fromDate, DateOnly? toDate,
        DateTime? fromTime, DateTime? toTime, decimal? totalDays, string? totalHours, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";
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
            <p>A new work from home requisition has been submitted.</p>
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

        public async Task SendShiftChangeRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, string shiftName,
        DateOnly? fromDate, DateOnly? toDate, string reason, int createdBy, string creatorName, string requestDate)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";

            string details = $@"
            <p><strong>Shift Name:</strong> {shiftName}</p>
            <p><strong>From Date:</strong> {fromDate?.ToString("dd-MMM-yyyy") ?? "N/A"}</p>
            <p><strong>To Date:</strong> {toDate?.ToString("dd-MMM-yyyy") ?? "N/A"}</p>";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>A new shift change requisition has been submitted.</p>
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

        public async Task SendShiftExtensionRequestEmail(
        string recipientEmail, int recipientId, string recipientName, int id, int applicationTypeId, DateOnly? transactionDate, string? durationHours,
        DateTime? beforeShiftHours, DateTime? afterShiftHours, string remarks, int createdBy, string creatorName, string requestDate)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";
            string details = $@"
            <p><strong>Transaction Date:</strong> {transactionDate:dd-MMM-yyyy}</p>
            <p><strong>Duration Hours:</strong> {durationHours}</p>";

            if (beforeShiftHours.HasValue)
                details += $"<p><strong>Before Shift Hours:</strong> {beforeShiftHours}</p>";

            if (afterShiftHours.HasValue)
                details += $"<p><strong>After Shift Hours:</strong> {afterShiftHours}</p>";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>A new shift extension requisition has been submitted.</p>
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

        public async Task SendWeeklyOffHolidayWorkingRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, string selectShiftType, int id, int applicationTypeId,
        DateOnly txnDate, string shiftName, DateTime? shiftInTime, DateTime? shiftOutTime, string requestDate, int createdBy)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string subject = "Weekly Off/Holiday Working Request Submitted";
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";
            string shiftInTimeFormatted = shiftInTime.HasValue ? shiftInTime.Value.ToString("dd-MMM-yyyy hh:mm tt") : "N/A";
            string shiftOutTimeFormatted = shiftOutTime.HasValue ? shiftOutTime.Value.ToString("dd-MMM-yyyy hh:mm tt") : "N/A";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>A new weekly off/holiday working request has been submitted.</p>
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

        public async Task SendCompOffCreditRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId,
        DateOnly workedDate, decimal totalDays, int? balance, string reason, string requestDate, int createdBy)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string subject = "Comp-Off Credit Request Submitted";
            string Base64UrlEncode(string input)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";
            string balanceText = balance.HasValue ? balance.Value.ToString() : "N/A";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>A new Comp-Off credit request has been submitted.</p>
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

        public async Task SendCompOffApprovalRequestEmail(
        string recipientEmail, int recipientId, string recipientName, string staffName, int id, int applicationTypeId,
        DateOnly workedDate, DateOnly fromDate, DateOnly toDate, decimal totalDays, string reason, string requestDate, int createdBy)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string subject = "Comp-Off Avail Request Submitted";
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>A new Comp-Off avail request has been submitted.</p>
            <p><strong>Worked Date:</strong> {workedDate:dd-MMM-yyyy}</p>
            <p><strong>Comp-Off Period:</strong> {fromDate:dd-MMM-yyyy} to {toDate:dd-MMM-yyyy}</p>
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

        public async Task SendReimbursementRequestEmail(
        int id, int? applicationTypeId, string recipientEmail, int recipientId, string recipientName, string staffName,
        string requestDate, DateOnly billDate, string billNo, string description, string billPeriod, decimal amount, int createdBy)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Recipient email not found");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == createdBy && s.IsActive == true);
            if (staff == null) throw new Exception("Staff not found");
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
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={approvalEncoded}";
            string rejectLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?data={rejectEncoded}";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>A new reimbursement request has been submitted.</p>
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

        public async Task SendLeaveApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, string leaveType, decimal totalDays,
        DateOnly fromDate, DateOnly toDate, string reason, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string subject = isApproved ? "Leave Request Approved" : "Leave Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/approve?staffId={recipientId}";
            string actionText = isApproved ? $"You can view the details <a href='{webApprovalLink}'>here</a>." : "If you have any questions, please contact HR.";
            string fromDateFormatted = fromDate.ToString("dd-MMM-yyyy");
            string toDateFormatted = toDate.ToString("dd-MMM-yyyy");

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your leave request has been <strong>{statusMessage}</strong>.</p>
            <p><strong>Leave Details:</strong></p>
            <ul>
                <li><strong>Leave Type:</strong> {leaveType}</li>
                <li><strong>From Date:</strong> {fromDateFormatted}</li>
                <li><strong>To Date:</strong> {toDateFormatted}</li>
                <li><strong>Total Days:</strong> {totalDays}</li>
                <li><strong>Reason:</strong> {reason}</li>
            </ul>
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendCommonPermissionApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, string permissionType,
        DateOnly permissionDate, TimeOnly startTime, TimeOnly endTime, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string subject = isApproved ? "Common Permission Request Approved" : "Common Permission Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/permission-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

            string permissionDateFormatted = permissionDate.ToString("dd-MMM-yyyy");
            string startTimeFormatted = startTime.ToString("hh:mm tt");
            string endTimeFormatted = endTime.ToString("hh:mm tt");

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your request for <strong>{permissionType}</strong> has been <strong>{statusMessage}</strong>.</p>
            <p><strong>Permission Details:</strong></p>
            <ul>
                <li><strong>Permission Type:</strong> {permissionType}</li>
                <li><strong>Permission Date:</strong> {permissionDateFormatted}</li>
                <li><strong>Start Time:</strong> {startTimeFormatted}</li>
                <li><strong>End Time:</strong> {endTimeFormatted}</li>
            </ul>
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendManualPunchApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved,
        string punchType, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "Manual Punch Request Approved" : "Manual Punch Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/manual-punch-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your manual punch request has been <strong>{statusMessage}</strong>.</p>
            <p><strong>Punch Type:</strong> {punchType}</p>
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendOnDutyApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, DateOnly? startDate, DateOnly? endDate,
        DateTime? startTime, DateTime? endTime, decimal? totalDays, string totalHours, string reason, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "On-Duty Request Approved" : "On-Duty Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/on-duty-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

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
            <p>Your on-duty request has been <strong>{statusMessage}</strong>.</p>
            {details}
            <p><strong>Reason:</strong> {reason}</p>
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }


        public async Task SendBusinessTravelApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, DateOnly? fromDate, DateOnly? toDate, DateTime? fromTime, DateTime? toTime,
        string reason, decimal? totalDays, string totalHours, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "Business Travel Request Approved" : "Business Travel Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/business-travel-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

            string details;
            if (fromDate.HasValue && toDate.HasValue)
            {
                details = $@"
            <p><strong>From Date:</strong> {fromDate.Value:dd-MMM-yyyy}</p>
            <p><strong>To Date:</strong> {toDate.Value:dd-MMM-yyyy}</p>
            <p><strong>Reason:</strong> {reason}</p>
            <p><strong>Total Days:</strong> {totalDays}</p>";
            }
            else
            {
                details = $@"
            <p><strong>From Time:</strong> {fromTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
            <p><strong>To Time:</strong> {toTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
            <p><strong>Reason:</strong> {reason}</p>
            <p><strong>Total Hours:</strong> {totalHours}</p>";
            }

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your business travel request has been <strong>{statusMessage}</strong>.</p>
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            {details}
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendWorkFromHomeApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, DateOnly? fromDate, DateOnly? toDate, DateTime? fromTime, DateTime? toTime,
        string reason, decimal? totalDays, string? totalHours, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "Work From Home Request Approved" : "Work From Home Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/wfh-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

            string details;

            if (fromDate.HasValue && toDate.HasValue)
            {
                details = $@"
            <p><strong>From Date:</strong> {fromDate.Value:dd-MMM-yyyy}</p>
            <p><strong>To Date:</strong> {toDate.Value:dd-MMM-yyyy}</p>
            <p><strong>Reason:</strong> {reason}</p>
            <p><strong>Total Days:</strong> {totalDays}</p>";
            }
            else
            {
                details = $@"
            <p><strong>From Time:</strong> {fromTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
            <p><strong>To Time:</strong> {toTime?.ToString("dd-MMM-yyyy hh:mm tt")}</p>
            <p><strong>Reason:</strong> {reason}</p>
            <p><strong>Total Hours:</strong> {totalHours}</p>";
            }

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your Work From Home request has been <strong>{statusMessage}</strong>.</p>
            {details}
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendShiftChangeApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, string shiftName,
        DateOnly fromDate, DateOnly toDate, string reason, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "Shift Change Request Approved" : "Shift Change Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/shift-change-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your shift change request has been <strong>{statusMessage}</strong>.</p>
            <p><strong>Shift Name:</strong> {shiftName}</p>
            <p><strong>From Date:</strong> {fromDate:dd-MMM-yyyy}</p>
            <p><strong>To Date:</strong> {toDate:dd-MMM-yyyy}</p>
            <p><strong>Reason:</strong> {reason}</p>
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendShiftExtensionApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, DateOnly transactionDate, string durationHours,
        DateTime? beforeShiftHours, DateTime? afterShiftHours, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "Shift Extension Request Approved" : "Shift Extension Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/shift-extension-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your shift extension request has been <strong>{statusMessage}</strong>.</p>
            <p><strong>Transaction Date:</strong> {transactionDate:dd-MMM-yyyy}</p>
            <p><strong>Duration Hours:</strong> {durationHours}</p>";

            if (beforeShiftHours.HasValue)
                emailBody += $"<p><strong>Before Shift Hours:</strong> {beforeShiftHours}</p>";

            if (afterShiftHours.HasValue)
                emailBody += $"<p><strong>After Shift Hours:</strong> {afterShiftHours}</p>";

            emailBody += $@"
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendWeeklyOffHolidayApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, string shiftSelectType, string shiftName,
        DateTime? shiftInTime, DateTime? shiftOutTime, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "Weekly Off/Holiday Working Request Approved" : "Weekly Off/Holiday Working Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/weekly-off-holiday-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your weekly off/holiday working request has been <strong>{statusMessage}</strong>.</p>
            <p><strong>Shift Select Type:</strong> {shiftSelectType}</p>
            <p><strong>Shift Name:</strong> {shiftName}</p>";

            if (shiftInTime.HasValue)
                emailBody += $"<p><strong>Shift In Time:</strong> {shiftInTime.Value:dd-MMM-yyyy hh:mm tt}</p>";

            if (shiftOutTime.HasValue)
                emailBody += $"<p><strong>Shift Out Time:</strong> {shiftOutTime.Value:dd-MMM-yyyy hh:mm tt}</p>";

            emailBody += $@"
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendCompOffAvailApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, DateOnly workedDate,
        DateOnly fromDate, DateOnly toDate, decimal totalDays, string reason, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "Comp-Off Avail Request Approved" : "Comp-Off Avail Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/comp-off-avail-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

            string workedDateFormatted = workedDate.ToString("dd-MMM-yyyy");
            string fromDateFormatted = fromDate.ToString("dd-MMM-yyyy");
            string toDateFormatted = toDate.ToString("dd-MMM-yyyy");

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your Comp-Off avail request has been <strong>{statusMessage}</strong>.</p>
            <p><strong>Worked Date:</strong> {workedDateFormatted}</p>
            <p><strong>From Date:</strong> {fromDateFormatted}</p>
            <p><strong>To Date:</strong> {toDateFormatted}</p>
            <p><strong>Total Days:</strong> {totalDays}</p>
            <p><strong>Reason:</strong> {reason}</p>
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendCompOffCreditApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, DateOnly workedDate, int? balance,
        decimal totalDays, string reason, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Receiver email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "Comp-Off Credit Approved" : "Comp-Off Credit Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/comp-off-credit-details?staffId={recipientId}";
            string actionText = isApproved
                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                : "If you have any questions, please contact HR.";

            string workedDateFormatted = workedDate.ToString("dd-MMM-yyyy");
            string balanceText = balance.HasValue ? balance.Value.ToString() : "0";

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your Comp-Off Credit request has been <strong>{statusMessage}</strong>.</p>
            <p><strong>Worked Date:</strong> {workedDateFormatted}</p>
            <p><strong>Balance:</strong> {balanceText}</p>
            <p><strong>Total Days:</strong> {totalDays}</p>
            <p><strong>Reason:</strong> {reason}</p>
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendReimbursementApprovalEmail(
        string recipientEmail, int recipientId, string recipientName, bool isApproved, DateOnly billDate, string billNo,
        string description, string billPeriod, decimal amount, int approvedBy, string approverName, string approvedTime)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Recipient email not found");
            var receiver = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver == null) throw new Exception("Receiver not found or inactive.");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];

            string subject = isApproved ? "Reimbursement Request Approved" : "Reimbursement Request Rejected";
            string statusMessage = isApproved ? "approved" : "rejected";
            string actionBy = isApproved ? "Approved by" : "Rejected by";
            string webApprovalLink = $"{frontEndUrl}/comp-off-credit-details?staffId={recipientId}";
            string actionText = isApproved
                                ? $"You can view the details <a href='{webApprovalLink}'>here</a>."
                                : "If you have any questions, please contact HR.";
            string billDateFormatted = billDate.ToString("dd-MMM-yyyy");

            string emailBody = $@"
            <p>Dear {recipientName},</p>
            <p>Your reimbursement request has been <strong>{statusMessage}</strong>.</p>
            <p><strong>Bill Date:</strong> {billDateFormatted}</p>
            <p><strong>Bill No:</strong> {billNo}</p>
            <p><strong>Description:</strong> {description}</p>
            <p><strong>Bill Period:</strong> {billPeriod}</p>
            <p><strong>Amount:</strong> ₹{amount:F2}</p>
            <p><strong>{actionBy}:</strong> {approverName} on {approvedTime}</p>
            <br>
            <p>{actionText}</p>
            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }

        public async Task SendProbationNotificationToHrAsync(string probationerName, DateOnly probationStartDate, DateOnly probationEndDate)
        {
            var toEmail = _configuration["Smtp:to"];
            if (toEmail == null) throw new MessageNotFoundException("Recipient email not found");
            var createdBy = int.Parse(_configuration["Smtp:mailTriggerId"]);
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?staffId={createdBy}";
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
            <p>
                <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
            </p>
            <p>Regards,<br/>Attendance Management System</p>";

            await SendApprovalEmail(toEmail, subject, body, createdBy);
        }

        public async Task AssignManager(string recipientEmail, int recipientId, string recipientName, string probationerName, DateOnly startDate, DateOnly endDate, int createdBy)
        {
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("Approver email not found");
            var receiver1 = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == recipientId && u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (receiver1 == null) throw new Exception("Approver not found or inactive.");

            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?staffId={recipientId}";
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
            <p>
                <a href='{approvalLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; margin-right: 10px;'>Approve</a>
            </p>

            <br>Best Regards,<br>
            HR Team";

            await SendApprovalEmail(recipientEmail, subject, emailBody, createdBy);
        }

        public async Task SendProbationConfirmationNotificationToHrAsync(int recipientId, string probationerName, DateOnly startDate, DateOnly endDate, DateOnly? extensionPeriod, bool isApproved, string approverName, string approvedTime, int approvedBy)
        {
            var recipientEmail = _configuration["Smtp:to"];
            if (recipientEmail == null) throw new MessageNotFoundException("Recipient email not found");
            if (string.IsNullOrEmpty(recipientEmail)) throw new MessageNotFoundException("HR email not found");
            var recipient = await _context.StaffCreations.FirstOrDefaultAsync(u => u.OfficialEmail == recipientEmail && u.IsActive == true);
            if (recipient == null) throw new Exception("HR recipient not found or inactive.");
            string subject = isApproved ? "Probation Confirmation Approved" : "Probation Confirmation Extended";
            string statusMessage = isApproved ? "approved" : "extended";
            string actionBy = isApproved ? "Approved by" : "Extended by";
            string startDateFormatted = startDate.ToString("dd-MMM-yyyy");
            string endDateFormatted = endDate.ToString("dd-MMM-yyyy");
            var frontEndUrl = _configuration["FrontEnd:FrontEndUrl"];
            string approvalLink = $"{frontEndUrl}/#/main/Tools/OnBehalfApplicationApproval?staffId={recipientId}";
            string actionText = $"You can view the details <a href='{approvalLink}'>here</a>.";
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
                    <p>{actionText}</p>
                    <br>Best Regards,<br>
                    Attendance Management System";

            await SendApprovalEmail(recipientEmail, subject, emailBody, approvedBy);
        }
    }
}