using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AttendanceManagement.Services
{
    public class StaffCreationService
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly StoredProcedureDbContext _storedProcedureDbContext;
        private readonly IConfiguration _configuration;

        public StaffCreationService(AttendanceManagementSystemContext context, IConfiguration configuration, StoredProcedureDbContext storedProcedureDbContext)
        {
            _context = context;
            _configuration = configuration;
            _storedProcedureDbContext = storedProcedureDbContext;
        }
        public async Task<string> UpdateApproversAsync(int staffId, int? approverId1, int? approverId2, int updatedBy)
        {
            var message = "Approver chnaged sucessfully";
            var staff = await _context.StaffCreations.FindAsync(staffId);
            if (staff == null)
            {
                return "Staff not found";
            }
            if (approverId1.HasValue && !await _context.StaffCreations.AnyAsync(s => s.Id == approverId1.Value))
            {
                return $"ApproverId1 ({approverId1}) does not exist.";
            }
            if (approverId2.HasValue && !await _context.StaffCreations.AnyAsync(s => s.Id == approverId2.Value))
            {
                return $"ApproverId2 ({approverId2}) does not exist.";
            }
            staff.ApprovalLevel1 = approverId1 ?? staff.ApprovalLevel1;
            staff.ApprovalLevel2 = approverId2 ?? staff.ApprovalLevel2;
            staff.UpdatedBy = updatedBy;
            staff.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }
        public async Task<StaffCreationResponse> GetByUserManagementIdAsync(int staffId)
        {
            var getUser = await (from s in _context.StaffCreations
                                 join branch in _context.BranchMasters on s.BranchId equals branch.Id
                                 join department in _context.DepartmentMasters on s.DepartmentId equals department.Id
                                 join division in _context.DivisionMasters on s.DivisionId equals division.Id
                                 join designation in _context.DesignationMasters on s.DesignationId equals designation.Id
                                 join grade in _context.GradeMasters on s.GradeId equals grade.Id
                                 join category in _context.CategoryMasters on s.CategoryId equals category.Id
                                 join cost in _context.CostCentreMasters on s.CostCenterId equals cost.Id
                                 join work in _context.WorkstationMasters on s.WorkStationId equals work.Id
                                 join status in _context.Statuses on s.StatusId equals status.Id
                                 join org in _context.OrganizationTypes on s.OrganizationTypeId equals org.Id
                                 join leaveGroup in _context.LeaveGroups on s.LeaveGroupId equals leaveGroup.Id
                                 join company in _context.CompanyMasters on s.CompanyMasterId equals company.Id
                                 join holiday in _context.HolidayCalendarConfigurations on s.HolidayCalendarId equals holiday.Id
                                 join location in _context.LocationMasters on s.LocationMasterId equals location.Id
                                 join geoStatus in _context.GeoStatuses on s.GeoStatus equals geoStatus.Name
                                 join workingStatus in _context.WorkingStatuses on s.WorkingStatus equals workingStatus.Name
                                 join accessLevel in _context.AccessLevels on s.AccessLevel equals accessLevel.Name
                                 join policyGroup in _context.PolicyGroups on s.PolicyGroup equals policyGroup.Name
                                 join workingDayPattern in _context.WorkingDayPatterns on s.WorkingDayPattern equals workingDayPattern.Name
                                 join volume in _context.Volumes on s.Volume equals volume.Name
                                 join maritalStatus in _context.MaritalStatuses on s.MaritalStatus equals maritalStatus.Name
                                 where s.Id == staffId && s.IsActive == true
                                 select new StaffCreationResponse
                                 {
                                     StaffId = s.Id,
                                     StaffCreationId = $"{org.ShortName}{s.Id}",
                                     CardCode = s.CardCode,
                                     Title = s.Title,
                                     FirstName = s.FirstName,
                                     LastName = s.LastName,
                                     ShortName = s.ShortName,
                                     StatusId = status.Id,
                                     Status = status.Name,
                                     Gender = s.Gender,
                                     BloodGroup = s.BloodGroup,
                                     ProfilePhoto = s.ProfilePhoto,
                                     Dob = s.Dob,
                                     MarriageDate = s.MarriageDate,
                                     PersonalPhone = s.PersonalPhone,
                                     OfficialPhone = s.OfficialPhone,
                                     JoiningDate = s.JoiningDate,
                                     Confirmed = s.Confirmed,
                                     ConfirmationDate = s.ConfirmationDate,
                                     PersonalEmail = s.PersonalEmail,
                                     OfficialEmail = s.OfficialEmail,
                                     City = s.City,
                                     AccessLevelId = accessLevel.Id,
                                     AccessLevel = accessLevel.Name,
                                     MiddleName = s.MiddleName,
                                     PersonalLocation = s.PersonalLocation,
                                     PolicyGroupId = policyGroup.Id,
                                     PolicyGroup = policyGroup.Name,
                                     WorkingDayPatternId = workingDayPattern.Id,
                                     WorkingDayPattern = workingDayPattern.Name,
                                     WorkingStatusId = workingStatus.Id,
                                     WorkingStatus = workingStatus.Name,
                                     GeoStatusId = geoStatus.Id,
                                     GeoStatus = geoStatus.Name,
                                     Tenure = s.Tenure,
                                     ApprovalLevel = s.ApprovalLevel,
                                     ApprovalLevelId1 = s.ApprovalLevel1,
                                     ApprovalLevel1 = $"{s.ApprovalLevel1Navigation.FirstName} {s.ApprovalLevel1Navigation.LastName}",
                                     ApprovalLevelId2 = s.ApprovalLevel2,
                                     ApprovalLevel2 = s.ApprovalLevel2Navigation != null ? $"{s.ApprovalLevel2Navigation.FirstName ?? string.Empty} {s.ApprovalLevel2Navigation.LastName ?? string.Empty}".Trim() : null,
                                     UanNumber = s.UanNumber,
                                     EsiNumber = s.EsiNumber,
                                     IsMobileAppEligible = s.IsMobileAppEligible,
                                     District = s.District,
                                     State = s.State,
                                     Country = s.Country,
                                     PostalCode = s.PostalCode,
                                     OtEligible = s.OtEligible,
                                     BranchId = s.BranchId,
                                     Branch = branch.FullName,
                                     DepartmentId = s.DepartmentId,
                                     Department = department.FullName,
                                     DivisionId = s.DivisionId,
                                     Division = division.FullName,
                                     MaritalStatusId = maritalStatus.Id,
                                     MaritalStatus = maritalStatus.Name,
                                     VolumeId = volume.Id,
                                     Volume = volume.Name,
                                     DesignationId = s.DesignationId,
                                     Designation = designation.FullName,
                                     GradeId = s.GradeId,
                                     Grade = grade.FullName,
                                     CategoryId = s.CategoryId,
                                     Category = category.FullName,
                                     CostCenterId = s.CostCenterId,
                                     CostCenter = cost.FullName,
                                     WorkStationId = s.WorkStationId,
                                     WorkStation = work.FullName,
                                     LeaveGroupId = s.LeaveGroupId,
                                     LeaveGroup = leaveGroup.LeaveGroupName,
                                     CompanyMasterId = s.CompanyMasterId,
                                     Company = company.FullName,
                                     HolidayCalendarId = s.HolidayCalendarId,
                                     HolidayCalendar = holiday.GroupName,
                                     LocationMasterId = s.LocationMasterId,
                                     Location = location.FullName,
                                     AadharNo = s.AadharNo,
                                     PanNo = s.PanNo,
                                     PassportNo = s.PassportNo,
                                     DrivingLicense = s.DrivingLicense,
                                     BankName = s.BankName,
                                     BankAccountNo = s.BankAccountNo,
                                     BankIfscCode = s.BankIfscCode,
                                     BankBranch = s.BankBranch,
                                     Qualification = s.Qualification,
                                     HomeAddress = s.HomeAddress,
                                     FatherName = s.FatherName,
                                     MotherName = s.MotherName,
                                     FatherAadharNo = s.FatherAadharNo,
                                     MotherAadharNo = s.MotherAadharNo,
                                     EmergencyContactPerson1 = s.EmergencyContactPerson1,
                                     EmergencyContactPerson2 = s.EmergencyContactPerson2,
                                     EmergencyContactNo1 = s.EmergencyContactNo1,
                                     EmergencyContactNo2 = s.EmergencyContactNo2,
                                     OrganizationTypeId = s.OrganizationTypeId,
                                     OrganizationTypeName = org.Name,
                                     CreatedBy = s.CreatedBy
                                 })
                                .FirstOrDefaultAsync();
            if (getUser == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }

            return getUser;
        }
        public async Task<List<StaffDto>> GetStaffAsync(GetStaff getStaff)
        {
            var parameters = new[]
            {
                new SqlParameter("@ApproverId", getStaff.ApproverId ?? (object)DBNull.Value),
                new SqlParameter("@ShiftName", string.IsNullOrWhiteSpace(getStaff.ShiftName) ? (object)DBNull.Value : getStaff.ShiftName),
                new SqlParameter("@OrganizationTypeName", string.IsNullOrWhiteSpace(getStaff.OrganizationTypeName) ? (object)DBNull.Value : getStaff.OrganizationTypeName),
                new SqlParameter("@CompanyName", string.IsNullOrWhiteSpace(getStaff.CompanyName) ? (object)DBNull.Value : getStaff.CompanyName),
                new SqlParameter("@CategoryName", string.IsNullOrWhiteSpace(getStaff.CategoryName) ? (object)DBNull.Value : getStaff.CategoryName),
                new SqlParameter("@CostCentreName", string.IsNullOrWhiteSpace(getStaff.CostCentreName) ? (object)DBNull.Value : getStaff.CostCentreName),
                new SqlParameter("@BranchName", string.IsNullOrWhiteSpace(getStaff.BranchName) ? (object)DBNull.Value : getStaff.BranchName),
                new SqlParameter("@DepartmentName", string.IsNullOrWhiteSpace(getStaff.DepartmentName) ? (object)DBNull.Value : getStaff.DepartmentName),
                new SqlParameter("@DesignationName", string.IsNullOrWhiteSpace(getStaff.DesignationName) ? (object)DBNull.Value : getStaff.DesignationName),
                new SqlParameter("@StaffName", string.IsNullOrWhiteSpace(getStaff.StaffName) ? (object)DBNull.Value : getStaff.StaffName),
                new SqlParameter("@LocationName", string.IsNullOrWhiteSpace(getStaff.LocationName) ? (object)DBNull.Value : getStaff.LocationName),
                new SqlParameter("@GradeName", string.IsNullOrWhiteSpace(getStaff.GradeName) ? (object)DBNull.Value : getStaff.GradeName),
                new SqlParameter("@Status", string.IsNullOrWhiteSpace(getStaff.Status) ? (object)DBNull.Value : getStaff.Status),
                new SqlParameter("@LoginUserName", string.IsNullOrWhiteSpace(getStaff.LoginUserName) ? (object)DBNull.Value : getStaff.LoginUserName)
            };

            var staffList = await _storedProcedureDbContext.StaffDto
                .FromSqlRaw("EXEC GetStaffByFilters @ApproverId, @ShiftName, @OrganizationTypeName, @CompanyName, @CategoryName, @CostCentreName, @BranchName, @DepartmentName, @DesignationName, @StaffName, @LocationName, @GradeName, @Status, @LoginUserName", parameters)
                .ToListAsync();
            if(staffList.Count == 0)
            {
                throw new MessageNotFoundException("No staffs found");
            }
            return staffList;
        }

        public async Task<string> AddStaff(StaffCreationInputModel staffInput)
        {
            var message = "Staff added, mail sent successfully.";
            string profilePhotoPath = string.Empty;
            string profilePhotoBase64 = string.Empty;

            if (staffInput.ProfilePhoto != null)
            {
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfilePhotos");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string fileName = $"{Guid.NewGuid()}_{staffInput.ProfilePhoto.FileName}";
                string filePath = Path.Combine(directoryPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await staffInput.ProfilePhoto.CopyToAsync(fileStream);
                }
                using (var memoryStream = new MemoryStream())
                {
                    await staffInput.ProfilePhoto.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    profilePhotoBase64 = Convert.ToBase64String(fileBytes);
                }
                profilePhotoPath = $"/ProfilePhotos/{fileName}";
            }

            var staff = new StaffCreation
            {
                CardCode = staffInput.CardCode,
                Title = staffInput.Title,
                FirstName = staffInput.FirstName,
                LastName = staffInput.LastName,
                ShortName = staffInput.ShortName,
                Gender = staffInput.Gender,
                Hide = staffInput.Hide,
                BloodGroup = staffInput.BloodGroup,
                ProfilePhoto = profilePhotoPath, 
                MaritalStatus = staffInput.MaritalStatus,
                Dob = staffInput.Dob,
                MarriageDate = staffInput.MarriageDate,
                PersonalPhone = staffInput.PersonalPhone,
                OfficialPhone = staffInput.OfficialPhone,
                JoiningDate = staffInput.JoiningDate,
                Confirmed = staffInput.Confirmed,
                ConfirmationDate = staffInput.ConfirmationDate,
                BranchId = staffInput.BranchId,
                DepartmentId = staffInput.DepartmentId,
                HolidayCalendarId = staffInput.HolidayCalendarId,
                DivisionId = staffInput.DivisionId,
                Volume = staffInput.Volume,
                DesignationId = staffInput.DesignationId,
                GradeId = staffInput.GradeId,
                CategoryId = staffInput.CategoryId,
                CostCenterId = staffInput.CostCenterId,
                WorkStationId = staffInput.WorkStationId,
                City = staffInput.City,
                District = staffInput.District,
                State = staffInput.State,
                Country = staffInput.Country,
                PostalCode = staffInput.PostalCode,
                OtEligible = staffInput.OtEligible,
                ApprovalLevel = staffInput.ApprovalLevel,
                ApprovalLevel1 = staffInput.ApprovalLevel1,
                ApprovalLevel2 = staffInput.ApprovalLevel2,
                AccessLevel = staffInput.AccessLevel,
                PolicyGroup = staffInput.PolicyGroup,
                WorkingDayPattern = staffInput.WorkingDayPattern,
                Tenure = staffInput.Tenure,
                UanNumber = staffInput.UanNumber,
                EsiNumber = staffInput.EsiNumber,
                IsMobileAppEligible = staffInput.IsMobileAppEligible,
                GeoStatus = staffInput.GeoStatus,
                CreatedBy = staffInput.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                MiddleName = staffInput.MiddleName,
                PersonalLocation = staffInput.PersonalLocation,
                PersonalEmail = staffInput.PersonalEmail,
                OfficialEmail = staffInput.OfficialEmail,
                LeaveGroupId = staffInput.LeaveGroupId,
                CompanyMasterId = staffInput.CompanyMasterId,
                LocationMasterId = staffInput.LocationMasterId,
                StatusId = staffInput.StatusId,
                AadharNo = staffInput.AadharNo,
                PanNo = staffInput.PanNo,
                PassportNo = staffInput.PassportNo,
                DrivingLicense = staffInput.DrivingLicense,
                BankName = staffInput.BankName,
                BankAccountNo = staffInput.BankAccountNo,
                BankIfscCode = staffInput.BankIfscCode,
                BankBranch = staffInput.BankBranch,
                Qualification = staffInput.Qualification,
                HomeAddress = staffInput.HomeAddress,
                FatherName = staffInput.FatherName,
                MotherName = staffInput.MotherName,
                FatherAadharNo = staffInput.FatherAadharNo,
                MotherAadharNo = staffInput.MotherAadharNo,
                EmergencyContactPerson1 = staffInput.EmergencyContactPerson1,
                EmergencyContactPerson2 = staffInput.EmergencyContactPerson2,
                EmergencyContactNo1 = staffInput.EmergencyContactNo1,
                EmergencyContactNo2 = staffInput.EmergencyContactNo2,
                OrganizationTypeId = staffInput.OrganizationTypeId,
                WorkingStatus = staffInput.WorkingStatus
            };

            _context.StaffCreations.Add(staff);
            await _context.SaveChangesAsync();

            var reportingManager = await _context.StaffCreations
                .Where(u => u.Id == staffInput.ApprovalLevel1 && u.IsActive == true)
                .Select(u => u.OfficialEmail)
                .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(reportingManager))
            {
                await SendApprovalEmail(reportingManager, staff);
            }

            return message;
        }
        private async Task SendApprovalEmail(string managerEmail, StaffCreation staff)
        {
            var message = new MailMessage();
            if (staff == null || string.IsNullOrEmpty(managerEmail))
                throw new ArgumentNullException("Staff or Manager Email is null.");

            var reportingManager = await _context.StaffCreations
               .Where(u => u.Id == staff.ApprovalLevel1 && u.OfficialEmail == managerEmail && u.IsActive == true)
               .FirstOrDefaultAsync();

            if (reportingManager == null)
                throw new Exception("Reporting manager not found or inactive.");

            var host = _configuration.GetSection("Smtp").GetValue<string>("host");
            var port = _configuration.GetSection("Smtp").GetValue<int>("port");
            var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
            var password = _configuration.GetSection("Smtp").GetValue<string>("password");
            var from = _configuration.GetSection("Smtp").GetValue<string>("from");
            var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
            var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");

            string deepApprovalLink = $"myapp://approve?staffId={staff.Id}";
            string deepRejectionLink = $"myapp://reject?staffId={staff.Id}";

            string reportingManagerFullName = $"{reportingManager.FirstName} {reportingManager.LastName}";
            string staffFullName = $"{staff.FirstName} {staff.LastName}";

            string emailBody = $@"
                        <p>Dear {reportingManagerFullName},</p>
                        <p>A new staff member <strong>{staffFullName}</strong> has been added and requires your approval.</p>       
                        <p><strong>Approve/Reject staff</strong></p>
                        <p><a href='{deepApprovalLink}'>Approve</a> | <a href='{deepRejectionLink}'>Reject</a></p>
                        <br>Best Regards,</br>
                        HR Team";

            try
            {
                message.Subject = "Staff Approval Request";
                message.Body = emailBody;
                message.IsBodyHtml = true;
                if(from != null)
                {
                    message.From = new MailAddress(from, "Test Mail");
                    message.To.Add(new MailAddress(managerEmail));

                    using var smtp = new SmtpClient(host, port)
                    {
                        UseDefaultCredentials = defaultCredential,
                        Credentials = new NetworkCredential(userName, password),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = true
                    };

                    await smtp.SendMailAsync(message);
                    var log = new EmailLog
                    {
                        From = from,
                        To = managerEmail,
                        EmailSubject = message.Subject,
                        EmailBody = emailBody,
                        IsSent = true,
                        IsError = false,
                        CreatedBy = staff.CreatedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    _context.EmailLogs.Add(log);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if(from != null)
                {
                    var log = new EmailLog
                    {
                        From = from,
                        To = managerEmail,
                        EmailSubject = message.Subject,
                        EmailBody = emailBody,
                        IsSent = false,
                        IsError = true,
                        ErrorDescription = ex.Message,
                        CreatedBy = staff.CreatedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    _context.EmailLogs.Add(log);
                    await _context.SaveChangesAsync();
                }
            }
        }
        public async Task<string> UpdateStaffCreationAsync(UpdateStaff updatedStaff)
        {
            var message = "Staff updated successfully";
            var existingStaff = await _context.StaffCreations
                .FirstOrDefaultAsync(s => s.Id == updatedStaff.StaffCreationId && s.IsActive == true);

            if (existingStaff == null)
                throw new MessageNotFoundException("Staff not found");
            if (updatedStaff.ProfilePhoto != null)
            {
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfilePhotos");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string fileName = $"{Guid.NewGuid()}_{updatedStaff.ProfilePhoto.FileName}";
                string filePath = Path.Combine(directoryPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await updatedStaff.ProfilePhoto.CopyToAsync(fileStream);
                }

                existingStaff.ProfilePhoto = $"/ProfilePhotos/{fileName}"; 
            }
            existingStaff.CardCode = updatedStaff.CardCode;
            existingStaff.Title = updatedStaff.Title;
            existingStaff.FirstName = updatedStaff.FirstName;
            existingStaff.LastName = updatedStaff.LastName;
            existingStaff.ShortName = updatedStaff.ShortName;
            existingStaff.Gender = updatedStaff.Gender;
            existingStaff.Hide = updatedStaff.Hide;
            existingStaff.BloodGroup = updatedStaff.BloodGroup;
            existingStaff.MaritalStatus = updatedStaff.MaritalStatus;
            existingStaff.Dob = updatedStaff.Dob;
            existingStaff.MarriageDate = updatedStaff.MarriageDate;
            existingStaff.PersonalPhone = updatedStaff.PersonalPhone;
            existingStaff.OfficialPhone = updatedStaff.OfficialPhone;
            existingStaff.JoiningDate = updatedStaff.JoiningDate;
            existingStaff.Confirmed = updatedStaff.Confirmed;
            existingStaff.ConfirmationDate = updatedStaff.ConfirmationDate;
            existingStaff.CompanyMasterId = updatedStaff.CompanyMasterId;
            existingStaff.LocationMasterId = updatedStaff.LocationMasterId;
            existingStaff.BranchId = updatedStaff.BranchId;
            existingStaff.DepartmentId = updatedStaff.DepartmentId;
            existingStaff.DivisionId = updatedStaff.DivisionId;
            existingStaff.Volume = updatedStaff.Volume;
            existingStaff.DesignationId = updatedStaff.DesignationId;
            existingStaff.GradeId = updatedStaff.GradeId;
            existingStaff.CategoryId = updatedStaff.CategoryId;
            existingStaff.CostCenterId = updatedStaff.CostCenterId;
            existingStaff.WorkStationId = updatedStaff.WorkStationId;
            existingStaff.City = updatedStaff.City;
            existingStaff.District = updatedStaff.District;
            existingStaff.State = updatedStaff.State;
            existingStaff.Country = updatedStaff.Country;
            existingStaff.PostalCode = updatedStaff.PostalCode;
            existingStaff.OtEligible = updatedStaff.OtEligible;
            existingStaff.ApprovalLevel = updatedStaff.ApprovalLevel;
            existingStaff.ApprovalLevel1 = updatedStaff.ApprovalLevel1;
            existingStaff.ApprovalLevel2 = updatedStaff.ApprovalLevel2;
            existingStaff.AccessLevel = updatedStaff.AccessLevel;
            existingStaff.PolicyGroup = updatedStaff.PolicyGroup;
            existingStaff.LeaveGroupId = updatedStaff.LeaveGroupId;
            existingStaff.WorkingDayPattern = updatedStaff.WorkingDayPattern;
            existingStaff.HolidayCalendarId = updatedStaff.HolidayCalendarId;
            existingStaff.Tenure = updatedStaff.Tenure;
            existingStaff.UanNumber = updatedStaff.UanNumber;
            existingStaff.EsiNumber = updatedStaff.EsiNumber;
            existingStaff.IsMobileAppEligible = updatedStaff.IsMobileAppEligible;
            existingStaff.GeoStatus = updatedStaff.GeoStatus;
            existingStaff.MiddleName = updatedStaff.MiddleName;
            existingStaff.PersonalLocation = updatedStaff.PersonalLocation;
            existingStaff.PersonalEmail = updatedStaff.PersonalEmail;
            existingStaff.OfficialEmail = updatedStaff.OfficialEmail;
            existingStaff.StatusId = updatedStaff.StatusId;
            existingStaff.AadharNo = updatedStaff.AadharNo;
            existingStaff.PanNo = updatedStaff.PanNo;
            existingStaff.PassportNo = updatedStaff.PassportNo;
            existingStaff.DrivingLicense = updatedStaff.DrivingLicense;
            existingStaff.BankName = updatedStaff.BankName;
            existingStaff.BankAccountNo = updatedStaff.BankAccountNo;
            existingStaff.BankIfscCode = updatedStaff.BankIfscCode;
            existingStaff.BankBranch = updatedStaff.BankBranch;
            existingStaff.Qualification = updatedStaff.Qualification;
            existingStaff.HomeAddress = updatedStaff.HomeAddress;
            existingStaff.FatherName = updatedStaff.FatherName;
            existingStaff.MotherName = updatedStaff.MotherName;
            existingStaff.FatherAadharNo = updatedStaff.FatherAadharNo;
            existingStaff.MotherAadharNo = updatedStaff.MotherAadharNo;
            existingStaff.EmergencyContactPerson1 = updatedStaff.EmergencyContactPerson1;
            existingStaff.EmergencyContactPerson2 = updatedStaff.EmergencyContactPerson2;
            existingStaff.EmergencyContactNo1 = updatedStaff.EmergencyContactNo1;
            existingStaff.EmergencyContactNo2 = updatedStaff.EmergencyContactNo2;
            existingStaff.OrganizationTypeId = updatedStaff.OrganizationTypeId;
            existingStaff.WorkingStatus = updatedStaff.WorkingStatus;
            existingStaff.UpdatedBy = updatedStaff.UpdatedBy;
            existingStaff.UpdatedUtc = DateTime.UtcNow;

            _context.StaffCreations.Update(existingStaff);
            await _context.SaveChangesAsync();

            return message;
        }
        public async Task<IEnumerable<StaffCreationResponse>> GetStaffRecordsByApprovalLevelAsync(int currentApprovar1)
        {
            var staff = await _context.StaffCreations
                .FirstOrDefaultAsync(s => s.Id == currentApprovar1 && s.IsActive == true);

            if (staff == null)
                throw new MessageNotFoundException("Approver not found");
            var records = await (from s in _context.StaffCreations
                                 join branch in _context.BranchMasters
                                 on s.BranchId equals branch.Id
                                 join department in _context.DepartmentMasters
                                 on s.DepartmentId equals department.Id
                                 join division in _context.DivisionMasters
                                 on s.DivisionId equals division.Id
                                 join designation in _context.DesignationMasters
                                 on s.DesignationId equals designation.Id
                                 join grade in _context.GradeMasters
                                 on s.GradeId equals grade.Id
                                 join category in _context.CategoryMasters
                                 on s.CategoryId equals category.Id
                                 join cost in _context.CostCentreMasters
                                 on s.CostCenterId equals cost.Id
                                 join work in _context.WorkstationMasters
                                 on s.WorkStationId equals work.Id
                                 join status in _context.Statuses
                                 on s.StatusId equals status.Id
                                 join org in _context.OrganizationTypes
                                 on s.OrganizationTypeId equals org.Id
                                 join leaveGroup in _context.LeaveGroups
                                 on s.LeaveGroupId equals leaveGroup.Id
                                 join company in _context.CompanyMasters
                                 on s.CompanyMasterId equals company.Id
                                 join holiday in _context.HolidayCalendarConfigurations
                                 on s.HolidayCalendarId equals holiday.Id
                                 join location in _context.LocationMasters
                                 on s.LocationMasterId equals location.Id
                                 join geoStatus in _context.GeoStatuses on s.GeoStatus equals geoStatus.Name
                                 join workingStatus in _context.WorkingStatuses on s.WorkingStatus equals workingStatus.Name
                                 join accessLevel in _context.AccessLevels on s.AccessLevel equals accessLevel.Name
                                 join policyGroup in _context.PolicyGroups on s.PolicyGroup equals policyGroup.Name
                                 join workingDayPattern in _context.WorkingDayPatterns on s.WorkingDayPattern equals workingDayPattern.Name
                                 join volume in _context.Volumes on s.Volume equals volume.Name
                                 join maritalStatus in _context.MaritalStatuses on s.MaritalStatus equals maritalStatus.Name
                                 where s.ApprovalLevel1 == staff.Id && s.IsActive == true
                                 select new StaffCreationResponse
                                 {
                                     StaffId = s.Id,
                                     StaffCreationId = $"{org.ShortName}{s.Id}",
                                     CardCode = s.CardCode,
                                     Title = s.Title,
                                     FirstName = s.FirstName,
                                     LastName = s.LastName,
                                     ShortName = s.ShortName,
                                     StatusId = status.Id,
                                     Status = status.Name,
                                     Gender = s.Gender,
                                     BloodGroup = s.BloodGroup,
                                     ProfilePhoto = s.ProfilePhoto,
                                     Dob = s.Dob,
                                     MarriageDate = s.MarriageDate,
                                     PersonalPhone = s.PersonalPhone,
                                     OfficialPhone = s.OfficialPhone,
                                     JoiningDate = s.JoiningDate,
                                     Confirmed = s.Confirmed,
                                     ConfirmationDate = s.ConfirmationDate,
                                     PersonalEmail = s.PersonalEmail,
                                     OfficialEmail = s.OfficialEmail,
                                     City = s.City,
                                     AccessLevelId = accessLevel.Id,
                                     AccessLevel = accessLevel.Name,
                                     MiddleName = s.MiddleName,
                                     PersonalLocation = s.PersonalLocation,
                                     PolicyGroupId = policyGroup.Id,
                                     PolicyGroup = policyGroup.Name,
                                     WorkingDayPatternId = workingDayPattern.Id,
                                     WorkingDayPattern = workingDayPattern.Name,
                                     WorkingStatusId = workingStatus.Id,
                                     WorkingStatus = workingStatus.Name,
                                     GeoStatusId = geoStatus.Id,
                                     GeoStatus = geoStatus.Name,
                                     Tenure = s.Tenure,
                                     ApprovalLevel = s.ApprovalLevel,
                                     ApprovalLevelId1 = s.ApprovalLevel1,
                                     ApprovalLevel1 = $"{s.ApprovalLevel1Navigation.FirstName} {s.ApprovalLevel1Navigation.LastName}",
                                     ApprovalLevelId2 = s.ApprovalLevel2,
                                     ApprovalLevel2 = s.ApprovalLevel2Navigation != null ? $"{s.ApprovalLevel2Navigation.FirstName ?? string.Empty} {s.ApprovalLevel2Navigation.LastName ?? string.Empty}".Trim() : null,
                                     UanNumber = s.UanNumber,
                                     EsiNumber = s.EsiNumber,
                                     IsMobileAppEligible = s.IsMobileAppEligible,
                                     District = s.District,
                                     State = s.State,
                                     Country = s.Country,
                                     PostalCode = s.PostalCode,
                                     OtEligible = s.OtEligible,
                                     BranchId = s.BranchId,
                                     Branch = branch.FullName,
                                     DepartmentId = s.DepartmentId,
                                     Department = department.FullName,
                                     DivisionId = s.DivisionId,
                                     Division = division.FullName,
                                     MaritalStatusId = maritalStatus.Id,
                                     MaritalStatus = maritalStatus.Name,
                                     VolumeId = volume.Id,
                                     Volume = volume.Name,
                                     DesignationId = s.DesignationId,
                                     Designation = designation.FullName,
                                     GradeId = s.GradeId,
                                     Grade = grade.FullName,
                                     CategoryId = s.CategoryId,
                                     Category = category.FullName,
                                     CostCenterId = s.CostCenterId,
                                     CostCenter = cost.FullName,
                                     WorkStationId = s.WorkStationId,
                                     WorkStation = work.FullName,
                                     LeaveGroupId = s.LeaveGroupId,
                                     LeaveGroup = leaveGroup.LeaveGroupName,
                                     CompanyMasterId = s.CompanyMasterId,
                                     Company = company.FullName,
                                     HolidayCalendarId = s.HolidayCalendarId,
                                     HolidayCalendar = holiday.GroupName,
                                     LocationMasterId = s.LocationMasterId,
                                     Location = location.FullName,
                                     AadharNo = s.AadharNo,
                                     PanNo = s.PanNo,
                                     PassportNo = s.PassportNo,
                                     DrivingLicense = s.DrivingLicense,
                                     BankName = s.BankName,
                                     BankAccountNo = s.BankAccountNo,
                                     BankIfscCode = s.BankIfscCode,
                                     BankBranch = s.BankBranch,
                                     Qualification = s.Qualification,
                                     HomeAddress = s.HomeAddress,
                                     FatherName = s.FatherName,
                                     MotherName = s.MotherName,
                                     FatherAadharNo = s.FatherAadharNo,
                                     MotherAadharNo = s.MotherAadharNo,
                                     EmergencyContactPerson1 = s.EmergencyContactPerson1,
                                     EmergencyContactPerson2 = s.EmergencyContactPerson2,
                                     EmergencyContactNo1 = s.EmergencyContactNo1,
                                     EmergencyContactNo2 = s.EmergencyContactNo2,
                                     OrganizationTypeId = s.OrganizationTypeId,
                                     OrganizationTypeName = org.Name,
                                     CreatedBy = s.CreatedBy
                                 })
                                 .ToListAsync();
            if (records.Count == 0)
            {
                throw new MessageNotFoundException("No staffs found");
            }
            return records;
        }

        public async Task<List<StaffCreationResponse>> GetPendingStaffForManagerApproval(int approverId)
        {
            var records = await (from s in _context.StaffCreations
                                 join branch in _context.BranchMasters
                                 on s.BranchId equals branch.Id
                                 join department in _context.DepartmentMasters
                                 on s.DepartmentId equals department.Id
                                 join division in _context.DivisionMasters
                                 on s.DivisionId equals division.Id
                                 join designation in _context.DesignationMasters
                                 on s.DesignationId equals designation.Id
                                 join grade in _context.GradeMasters
                                 on s.GradeId equals grade.Id
                                 join category in _context.CategoryMasters
                                 on s.CategoryId equals category.Id
                                 join cost in _context.CostCentreMasters
                                 on s.CostCenterId equals cost.Id
                                 join work in _context.WorkstationMasters
                                 on s.WorkStationId equals work.Id
                                 join status in _context.Statuses
                                 on s.StatusId equals status.Id
                                 join leaveGroup in _context.LeaveGroups
                                 on s.LeaveGroupId equals leaveGroup.Id
                                 join org in _context.OrganizationTypes
                                 on s.OrganizationTypeId equals org.Id
                                 join company in _context.CompanyMasters
                                 on s.CompanyMasterId equals company.Id
                                 join holiday in _context.HolidayCalendarConfigurations
                                 on s.HolidayCalendarId equals holiday.Id
                                 join location in _context.LocationMasters
                                 on s.LocationMasterId equals location.Id
                                 join geoStatus in _context.GeoStatuses on s.GeoStatus equals geoStatus.Name
                                 join workingStatus in _context.WorkingStatuses on s.WorkingStatus equals workingStatus.Name
                                 join accessLevel in _context.AccessLevels on s.AccessLevel equals accessLevel.Name
                                 join policyGroup in _context.PolicyGroups on s.PolicyGroup equals policyGroup.Name
                                 join workingDayPattern in _context.WorkingDayPatterns on s.WorkingDayPattern equals workingDayPattern.Name
                                 join volume in _context.Volumes on s.Volume equals volume.Name
                                 join maritalStatus in _context.MaritalStatuses on s.MaritalStatus equals maritalStatus.Name
                                 where s.ApprovalLevel1 == approverId && s.IsActive == null
                                 select new StaffCreationResponse
                                 {
                                     StaffId = s.Id,
                                     StaffCreationId = $"{org.ShortName}{s.Id}",
                                     CardCode = s.CardCode,
                                     Title = s.Title,
                                     FirstName = s.FirstName,
                                     LastName = s.LastName,
                                     ShortName = s.ShortName,
                                     StatusId = status.Id,
                                     Status = status.Name,
                                     Gender = s.Gender,
                                     BloodGroup = s.BloodGroup,
                                     ProfilePhoto = s.ProfilePhoto,
                                     Dob = s.Dob,
                                     MarriageDate = s.MarriageDate,
                                     PersonalPhone = s.PersonalPhone,
                                     OfficialPhone = s.OfficialPhone,
                                     JoiningDate = s.JoiningDate,
                                     Confirmed = s.Confirmed,
                                     ConfirmationDate = s.ConfirmationDate,
                                     PersonalEmail = s.PersonalEmail,
                                     OfficialEmail = s.OfficialEmail,
                                     City = s.City,
                                     ApprovalLevel = s.ApprovalLevel,
                                     ApprovalLevelId1 = s.ApprovalLevel1,
                                     ApprovalLevel1 = $"{s.ApprovalLevel1Navigation.FirstName} {s.ApprovalLevel1Navigation.LastName}",
                                     ApprovalLevelId2 = s.ApprovalLevel2,
                                     ApprovalLevel2 = s.ApprovalLevel2Navigation != null ? $"{s.ApprovalLevel2Navigation.FirstName ?? string.Empty} {s.ApprovalLevel2Navigation.LastName ?? string.Empty}".Trim() : null,
                                     PersonalLocation = s.PersonalLocation,
                                     AccessLevelId = accessLevel.Id,
                                     AccessLevel = accessLevel.Name,
                                     MiddleName = s.MiddleName,
                                     PolicyGroupId = policyGroup.Id,
                                     PolicyGroup = policyGroup.Name,
                                     WorkingDayPatternId = workingDayPattern.Id,
                                     WorkingDayPattern = workingDayPattern.Name,
                                     WorkingStatusId = workingStatus.Id,
                                     WorkingStatus = workingStatus.Name,
                                     GeoStatusId = geoStatus.Id,
                                     GeoStatus = geoStatus.Name,
                                     Tenure = s.Tenure,
                                     UanNumber = s.UanNumber,
                                     EsiNumber = s.EsiNumber,
                                     IsMobileAppEligible = s.IsMobileAppEligible,
                                     District = s.District,
                                     State = s.State,
                                     Country = s.Country,
                                     PostalCode = s.PostalCode,
                                     OtEligible = s.OtEligible,
                                     BranchId = s.BranchId,
                                     Branch = branch.FullName,
                                     DepartmentId = s.DepartmentId,
                                     Department = department.FullName,
                                     DivisionId = s.DivisionId,
                                     Division = division.FullName,
                                     MaritalStatusId = maritalStatus.Id,
                                     MaritalStatus = maritalStatus.Name,
                                     VolumeId = volume.Id,
                                     Volume = volume.Name,
                                     DesignationId = s.DesignationId,
                                     Designation = designation.FullName,
                                     GradeId = s.GradeId,
                                     Grade = grade.FullName,
                                     CategoryId = s.CategoryId,
                                     Category = category.FullName,
                                     CostCenterId = s.CostCenterId,
                                     CostCenter = cost.FullName,
                                     WorkStationId = s.WorkStationId,
                                     WorkStation = work.FullName,
                                     LeaveGroupId = s.LeaveGroupId,
                                     LeaveGroup = leaveGroup.LeaveGroupName,
                                     CompanyMasterId = s.CompanyMasterId,
                                     Company = company.FullName,
                                     HolidayCalendarId = s.HolidayCalendarId,
                                     HolidayCalendar = holiday.GroupName,
                                     LocationMasterId = s.LocationMasterId,
                                     Location = location.FullName,
                                     AadharNo = s.AadharNo,
                                     PanNo = s.PanNo,
                                     PassportNo = s.PassportNo,
                                     DrivingLicense = s.DrivingLicense,
                                     BankName = s.BankName,
                                     BankAccountNo = s.BankAccountNo,
                                     BankIfscCode = s.BankIfscCode,
                                     BankBranch = s.BankBranch,
                                     Qualification = s.Qualification,
                                     HomeAddress = s.HomeAddress,
                                     FatherName = s.FatherName,
                                     MotherName = s.MotherName,
                                     FatherAadharNo = s.FatherAadharNo,
                                     MotherAadharNo = s.MotherAadharNo,
                                     EmergencyContactPerson1 = s.EmergencyContactPerson1,
                                     EmergencyContactPerson2 = s.EmergencyContactPerson2,
                                     EmergencyContactNo1 = s.EmergencyContactNo1,
                                     EmergencyContactNo2 = s.EmergencyContactNo2,
                                     OrganizationTypeId = s.OrganizationTypeId,
                                     OrganizationTypeName = org.Name,
                                     CreatedBy = s.CreatedBy
                                 })
                     .ToListAsync();
            if (records.Count == 0)
            {
                throw new MessageNotFoundException("No staffs found");
            }
            return records;
        }
        public async Task<string> ApprovePendingStaffs(ApprovePendingStaff approvePendingStaff)
        {
            var message = "";
            var selectedRows = approvePendingStaff.SelectedRows;
            foreach (var item in selectedRows)
            {
                if (approvePendingStaff.IsApproved)
                {
                    var staff = await _context.StaffCreations.Where(s => s.Id == item.StaffId && s.IsActive == null).FirstOrDefaultAsync();
                    if (staff != null)
                    {
                        staff.IsActive = true;
                        staff.UpdatedBy = approvePendingStaff.ApprovedBy;
                        staff.UpdatedUtc = DateTime.UtcNow;
                    }
                    await _context.SaveChangesAsync();
                    message = "Staff profile approved successfully";
                }
                else if (!approvePendingStaff.IsApproved)
                {
                    var staff = await _context.StaffCreations.Where(s => s.Id == item.StaffId && s.IsActive == null).FirstOrDefaultAsync();
                    if (staff != null)
                    {
                        staff.IsActive = false;
                        staff.UpdatedBy = approvePendingStaff.ApprovedBy;
                        staff.UpdatedUtc = DateTime.UtcNow;
                    }
                    await _context.SaveChangesAsync();
                    message = "Staff profile rejected successfully";
                }
            }

            return message;
        }

        public async Task<string> CreateDropDownMaster(DropDownRequest dropDownRequest)
        {
            var message = "Dropdown master created successfully";
            var dropDown = new DropDownMaster
            {
                Name = dropDownRequest.Name,
                IsActive = true,
                CreatedBy = dropDownRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            _context.DropDownMasters.Add(dropDown);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<DropDownResponse>> GetDropDownMaster()
        {
           var dropDown = await (from d in _context.DropDownMasters
                                 where d.IsActive
                                 select new DropDownResponse
                                 {
                                     Id = d.Id,
                                     Name = d.Name,
                                     CreatedBy = d.CreatedBy
                                 }).ToListAsync();
            if (dropDown.Count == 0)
            {
                throw new MessageNotFoundException("Dropdown master not found");
            }
            return dropDown;
        }

        public async Task<string> UpdateDropDownMaster(UpdateDropDown updateDropDown)
        {
            var message = "Dropdown master updated successfully";
            var dropDown = await _context.DropDownMasters.Where(d => d.Id == updateDropDown.Id && d.IsActive).FirstOrDefaultAsync();
            if (dropDown != null)
            {
                dropDown.Name = updateDropDown.Name;
                dropDown.UpdatedBy = updateDropDown.UpdatedBy;
                dropDown.UpdatedUtc = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> CreateDropDownDetails(DropDownDetailsRequest dropDownDetailsRequest)
        {
            var entityMapping = new Dictionary<int, object>
            {
                { 1, new Title() },
                { 2, new Status() },
                { 3, new Gender() },
                { 4, new BloodGroup() },
                { 5, new MaritalStatus() },
                { 6, new Volume() },
                { 7, new OrganizationType() },
                { 8, new ApprovalLeval() },
                { 9, new AccessLevel() },
                { 10, new PolicyGroup() },
                { 11, new WorkingDayPattern() },
                { 12, new GeoStatus() },
                {1002, new WorkingStatus() },
                {1003, new ApprovalOwner() },
                {1004, new LeaveCreditDebitReason() },
                {1005, new ExcelImport() }
            };

            if (!entityMapping.TryGetValue(dropDownDetailsRequest.DropDownMasterId, out var entity))
            {
                throw new MessageNotFoundException("Dropdown details not found");
            }

            dynamic newEntity = entity;
            newEntity.Name = dropDownDetailsRequest.Name;
            newEntity.IsActive = true;
            newEntity.CreatedBy = dropDownDetailsRequest.CreatedBy;
            newEntity.CreatedUtc = DateTime.UtcNow;

            _context.Add(newEntity);
            await _context.SaveChangesAsync();

            return $"{newEntity.GetType().Name} created successfully";
        }

        public async Task<List<DropDownResponse>> GetAllDropDowns(int id)
        {
            var dropDownQueries = new Dictionary<int, IQueryable<DropDownResponse>>
            {
                { 1, _context.Titles.Where(t => t.IsActive).Select(t => new DropDownResponse { Id = t.Id, Name = t.Name, CreatedBy = t.CreatedBy }) },
                { 2, _context.Statuses.Where(ss => ss.IsActive).Select(ss => new DropDownResponse { Id = ss.Id, Name = ss.Name, CreatedBy = ss.CreatedBy }) },
                { 3, _context.Genders.Where(g => g.IsActive).Select(g => new DropDownResponse { Id = g.Id, Name = g.Name, CreatedBy = g.CreatedBy }) },
                { 4, _context.BloodGroups.Where(bg => bg.IsActive).Select(bg => new DropDownResponse { Id = bg.Id, Name = bg.Name, CreatedBy = bg.CreatedBy }) },
                { 5, _context.MaritalStatuses.Where(ms => ms.IsActive).Select(ms => new DropDownResponse { Id = ms.Id, Name = ms.Name, CreatedBy = ms.CreatedBy }) },
                { 6, _context.Volumes.Where(v => v.IsActive).Select(v => new DropDownResponse { Id = v.Id, Name = v.Name, CreatedBy = v.CreatedBy }) },
                { 7, _context.OrganizationTypes.Where(ot => ot.IsActive).Select(ot => new DropDownResponse { Id = ot.Id, Name = ot.Name, CreatedBy = ot.CreatedBy }) },
                { 8, _context.ApprovalLevels.Where(al => al.IsActive).Select(al => new DropDownResponse { Id = al.Id, Name = al.Name, CreatedBy = al.CreatedBy }) },
                { 9, _context.AccessLevels.Where(acl => acl.IsActive).Select(acl => new DropDownResponse { Id = acl.Id, Name = acl.Name, CreatedBy = acl.CreatedBy }) },
                { 10, _context.PolicyGroups.Where(pg => pg.IsActive).Select(pg => new DropDownResponse { Id = pg.Id, Name = pg.Name, CreatedBy = pg.CreatedBy }) },
                { 11, _context.WorkingDayPatterns.Where(wdp => wdp.IsActive).Select(wdp => new DropDownResponse { Id = wdp.Id, Name = wdp.Name, CreatedBy = wdp.CreatedBy }) },
                { 12, _context.GeoStatuses.Where(gs => gs.IsActive).Select(gs => new DropDownResponse { Id = gs.Id, Name = gs.Name, CreatedBy = gs.CreatedBy }) },
                { 1002, _context.WorkingStatuses.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1003, _context.ApprovalOwners.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1004, _context.LeaveCreditDebitReasons.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1005, _context.ExcelImports.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) }

            };

            if (!dropDownQueries.TryGetValue(id, out var query))
            {
                throw new MessageNotFoundException("Dropdown master not found");
            }

            var dropDown = await query.ToListAsync();

            if (!dropDown.Any())
            {
                throw new MessageNotFoundException("No data found for the given dropdown master");
            }

            return dropDown;
        }

        public async Task<string> UpdateDropDownDetails(DropDownDetailsUpdate dropDownDetailsRequest)
        {
            var entityMapping = new Dictionary<int, Func<Task<object?>>>
            {
                { 1, async () => await _context.Titles.FirstOrDefaultAsync(t => t.Id == dropDownDetailsRequest.DropDownDetailId && t.IsActive) },
                { 2, async () => await _context.Statuses.FirstOrDefaultAsync(s => s.Id == dropDownDetailsRequest.DropDownDetailId && s.IsActive) },
                { 3, async () => await _context.Genders.FirstOrDefaultAsync(g => g.Id == dropDownDetailsRequest.DropDownDetailId && g.IsActive) },
                { 4, async () => await _context.BloodGroups.FirstOrDefaultAsync(bg => bg.Id == dropDownDetailsRequest.DropDownDetailId && bg.IsActive) },
                { 5, async () => await _context.MaritalStatuses.FirstOrDefaultAsync(ms => ms.Id == dropDownDetailsRequest.DropDownDetailId && ms.IsActive) },
                { 6, async () => await _context.Volumes.FirstOrDefaultAsync(v => v.Id == dropDownDetailsRequest.DropDownDetailId && v.IsActive) },
                { 7, async () => await _context.OrganizationTypes.FirstOrDefaultAsync(ot => ot.Id == dropDownDetailsRequest.DropDownDetailId && ot.IsActive) },
                { 8, async () => await _context.ApprovalLevels.FirstOrDefaultAsync(al => al.Id == dropDownDetailsRequest.DropDownDetailId && al.IsActive) },
                { 9, async () => await _context.AccessLevels.FirstOrDefaultAsync(al => al.Id == dropDownDetailsRequest.DropDownDetailId && al.IsActive) },
                { 10, async () => await _context.PolicyGroups.FirstOrDefaultAsync(pg => pg.Id == dropDownDetailsRequest.DropDownDetailId && pg.IsActive) },
                { 11, async () => await _context.WorkingDayPatterns.FirstOrDefaultAsync(wdp => wdp.Id == dropDownDetailsRequest.DropDownDetailId && wdp.IsActive) },
                { 12, async () => await _context.GeoStatuses.FirstOrDefaultAsync(gs => gs.Id == dropDownDetailsRequest.DropDownDetailId && gs.IsActive) },
                { 1002, async () => await _context.WorkingStatuses.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1003, async () => await _context.ApprovalOwners.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1004, async () => await _context.LeaveCreditDebitReasons.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1005, async () => await _context.ExcelImports.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) }
            };

            if (!entityMapping.TryGetValue(dropDownDetailsRequest.DropDownMasterId, out var getEntity))
            {
                throw new MessageNotFoundException("Dropdown details not found.");
            }

            var entity = await getEntity();
            if (entity == null)
            {
                throw new MessageNotFoundException("Requested entity not found.");
            }

            var entityType = entity.GetType();
            entityType.GetProperty("Name")?.SetValue(entity, dropDownDetailsRequest.Name);
            entityType.GetProperty("UpdatedBy")?.SetValue(entity, dropDownDetailsRequest.UpdatedBy);
            entityType.GetProperty("UpdatedUtc")?.SetValue(entity, DateTime.UtcNow);

            _context.Update(entity);
            await _context.SaveChangesAsync();

            return $"{entityType.Name.Replace("Proxy", "").Replace("_", " ")} updated successfully";
        }
    }
}