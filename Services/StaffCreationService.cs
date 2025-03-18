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
        public async Task<string> UpdateApproversAsync(List<int> staffIds, int? approverId1, int? approverId2, int updatedBy)
        {
            var message = "Approvers updated successfully";

            var staffList = await _context.StaffCreations.Where(s => staffIds.Contains(s.Id)).ToListAsync();
            if (staffList == null || !staffList.Any())
            {
                return "No valid staff members found.";
            }

            if (approverId1.HasValue && !await _context.StaffCreations.AnyAsync(s => s.Id == approverId1.Value))
            {
                return $"ApproverId1 ({approverId1}) does not exist.";
            }

            if (approverId2.HasValue && !await _context.StaffCreations.AnyAsync(s => s.Id == approverId2.Value))
            {
                return $"ApproverId2 ({approverId2}) does not exist.";
            }

            foreach (var staff in staffList)
            {
                staff.ApprovalLevel1 = approverId1 ?? staff.ApprovalLevel1;
                staff.ApprovalLevel2 = approverId2 ?? staff.ApprovalLevel2;
                staff.UpdatedBy = updatedBy;
                staff.UpdatedUtc = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return message;
        }
        public async Task<StaffCreationResponse> GetByUserManagementIdAsync(int staffId)
        {
            var getUser = await _context.StaffCreations
                .Where(s => s.Id == staffId && s.IsActive == true)
                .Select(s => new StaffCreationResponse
                {
                    StaffId = s.Id,
                    StaffCreationId = $"{_context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId).Select(o => o.ShortName).FirstOrDefault()}{s.Id}",
                    CardCode = s.CardCode,
                    Title = s.Title,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ShortName = s.ShortName,
                    StatusId = s.StatusId,
                    Status = _context.Statuses.Where(st => st.Id == s.StatusId && st.IsActive).Select(st => st.Name).FirstOrDefault() ?? string.Empty,
                    Gender = s.Gender,
                    BloodGroup = s.BloodGroup,
                    ProfilePhoto = s.ProfilePhoto,
                    AadharFilePath = s.AadharCardFilePath,
                    PanCardFilePath = s.PanCardFilePath,
                    DrivingLicenseFilePath = s.DrivingLicenseFilePath,
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
                    AccessLevelId = _context.AccessLevels.Where(a => a.Name == s.AccessLevel).Select(a => a.Id).FirstOrDefault(),
                    AccessLevel = s.AccessLevel,
                    MiddleName = s.MiddleName,
                    PersonalLocation = s.PersonalLocation,
                    PolicyGroupId = _context.PolicyGroups.Where(p => p.Name == s.PolicyGroup).Select(p => p.Id).FirstOrDefault(),
                    PolicyGroup = s.PolicyGroup,
                    WorkingDayPatternId = _context.WorkingDayPatterns.Where(wp => wp.Name == s.WorkingDayPattern).Select(wp => wp.Id).FirstOrDefault(),
                    WorkingDayPattern = s.WorkingDayPattern,
                    WorkingStatusId = _context.WorkingStatuses.Where(w => w.Name == s.WorkingStatus).Select(w => w.Id).FirstOrDefault(),
                    WorkingStatus = s.WorkingStatus,
                    GeoStatusId = _context.GeoStatuses.Where(g => g.Name == s.GeoStatus).Select(g => g.Id).FirstOrDefault(),
                    GeoStatus = s.GeoStatus,
                    Tenure = s.Tenure,
                    ApprovalLevel = s.ApprovalLevel,
                    ApprovalLevelId1 = s.ApprovalLevel1,
                    ApprovalLevel1 = s.ApprovalLevel1Navigation != null ? $"{s.ApprovalLevel1Navigation.FirstName} {s.ApprovalLevel1Navigation.LastName}" : null,
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
                    Branch = _context.BranchMasters.Where(b => b.Id == s.BranchId).Select(b => b.FullName).FirstOrDefault() ?? string.Empty,
                    DepartmentId = s.DepartmentId,
                    Department = _context.DepartmentMasters.Where(d => d.Id == s.DepartmentId).Select(d => d.FullName).FirstOrDefault() ?? string.Empty,
                    DivisionId = s.DivisionId,
                    Division = _context.DivisionMasters.Where(d => d.Id == s.DivisionId).Select(d => d.FullName).FirstOrDefault() ?? string.Empty,
                    MaritalStatusId = _context.MaritalStatuses.Where(m => m.Name == s.MaritalStatus).Select(m => m.Id).FirstOrDefault(),
                    MaritalStatus = s.MaritalStatus,
                    VolumeId = _context.Volumes.Where(v => v.Name == s.Volume).Select(v => v.Id).FirstOrDefault(),
                    Volume = s.Volume,
                    DesignationId = s.DesignationId,
                    Designation = _context.DesignationMasters.Where(d => d.Id == s.DesignationId).Select(d => d.FullName).FirstOrDefault() ?? string.Empty,
                    GradeId = s.GradeId,
                    Grade = _context.GradeMasters.Where(g => g.Id == s.GradeId).Select(g => g.FullName).FirstOrDefault() ?? string.Empty,
                    CategoryId = s.CategoryId,
                    Category = _context.CategoryMasters.Where(c => c.Id == s.CategoryId).Select(c => c.FullName).FirstOrDefault() ?? string.Empty,
                    CostCenterId = s.CostCenterId,
                    CostCenter = _context.CostCentreMasters.Where(c => c.Id == s.CostCenterId).Select(c => c.FullName).FirstOrDefault() ?? string.Empty,
                    WorkStationId = s.WorkStationId,
                    WorkStation = _context.WorkstationMasters.Where(w => w.Id == s.WorkStationId).Select(w => w.FullName).FirstOrDefault() ?? string.Empty,
                    LeaveGroupId = s.LeaveGroupId,
                    LeaveGroup = _context.LeaveGroups.Where(l => l.Id == s.LeaveGroupId).Select(l => l.LeaveGroupName).FirstOrDefault() ?? string.Empty,
                    CompanyMasterId = s.CompanyMasterId,
                    Company = _context.CompanyMasters.Where(c => c.Id == s.CompanyMasterId).Select(c => c.FullName).FirstOrDefault() ?? string.Empty,
                    HolidayCalendarId = s.HolidayCalendarId,
                    HolidayCalendar = _context.HolidayCalendarConfigurations.Where(h => h.Id == s.HolidayCalendarId).Select(h => h.GroupName).FirstOrDefault() ?? string.Empty,
                    LocationMasterId = s.LocationMasterId,
                    Location = _context.LocationMasters.Where(l => l.Id == s.LocationMasterId).Select(l => l.FullName).FirstOrDefault() ?? string.Empty,
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
                    EmergencyContactPerson1 = s.EmergencyContactPerson1 ?? string.Empty,
                    EmergencyContactPerson2 = s.EmergencyContactPerson2,
                    EmergencyContactNo1 = s.EmergencyContactNo1,
                    EmergencyContactNo2 = s.EmergencyContactNo2,
                    OrganizationTypeId = s.OrganizationTypeId,
                    OrganizationTypeName = _context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId).Select(o => o.Name).FirstOrDefault() ?? string.Empty,
                    ResignationDate = s.ResignationDate,
                    RelievingDate = s.RelievingDate,
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
                new SqlParameter("@LoginUserName", string.IsNullOrWhiteSpace(getStaff.LoginUserName) ? (object)DBNull.Value : getStaff.LoginUserName),
                new SqlParameter("@IncludeTerminated", getStaff.IncludeTerminated.HasValue ? (object)getStaff.IncludeTerminated.Value : DBNull.Value)
            };

            var staffList = await _storedProcedureDbContext.StaffDto
                .FromSqlRaw("EXEC GetStaffByFilters @ApproverId, @ShiftName, @OrganizationTypeName, @CompanyName, @CategoryName, @CostCentreName, @BranchName, @DepartmentName, @DesignationName, @StaffName, @LocationName, @GradeName, @Status, @LoginUserName, @IncludeTerminated", parameters)
                .ToListAsync();
            if (staffList.Count == 0)
            {
                throw new MessageNotFoundException("No staffs found");
            }
            return staffList;
        }

        public async Task<string> UpdateMyProfile(IndividualStaffUpdate individualStaffUpdate)
        {
            var message = "Profile updated successfully";
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == individualStaffUpdate.UpdatedBy && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            if (individualStaffUpdate.ProfilePhoto != null)
            {
                staff.ProfilePhoto = await SaveFileAsync(individualStaffUpdate.ProfilePhoto, "ProfilePhotos");
            }
            if (individualStaffUpdate.AadharFilePath != null)
            {
                staff.AadharCardFilePath = await SaveFileAsync(individualStaffUpdate.AadharFilePath, "AadharCards");
            }
            if (individualStaffUpdate.PanCardFilePath != null)
            {
                staff.PanCardFilePath = await SaveFileAsync(individualStaffUpdate.PanCardFilePath, "PanCards");
            }
            if (individualStaffUpdate.DrivingLicenseFilePath != null)
            {
                staff.DrivingLicenseFilePath = await SaveFileAsync(individualStaffUpdate.DrivingLicenseFilePath, "DrivingLicenses");
            }
            staff.Title = individualStaffUpdate.Title;
            staff.FirstName = individualStaffUpdate.FirstName;
            staff.LastName = individualStaffUpdate.LastName;
            staff.Gender = individualStaffUpdate.Gender;
            staff.MaritalStatus = individualStaffUpdate.MaritalStatus;
            staff.PersonalPhone = individualStaffUpdate.PersonalPhone;
            staff.OfficialPhone = individualStaffUpdate.OfficialPhone;
            staff.City = individualStaffUpdate.City;
            staff.District = individualStaffUpdate.District;
            staff.State = individualStaffUpdate.State;
            staff.Country = individualStaffUpdate.Country;
            staff.PostalCode = individualStaffUpdate.PostalCode;
            staff.MiddleName = individualStaffUpdate.MiddleName;
            staff.ShortName = individualStaffUpdate.ShortName;
            staff.PersonalLocation = individualStaffUpdate.PersonalLocation;
            staff.PersonalEmail = individualStaffUpdate.PersonalEmail;
            staff.OfficialEmail = individualStaffUpdate.OfficialEmail;
            staff.BankName = individualStaffUpdate.BankName;
            staff.BankAccountNo = individualStaffUpdate.BankAccountNo;
            staff.BankIfscCode = individualStaffUpdate.BankIfscCode;
            staff.BankBranch = individualStaffUpdate.BankBranch;
            staff.Qualification = individualStaffUpdate.Qualification;
            staff.HomeAddress = individualStaffUpdate.HomeAddress;
            staff.FatherName = individualStaffUpdate.FatherName;
            staff.MotherName = individualStaffUpdate.MotherName;
            staff.EmergencyContactPerson1 = individualStaffUpdate.EmergencyContactPerson1;
            staff.EmergencyContactPerson2 = individualStaffUpdate.EmergencyContactPerson2;
            staff.EmergencyContactNo1 = individualStaffUpdate.EmergencyContactNo1;
            staff.EmergencyContactNo2 = individualStaffUpdate.EmergencyContactNo2;
            staff.UpdatedBy = individualStaffUpdate.UpdatedBy;
            _context.StaffCreations.Update(staff);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<IndividualStaffResponse> GetMyProfile(int staffId)
        {
            var staff = await _context.StaffCreations
                .FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            var getUser = await (from s in _context.StaffCreations
                                 join department in _context.DepartmentMasters on s.DepartmentId equals department.Id
                                 join division in _context.DivisionMasters on s.DivisionId equals division.Id
                                 join designation in _context.DesignationMasters on s.DesignationId equals designation.Id
                                 join grade in _context.GradeMasters on s.GradeId equals grade.Id
                                 join org in _context.OrganizationTypes on s.OrganizationTypeId equals org.Id
                                 join workingStatus in _context.WorkingStatuses on s.WorkingStatus equals workingStatus.Name
                                 join maritalStatus in _context.MaritalStatuses on s.MaritalStatus equals maritalStatus.Name
                                 where s.Id == staffId && s.IsActive == true
                                 select new IndividualStaffResponse
                                 {
                                     StaffCreationId = $"{org.ShortName}{s.Id}",
                                     Title = s.Title,
                                     FirstName = s.FirstName,
                                     LastName = s.LastName,
                                     ShortName = s.ShortName,
                                     Gender = s.Gender,
                                     BloodGroup = s.BloodGroup,
                                     ProfilePhoto = s.ProfilePhoto,
                                     AadharFilePath = s.AadharCardFilePath,
                                     PanCardFilePath = s.PanCardFilePath,
                                     DrivingLicenseFilePath = s.DrivingLicenseFilePath,
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
                                     MiddleName = s.MiddleName,
                                     PersonalLocation = s.PersonalLocation,
                                     Tenure = s.Tenure,
                                     ApprovalLevelId1 = s.ApprovalLevel1,
                                     ApprovalLevel1 = $"{s.ApprovalLevel1Navigation.FirstName} {s.ApprovalLevel1Navigation.LastName}",
                                     ApprovalLevelId2 = s.ApprovalLevel2,
                                     ApprovalLevel2 = s.ApprovalLevel2Navigation != null ? $"{s.ApprovalLevel2Navigation.FirstName ?? string.Empty} {s.ApprovalLevel2Navigation.LastName ?? string.Empty}".Trim() : null,
                                     UanNumber = s.UanNumber,
                                     EsiNumber = s.EsiNumber,
                                     District = s.District,
                                     State = s.State,
                                     Country = s.Country,
                                     PostalCode = s.PostalCode,
                                     DepartmentId = s.DepartmentId,
                                     Department = department.FullName,
                                     DivisionId = s.DivisionId,
                                     Division = division.FullName,
                                     MaritalStatusId = maritalStatus.Id,
                                     MaritalStatus = maritalStatus.Name,
                                     DesignationId = s.DesignationId,
                                     Designation = designation.FullName,
                                     GradeId = s.GradeId,
                                     Grade = grade.FullName,
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
                                     EmergencyContactPerson1 = s.EmergencyContactPerson1 ?? string.Empty,
                                     EmergencyContactPerson2 = s.EmergencyContactPerson2,
                                     EmergencyContactNo1 = s.EmergencyContactNo1,
                                     EmergencyContactNo2 = s.EmergencyContactNo2,
                                     OrganizationTypeId = s.OrganizationTypeId,
                                     OrganizationTypeName = org.Name,
                                     WorkingStatusId = workingStatus.Id,
                                     WorkingStatus = workingStatus.Name,
                                     CreatedBy = s.CreatedBy
                                 })
                                .FirstOrDefaultAsync();
            return getUser;
        }

        public async Task<string> AddStaff(StaffCreationInputModel staffInput)
        {
            var message = "Staff added, mail sent successfully.";

            string profilePhotoPath = string.Empty;
            string aadharCardPath = string.Empty;
            string panCardPath = string.Empty;
            string drivingLicensePath = string.Empty;

            string baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            async Task<string> SaveFile(IFormFile file, string folderName)
            {
                if (file == null) return null;

                string directoryPath = Path.Combine(baseDirectory, folderName);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileName = $"{Guid.NewGuid()}_{file.FileName}";
                string filePath = Path.Combine(directoryPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return $"/{folderName}/{fileName}";
            }
            profilePhotoPath = await SaveFile(staffInput.ProfilePhoto, "ProfilePhotos");
            aadharCardPath = await SaveFile(staffInput.AadharCardFilePath, "AadharCards");
            panCardPath = await SaveFile(staffInput.PanCardFilePath, "PanCards");
            drivingLicensePath = await SaveFile(staffInput.DrivingLicenseFilePath, "DrivingLicenses");
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
                WorkingStatus = staffInput.WorkingStatus,
                AadharCardFilePath = aadharCardPath,
                PanCardFilePath = panCardPath,
                DrivingLicenseFilePath = drivingLicensePath,
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
                if (from != null)
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
                if (from != null)
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
                existingStaff.ProfilePhoto = await SaveFileAsync(updatedStaff.ProfilePhoto, "ProfilePhotos");
            }
            if (updatedStaff.AadharCardFilePath != null)
            {
                existingStaff.AadharCardFilePath = await SaveFileAsync(updatedStaff.AadharCardFilePath, "AadharCards");
            }
            if (updatedStaff.PanCardFilePath != null)
            {
                existingStaff.PanCardFilePath = await SaveFileAsync(updatedStaff.PanCardFilePath, "PanCards");
            }
            if (updatedStaff.DrivingLicenseFilePath != null)
            {
                existingStaff.DrivingLicenseFilePath = await SaveFileAsync(updatedStaff.DrivingLicenseFilePath, "DrivingLicenses");
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
            existingStaff.ResignationDate = updatedStaff.ResignationDate;
            existingStaff.RelievingDate = updatedStaff.RelievingDate;
            existingStaff.UpdatedBy = updatedStaff.UpdatedBy;
            existingStaff.UpdatedUtc = DateTime.UtcNow;

            _context.StaffCreations.Update(existingStaff);
            await _context.SaveChangesAsync();

            return message;
        }
        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(directoryPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/{folderName}/{fileName}";
        }
        public async Task<IEnumerable<StaffCreationResponse>> GetStaffRecordsByApprovalLevelAsync(int currentApprovar1)
        {
            var staff = await _context.StaffCreations
                .FirstOrDefaultAsync(s => s.Id == currentApprovar1 && s.IsActive == true);

            if (staff == null)
                throw new MessageNotFoundException("Approver not found");
            var records = await _context.StaffCreations
                .Where(s => s.ApprovalLevel1 == currentApprovar1 && s.IsActive == true)
                .Select(s => new StaffCreationResponse
                {
                    StaffId = s.Id,
                    StaffCreationId = $"{_context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId).Select(o => o.ShortName).FirstOrDefault()}{s.Id}",
                    CardCode = s.CardCode,
                    Title = s.Title,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ShortName = s.ShortName,
                    StatusId = s.StatusId,
                    Status = _context.Statuses.Where(st => st.Id == s.StatusId && st.IsActive).Select(st => st.Name).FirstOrDefault() ?? string.Empty,
                    Gender = s.Gender,
                    BloodGroup = s.BloodGroup,
                    ProfilePhoto = s.ProfilePhoto,
                    AadharFilePath = s.AadharCardFilePath,
                    PanCardFilePath = s.PanCardFilePath,
                    DrivingLicenseFilePath = s.DrivingLicenseFilePath,
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
                    AccessLevelId = _context.AccessLevels.Where(a => a.Name == s.AccessLevel).Select(a => a.Id).FirstOrDefault(),
                    AccessLevel = s.AccessLevel,
                    MiddleName = s.MiddleName,
                    PersonalLocation = s.PersonalLocation,
                    PolicyGroupId = _context.PolicyGroups.Where(p => p.Name == s.PolicyGroup).Select(p => p.Id).FirstOrDefault(),
                    PolicyGroup = s.PolicyGroup,
                    WorkingDayPatternId = _context.WorkingDayPatterns.Where(wp => wp.Name == s.WorkingDayPattern).Select(wp => wp.Id).FirstOrDefault(),
                    WorkingDayPattern = s.WorkingDayPattern,
                    WorkingStatusId = _context.WorkingStatuses.Where(w => w.Name == s.WorkingStatus).Select(w => w.Id).FirstOrDefault(),
                    WorkingStatus = s.WorkingStatus,
                    GeoStatusId = _context.GeoStatuses.Where(g => g.Name == s.GeoStatus).Select(g => g.Id).FirstOrDefault(),
                    GeoStatus = s.GeoStatus,
                    Tenure = s.Tenure,
                    ApprovalLevel = s.ApprovalLevel,
                    ApprovalLevelId1 = s.ApprovalLevel1,
                    ApprovalLevel1 = s.ApprovalLevel1Navigation != null ? $"{s.ApprovalLevel1Navigation.FirstName} {s.ApprovalLevel1Navigation.LastName}" : null,
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
                    Branch = _context.BranchMasters.Where(b => b.Id == s.BranchId).Select(b => b.FullName).FirstOrDefault() ?? string.Empty,
                    DepartmentId = s.DepartmentId,
                    Department = _context.DepartmentMasters.Where(d => d.Id == s.DepartmentId).Select(d => d.FullName).FirstOrDefault() ?? string.Empty,
                    DivisionId = s.DivisionId,
                    Division = _context.DivisionMasters.Where(d => d.Id == s.DivisionId).Select(d => d.FullName).FirstOrDefault() ?? string.Empty,
                    MaritalStatusId = _context.MaritalStatuses.Where(m => m.Name == s.MaritalStatus).Select(m => m.Id).FirstOrDefault(),
                    MaritalStatus = s.MaritalStatus,
                    VolumeId = _context.Volumes.Where(v => v.Name == s.Volume).Select(v => v.Id).FirstOrDefault(),
                    Volume = s.Volume,
                    DesignationId = s.DesignationId,
                    Designation = _context.DesignationMasters.Where(d => d.Id == s.DesignationId).Select(d => d.FullName).FirstOrDefault() ?? string.Empty,
                    GradeId = s.GradeId,
                    Grade = _context.GradeMasters.Where(g => g.Id == s.GradeId).Select(g => g.FullName).FirstOrDefault() ?? string.Empty,
                    CategoryId = s.CategoryId,
                    Category = _context.CategoryMasters.Where(c => c.Id == s.CategoryId).Select(c => c.FullName).FirstOrDefault() ?? string.Empty,
                    CostCenterId = s.CostCenterId,
                    CostCenter = _context.CostCentreMasters.Where(c => c.Id == s.CostCenterId).Select(c => c.FullName).FirstOrDefault() ?? string.Empty,
                    WorkStationId = s.WorkStationId,
                    WorkStation = _context.WorkstationMasters.Where(w => w.Id == s.WorkStationId).Select(w => w.FullName).FirstOrDefault() ?? string.Empty,
                    LeaveGroupId = s.LeaveGroupId,
                    LeaveGroup = _context.LeaveGroups.Where(l => l.Id == s.LeaveGroupId).Select(l => l.LeaveGroupName).FirstOrDefault() ?? string.Empty,
                    CompanyMasterId = s.CompanyMasterId,
                    Company = _context.CompanyMasters.Where(c => c.Id == s.CompanyMasterId).Select(c => c.FullName).FirstOrDefault() ?? string.Empty,
                    HolidayCalendarId = s.HolidayCalendarId,
                    HolidayCalendar = _context.HolidayCalendarConfigurations.Where(h => h.Id == s.HolidayCalendarId).Select(h => h.GroupName).FirstOrDefault() ?? string.Empty,
                    LocationMasterId = s.LocationMasterId,
                    Location = _context.LocationMasters.Where(l => l.Id == s.LocationMasterId).Select(l => l.FullName).FirstOrDefault() ?? string.Empty,
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
                    OrganizationTypeName = _context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId).Select(o => o.Name).FirstOrDefault() ?? string.Empty,
                    ResignationDate = s.ResignationDate,
                    RelievingDate = s.RelievingDate,
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
            var records = await _context.StaffCreations
                .Where(s => s.ApprovalLevel1 == approverId && s.IsActive == null)
                .Select(s => new StaffCreationResponse
                {
                    StaffId = s.Id,
                    StaffCreationId = $"{_context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId).Select(o => o.ShortName).FirstOrDefault()}{s.Id}",
                    CardCode = s.CardCode,
                    Title = s.Title,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ShortName = s.ShortName,
                    StatusId = s.StatusId,
                    Status = _context.Statuses.Where(st => st.Id == s.StatusId && st.IsActive).Select(st => st.Name).FirstOrDefault() ?? string.Empty,
                    Gender = s.Gender,
                    BloodGroup = s.BloodGroup,
                    ProfilePhoto = s.ProfilePhoto,
                    AadharFilePath = s.AadharCardFilePath,
                    PanCardFilePath = s.PanCardFilePath,
                    DrivingLicenseFilePath = s.DrivingLicenseFilePath,
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
                    AccessLevelId = _context.AccessLevels.Where(a => a.Name == s.AccessLevel).Select(a => a.Id).FirstOrDefault(),
                    AccessLevel = s.AccessLevel,
                    MiddleName = s.MiddleName,
                    PersonalLocation = s.PersonalLocation,
                    PolicyGroupId = _context.PolicyGroups.Where(p => p.Name == s.PolicyGroup).Select(p => p.Id).FirstOrDefault(),
                    PolicyGroup = s.PolicyGroup,
                    WorkingDayPatternId = _context.WorkingDayPatterns.Where(wp => wp.Name == s.WorkingDayPattern).Select(wp => wp.Id).FirstOrDefault(),
                    WorkingDayPattern = s.WorkingDayPattern,
                    WorkingStatusId = _context.WorkingStatuses.Where(w => w.Name == s.WorkingStatus).Select(w => w.Id).FirstOrDefault(),
                    WorkingStatus = s.WorkingStatus,
                    GeoStatusId = _context.GeoStatuses.Where(g => g.Name == s.GeoStatus).Select(g => g.Id).FirstOrDefault(),
                    GeoStatus = s.GeoStatus,
                    Tenure = s.Tenure,
                    ApprovalLevel = s.ApprovalLevel,
                    ApprovalLevelId1 = s.ApprovalLevel1,
                    ApprovalLevel1 = s.ApprovalLevel1Navigation != null ? $"{s.ApprovalLevel1Navigation.FirstName} {s.ApprovalLevel1Navigation.LastName}" : null,
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
                    Branch = _context.BranchMasters.Where(b => b.Id == s.BranchId).Select(b => b.FullName).FirstOrDefault() ?? string.Empty,
                    DepartmentId = s.DepartmentId,
                    Department = _context.DepartmentMasters.Where(d => d.Id == s.DepartmentId).Select(d => d.FullName).FirstOrDefault() ?? string.Empty,
                    DivisionId = s.DivisionId,
                    Division = _context.DivisionMasters.Where(d => d.Id == s.DivisionId).Select(d => d.FullName).FirstOrDefault() ?? string.Empty,
                    MaritalStatusId = _context.MaritalStatuses.Where(m => m.Name == s.MaritalStatus).Select(m => m.Id).FirstOrDefault(),
                    MaritalStatus = s.MaritalStatus,
                    VolumeId = _context.Volumes.Where(v => v.Name == s.Volume).Select(v => v.Id).FirstOrDefault(),
                    Volume = s.Volume,
                    DesignationId = s.DesignationId,
                    Designation = _context.DesignationMasters.Where(d => d.Id == s.DesignationId).Select(d => d.FullName).FirstOrDefault() ?? string.Empty,
                    GradeId = s.GradeId,
                    Grade = _context.GradeMasters.Where(g => g.Id == s.GradeId).Select(g => g.FullName).FirstOrDefault() ?? string.Empty,
                    CategoryId = s.CategoryId,
                    Category = _context.CategoryMasters.Where(c => c.Id == s.CategoryId).Select(c => c.FullName).FirstOrDefault() ?? string.Empty,
                    CostCenterId = s.CostCenterId,
                    CostCenter = _context.CostCentreMasters.Where(c => c.Id == s.CostCenterId).Select(c => c.FullName).FirstOrDefault() ?? string.Empty,
                    WorkStationId = s.WorkStationId,
                    WorkStation = _context.WorkstationMasters.Where(w => w.Id == s.WorkStationId).Select(w => w.FullName).FirstOrDefault() ?? string.Empty,
                    LeaveGroupId = s.LeaveGroupId,
                    LeaveGroup = _context.LeaveGroups.Where(l => l.Id == s.LeaveGroupId).Select(l => l.LeaveGroupName).FirstOrDefault() ?? string.Empty,
                    CompanyMasterId = s.CompanyMasterId,
                    Company = _context.CompanyMasters.Where(c => c.Id == s.CompanyMasterId).Select(c => c.FullName).FirstOrDefault() ?? string.Empty,
                    HolidayCalendarId = s.HolidayCalendarId,
                    HolidayCalendar = _context.HolidayCalendarConfigurations.Where(h => h.Id == s.HolidayCalendarId).Select(h => h.GroupName).FirstOrDefault() ?? string.Empty,
                    LocationMasterId = s.LocationMasterId,
                    Location = _context.LocationMasters.Where(l => l.Id == s.LocationMasterId).Select(l => l.FullName).FirstOrDefault() ?? string.Empty,
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
                    EmergencyContactPerson1 = s.EmergencyContactPerson1 ?? string.Empty,
                    EmergencyContactPerson2 = s.EmergencyContactPerson2,
                    EmergencyContactNo1 = s.EmergencyContactNo1,
                    EmergencyContactNo2 = s.EmergencyContactNo2,
                    OrganizationTypeId = s.OrganizationTypeId,
                    OrganizationTypeName = _context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId).Select(o => o.Name).FirstOrDefault() ?? string.Empty,
                    ResignationDate = s.ResignationDate,
                    RelievingDate = s.RelievingDate,
                    CreatedBy = s.CreatedBy
                })
                .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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
                {1005, new ExcelImport() },
                {1006, new StaffLeaveOption() },
                {1007, new PermissionType() },
                {1008, new ShiftPattern() },
                {1009, new ShiftType() },
                {1010, new WeeklyOffType() },
                {1011, new WeeklyOff() },
                {1012, new DailyReport() },
                {1013, new MonthRange() }
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
                { 1005, _context.ExcelImports.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1006, _context.StaffLeaveOptions.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1007, _context.PermissionTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1008, _context.ShiftPatterns.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1009, _context.ShiftTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1010, _context.WeeklyOffTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1011, _context.WeeklyOffs.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1012, _context.DailyReports.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1013, _context.MonthRanges.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) }
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
                { 1005, async () => await _context.ExcelImports.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1006, async () => await _context.StaffLeaveOptions.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1007, async () => await _context.PermissionTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1008, async () => await _context.ShiftPatterns.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1009, async () => await _context.ShiftTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1010, async () => await _context.WeeklyOffTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1011, async () => await _context.WeeklyOffs.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1012, async () => await _context.DailyReports.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1013, async () => await _context.MonthRanges.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) }

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