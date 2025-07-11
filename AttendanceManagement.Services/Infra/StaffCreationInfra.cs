using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Infrastructure.Data;
using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Domain.Entities.Attendance;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class StaffCreationInfra : IStaffCreationInfra
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly StoredProcedureDbContext _storedProcedureDbContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailInfra _emailService;
        private readonly string _workspacePath;
        public StaffCreationInfra(AttendanceManagementSystemContext context, IConfiguration configuration, StoredProcedureDbContext storedProcedureDbContext, IEmailInfra emailService, IWebHostEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _storedProcedureDbContext = storedProcedureDbContext;
            _emailService = emailService;
            _workspacePath = Path.Combine(env.ContentRootPath, "wwwroot");
            if (!Directory.Exists(_workspacePath))
            {
                Directory.CreateDirectory(_workspacePath);
            }
        }
        public async Task<string> UpdateApproversAsync(List<int> staffIds, int? approverId1, int? approverId2, int updatedBy)
        {
            var message = "Approvers updated successfully";
            if (approverId1.HasValue && approverId2.HasValue && approverId1 == approverId2)
            {
                throw new InvalidOperationException("Approver Level 1 and Approver Level 2 cannot be the same person");
            }
            var staffList = await _context.StaffCreations.Where(s => staffIds.Contains(s.Id)).ToListAsync();
            if(approverId2 != null)
            {
            }
            if (staffList == null || !staffList.Any())
            {
                throw new MessageNotFoundException("No valid staff members found");
            }
            string fullName = "";
            if (approverId1.HasValue && !await _context.StaffCreations.AnyAsync(s => s.Id == approverId1.Value))
            {
                var approver1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == approverId1 && s.IsActive == true);
                if (approver1 == null) throw new MessageNotFoundException("Approver1 not found");
                fullName = $"{approver1.FirstName}{(string.IsNullOrWhiteSpace(approver1.LastName) ? "" : " " + approver1.LastName)}";
                throw new MessageNotFoundException($"ApproverId1 {fullName} does not exist");
            }
            if (approverId2.HasValue && !await _context.StaffCreations.AnyAsync(s => s.Id == approverId2.Value))
            {
                var approver2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == approverId2 && s.IsActive == true);
                if (approver2 == null) throw new MessageNotFoundException("Approver2 not found");
                fullName = $"{approver2.FirstName}{(string.IsNullOrWhiteSpace(approver2.LastName) ? "" : " " + approver2.LastName)}";
                throw new MessageNotFoundException($"ApproverId2 {fullName} does not exist");
            }
            foreach (var staff in staffList)
            {
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (staff.ApprovalLevel1 != null && staff.ApprovalLevel2 != null)
                {
                    if ((staff.ApprovalLevel1 == approverId2) || (staff.ApprovalLevel2 == approverId1))
                    {
                        throw new InvalidOperationException("Approval Level 1 and Level 2 cannot be the same staff");
                    }
                }
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
#pragma warning disable CS8601 // Possible null reference assignment.
            var getUser = await _context.StaffCreations
                .Where(s => s.Id == staffId && s.IsActive == true)
                .Select(s => new StaffCreationResponse
                {
                    StaffId = s.Id,
                    StaffCreationId = s.StaffId,
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
                    AccessLevelId = _context.AccessLevels.Where(a => a.Name == s.AccessLevel && a.IsActive).Select(a => a.Id).FirstOrDefault(),
                    AccessLevel = s.AccessLevel,
                    MiddleName = s.MiddleName,
                    PersonalLocation = s.PersonalLocation,
                    PolicyGroupId = _context.PolicyGroups.Where(p => p.Name == s.PolicyGroup && p.IsActive).Select(p => p.Id).FirstOrDefault(),
                    PolicyGroup = s.PolicyGroup,
                    WorkingDayPatternId = _context.WorkingDayPatterns.Where(wp => wp.Name == s.WorkingDayPattern && wp.IsActive).Select(wp => wp.Id).FirstOrDefault(),
                    WorkingDayPattern = s.WorkingDayPattern,
                    WorkingStatusId = _context.WorkingStatuses.Where(w => w.Name == s.WorkingStatus && w.IsActive).Select(w => w.Id).FirstOrDefault(),
                    WorkingStatus = s.WorkingStatus,
                    GeoStatusId = _context.GeoStatuses.Where(g => g.Name == s.GeoStatus && g.IsActive).Select(g => g.Id).FirstOrDefault(),
                    GeoStatus = s.GeoStatus,
                    Tenure = GetTenure(s.JoiningDate),
                    ApprovalLevel = s.ApprovalLevel,
                    ApprovalLevelId1 = s.ApprovalLevel1,
                    ApprovalLevel1 = s.ApprovalLevel1Navigation != null ? $"{s.ApprovalLevel1Navigation.FirstName}{(string.IsNullOrWhiteSpace(s.ApprovalLevel1Navigation.LastName) ? "" : " " + s.ApprovalLevel1Navigation.LastName)}" : null,
                    ApprovalLevelId2 = s.ApprovalLevel2,
                    ApprovalLevel2 = s.ApprovalLevel2Navigation != null ? $"{s.ApprovalLevel2Navigation.FirstName}{(string.IsNullOrWhiteSpace(s.ApprovalLevel2Navigation.LastName) ? "" : " " + s.ApprovalLevel2Navigation.LastName)}" : null,
                    UanNumber = s.UanNumber,
                    EsiNumber = s.EsiNumber,
                    IsMobileAppEligible = s.IsMobileAppEligible,
                    District = s.District,
                    State = s.State,
                    Country = s.Country,
                    PostalCode = s.PostalCode,
                    OtEligible = s.OtEligible,
                    BranchId = s.BranchId,
                    Branch = _context.BranchMasters.Where(b => b.Id == s.BranchId && b.IsActive).Select(b => b.Name).FirstOrDefault() ?? string.Empty,
                    DepartmentId = s.DepartmentId,
                    Department = _context.DepartmentMasters.Where(d => d.Id == s.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefault() ?? string.Empty,
                    DivisionId = s.DivisionId,
                    Division = _context.DivisionMasters.Where(d => d.Id == s.DivisionId && d.IsActive).Select(d => d.Name).FirstOrDefault() ?? string.Empty,
                    MaritalStatusId = _context.MaritalStatuses.Where(m => m.Name == s.MaritalStatus && m.IsActive).Select(m => m.Id).FirstOrDefault(),
                    MaritalStatus = s.MaritalStatus,
                    VolumeId = _context.Volumes.Where(v => v.Name == s.Volume && v.IsActive).Select(v => v.Id).FirstOrDefault(),
                    Volume = s.Volume,
                    DesignationId = s.DesignationId,
                    Designation = _context.DesignationMasters.Where(d => d.Id == s.DesignationId && d.IsActive).Select(d => d.Name).FirstOrDefault() ?? string.Empty,
                    GradeId = s.GradeId,
                    Grade = _context.GradeMasters.Where(g => g.Id == s.GradeId && g.IsActive).Select(g => g.Name).FirstOrDefault() ?? string.Empty,
                    CategoryId = s.CategoryId,
                    Category = _context.CategoryMasters.Where(c => c.Id == s.CategoryId && c.IsActive).Select(c => c.Name).FirstOrDefault() ?? string.Empty,
                    CostCenterId = s.CostCenterId,
                    CostCenter = _context.CostCentreMasters.Where(c => c.Id == s.CostCenterId && c.IsActive).Select(c => c.Name).FirstOrDefault() ?? string.Empty,
                    WorkStationId = s.WorkStationId,
                    WorkStation = _context.WorkstationMasters.Where(w => w.Id == s.WorkStationId && w.IsActive).Select(w => w.Name).FirstOrDefault() ?? string.Empty,
                    LeaveGroupId = s.LeaveGroupId,
                    LeaveGroup = _context.LeaveGroups.Where(l => l.Id == s.LeaveGroupId && l.IsActive).Select(l => l.Name).FirstOrDefault() ?? string.Empty,
                    CompanyMasterId = s.CompanyMasterId,
                    Company = _context.CompanyMasters.Where(c => c.Id == s.CompanyMasterId && c.IsActive).Select(c => c.Name).FirstOrDefault() ?? string.Empty,
                    HolidayCalendarId = s.HolidayCalendarId,
                    HolidayCalendar = _context.HolidayCalendarConfigurations.Where(h => h.Id == s.HolidayCalendarId && h.IsActive).Select(h => h.Name).FirstOrDefault() ?? string.Empty,
                    LocationMasterId = s.LocationMasterId,
                    Location = _context.LocationMasters.Where(l => l.Id == s.LocationMasterId && l.IsActive).Select(l => l.Name).FirstOrDefault() ?? string.Empty,
                    AadharNo = s.AadharNo,
                    PanNo = s.PanNo,
                    PassportNo = s.PassportNo,
                    DrivingLicense = s.DrivingLicense,
                    BankName = s.BankName,
                    BankAccountNo = s.BankAccountNo,
                    BankIfscCode = s.BankIfscCode,
                    BankBranch = s.BankBranch,
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
                    OrganizationTypeName = _context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId && o.IsActive).Select(o => o.Name).FirstOrDefault() ?? string.Empty,
                    IsNonProduction = s.IsNonProduction,
                    ResignationDate = s.ResignationDate,
                    RelievingDate = s.RelievingDate,               
                    CreatedBy = s.CreatedBy,
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
                    new SqlParameter("@DivisionName", string.IsNullOrWhiteSpace(getStaff.DivisionName) ? (object)DBNull.Value : getStaff.DivisionName),
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
                    new SqlParameter("@AccessLevel", string.IsNullOrWhiteSpace(getStaff.AccessLevel) ? (object)DBNull.Value : getStaff.AccessLevel),
                    new SqlParameter("@IncludeTerminated", getStaff.IncludeTerminated.HasValue ? (object)getStaff.IncludeTerminated.Value : DBNull.Value)
            };
            var staffList = await _storedProcedureDbContext.StaffDto
                .FromSqlRaw("EXEC GetStaffByFilters @ApproverId, @ShiftName, @OrganizationTypeName, @CompanyName, @DivisionName, @CategoryName, @CostCentreName, @BranchName, @DepartmentName, @DesignationName, @StaffName, @LocationName, @GradeName, @Status, @LoginUserName, @AccessLevel, @IncludeTerminated", parameters)
                .ToListAsync();
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
            staff.HomeAddress = individualStaffUpdate.HomeAddress;
            staff.FatherName = individualStaffUpdate.FatherName;
            staff.MotherName = individualStaffUpdate.MotherName;
            staff.PanNo = individualStaffUpdate.PanNo;
            staff.PassportNo = individualStaffUpdate.PassportNo;
            staff.DrivingLicense = individualStaffUpdate.DrivingLicense;
            staff.EmergencyContactPerson1 = individualStaffUpdate.EmergencyContactPerson1;
            staff.EmergencyContactPerson2 = individualStaffUpdate.EmergencyContactPerson2;
            staff.EmergencyContactNo1 = individualStaffUpdate.EmergencyContactNo1;
            staff.EmergencyContactNo2 = individualStaffUpdate.EmergencyContactNo2;
            staff.UpdatedBy = individualStaffUpdate.UpdatedBy;
            staff.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }
        public async Task<IndividualStaffResponse> GetMyProfile(int staffId)
        {
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var getUser = await (from s in _context.StaffCreations
                                 join department in _context.DepartmentMasters on s.DepartmentId equals department.Id
                                 join division in _context.DivisionMasters on s.DivisionId equals division.Id
                                 join designation in _context.DesignationMasters on s.DesignationId equals designation.Id
                                 join org in _context.OrganizationTypes on s.OrganizationTypeId equals org.Id
                                 join workingStatus in _context.WorkingStatuses on s.WorkingStatus equals workingStatus.Name
                                 join maritalStatus in _context.MaritalStatuses on s.MaritalStatus equals maritalStatus.Name
                                 join grade in _context.GradeMasters on s.GradeId equals grade.Id into gradeJoin
                                 from grade in gradeJoin.DefaultIfEmpty()
                                 where s.Id == staffId && s.IsActive == true && division.IsActive && department.IsActive &&  (grade == null || grade.IsActive)
                                 && org.IsActive && workingStatus.IsActive && maritalStatus.IsActive
                                 select new IndividualStaffResponse
                                 {
                                     StaffCreationId = s.StaffId,
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
                                     Tenure = GetTenure(s.JoiningDate),
                                     ApprovalLevelId1 = s.ApprovalLevel1,
                                     ApprovalLevel1 = $"{s.ApprovalLevel1Navigation.FirstName}{(string.IsNullOrWhiteSpace(s.ApprovalLevel1Navigation.LastName) ? "" : " " + s.ApprovalLevel1Navigation.LastName)}",
                                     ApprovalLevelId2 = s.ApprovalLevel2,
                                     ApprovalLevel2 = s.ApprovalLevel2Navigation != null ? $"{s.ApprovalLevel2Navigation.FirstName}{(string.IsNullOrWhiteSpace(s.ApprovalLevel2Navigation.LastName) ? "" : " " + s.ApprovalLevel2Navigation.LastName)}" : null,
                                     UanNumber = s.UanNumber,
                                     EsiNumber = s.EsiNumber,
                                     District = s.District,
                                     State = s.State,
                                     Country = s.Country,
                                     PostalCode = s.PostalCode,
                                     DepartmentId = s.DepartmentId,
                                     Department = department.Name,
                                     DivisionId = s.DivisionId,
                                     Division = division.Name,
                                     MaritalStatusId = maritalStatus.Id,
                                     MaritalStatus = maritalStatus.Name,
                                     DesignationId = s.DesignationId,
                                     Designation = designation.Name,
                                     GradeId = s.GradeId,
                                     Grade = grade != null ? grade.Name : "",
                                     AadharNo = s.AadharNo,
                                     PanNo = s.PanNo,
                                     PassportNo = s.PassportNo,
                                     DrivingLicense = s.DrivingLicense,
                                     BankName = s.BankName,
                                     BankAccountNo = s.BankAccountNo,
                                     BankIfscCode = s.BankIfscCode,
                                     BankBranch = s.BankBranch,
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
                                     CreatedBy = s.CreatedBy,
                                 })
                                .FirstOrDefaultAsync();
            if (getUser == null)
            {
                throw new MessageNotFoundException("Staff details not found");
            }
            return getUser;
        }

        public async Task<string> AddStaff(StaffCreationInputModel staffInput)
        {
            bool staffExists = await _context.StaffCreations.AnyAsync(s => s.StaffId == staffInput.StaffId);
            if (staffExists)
            {
                throw new ConflictException($"Staff ID {staffInput.StaffId} already exists.");
            }
            if (staffInput.ApprovalLevel2.HasValue && staffInput.ApprovalLevel1 == staffInput.ApprovalLevel2)
            {
                throw new InvalidOperationException("Approver Level 1 and Approver Level 2 cannot be the same person");
            }
            var message = "The staff member has been successfully created.";
            string profilePhotoPath = string.Empty;
            string aadharCardPath = string.Empty;
            string panCardPath = string.Empty;
            string drivingLicensePath = string.Empty;
            string baseDirectory = Path.Combine(_workspacePath);
            async Task<string> SaveFile(IFormFile file, string folderName)
            {
                if (file == null) throw new ArgumentNullException(nameof(file), "File cannot be null");
                if (file.Length == 0) throw new InvalidOperationException("Uploaded file is empty");
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
            if(staffInput.ProfilePhoto != null)
            {
                profilePhotoPath = await SaveFile(staffInput.ProfilePhoto, "ProfilePhotos");
            }
            if(staffInput.AadharCardFilePath != null)
            {
                aadharCardPath = await SaveFile(staffInput.AadharCardFilePath, "AadharCards");
            }
            if(staffInput.PanCardFilePath != null)
            {
                panCardPath = await SaveFile(staffInput.PanCardFilePath, "PanCards");
            }
            if(staffInput.DrivingLicenseFilePath != null)
            {
                drivingLicensePath = await SaveFile(staffInput.DrivingLicenseFilePath, "DrivingLicenses");
            }
            var departmentId = await _context.DepartmentMasters.AnyAsync(d => d.Id == staffInput.DepartmentId && d.IsActive);
            if (!departmentId) throw new MessageNotFoundException("Department not found");
            var designationId = await _context.DesignationMasters.AnyAsync(d => d.Id == staffInput.DesignationId && d.IsActive);
            if (!designationId) throw new MessageNotFoundException("Designation not found");
            var divisionId = await _context.DivisionMasters.AnyAsync(d => d.Id == staffInput.DivisionId && d.IsActive);
            if (!divisionId) throw new MessageNotFoundException("Division not found");
            var branchId = await _context.BranchMasters.AnyAsync(d => d.Id == staffInput.BranchId && d.IsActive);
            if (!branchId) throw new MessageNotFoundException("Branch not found");
            var companyId = await _context.CompanyMasters.AnyAsync(d => d.Id == staffInput.CompanyMasterId && d.IsActive);
            if (!companyId) throw new MessageNotFoundException("Company not found");
            var locationId = await _context.LocationMasters.AnyAsync(d => d.Id == staffInput.LocationMasterId && d.IsActive);
            if (!locationId) throw new MessageNotFoundException("Location not found");
            var categoryId = await _context.CategoryMasters.AnyAsync(d => d.Id == staffInput.CategoryId && d.IsActive);
            if (!categoryId) throw new MessageNotFoundException("Category not found");
            var workStationId = await _context.WorkstationMasters.AnyAsync(d => d.Id == staffInput.WorkStationId && d.IsActive);
            if (!workStationId) throw new MessageNotFoundException("WorkStation not found");
            var leaveGroupId = await _context.LeaveGroups.AnyAsync(d => d.Id == staffInput.LeaveGroupId && d.IsActive);
            if (!leaveGroupId) throw new MessageNotFoundException("Leave group not found");
            var statusId = await _context.Statuses.AnyAsync(d => d.Id == staffInput.StatusId && d.IsActive);
            if (!statusId) throw new MessageNotFoundException("Status not found");
            var holidayCalanderId = await _context.HolidayCalendarConfigurations.AnyAsync(d => d.Id == staffInput.HolidayCalendarId && d.IsActive);
            if (!holidayCalanderId) throw new MessageNotFoundException("Holiday Calander not found");
            var approvalLevel1 = await _context.StaffCreations.FirstOrDefaultAsync(d => d.Id == staffInput.ApprovalLevel1 && d.IsActive == true);
            if (approvalLevel1 == null) throw new MessageNotFoundException("Approval level 1 not found");
            if(staffInput.ApprovalLevel2 != null)
            {
                var approvalLevel2 = await _context.StaffCreations.AnyAsync(d => d.Id == staffInput.ApprovalLevel2 && d.IsActive == true);
                if (!approvalLevel2) throw new MessageNotFoundException("Approval level 2 not found");
            }
            var staff = new StaffCreation
            {
                CardCode = staffInput.CardCode,
                StaffId = staffInput.StaffId,
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
                IsNonProduction = staffInput.IsNonProduction
            };
            await _context.StaffCreations.AddAsync(staff);
            await _context.SaveChangesAsync();

            var reportingManager = approvalLevel1.OfficialEmail;
            if (!string.IsNullOrEmpty(reportingManager))
            {
                await _emailService.SendPendingStaffRequestEmail(reportingManager, staff);
            }
            return message;
        }

        public async Task<string> UpdateStaffCreationAsync(UpdateStaff updatedStaff)
        {
            var message = "Staff updated successfully";
            if (updatedStaff.ApprovalLevel2.HasValue && updatedStaff.ApprovalLevel1 == updatedStaff.ApprovalLevel2)
            {
                throw new InvalidOperationException("Approver Level 1 and Approver Level 2 cannot be the same person");
            }
            var existingStaff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == updatedStaff.StaffCreationId && s.IsActive == true);
            if (existingStaff == null) throw new MessageNotFoundException("Staff not found");
            var departmentId = await _context.DepartmentMasters.AnyAsync(d => d.Id == updatedStaff.DepartmentId && d.IsActive);
            if (!departmentId) throw new MessageNotFoundException("Department not found");
            var designationId = await _context.DesignationMasters.AnyAsync(d => d.Id == updatedStaff.DesignationId && d.IsActive);
            if (!designationId) throw new MessageNotFoundException("Designation not found");
            var divisionId = await _context.DivisionMasters.AnyAsync(d => d.Id == updatedStaff.DivisionId && d.IsActive);
            if (!divisionId) throw new MessageNotFoundException("Division not found");
            var branchId = await _context.BranchMasters.AnyAsync(d => d.Id == updatedStaff.BranchId && d.IsActive);
            if (!branchId) throw new MessageNotFoundException("Branch not found");
            var companyId = await _context.CompanyMasters.AnyAsync(d => d.Id == updatedStaff.CompanyMasterId && d.IsActive);
            if (!companyId) throw new MessageNotFoundException("Company not found");
            var locationId = await _context.LocationMasters.AnyAsync(d => d.Id == updatedStaff.LocationMasterId && d.IsActive);
            if (!locationId) throw new MessageNotFoundException("Location not found");
            var categoryId = await _context.CategoryMasters.AnyAsync(d => d.Id == updatedStaff.CategoryId && d.IsActive);
            if (!categoryId) throw new MessageNotFoundException("Category not found");
            var workStationId = await _context.WorkstationMasters.AnyAsync(d => d.Id == updatedStaff.WorkStationId && d.IsActive);
            if (!workStationId) throw new MessageNotFoundException("WorkStation not found");
            var leaveGroupId = await _context.LeaveGroups.AnyAsync(d => d.Id == updatedStaff.LeaveGroupId && d.IsActive);
            if (!leaveGroupId) throw new MessageNotFoundException("Leave group not found");
            var statusId = await _context.Statuses.AnyAsync(d => d.Id == updatedStaff.StatusId && d.IsActive);
            if (!statusId) throw new MessageNotFoundException("Status not found");
            var holidayCalanderId = await _context.HolidayCalendarConfigurations.AnyAsync(d => d.Id == updatedStaff.HolidayCalendarId && d.IsActive);
            if (!holidayCalanderId) throw new MessageNotFoundException("Holiday Calander not found");
            var approvalLevel1 = await _context.StaffCreations.AnyAsync(d => d.Id == updatedStaff.ApprovalLevel1 && d.IsActive == true);
            if (!approvalLevel1) throw new MessageNotFoundException("Approval level 1 not found");
            if (updatedStaff.ApprovalLevel2 != null)
            {
                var approvalLevel2 = await _context.StaffCreations.AnyAsync(d => d.Id == updatedStaff.ApprovalLevel2 && d.IsActive == true);
                if (!approvalLevel2) throw new MessageNotFoundException("Approval level 2 not found");
            }
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
            existingStaff.StaffId = updatedStaff.StaffId;
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
            existingStaff.IsNonProduction = updatedStaff.IsNonProduction;
            existingStaff.ResignationDate = updatedStaff.ResignationDate;
            existingStaff.RelievingDate = updatedStaff.RelievingDate;
            existingStaff.UpdatedBy = updatedStaff.UpdatedBy;
            existingStaff.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null) throw new ArgumentNullException(nameof(file), "File cannot be null.");
            if (file.Length == 0) throw new InvalidOperationException("Uploaded file is empty.");
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

        public async Task<List<StaffCreationResponse>> GetStaffRecordsByApprovalLevelAsync(int currentApprovar1, bool? isApprovalLevel1, bool? isApprovalLevel2)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == currentApprovar1 && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == currentApprovar1 && s.IsActive == true);
            if (staff == null) throw new MessageNotFoundException("Approver not found");
            var query = _context.StaffCreations
                   .Where(s => s.IsActive == true &&
                   (
                       isSuperAdmin ||
                       (isApprovalLevel1 == true && isApprovalLevel2 == null && s.ApprovalLevel1 == currentApprovar1) ||
                       (isApprovalLevel2 == true && isApprovalLevel1 == null && s.ApprovalLevel2 == currentApprovar1) ||
                       (isApprovalLevel1 == null && isApprovalLevel2 == null && (s.ApprovalLevel1 == currentApprovar1 || s.ApprovalLevel2 == currentApprovar1))
                   ));
            var records = await query
                .Select(s => new StaffCreationResponse
                {
                    StaffId = s.Id,
                    StaffCreationId = s.StaffId,
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
                    AccessLevelId = _context.AccessLevels.Where(a => a.Name == s.AccessLevel && a.IsActive).Select(a => a.Id).FirstOrDefault(),
                    AccessLevel = s.AccessLevel,
                    MiddleName = s.MiddleName,
                    PersonalLocation = s.PersonalLocation,
                    PolicyGroupId = _context.PolicyGroups.Where(p => p.Name == s.PolicyGroup && p.IsActive).Select(p => p.Id).FirstOrDefault(),
                    PolicyGroup = s.PolicyGroup,
                    WorkingDayPatternId = _context.WorkingDayPatterns.Where(wp => wp.Name == s.WorkingDayPattern && wp.IsActive).Select(wp => wp.Id).FirstOrDefault(),
                    WorkingDayPattern = s.WorkingDayPattern,
                    WorkingStatusId = _context.WorkingStatuses.Where(w => w.Name == s.WorkingStatus && w.IsActive).Select(w => w.Id).FirstOrDefault(),
                    WorkingStatus = s.WorkingStatus,
                    GeoStatusId = _context.GeoStatuses.Where(g => g.Name == s.GeoStatus && g.IsActive).Select(g => g.Id).FirstOrDefault(),
                    GeoStatus = s.GeoStatus,
                    Tenure = GetTenure(s.JoiningDate),
                    ApprovalLevel = s.ApprovalLevel,
                    ApprovalLevelId1 = s.ApprovalLevel1,
                    ApprovalLevel1 = s.ApprovalLevel1Navigation != null ? $"{s.ApprovalLevel1Navigation.FirstName}{(string.IsNullOrWhiteSpace(s.ApprovalLevel1Navigation.LastName) ? "" : " " + s.ApprovalLevel1Navigation.LastName)}" : null,
                    ApprovalLevelId2 = s.ApprovalLevel2,
                    ApprovalLevel2 = s.ApprovalLevel2Navigation != null ? $"{s.ApprovalLevel2Navigation.FirstName}{(string.IsNullOrWhiteSpace(s.ApprovalLevel2Navigation.LastName) ? "" : " " + s.ApprovalLevel2Navigation.LastName)}" : null,
                    UanNumber = s.UanNumber,
                    EsiNumber = s.EsiNumber,
                    IsMobileAppEligible = s.IsMobileAppEligible,
                    District = s.District,
                    State = s.State,
                    Country = s.Country,
                    PostalCode = s.PostalCode,
                    OtEligible = s.OtEligible,
                    BranchId = s.BranchId,
                    Branch = _context.BranchMasters.Where(b => b.Id == s.BranchId && b.IsActive).Select(b => b.Name).FirstOrDefault() ?? string.Empty,
                    DepartmentId = s.DepartmentId,
                    Department = _context.DepartmentMasters.Where(d => d.Id == s.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefault() ?? string.Empty,
                    DivisionId = s.DivisionId,
                    Division = _context.DivisionMasters.Where(d => d.Id == s.DivisionId && d.IsActive).Select(d => d.Name).FirstOrDefault() ?? string.Empty,
                    MaritalStatusId = _context.MaritalStatuses.Where(m => m.Name == s.MaritalStatus && m.IsActive).Select(m => m.Id).FirstOrDefault(),
                    MaritalStatus = s.MaritalStatus,
                    VolumeId = _context.Volumes.Where(v => v.Name == s.Volume&& v.IsActive).Select(v => v.Id).FirstOrDefault(),
                    Volume = s.Volume,
                    DesignationId = s.DesignationId,
                    Designation = _context.DesignationMasters.Where(d => d.Id == s.DesignationId && d.IsActive).Select(d => d.Name).FirstOrDefault() ?? string.Empty,
                    GradeId = s.GradeId,
                    Grade = _context.GradeMasters.Where(g => g.Id == s.GradeId && g.IsActive).Select(g => g.Name).FirstOrDefault() ?? string.Empty,
                    CategoryId = s.CategoryId,
                    Category = _context.CategoryMasters.Where(c => c.Id == s.CategoryId && c.IsActive).Select(c => c.Name).FirstOrDefault() ?? string.Empty,
                    CostCenterId = s.CostCenterId,
                    CostCenter = _context.CostCentreMasters.Where(c => c.Id == s.CostCenterId && c.IsActive).Select(c => c.Name).FirstOrDefault() ?? string.Empty,
                    WorkStationId = s.WorkStationId,
                    WorkStation = _context.WorkstationMasters.Where(w => w.Id == s.WorkStationId && w.IsActive).Select(w => w.Name).FirstOrDefault() ?? string.Empty,
                    LeaveGroupId = s.LeaveGroupId,
                    LeaveGroup = _context.LeaveGroups.Where(l => l.Id == s.LeaveGroupId && l.IsActive).Select(l => l.Name).FirstOrDefault() ?? string.Empty,
                    CompanyMasterId = s.CompanyMasterId,
                    Company = _context.CompanyMasters.Where(c => c.Id == s.CompanyMasterId && c.IsActive).Select(c => c.Name).FirstOrDefault() ?? string.Empty,
                    HolidayCalendarId = s.HolidayCalendarId,
                    HolidayCalendar = _context.HolidayCalendarConfigurations.Where(h => h.Id == s.HolidayCalendarId && h.IsActive).Select(h => h.Name).FirstOrDefault() ?? string.Empty,
                    LocationMasterId = s.LocationMasterId,
                    Location = _context.LocationMasters.Where(l => l.Id == s.LocationMasterId && l.IsActive).Select(l => l.Name).FirstOrDefault() ?? string.Empty,
                    AadharNo = s.AadharNo,
                    PanNo = s.PanNo,
                    PassportNo = s.PassportNo,
                    DrivingLicense = s.DrivingLicense,
                    BankName = s.BankName,
                    BankAccountNo = s.BankAccountNo,
                    BankIfscCode = s.BankIfscCode,
                    BankBranch = s.BankBranch,
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
                    OrganizationTypeName = _context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId && o.IsActive).Select(o => o.Name).FirstOrDefault() ?? string.Empty,
                    IsNonProduction = s.IsNonProduction,
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

        private static string GetTenure(DateOnly joiningDate)
        {
            var now = DateTime.Now;
            int years = now.Year - joiningDate.Year;
            int months = now.Month - joiningDate.Month;
            if (now.Day < joiningDate.Day)
            {
                months--;
            }
            if (months < 0)
            {
                years--;
                months += 12;
            }

            return $"{years} Year{(years != 1 ? "s" : "")} {months} Month{(months != 1 ? "s" : "")}";
        }
        public async Task<List<StaffCreationResponse>> GetPendingStaffForManagerApproval(int approverId)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == approverId && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";
            var isApprovalLevel1 = await _context.StaffCreations.AnyAsync(s => s.ApprovalLevel1 == approverId && s.IsActive == true);
            var isApprovalLevel2 = await _context.StaffCreations.AnyAsync(s => s.ApprovalLevel2 == approverId && s.IsActive == true);
            var records = await _context.StaffCreations
                .Where(s => (isSuperAdmin || (isApprovalLevel1 && s.ApprovalLevel1 == approverId) || (isApprovalLevel2 && s.ApprovalLevel2 == approverId)) && s.IsActive == null)
                .Select(s => new StaffCreationResponse
                {
                    StaffId = s.Id,
                    StaffCreationId = s.StaffId,
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
                    AccessLevelId = _context.AccessLevels.Where(a => a.Name == s.AccessLevel && a.IsActive).Select(a => a.Id).FirstOrDefault(),
                    AccessLevel = s.AccessLevel,
                    MiddleName = s.MiddleName,
                    PersonalLocation = s.PersonalLocation,
                    PolicyGroupId = _context.PolicyGroups.Where(p => p.Name == s.PolicyGroup && p.IsActive).Select(p => p.Id).FirstOrDefault(),
                    PolicyGroup = s.PolicyGroup,
                    WorkingDayPatternId = _context.WorkingDayPatterns.Where(wp => wp.Name == s.WorkingDayPattern && wp.IsActive).Select(wp => wp.Id).FirstOrDefault(),
                    WorkingDayPattern = s.WorkingDayPattern,
                    WorkingStatusId = _context.WorkingStatuses.Where(w => w.Name == s.WorkingStatus && w.IsActive).Select(w => w.Id).FirstOrDefault(),
                    WorkingStatus = s.WorkingStatus,
                    GeoStatusId = _context.GeoStatuses.Where(g => g.Name == s.GeoStatus && g.IsActive).Select(g => g.Id).FirstOrDefault(),
                    GeoStatus = s.GeoStatus,
                    Tenure = s.Tenure,
                    ApprovalLevel = s.ApprovalLevel,
                    ApprovalLevelId1 = s.ApprovalLevel1,
                    ApprovalLevel1 = s.ApprovalLevel1Navigation != null ? $"{s.ApprovalLevel1Navigation.FirstName}{(string.IsNullOrWhiteSpace(s.ApprovalLevel1Navigation.LastName) ? "" : " " + s.ApprovalLevel1Navigation.LastName)}" : null,
                    ApprovalLevelId2 = s.ApprovalLevel2,
                    ApprovalLevel2 = s.ApprovalLevel2Navigation != null ? $"{s.ApprovalLevel2Navigation.FirstName}{(string.IsNullOrWhiteSpace(s.ApprovalLevel2Navigation.LastName) ? "" : " " + s.ApprovalLevel2Navigation.LastName)}" : null,
                    UanNumber = s.UanNumber,
                    EsiNumber = s.EsiNumber,
                    IsMobileAppEligible = s.IsMobileAppEligible,
                    District = s.District,
                    State = s.State,
                    Country = s.Country,
                    PostalCode = s.PostalCode,
                    OtEligible = s.OtEligible,
                    BranchId = s.BranchId,
                    Branch = _context.BranchMasters.Where(b => b.Id == s.BranchId && b.IsActive).Select(b => b.Name).FirstOrDefault() ?? string.Empty,
                    DepartmentId = s.DepartmentId,
                    Department = _context.DepartmentMasters.Where(d => d.Id == s.DepartmentId && d.IsActive).Select(d => d.Name).FirstOrDefault() ?? string.Empty,
                    DivisionId = s.DivisionId,
                    Division = _context.DivisionMasters.Where(d => d.Id == s.DivisionId && d.IsActive).Select(d => d.Name).FirstOrDefault() ?? string.Empty,
                    MaritalStatusId = _context.MaritalStatuses.Where(m => m.Name == s.MaritalStatus && m.IsActive).Select(m => m.Id).FirstOrDefault(),
                    MaritalStatus = s.MaritalStatus,
                    VolumeId = _context.Volumes.Where(v => v.Name == s.Volume && v.IsActive).Select(v => v.Id).FirstOrDefault(),
                    Volume = s.Volume,
                    DesignationId = s.DesignationId,
                    Designation = _context.DesignationMasters.Where(d => d.Id == s.DesignationId && d.IsActive).Select(d => d.Name).FirstOrDefault() ?? string.Empty,
                    GradeId = s.GradeId,
                    Grade = _context.GradeMasters.Where(g => g.Id == s.GradeId && g.IsActive).Select(g => g.Name).FirstOrDefault() ?? string.Empty,
                    CategoryId = s.CategoryId,
                    Category = _context.CategoryMasters.Where(c => c.Id == s.CategoryId && c.IsActive).Select(c => c.Name).FirstOrDefault() ?? string.Empty,
                    CostCenterId = s.CostCenterId,
                    CostCenter = _context.CostCentreMasters.Where(c => c.Id == s.CostCenterId && c.IsActive).Select(c => c.Name).FirstOrDefault() ?? string.Empty,
                    WorkStationId = s.WorkStationId,
                    WorkStation = _context.WorkstationMasters.Where(w => w.Id == s.WorkStationId && w.IsActive).Select(w => w.Name).FirstOrDefault() ?? string.Empty,
                    LeaveGroupId = s.LeaveGroupId,
                    LeaveGroup = _context.LeaveGroups.Where(l => l.Id == s.LeaveGroupId && l.IsActive).Select(l => l.Name).FirstOrDefault() ?? string.Empty,
                    CompanyMasterId = s.CompanyMasterId,
                    Company = _context.CompanyMasters.Where(c => c.Id == s.CompanyMasterId && c.IsActive).Select(c => c.Name).FirstOrDefault() ?? string.Empty,
                    HolidayCalendarId = s.HolidayCalendarId,
                    HolidayCalendar = _context.HolidayCalendarConfigurations.Where(h => h.Id == s.HolidayCalendarId && h.IsActive).Select(h => h.Name).FirstOrDefault() ?? string.Empty,
                    LocationMasterId = s.LocationMasterId,
                    Location = _context.LocationMasters.Where(l => l.Id == s.LocationMasterId && l.IsActive).Select(l => l.Name).FirstOrDefault() ?? string.Empty,
                    AadharNo = s.AadharNo,
                    PanNo = s.PanNo,
                    PassportNo = s.PassportNo,
                    DrivingLicense = s.DrivingLicense,
                    BankName = s.BankName,
                    BankAccountNo = s.BankAccountNo,
                    BankIfscCode = s.BankIfscCode,
                    BankBranch = s.BankBranch,
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
                    OrganizationTypeName = _context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId && o.IsActive).Select(o => o.Name).FirstOrDefault() ?? string.Empty,
                    IsNonProduction = s.IsNonProduction,
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

        public async Task<string> ApprovePendingStaffs(ApprovePendingStaff approvePendingStaff)
        {
            var message = "";
            if (approvePendingStaff.SelectedRows.Count() == 0) throw new MessageNotFoundException("No rows selected");
            var selectedRows = approvePendingStaff.SelectedRows;
            foreach (var item in selectedRows)
            {
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == item.Id);
                if (staff == null) throw new MessageNotFoundException("Staff not found");
                if (staff.IsActive == true) throw new ConflictException("Staff already approved");
                if (staff.IsActive == false) throw new ConflictException("Staff already rejected");
                staff.IsActive = approvePendingStaff.IsApproved;
                staff.UpdatedBy = approvePendingStaff.ApprovedBy;
                staff.UpdatedUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var staffApprove = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == item.Id && s.IsActive == true);
                if (staffApprove != null)
                {
                    var employee = staffApprove.StaffId;
                    if (staff.OrganizationTypeId != 1)
                    {
                        DateOnly probationEndDate;
                        if (staffApprove.OrganizationTypeId == 2)
                        {
                            probationEndDate = staffApprove.JoiningDate.AddMonths(6);
                        }
                        else if (staffApprove.OrganizationTypeId == 3)
                        {
                            probationEndDate = staffApprove.JoiningDate.AddMonths(3);
                        }
                        else
                        {
                            probationEndDate = staffApprove.JoiningDate.AddMonths(3);
                        }
                        var probation = new Probation
                        {
                            StaffCreationId = staffApprove.Id,
                            ProbationStartDate = staffApprove.JoiningDate,
                            ProbationEndDate = probationEndDate,
                            IsActive = true,
                            CreatedBy = staffApprove.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        await _context.Probations.AddAsync(probation);
                        await _context.SaveChangesAsync();
                    }
                }
                var createdByUser = await _context.StaffCreations.FirstOrDefaultAsync(u => u.Id == staff.CreatedBy && u.IsActive == true);
                if (createdByUser != null)
                {
                    await _emailService.SendPendingStaffApprovalEmail(staff, createdByUser, approvePendingStaff);
                }
                message = approvePendingStaff.IsApproved ? "Staff profile approved successfully" : "Staff profile rejected successfully";
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
            await _context.DropDownMasters.AddAsync(dropDown);
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
                {1013, new MonthRange() },
                {1014, new ReaderType() },
                {1015, new StatusDropdown() },
                {1016, new GraceTimeDropdown() },
                {1017, new ShiftTypeDropDown() },
                {1018, new ReimbursementType() },
                {1020, new ApplicationType() },
                {1021, new EventType() },
                {1022, new CompanyMaster() },
                {1023, new BranchMaster() },
                {1024, new LocationMaster() },
                {1025, new CategoryMaster() },
                {1026, new DepartmentMaster() },
                {1027, new DesignationMaster() },
                {1028, new GradeMaster() },
                {1029, new CostCentreMaster() },
                {1030, new Shift() },
                {1031, new DivisionMaster() },
                {1032, new LeaveGroup() },
                {1033, new HolidayCalendarConfiguration() },
                {1034, new WorkstationMaster() },
                {1035, new PrefixLeaveType() },
                {1036, new SuffixLeaveType() },
                {1037, new HolidayType() },
                {1038, new TypesOfReport() },
                {1039, new WorkingType() },
                {1040, new PerformanceRatingScale() },
                {1041, new HolidayMaster() },
                {1042, new LeaveType() },
                {1043, new AttendanceStatusColor() },
                {1044, new PerformanceUploadType() },
                {1045, new AppraisalSelectionDropDown() }
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

            await _context.AddAsync(newEntity);
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
                { 1013, _context.MonthRanges.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1014, _context.ReaderTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1015, _context.StatusDropdowns.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1016, _context.GraceTimeDropdowns.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1017, _context.ShiftTypeDropDowns.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1018, _context.ReimbursementTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1020, _context.ApplicationTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1021, _context.EventTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1022, _context.CompanyMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1023, _context.BranchMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1024, _context.LocationMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1025, _context.CategoryMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1026, _context.DepartmentMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1027, _context.DesignationMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1028, _context.GradeMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1029, _context.CostCentreMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1030, _context.Shifts.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = $"{ws.Name}({ws.StartTime}-{ws.EndTime})", CreatedBy = ws.CreatedBy }) },
                { 1031, _context.DivisionMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1032, _context.LeaveGroups.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1033, _context.HolidayCalendarConfigurations.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1034, _context.WorkstationMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1035, _context.PrefixLeaveTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1036, _context.SuffixLeaveTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1037, _context.HolidayTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1038, _context.TypesOfReports.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1039, _context.WorkingTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1040, _context.PerformanceRatingScales.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1041, _context.HolidayMasters.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1042, _context.LeaveTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1043, _context.AttendanceStatusColors.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1044, _context.PerformanceUploadTypes.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) },
                { 1045, _context.AppraisalSelectionDropDowns.Where(ws => ws.IsActive).Select(ws => new DropDownResponse { Id = ws.Id, Name = ws.Name, CreatedBy = ws.CreatedBy }) }
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
                { 1013, async () => await _context.MonthRanges.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1014, async () => await _context.ReaderTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1015, async () => await _context.StatusDropdowns.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1016, async () => await _context.GraceTimeDropdowns.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1017, async () => await _context.ShiftTypeDropDowns.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1018, async () => await _context.ReimbursementTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1020, async () => await _context.ApplicationTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1021, async () => await _context.EventTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1022, async () => await _context.CompanyMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1023, async () => await _context.BranchMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1024, async () => await _context.LocationMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1025, async () => await _context.CategoryMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1026, async () => await _context.DepartmentMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1027, async () => await _context.DesignationMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1028, async () => await _context.GradeMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1029, async () => await _context.CostCentreMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1030, async () => await _context.Shifts.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1031, async () => await _context.DivisionMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1032, async () => await _context.LeaveGroups.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1033, async () => await _context.HolidayCalendarConfigurations.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1034, async () => await _context.WorkstationMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1035, async () => await _context.PrefixLeaveTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1036, async () => await _context.SuffixLeaveTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1037, async () => await _context.HolidayTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1038, async () => await _context.TypesOfReports.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1039, async () => await _context.WorkingTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1040, async () => await _context.PerformanceRatingScales.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1041, async () => await _context.HolidayMasters.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1042, async () => await _context.LeaveTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1043, async () => await _context.AttendanceStatusColors.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1044, async () => await _context.PerformanceUploadTypes.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) },
                { 1045, async () => await _context.AppraisalSelectionDropDowns.FirstOrDefaultAsync(ws => ws.Id == dropDownDetailsRequest.DropDownDetailId && ws.IsActive) }
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