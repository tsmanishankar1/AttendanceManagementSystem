using AttendanceManagement.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using OfficeOpenXml.Drawing;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using AttendanceManagement.Input_Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Asn1.Ocsp;
public class ExcelImportService
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _workspacePath = "ExcelTemplates";
    public ExcelImportService(AttendanceManagementSystemContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), _workspacePath);
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }
    }

    public async Task<byte[]> GetExcelTemplateBytes(int excelImportId)
    {
        var excelTemplate = await _context.ExcelImports.FirstOrDefaultAsync(x => x.Id == excelImportId && x.IsActive == true);
        if (excelTemplate == null)
        {
            throw new MessageNotFoundException("Excel template not found");
        }
        string fileName = $"{excelTemplate.Name}.xlsx";
        string filePath = Path.Combine(_workspacePath, fileName);
        if (!System.IO.File.Exists(filePath))
        {
            throw new MessageNotFoundException("Excel template not found in workspace");
        }
        return await System.IO.File.ReadAllBytesAsync(filePath);
    }

    public async Task<string> GetExcelTemplateFilePath(int excelImportId)
    {
        var excelTemplate = await _context.ExcelImports.FirstOrDefaultAsync(x => x.Id == excelImportId && x.IsActive == true);
        if (excelTemplate == null)
        {
            throw new MessageNotFoundException("Excel template not found");
        }
        string fileName = $"{excelTemplate.Name}.xlsx";
        return Path.Combine(_workspacePath, fileName);
    }

    public async Task<string> ImportExcelAsync(ExcelImportDto excelImportDto)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        try
        {
            var excelImportType = await _context.ExcelImports.FirstOrDefaultAsync(e => e.Id == excelImportDto.ExcelImportId && e.IsActive);
            if (excelImportType == null) throw new MessageNotFoundException("Excel import type not found");
            var staffExists = await _context.StaffCreations.AnyAsync(s => s.Id == excelImportDto.CreatedBy && s.IsActive == true);
            if (!staffExists)
            {
                throw new MessageNotFoundException($"Staff not found");
            }
            using (var stream = new MemoryStream())
            {
                await excelImportDto.File.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null) throw new MessageNotFoundException("Worksheet not found in the uploaded file.");
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(cell => cell.Text.Trim()).ToList();
                    var requiredHeaders = new List<string>();
                    if (excelImportDto.ExcelImportId == 1)
                    {
                        requiredHeaders = new List<string>
                        {
                            "CardCode", "StaffId", "Title", "FirstName", "LastName", "ShortName", "Gender", "Hide", "BloodGroup", "MaritalStatus", "Dob", "MarriageDate",
                            "PersonalPhone", "JoiningDate", "Confirmed", "Branch", "Department", "Division", "Volume", "Designation", "Grade", "Category",
                            "CostCenter", "WorkStation", "City", "District", "State","Country", "OtEligible", "ApprovalLevel1", "ApprovalLevel2", "AccessLevel",
                            "PolicyGroup", "WorkingDayPattern", "Tenure", "UanNumber", "EsiNumber", "IsMobileAppEligible", "GeoStatus", "MiddleName",
                            "OfficialPhone", "PersonalLocation", "PersonalEmail", "LeaveGroup", "Company", "Location", "HolidayCalendar", "Status",
                            "AadharNo", "PanNo", "PassportNo", "DrivingLicense", "BankName", "BankAccountNo", "BankIfscCode", "BankBranch", "Qualification",
                            "HomeAddress", "FatherName", "EmergencyContactPerson1", "EmergencyContactPerson2", "EmergencyContactNo1", "EmergencyContactNo2", "MotherName",
                            "FatherAadharNo", "MotherAadharNo", "OrganizationType", "WorkingStatus", "ConfirmationDate", "PostalCode", "ApprovalLevel", "OfficialEmail"
                        };
                    }
                    else if (excelImportDto.ExcelImportId == 2)
                    {
                        requiredHeaders = new List<string> {"LeaveTypeName", "StaffCreationId", "TransactionFlag", "Month", "Year", "Remarks", "LeaveCount", "LeaveReason"};
                    }
                    else if (excelImportDto.ExcelImportId == 3)
                    {
                        requiredHeaders = new List<string> {"FullName", "ShortName", "Phone", "Fax", "Email" };
                    }
                    else if (excelImportDto.ExcelImportId == 4 || excelImportDto.ExcelImportId == 5 || excelImportDto.ExcelImportId == 6)
                    {
                        requiredHeaders = new List<string> {"FullName", "ShortName" };
                    }
                    else if (excelImportDto.ExcelImportId == 7)
                    {
                        requiredHeaders = new List<string> {"Name", "ShortName" };
                    }
                    else if (excelImportDto.ExcelImportId == 8)
                    {
                        requiredHeaders = new List<string> {"StaffId", "SelectPunch", "InPunch", "OutPunch", "Remarks", "ApplicationTypeName" };
                    }
                    else if (excelImportDto.ExcelImportId == 9)
                    {
                        requiredHeaders = new List<string> {"StartTime", "EndTime", "StaffId", "Remarks", "PermissionDate", "PermissionType", "ApplicationTypeName" };
                    }
                    else if (excelImportDto.ExcelImportId == 10)
                    {
                        requiredHeaders = new List<string> {"StaffId", "ResignationDate", "RelievingDate", "Status" };
                    }
                    else if (excelImportDto.ExcelImportId == 11)
                    {
                        requiredHeaders = new List<string> {"ShiftName", "StartTime", "EndTime", "ShortName" };
                    }
                    else if (excelImportDto.ExcelImportId == 12)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "FromDate", "ToDate", "Reason", "StaffId", "StartDuration", "EndDuration", "LeaveTypeName" };
                    }
                    else if (excelImportDto.ExcelImportId == 13)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "StartTime", "EndTime", "StartDate", "EndDate", "Reason", "StaffId", "StartDuration", "EndDuration" };
                    }
                    else if (excelImportDto.ExcelImportId == 14)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "FromTime", "ToTime", "FromDate", "ToDate", "Reason", "StartDuration", "EndDuration", "StaffId" };
                    }
                    else if (excelImportDto.ExcelImportId == 15)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "StaffId", "OTDate", "StartTime", "EndTime", "OTType" };
                    }
                    else if (excelImportDto.ExcelImportId == 16)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "StaffId", "TransactionDate", "BeforeShiftHours", "AfterShiftHours", "Remarks", "DurationHours" };
                    }
                    else if (excelImportDto.ExcelImportId == 17)
                    {
                        requiredHeaders = new List<string> {"StaffId", "VaccinatedDate", "VaccinationNumber", "IsExempted", "Comments" };
                    }
                    else if(excelImportDto.ExcelImportId == 18)
                    {
                        requiredHeaders = new List<string>
                        {
                            "Emp ID", "Name", "Department", "Prod Score", "Prod %", "Prod Grade", "Quality Score", "Qual %", "No Of Absent",
                            "Attd Score", "Attd %", "Attd Grade", "Final Total", "Total Score", "Final Score %", "Final Grade", "Production Achieved % Jan",
                            "Production Achieved % Feb", "Production Achieved % Mar", "Production Achieved % Apr", "Production Achieved % May",
                            "Production Achieved % Jun", "Production Achieved % Jul", "Production Achieved % Aug", "Production Achieved % Sep",
                            "Production Achieved % Oct","Production Achieved % Nov", "Production Achieved % Dec"
                        };
                    }
                    else if (excelImportDto.ExcelImportId == 19)
                    {
                        requiredHeaders = new List<string>
                        {
                            "Emp ID", "Name", "EMP Division", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct","Nov", "Dec"
                        };
                    }
                    else
                    {
                        throw new MessageNotFoundException("Excel import type not found");
                    }
                    var missingHeaders = requiredHeaders.Where(header => !headerRow.Contains(header)).ToList();
                    if (missingHeaders.Any())
                    {
                        throw new InvalidOperationException($"Invalid Excel file for excel Import type {excelImportType.Name}. Missing headers: {string.Join(", ", missingHeaders)}");
                    }
                    var extraHeaders = headerRow.Except(requiredHeaders).ToList();
                    if (extraHeaders.Any())
                    {
                        throw new InvalidOperationException($"Invalid Excel file for excel Import type {excelImportType.Name}. Contains unexpected headers: {string.Join(", ", extraHeaders)}");
                    }
                    var columnIndexes = requiredHeaders.ToDictionary(
                        header => header,
                        header => headerRow.IndexOf(header) + 1
                    );
                    var rowCount = worksheet.Dimension.Rows;
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            if (excelImportDto.ExcelImportId == 1)
                            {
                                var staffCreations = new List<StaffCreation>();
                                var validDepartmentIds = _context.DepartmentMasters.Where(d => d.IsActive).Select(d => d.Id).ToHashSet();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var branchName = worksheet.Cells[row, columnIndexes["BranchName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(branchName))
                                    {
                                        continue;
                                    }
                                    var branchId = await _context.BranchMasters
                                        .Where(d => d.Name.ToLower() == branchName.ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();
                                    if (branchId == 0)
                                    {
                                        continue;
                                    }
                                    var approvalLevel1 = worksheet.Cells[row, columnIndexes["ApprovalLevel1"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(approvalLevel1))
                                    {
                                        continue;
                                    }
                                    int numericPart1 = int.Parse(Regex.Match(approvalLevel1, @"\d+").Value);
                                    var approvalId1 = await (from staff in _context.StaffCreations
                                                       where staff.Id == numericPart1 && staff.IsActive == true
                                                       select staff.Id).FirstOrDefaultAsync();
                                    if (approvalId1 == 0)
                                    {
                                        continue;
                                    }
                                    var approvalLevel2 = worksheet.Cells[row, columnIndexes["ApprovalLevel2"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(approvalLevel2))
                                    {
                                        continue;
                                    }
                                    int numericPart = int.Parse(Regex.Match(approvalLevel2, @"\d+").Value);
                                    var approvalId2 = await (from staff in _context.StaffCreations
                                                       where staff.Id == numericPart1 && staff.IsActive == true
                                                       select staff.Id).FirstOrDefaultAsync();
                                    if (approvalId2 == 0)
                                    {
                                        continue;
                                    }
                                    var departmentName = worksheet.Cells[row, columnIndexes["DepartmentName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(departmentName))
                                    {
                                        continue;
                                    }
                                    var departmentId = await _context.DepartmentMasters
                                        .Where(d => d.Name.ToLower() == departmentName.ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();
                                    if (departmentId == 0) throw new MessageNotFoundException($"Department '{departmentName}' not found");
                                    var statusName = worksheet.Cells[row, columnIndexes["StatusName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(statusName))
                                    {
                                        continue;
                                    }
                                    var statusId = await _context.Statuses
                                        .Where(d => d.Name.ToLower() == statusName.ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();
                                    if (statusId == 0) throw new MessageNotFoundException($"Status '{statusName}' not found");
                                    var divisionName = worksheet.Cells[row, columnIndexes["DivisionName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(divisionName))
                                    {
                                        continue;
                                    }
                                    var divisionId = await _context.DivisionMasters
                                        .Where(d => d.Name.ToLower() == divisionName.ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();
                                    if (divisionId == 0) throw new MessageNotFoundException($"Division '{divisionName}' not found");
                                    var designationName = worksheet.Cells[row, columnIndexes["DesignationName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(designationName))
                                    {
                                        continue;
                                    }
                                    var designationId = await _context.DesignationMasters
                                        .Where(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();
                                    if (designationId == 0) throw new MessageNotFoundException($"Designation '{designationName}' not found");
                                    var gradeName = worksheet.Cells[row, columnIndexes["GradeName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(gradeName))
                                    {
                                        continue;
                                    }
                                    var gradeId = await _context.GradeMasters
                                        .Where(g => g.Name.ToLower() == gradeName.ToLower() && g.IsActive)
                                        .Select(g => g.Id)
                                        .FirstOrDefaultAsync();
                                    if (gradeId == 0) throw new MessageNotFoundException($"Grade '{gradeName}' not found");
                                    var organizationTypeName = worksheet.Cells[row, columnIndexes["OrganizationType"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(organizationTypeName))
                                    {
                                        continue;
                                    }
                                    var organizationTypeId = await _context.OrganizationTypes
                                        .Where(g => g.ShortName.ToLower() == organizationTypeName.ToLower() && g.IsActive)
                                        .Select(g => g.Id)
                                        .FirstOrDefaultAsync();
                                    if (organizationTypeId == 0) throw new MessageNotFoundException($"Organization '{organizationTypeName}' not found");
                                    var categoryName = worksheet.Cells[row, columnIndexes["CategoryName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(categoryName))
                                    {
                                        continue;
                                    }
                                    var categoryId = await _context.CategoryMasters
                                        .Where(c => c.Name.ToLower() == categoryName.ToLower() && c.IsActive)
                                        .Select(c => c.Id)
                                        .FirstOrDefaultAsync();
                                    if (categoryId == 0) throw new MessageNotFoundException($"Category '{categoryName}' not found");
                                    var costCenterName = worksheet.Cells[row, columnIndexes["CostCenterName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(costCenterName))
                                    {
                                        continue;
                                    }
                                    var costCenterId = await _context.CostCentreMasters
                                        .Where(c => c.Name.ToLower() == costCenterName.ToLower() && c.IsActive)
                                        .Select(c => c.Id)
                                        .FirstOrDefaultAsync();
                                    if (costCenterId == 0) throw new MessageNotFoundException($"Cost Center '{costCenterName}' not found");
                                    var workStationName = worksheet.Cells[row, columnIndexes["WorkStationName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(workStationName))
                                    {
                                        continue;
                                    }
                                    var workStationId = await _context.WorkstationMasters
                                        .Where(w => w.Name.ToLower() == workStationName.ToLower() && w.IsActive)
                                        .Select(w => w.Id)
                                        .FirstOrDefaultAsync();
                                    if (workStationId == 0) throw new MessageNotFoundException($"Workstation '{workStationName}' not found");
                                    var leaveGroupName = worksheet.Cells[row, columnIndexes["LeaveGroupName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(leaveGroupName))
                                    {
                                        continue;
                                    }
                                    var leaveGroupId = await _context.LeaveGroupConfigurations
                                        .Where(l => l.LeaveGroupConfigurationName.ToLower() == leaveGroupName.ToLower() && l.IsActive)
                                        .Select(l => l.Id)
                                        .FirstOrDefaultAsync();
                                    if (leaveGroupId == 0) throw new MessageNotFoundException($"Leave Group '{leaveGroupName}' not found");
                                    var companyName = worksheet.Cells[row, columnIndexes["CompanyMasterName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(companyName))
                                    {
                                        continue;
                                    }
                                    var companyMasterId = await _context.CompanyMasters
                                        .Where(c => c.Name.ToLower() == companyName.ToLower() && c.IsActive)
                                        .Select(c => c.Id)
                                        .FirstOrDefaultAsync();
                                    if (companyMasterId == 0) throw new MessageNotFoundException($"Company '{companyName}' not found");
                                    var locationName = worksheet.Cells[row, columnIndexes["LocationMasterName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(locationName))
                                    {
                                        continue;
                                    }
                                    var locationMasterId = await _context.LocationMasters
                                        .Where(l => l.Name.ToLower() == locationName.ToLower() && l.IsActive)
                                        .Select(l => l.Id)
                                        .FirstOrDefaultAsync();
                                    if (locationMasterId == 0) throw new MessageNotFoundException($"Location '{locationName}' not found");
                                    var holidayCalendarName = worksheet.Cells[row, columnIndexes["HolidayCalendarName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(holidayCalendarName))
                                    {
                                        continue;
                                    }
                                    var holidayCalendarId = await _context.HolidayCalendarConfigurations
                                        .Where(h => h.Name.ToLower() == holidayCalendarName.ToLower() && h.IsActive)
                                        .Select(h => h.Id)
                                        .FirstOrDefaultAsync();
                                    if (holidayCalendarId == 0) throw new MessageNotFoundException($"Holiday Calendar '{holidayCalendarName}' not found");
                                    var staffCreation = new StaffCreation
                                    {
                                        CardCode = worksheet.Cells[row, columnIndexes["CardCode"]].Text.Trim(),
                                        StaffId = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim(),
                                        Title = worksheet.Cells[row, columnIndexes["Title"]].Text.Trim(),
                                        FirstName = worksheet.Cells[row, columnIndexes["FirstName"]].Text.Trim(),
                                        LastName = worksheet.Cells[row, columnIndexes["LastName"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        Gender = worksheet.Cells[row, columnIndexes["Gender"]].Text.Trim(),
                                        Hide = bool.TryParse(worksheet.Cells[row, columnIndexes["Hide"]].Text, out var hide) ? hide : false,
                                        BloodGroup = worksheet.Cells[row, columnIndexes["BloodGroup"]].Text.Trim(),
                                        MaritalStatus = worksheet.Cells[row, columnIndexes["MaritalStatus"]].Text.Trim(),
                                        Dob = (DateOnly)(DateOnly.TryParse(worksheet.Cells[row, columnIndexes["Dob"]].Text, out var dob) ? dob : default),
                                        MarriageDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["MarriageDate"]].Text, out var marriageDate) ? marriageDate : (DateOnly?)null,
                                        PersonalPhone = long.TryParse(worksheet.Cells[row, columnIndexes["PersonalPhone"]].Text, out var personalPhone) ? personalPhone : 0,
                                        JoiningDate = (DateOnly)(DateOnly.TryParse(worksheet.Cells[row, columnIndexes["JoiningDate"]].Text, out var joiningDate) ? joiningDate : default),
                                        Confirmed = bool.TryParse(worksheet.Cells[row, columnIndexes["Confirmed"]].Text, out var confirmed) ? confirmed : false,
                                        BranchId = branchId,
                                        DepartmentId = departmentId,
                                        DivisionId = divisionId,
                                        Volume = worksheet.Cells[row, columnIndexes["Volume"]].Text.Trim(),
                                        DesignationId = designationId,
                                        GradeId = gradeId,
                                        CategoryId = categoryId,
                                        CostCenterId = costCenterId,
                                        WorkStationId = workStationId,
                                        City = worksheet.Cells[row, columnIndexes["City"]].Text.Trim(),
                                        District = worksheet.Cells[row, columnIndexes["District"]].Text.Trim(),
                                        State = worksheet.Cells[row, columnIndexes["State"]].Text.Trim(),
                                        Country = worksheet.Cells[row, columnIndexes["Country"]].Text.Trim(),
                                        OtEligible = bool.TryParse(worksheet.Cells[row, columnIndexes["Oteligible"]].Text, out var otEligible) ? otEligible : false,
                                        ApprovalLevel1 = approvalId1,
                                        ApprovalLevel2 = approvalId2,
                                        AccessLevel = worksheet.Cells[row, columnIndexes["AccessLevel"]]?.Text?.Trim() ?? string.Empty,
                                        PolicyGroup = worksheet.Cells[row, columnIndexes["PolicyGroup"]]?.Text?.Trim() ?? string.Empty,
                                        WorkingDayPattern = worksheet.Cells[row, columnIndexes["WorkingDayPattern"]]?.Text?.Trim() ?? string.Empty,
                                        Tenure = decimal.TryParse(worksheet.Cells[row, columnIndexes["Tenure"]]?.Text.Trim(), out decimal tenure) ? tenure : 0m,
                                        UanNumber = worksheet.Cells[row, columnIndexes["Uannumber"]]?.Text.Trim(),
                                        EsiNumber = worksheet.Cells[row, columnIndexes["EsiNumber"]]?.Text.Trim(),
                                        IsMobileAppEligible = (bool)(bool.TryParse(worksheet.Cells[row, columnIndexes["IsMobileAppEligible"]]?.Text, out var isMobileAppEligible) ? isMobileAppEligible : false),
                                        GeoStatus = worksheet.Cells[row, columnIndexes["GeoStatus"]]?.Text.Trim() ?? string.Empty,
                                        MiddleName = worksheet.Cells[row, columnIndexes["MiddleName"]].Text.Trim(),
                                        OfficialPhone = long.TryParse(worksheet.Cells[row, columnIndexes["OfficialPhone"]]?.Text, out var officialPhone) ? officialPhone : 0,
                                        PersonalLocation = worksheet.Cells[row, columnIndexes["PersonalLocation"]]?.Text.Trim() ?? string.Empty,
                                        PersonalEmail = worksheet.Cells[row, columnIndexes["PersonalEmail"]].Text.Trim(),
                                        LeaveGroupId = leaveGroupId,
                                        CompanyMasterId = companyMasterId,
                                        LocationMasterId = locationMasterId,
                                        HolidayCalendarId = holidayCalendarId,
                                        StatusId = statusId,
                                        AadharNo = long.TryParse(worksheet.Cells[row, columnIndexes["AadharNo"]].Text, out var aadharNo) ? aadharNo : 0,
                                        PanNo = worksheet.Cells[row, columnIndexes["PanNo"]]?.Text.Trim(),
                                        PassportNo = worksheet.Cells[row, columnIndexes["PassportNo"]]?.Text.Trim(),
                                        DrivingLicense = worksheet.Cells[row, columnIndexes["DrivingLicense"]]?.Text.Trim(),
                                        BankName = worksheet.Cells[row, columnIndexes["BankName"]]?.Text.Trim(),
                                        BankAccountNo = long.TryParse(worksheet.Cells[row, columnIndexes["BankAccountNo"]]?.Text, out var bankAccountNo) ? bankAccountNo : 0,
                                        BankIfscCode = worksheet.Cells[row, columnIndexes["BankIfscCode"]]?.Text.Trim(),
                                        BankBranch = worksheet.Cells[row, columnIndexes["BankBranch"]]?.Text.Trim(),
                                        HomeAddress = worksheet.Cells[row, columnIndexes["HomeAddress"]]?.Text.Trim() ?? string.Empty,
                                        FatherName = worksheet.Cells[row, columnIndexes["FatherName"]]?.Text.Trim() ?? string.Empty,
                                        EmergencyContactPerson1 = worksheet.Cells[row, columnIndexes["EmergencyContactPerson1"]]?.Text.Trim() ?? string.Empty,
                                        EmergencyContactPerson2 = worksheet.Cells[row, columnIndexes["EmergencyContactPerson2"]]?.Text.Trim() ?? string.Empty,
                                        EmergencyContactNo1 = long.TryParse(worksheet.Cells[row, columnIndexes["EmergencyContactNo1"]].Text, out var emergencyContactNo1) ? emergencyContactNo1 : 0,
                                        EmergencyContactNo2 = long.TryParse(worksheet.Cells[row, columnIndexes["EmergencyContactNo2"]].Text, out var emergencyContactNo2) ? emergencyContactNo2 : 0,
                                        MotherName = worksheet.Cells[row, columnIndexes["MotherName"]]?.Text.Trim() ?? string.Empty,
                                        FatherAadharNo = long.TryParse(worksheet.Cells[row, columnIndexes["FatherAadharNo"]]?.Text, out var fatherAadharNo) ? fatherAadharNo : 0,
                                        MotherAadharNo = long.TryParse(worksheet.Cells[row, columnIndexes["MotherAadharNo"]]?.Text, out var motherAadharNo) ? motherAadharNo : 0,
                                        OrganizationTypeId = organizationTypeId,
                                        WorkingStatus = worksheet.Cells[row, columnIndexes["WorkingStatus"]]?.Text.Trim() ?? string.Empty,
                                        ConfirmationDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["ConfirmationDate"]].Text, out var confirmationDate) ? confirmationDate : (DateOnly?)null,
                                        PostalCode = int.TryParse(worksheet.Cells[row, columnIndexes["PostalCode"]]?.Text, out var postalCode) ? postalCode : 0,
                                        ApprovalLevel = worksheet.Cells[row, columnIndexes["ApprovalLevel"]]?.Text.Trim() ?? string.Empty,
                                        OfficialEmail = worksheet.Cells[row, columnIndexes["OfficialEmail"]]?.Text.Trim() ?? string.Empty,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        IsActive = true,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    staffCreations.Add(staffCreation);
                                }
                                await _context.StaffCreations.AddRangeAsync(staffCreations);
                            }
                            else if (excelImportDto.ExcelImportId == 2)
                            {
                                var individualLeaveCreditDebits = new List<IndividualLeaveCreditDebit>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var leaveTypeName = worksheet.Cells[row, columnIndexes["LeaveTypeName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(leaveTypeName)) throw new MessageNotFoundException($"Leave type {leaveTypeName} not found");
                                    var leaveTypeId = await _context.LeaveTypes
                                        .Where(l => l.Name.ToLower() == leaveTypeName.ToLower() && l.IsActive)
                                        .Select(l => l.Id)
                                        .FirstOrDefaultAsync();
                                    if (leaveTypeId == 0) throw new MessageNotFoundException($"Leave type '{leaveTypeName}' not found");
                                    var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffCreationId"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(staffCreationIdStr)) throw new MessageNotFoundException($"Staff {staffCreationIdStr} not found");
                                    var match = Regex.Match(staffCreationIdStr, @"([A-Za-z]+)(\d+)");
                                    if (!match.Success) throw new Exception($"Invalid StaffCreationId format at row {row}.");
                                    var shortName = match.Groups[1].Value;
                                    var staffId = int.Parse(match.Groups[2].Value);
                                    var organizationId = await _context.OrganizationTypes
                                        .Where(o => o.ShortName.ToLower() == shortName.ToLower() && o.IsActive)
                                        .Select(o => o.Id)
                                        .FirstOrDefaultAsync();
                                    if (organizationId == 0) throw new MessageNotFoundException($"Organization short name '{shortName}' not found at row {row}.");
                                    var staffCreationId = await _context.StaffCreations
                                        .Where(s => (s.OrganizationTypeId == organizationId || s.Id == organizationId) && s.Id == staffId && s.IsActive == true)
                                        .Select(s => s.Id)
                                        .FirstOrDefaultAsync();
                                    var transactionFlagValue = worksheet.Cells[row, columnIndexes["TransactionFlag"]]?.Text.Trim().ToLower();
                                    var transactionFlag = transactionFlagValue == "1" || transactionFlagValue == "true";
                                    var leaveCount = decimal.TryParse(worksheet.Cells[row, columnIndexes["LeaveCount"]]?.Text, out var parsedLeaveCount) ? parsedLeaveCount : 0;
                                    if (leaveCount <= 0) throw new InvalidOperationException($"Invalid leave count at row {row}.");
                                    var actualBalance = await _context.IndividualLeaveCreditDebits
                                        .Where(l => l.StaffCreationId == staffCreationId && l.LeaveTypeId == leaveTypeId && l.IsActive)
                                        .OrderByDescending(l => l.CreatedUtc)
                                        .Select(l => (decimal?)l.ActualBalance ?? 0)
                                        .FirstOrDefaultAsync();
                                    var availableBalance = await _context.IndividualLeaveCreditDebits
                                        .Where(l => l.StaffCreationId == staffCreationId && l.LeaveTypeId == leaveTypeId && l.IsActive)
                                        .OrderByDescending(l => l.CreatedUtc)
                                        .Select(l => (decimal?)l.AvailableBalance ?? 0)
                                        .FirstOrDefaultAsync();
                                    if (transactionFlag)
                                    {
                                        actualBalance += leaveCount;
                                        availableBalance += leaveCount;
                                    }
                                    else
                                    {
                                        if (availableBalance < leaveCount) throw new InvalidOperationException($"Insufficient available balance for staff '{staffCreationId}' at row {row}.");
                                        availableBalance -= leaveCount;
                                    }
                                    var individualLeaveCreditDebit = new IndividualLeaveCreditDebit
                                    {
                                        LeaveTypeId = leaveTypeId,
                                        StaffCreationId = staffCreationId,
                                        LeaveReason = worksheet.Cells[row, columnIndexes["LeaveReason"]]?.Text.Trim() ?? string.Empty,
                                        Remarks = worksheet.Cells[row, columnIndexes["Remarks"]]?.Text.Trim(),
                                        TransactionFlag = transactionFlag,
                                        LeaveCount = leaveCount,
                                        Month = worksheet.Cells[row, columnIndexes["Month"]]?.Text.Trim() ?? string.Empty,
                                        Year = int.TryParse(worksheet.Cells[row, columnIndexes["Year"]]?.Text, out var parsedYear) ? parsedYear : DateTime.UtcNow.Year,
                                        ActualBalance = actualBalance,
                                        AvailableBalance = availableBalance,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    individualLeaveCreditDebits.Add(individualLeaveCreditDebit);
                                }
                                await _context.IndividualLeaveCreditDebits.AddRangeAsync(individualLeaveCreditDebits);
                            }
                            else if (excelImportDto.ExcelImportId == 3)
                            {
                                var departmentMasters = new List<DepartmentMaster>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var departmentMaster = new DepartmentMaster
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["FullName"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        Phone = long.TryParse(worksheet.Cells[row, columnIndexes["Phone"]].Text, out var phone) ? phone : 0,
                                        Fax = worksheet.Cells[row, columnIndexes["Fax"]].Text.Trim(),
                                        Email = worksheet.Cells[row, columnIndexes["Email"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    departmentMasters.Add(departmentMaster);
                                }
                                await _context.DepartmentMasters.AddRangeAsync(departmentMasters);
                            }
                            else if (excelImportDto.ExcelImportId == 4)
                            {
                                var designationMasters = new List<DesignationMaster>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var designationMaster = new DesignationMaster
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["FullName"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    designationMasters.Add(designationMaster);
                                }
                                await _context.DesignationMasters.AddRangeAsync(designationMasters);
                            }
                            else if (excelImportDto.ExcelImportId == 5)
                            {
                                var divisionMasters = new List<DivisionMaster>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var divisionMaster = new DivisionMaster
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["FullName"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    divisionMasters.Add(divisionMaster);
                                }
                                await _context.DivisionMasters.AddRangeAsync(divisionMasters);
                            }
                            else if (excelImportDto.ExcelImportId == 6)
                            {
                                var costCentreMasters = new List<CostCentreMaster>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var costCentreMaster = new CostCentreMaster
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["FullName"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    costCentreMasters.Add(costCentreMaster);
                                }
                                await _context.CostCentreMasters.AddRangeAsync(costCentreMasters);
                            }
                            else if (excelImportDto.ExcelImportId == 7)
                            {
                                var volumes = new List<Volume>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var volume = new Volume
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    volumes.Add(volume);
                                }
                                await _context.Volumes.AddRangeAsync(volumes);
                            }
                            else if (excelImportDto.ExcelImportId == 8)
                            {
                                var manualPunchRequisitions = new List<ManualPunchRequistion>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text?.Trim();
                                    if (string.IsNullOrEmpty(applicationTypeName))
                                    {
                                        throw new Exception($"ApplicationTypeName is required at row {row}.");
                                    }
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name.ToLower() == applicationTypeName.ToLower() && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"ApplicationType '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text?.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            string organizationCode = match.Groups[1].Value;
                                            int parsedStaffId = int.Parse(match.Groups[2].Value);
                                            var organizationType = await _context.OrganizationTypes.FirstOrDefaultAsync(o => o.ShortName == organizationCode && o.IsActive);
                                            if (organizationType == null)
                                            {
                                                throw new MessageNotFoundException($"Organization code '{organizationCode}' not found");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff ID '{parsedStaffId}' not found");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    DateTime? inPunch = DateTime.TryParse(worksheet.Cells[row, columnIndexes["InPunch"]]?.Text, out var parsedInPunch) ? parsedInPunch : null;
                                    DateTime? outPunch = DateTime.TryParse(worksheet.Cells[row, columnIndexes["OutPunch"]]?.Text, out var parsedOutPunch) ? parsedOutPunch : null;
                                    if (!inPunch.HasValue || !outPunch.HasValue)
                                    {
                                        throw new Exception($"Invalid InPunch or OutPunch format at row {row}.");
                                    }
                                    var manualPunchRequisition = new ManualPunchRequistion
                                    {
                                        StaffId = staffId,
                                        SelectPunch = worksheet.Cells[row, columnIndexes["SelectPunch"]]?.Text?.Trim() ?? string.Empty,
                                        InPunch = inPunch.Value,
                                        OutPunch = outPunch.Value,
                                        Remarks = worksheet.Cells[row, columnIndexes["Remarks"]]?.Text?.Trim() ?? string.Empty,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow,
                                        ApplicationTypeId = applicationType.Id
                                    };
                                    manualPunchRequisitions.Add(manualPunchRequisition);
                                }
                                if (manualPunchRequisitions.Count > 0)
                                {
                                    await _context.ManualPunchRequistions.AddRangeAsync(manualPunchRequisitions);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("No valid manual punch requisitions found in the Excel file.");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 9)
                            {
                                var commonPermissions = new List<CommonPermission>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text.Trim();
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"ApplicationTypeName '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            string organizationCode = match.Groups[1].Value;
                                            int parsedStaffId = int.Parse(match.Groups[2].Value);
                                            var organizationType = await _context.OrganizationTypes.FirstOrDefaultAsync(o => o.ShortName == organizationCode && o.IsActive);
                                            if (organizationType == null)
                                            {
                                                throw new MessageNotFoundException($"Organization code '{organizationCode}' not found");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff ID '{parsedStaffId}' not found");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    var startTimeText = worksheet.Cells[row, columnIndexes["StartTime"]].Text.Trim();
                                    var endTimeText = worksheet.Cells[row, columnIndexes["EndTime"]].Text.Trim();
                                    var permissionDateText = worksheet.Cells[row, columnIndexes["PermissionDate"]].Text.Trim();
                                    var startTime = TimeOnly.Parse(startTimeText);
                                    var endTime = TimeOnly.Parse(endTimeText);
                                    var permissionDate = DateOnly.Parse(permissionDateText);
                                    var startOfMonth = new DateOnly(permissionDate.Year, permissionDate.Month, 1);
                                    var endOfMonth = new DateOnly(permissionDate.Year, permissionDate.Month, DateTime.DaysInMonth(permissionDate.Year, permissionDate.Month));
                                    var monthName = permissionDate.ToString("MMMM");
                                    var existingPermissionOnDate = await _context.CommonPermissions.AnyAsync(p => p.StaffId == staffId && p.PermissionDate == permissionDate);
                                    if (existingPermissionOnDate)
                                    {
                                        throw new InvalidOperationException($"Permission for the date {permissionDate:yyyy-MM-dd} already exists.");
                                    }
                                    var permissionsThisMonth = await _context.CommonPermissions
                                        .Where(p => p.StaffId == staffId && p.PermissionDate >= startOfMonth && p.PermissionDate <= endOfMonth)
                                        .ToListAsync();
                                    var totalMinutesUsed = permissionsThisMonth.Sum(p => TimeSpan.Parse(p.TotalHours).TotalMinutes);
                                    var newRequestMinutes = (endTime - startTime).TotalMinutes;
                                    if (newRequestMinutes <= 0)
                                    {
                                        throw new Exception("End time must be greater than start time.");
                                    }
                                    if (newRequestMinutes > 120)
                                    {
                                        throw new Exception("Permission duration cannot exceed 2 hour per request.");
                                    }
                                    if (totalMinutesUsed + newRequestMinutes > 120)
                                    {
                                        throw new Exception($"Cumulative permission time for {monthName} cannot exceed 2 hours.");
                                    }
                                    var formattedDuration = $"{(int)newRequestMinutes / 60:D2}:{(int)newRequestMinutes % 60:D2}";
                                    var totalHours = (endTime - startTime).ToString("hh\\:mm");
                                    var commonPermission = new CommonPermission
                                    {
                                        PermissionType = worksheet.Cells[row, columnIndexes["PermissionType"]].Text.Trim(),
                                        StartTime = startTime,
                                        EndTime = endTime,
                                        TotalHours = totalHours,
                                        StaffId = staffId,
                                        PermissionDate = permissionDate,
                                        ApplicationTypeId = applicationType.Id,
                                        Remarks = worksheet.Cells[row, columnIndexes["Remarks"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    commonPermissions.Add(commonPermission);
                                }
                                await _context.CommonPermissions.AddRangeAsync(commonPermissions);
                            }
                            else if (excelImportDto.ExcelImportId == 10)
                            {
                                var staffCreations = new List<StaffCreation>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var staffText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(staffText))
                                    {
                                        continue;
                                    }
                                    if (string.IsNullOrEmpty(staffText) || !int.TryParse(Regex.Match(staffText, @"\d+").Value, out int staffId))
                                    {
                                        continue;
                                    }
                                    var existingStaff = _context.StaffCreations.FirstOrDefault(s => s.Id == staffId && s.IsActive == true);
                                    if (existingStaff == null)
                                    {
                                        continue;
                                    }
                                    var statusName = worksheet.Cells[row, columnIndexes["Status"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(statusName))
                                    {
                                        continue;
                                    }
                                    var statusId = await _context.Statuses
                                        .Where(d => d.Name.Trim().ToLower() == statusName.Trim().ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();
                                    if (statusId == 0)
                                    {
                                        continue;
                                    }
                                    var resignationDateText = worksheet.Cells[row, columnIndexes["ResignationDate"]]?.Text;
                                    var relievingDateText = worksheet.Cells[row, columnIndexes["RelievingDate"]]?.Text;
                                    DateOnly? resignationDate = DateOnly.TryParse(resignationDateText, out var parsedResignationDate) ? parsedResignationDate : (DateOnly?)null;
                                    DateOnly? relievingDate = DateOnly.TryParse(relievingDateText, out var parsedRelievingDate) ? parsedRelievingDate : (DateOnly?)null;
                                    existingStaff.ResignationDate = resignationDate;
                                    existingStaff.RelievingDate = relievingDate;
                                    existingStaff.StatusId = statusId;
                                    existingStaff.UpdatedBy = excelImportDto.CreatedBy;
                                    existingStaff.IsActive = false;
                                    existingStaff.UpdatedUtc = DateTime.UtcNow;
                                    staffCreations.Add(existingStaff);
                                }
                                if (staffCreations.Any())
                                {
                                    _context.StaffCreations.UpdateRange(staffCreations);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("No valid staff records found for update");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 11)
                            {
                                var shiftMasters = new List<Shift>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var startTimeCell = worksheet.Cells[row, columnIndexes["StartTime"]];
                                    var endTimeCell = worksheet.Cells[row, columnIndexes["EndTime"]];
                                    var shortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim();
                                    var startTime = ConvertExcelDateTime(startTimeCell)?.ToString("HH:mm")
                                    ?? throw new Exception($"Unable to parse StartTime. Raw value: '{startTimeCell.Text}'");
                                    var endTime = ConvertExcelDateTime(endTimeCell)?.ToString("HH:mm")
                                        ?? throw new Exception($"Unable to parse EndTime. Raw value: '{endTimeCell.Text}'");
                                    var shiftMaster = new Shift
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["ShiftName"]].Text.Trim(),
                                        StartTime = startTime,
                                        ShortName = shortName,
                                        EndTime = endTime,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    shiftMasters.Add(shiftMaster);
                                }
                                await _context.Shifts.AddRangeAsync(shiftMasters);
                            }
                            else if (excelImportDto.ExcelImportId == 12)
                            {
                                var leaveRequisitions = new List<LeaveRequisition>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text.Trim();
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"ApplicationTypeName '{applicationTypeName}' not found");
                                    }
                                    var leaveTypeName = worksheet.Cells[row, columnIndexes["LeaveTypeName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(leaveTypeName)) throw new Exception($"Leave type name is required at row {row}.");
                                    var leaveTypeId = await _context.LeaveTypes
                                        .Where(l => l.Name.ToLower() == leaveTypeName.ToLower() && l.IsActive)
                                        .Select(l => l.Id)
                                        .FirstOrDefaultAsync();
                                    if (leaveTypeId == 0) throw new MessageNotFoundException($"Leave type '{leaveTypeName}' not found");
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            string organizationCode = match.Groups[1].Value;
                                            int parsedStaffId = int.Parse(match.Groups[2].Value);
                                            var organizationType = await _context.OrganizationTypes.FirstOrDefaultAsync(o => o.ShortName == organizationCode && o.IsActive);
                                            if (organizationType == null)
                                            {
                                                throw new MessageNotFoundException($"Organization code '{organizationCode}' not found");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff ID '{parsedStaffId}' not found");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException($"Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    bool isFromDateValid = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["FromDate"]]?.Text, out var fromDate);
                                    bool isToDateValid = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["ToDate"]]?.Text, out var toDate);
                                    if (!isFromDateValid)
                                    {
                                        throw new Exception($"Invalid FromDate at row {row}.");
                                    }
                                    toDate = isToDateValid ? toDate : fromDate;
                                    decimal totalDays = (toDate.ToDateTime(TimeOnly.MinValue) - fromDate.ToDateTime(TimeOnly.MinValue)).Days + 1;
                                    var leaveRequisition = new LeaveRequisition
                                    {
                                        ApplicationTypeId = applicationType.Id,
                                        StartDuration = worksheet.Cells[row, columnIndexes["StartDuration"]]?.Text.Trim() ?? string.Empty,
                                        EndDuration = worksheet.Cells[row, columnIndexes["EndDuration"]]?.Text.Trim(),
                                        Reason = worksheet.Cells[row, columnIndexes["Reason"]]?.Text.Trim() ?? string.Empty,
                                        LeaveTypeId = leaveTypeId,
                                        FromDate = fromDate,
                                        ToDate = toDate,
                                        TotalDays = totalDays,
                                        StaffId = staffId,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    leaveRequisitions.Add(leaveRequisition);
                                }
                                if (leaveRequisitions.Count > 0)
                                {
                                    await _context.LeaveRequisitions.AddRangeAsync(leaveRequisitions);
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 13)
                            {
                                var onDutyRequisitions = new List<OnDutyRequisition>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text.Trim();
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"ApplicationTypeName '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            string organizationCode = match.Groups[1].Value;
                                            int parsedStaffId = int.Parse(match.Groups[2].Value);
                                            var organizationType = await _context.OrganizationTypes
                                                .FirstOrDefaultAsync(o => o.ShortName == organizationCode);
                                            if (organizationType == null)
                                            {
                                                throw new MessageNotFoundException($"Organization code '{organizationCode}' not found");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff ID '{parsedStaffId}' not found");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException($"Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    DateOnly? startDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["StartDate"]]?.Text, out var parsedStartDate) ? parsedStartDate : null;
                                    DateOnly? endDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["EndDate"]]?.Text, out var parsedEndDate) ? parsedEndDate : null;
                                    DateTime? startTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["StartTime"]]?.Text, out var parsedStartTime) ? parsedStartTime : null;
                                    DateTime? endTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["EndTime"]]?.Text, out var parsedEndTime) ? parsedEndTime : null;
                                    decimal? totalDays = null;
                                    if (startDate.HasValue && endDate.HasValue)
                                    {
                                        totalDays = (endDate.Value.DayNumber - startDate.Value.DayNumber) + 1;
                                    }
                                    string? totalHours = null;
                                    if (startTime.HasValue && endTime.HasValue)
                                    {
                                        TimeSpan timeDifference = endTime.Value - startTime.Value;
                                        totalHours = $"{(int)timeDifference.TotalHours}h {timeDifference.Minutes}m";
                                    }
                                    var onDutyRequisition = new OnDutyRequisition
                                    {
                                        ApplicationTypeId = applicationType.Id,
                                        StartTime = startTime,
                                        EndTime = endTime,
                                        StartDate = startDate,
                                        EndDate = endDate,
                                        StartDuration = worksheet.Cells[row, columnIndexes["StartDuration"]]?.Text.Trim() ?? string.Empty,
                                        EndDuration = worksheet.Cells[row, columnIndexes["EndDuration"]]?.Text.Trim(),
                                        Reason = worksheet.Cells[row, columnIndexes["Reason"]]?.Text.Trim() ?? string.Empty,
                                        TotalDays = totalDays,
                                        TotalHours = totalHours,
                                        StaffId = staffId,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    onDutyRequisitions.Add(onDutyRequisition);
                                }
                                if (onDutyRequisitions.Count > 0)
                                {
                                    await _context.OnDutyRequisitions.AddRangeAsync(onDutyRequisitions);
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 14)
                            {
                                var workFromHomes = new List<WorkFromHome>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text.Trim();
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"ApplicationTypeName '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            string organizationCode = match.Groups[1].Value;
                                            int parsedStaffId = int.Parse(match.Groups[2].Value);
                                            var organizationType = await _context.OrganizationTypes.FirstOrDefaultAsync(o => o.ShortName == organizationCode && o.IsActive);
                                            if (organizationType == null)
                                            {
                                                throw new MessageNotFoundException($"Organization code '{organizationCode}' not found");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff ID '{parsedStaffId}' not found");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException($"Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    DateTime? fromTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["FromTime"]]?.Text, out var parsedFromTime) ? parsedFromTime : null;
                                    DateTime? toTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["ToTime"]]?.Text, out var parsedToTime) ? parsedToTime : null;
                                    DateOnly? fromDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["FromDate"]]?.Text, out var parsedFromDate) ? parsedFromDate : null;
                                    DateOnly? toDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["ToDate"]]?.Text, out var parsedToDate) ? parsedToDate : null;

                                    string? totalHours = null;
                                    if (fromTime.HasValue && toTime.HasValue)
                                    {
                                        TimeSpan timeDifference = toTime.Value - fromTime.Value;
                                        totalHours = $"{(int)timeDifference.TotalHours}h {timeDifference.Minutes}m";
                                    }
                                    decimal? totalDays = null;
                                    if (fromDate.HasValue && toDate.HasValue)
                                    {
                                        totalDays = (toDate.Value.DayNumber - fromDate.Value.DayNumber) + 1;
                                    }
                                    var workFrom = new WorkFromHome
                                    {
                                        ApplicationTypeId = applicationType.Id,
                                        FromTime = fromTime,
                                        ToTime = toTime,
                                        FromDate = fromDate,
                                        ToDate = toDate,
                                        Reason = worksheet.Cells[row, columnIndexes["Reason"]]?.Text.Trim() ?? string.Empty,
                                        TotalDays = totalDays,
                                        TotalHours = totalHours,
                                        StartDuration = worksheet.Cells[row, columnIndexes["StartDuration"]]?.Text.Trim() ?? string.Empty,
                                        EndDuration = worksheet.Cells[row, columnIndexes["EndDuration"]]?.Text.Trim() ?? string.Empty,
                                        StaffId = staffId,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    workFromHomes.Add(workFrom);
                                }
                                if (workFromHomes.Count > 0)
                                {
                                    await _context.WorkFromHomes.AddRangeAsync(workFromHomes);
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 15)
                            {
                                var onDutyOvertimes = new List<OnDutyOvertime>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            string organizationCode = match.Groups[1].Value;
                                            int parsedStaffId = int.Parse(match.Groups[2].Value);
                                            var organizationType = await _context.OrganizationTypes.FirstOrDefaultAsync(o => o.ShortName == organizationCode && o.IsActive);
                                            if (organizationType == null)
                                            {
                                                throw new MessageNotFoundException($"Organization code '{organizationCode}' not found");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff ID '{parsedStaffId}' not found");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException($"Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    DateOnly? otDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["OTDate"]]?.Text, out var parsedOtDate) ? parsedOtDate : null;
                                    if (!otDate.HasValue)
                                    {
                                        throw new Exception($"Invalid or missing OTDate at row {row}.");
                                    }
                                    DateTime? startTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["StartTime"]]?.Text, out var parsedStartTime) ? parsedStartTime : null;
                                    DateTime? endTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["EndTime"]]?.Text, out var parsedEndTime) ? parsedEndTime : null;
                                    string otType = worksheet.Cells[row, columnIndexes["OTType"]]?.Text.Trim() ?? string.Empty;
                                    if (string.IsNullOrEmpty(otType))
                                    {
                                        throw new Exception($"OT Type is required at row {row}.");
                                    }
                                    var onDutyOvertime = new OnDutyOvertime
                                    {
                                        StaffId = staffId ?? throw new Exception($"Staff ID is required at row {row}."),
                                        Otdate = otDate.Value,
                                        StartTime = startTime,
                                        EndTime = endTime,
                                        Ottype = otType,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    onDutyOvertimes.Add(onDutyOvertime);
                                }
                                if (onDutyOvertimes.Count > 0)
                                {
                                    await _context.OnDutyOvertimes.AddRangeAsync(onDutyOvertimes);
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 16)
                            {
                                var shiftExtensions = new List<ShiftExtension>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text?.Trim();
                                    if (string.IsNullOrEmpty(applicationTypeName))
                                    {
                                        throw new Exception($"ApplicationTypeName is required at row {row}.");
                                    }
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name.ToLower() == applicationTypeName.ToLower() && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"ApplicationTypeName '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text?.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            string organizationCode = match.Groups[1].Value;
                                            int parsedStaffId = int.Parse(match.Groups[2].Value);
                                            var organizationType = await _context.OrganizationTypes.FirstOrDefaultAsync(o => o.ShortName == organizationCode && o.IsActive);
                                            if (organizationType == null)
                                            {
                                                throw new MessageNotFoundException($"Organization code '{organizationCode}' not found");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff ID '{parsedStaffId}' not found");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException($"Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }

                                    DateOnly? transactionDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["TransactionDate"]]?.Text, out var parsedDate) ? parsedDate : null;
                                    if (!transactionDate.HasValue)
                                    {
                                        throw new InvalidOperationException($"Invalid or missing TransactionDate at row {row}.");
                                    }
                                    DateTime? beforeShiftHours = DateTime.TryParse(worksheet.Cells[row, columnIndexes["BeforeShiftHours"]]?.Text, out var parsedBeforeShift) ? parsedBeforeShift : null;
                                    DateTime? afterShiftHours = DateTime.TryParse(worksheet.Cells[row, columnIndexes["AfterShiftHours"]]?.Text, out var parsedAfterShift) ? parsedAfterShift : null;

                                    var shiftExtension = new ShiftExtension
                                    {
                                        ApplicationTypeId = applicationType.Id,
                                        TransactionDate = transactionDate.Value,
                                        DurationHours = worksheet.Cells[row, columnIndexes["DurationHours"]]?.Text?.Trim(),
                                        BeforeShiftHours = beforeShiftHours,
                                        AfterShiftHours = afterShiftHours,
                                        Remarks = worksheet.Cells[row, columnIndexes["Remarks"]]?.Text?.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow,
                                        StaffId = staffId
                                    };
                                    shiftExtensions.Add(shiftExtension);
                                }
                                if (shiftExtensions.Count > 0)
                                {
                                    await _context.ShiftExtensions.AddRangeAsync(shiftExtensions);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("No valid shift extensions found in the Excel file.");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 17)
                            {
                                var staffVaccinations = new List<StaffVaccination>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text?.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            string organizationCode = match.Groups[1].Value;
                                            int parsedStaffId = int.Parse(match.Groups[2].Value);
                                            var organizationType = await _context.OrganizationTypes.FirstOrDefaultAsync(o => o.ShortName == organizationCode && o.IsActive);
                                            if (organizationType == null)
                                            {
                                                throw new MessageNotFoundException($"Organization code '{organizationCode}' not found");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff ID '{parsedStaffId}' not found");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    DateOnly? vaccinatedDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["VaccinatedDate"]]?.Text, out var parsedVaccinatedDate) ? parsedVaccinatedDate : null;
                                    if (!vaccinatedDate.HasValue)
                                    {
                                        throw new Exception($"Invalid or missing VaccinatedDate at row {row}.");
                                    }
                                    if (!int.TryParse(worksheet.Cells[row, columnIndexes["VaccinationNumber"]]?.Text, out int vaccinationNumber))
                                    {
                                        throw new Exception($"Invalid or missing VaccinationNumber at row {row}.");
                                    }
                                    bool isExempted = false;
                                    var isExemptedText = worksheet.Cells[row, columnIndexes["IsExempted"]]?.Text?.Trim().ToLower();
                                    if (isExemptedText == "yes")
                                    {
                                        isExempted = true;
                                    }
                                    else if (isExemptedText == "no")
                                    {
                                        isExempted = false;
                                    }
                                    else
                                    {
                                        throw new Exception($"Invalid IsExempted value '{isExemptedText}' at row {row}. Expected 'Yes' or 'No'.");
                                    }
                                    string? comments = worksheet.Cells[row, columnIndexes["Comments"]]?.Text?.Trim();
                                    var staffVaccination = new StaffVaccination
                                    {
                                        StaffId = staffId ?? throw new Exception($"Staff ID is required at row {row}."),
                                        VaccinatedDate = vaccinatedDate,
                                        VaccinationNumber = vaccinationNumber,
                                        IsExempted = isExempted,
                                        Comments = comments,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow,
                                    };
                                    staffVaccinations.Add(staffVaccination);
                                }
                                if (staffVaccinations.Count > 0)
                                {
                                    await _context.StaffVaccinations.AddRangeAsync(staffVaccinations);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("No valid staff vaccinations found in the Excel file.");
                                }
                            }
                            else if(excelImportDto.ExcelImportId == 18)
                            {
                                var probations = new List<ProbationReport>();
                                var validDepartmentIds = _context.DepartmentMasters.Where(d => d.IsActive).Select(d => d.Id).ToHashSet();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var departmentName = worksheet.Cells[row, columnIndexes["Department"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(departmentName))
                                    {
                                        continue;
                                    }
                                    var departmentId = await _context.DepartmentMasters
                                        .Where(d => d.Name.ToLower() == departmentName.ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();
                                    if (departmentId == 0) throw new MessageNotFoundException($"Department '{departmentName}' not found.");
                                    var addProbation = new ProbationReport
                                    {
                                        EmpId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim(),
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
                                        DepartmentId = departmentId,
                                        ProdScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Prod Score"]]?.Text.Trim(), out decimal prodeScore) ? prodeScore : 0m,
                                        ProdPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Prod %"]]?.Text.Trim(), out decimal prodPercentage) ? prodPercentage : 0m,
                                        ProdGrade = worksheet.Cells[row, columnIndexes["Prod Grade"]].Text.Trim(),
                                        QualityScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Quality Score"]]?.Text.Trim(), out decimal qualityScore) ? qualityScore : 0m,
                                        QualityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Qual %"]]?.Text.Trim(), out decimal qualityPercentage) ? qualityPercentage : 0m,
                                        NoOfAbsent = decimal.TryParse(worksheet.Cells[row, columnIndexes["No Of Absent"]]?.Text.Trim(), out decimal noOfAbsent) ? noOfAbsent : 0m,
                                        AttendanceScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Attd Score"]]?.Text.Trim(), out decimal attendanceScore) ? attendanceScore : 0m,
                                        AttendancePercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Attd %"]]?.Text.Trim(), out decimal attendancePercentage) ? attendancePercentage : 0m,
                                        AttendanceGrade = worksheet.Cells[row, columnIndexes["Attd Grade"]].Text.Trim(),
                                        FinalTotal = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final Total"]]?.Text.Trim(), out decimal finalTotal) ? finalTotal : 0m,
                                        TotalScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Total Score"]]?.Text.Trim(), out decimal totalScore) ? totalScore : 0m,
                                        FinalScorePercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final Score %"]]?.Text.Trim(), out decimal finalScorePercentage) ? finalScorePercentage : 0m,
                                        FinalGrade = worksheet.Cells[row, columnIndexes["Final Grade"]].Text.Trim(),
                                        ProductionAchievedPercentageJan = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Jan"]]?.Text.Trim(), out decimal jan) ? jan : (decimal?)null,
                                        ProductionAchievedPercentageFeb = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Feb"]]?.Text.Trim(), out decimal feb) ? feb : (decimal?)null,
                                        ProductionAchievedPercentageMar = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Mar"]]?.Text.Trim(), out decimal mar) ? mar : (decimal?)null,
                                        ProductionAchievedPercentageApr = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Apr"]]?.Text.Trim(), out decimal apr) ? apr : (decimal?)null,
                                        ProductionAchievedPercentageMay = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % May"]]?.Text.Trim(), out decimal may) ? may : (decimal?)null,
                                        ProductionAchievedPercentageJun = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Jun"]]?.Text.Trim(), out decimal jun) ? jun : (decimal?)null,
                                        ProductionAchievedPercentageJul = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Jul"]]?.Text.Trim(), out decimal jul) ? jul : (decimal?)null,
                                        ProductionAchievedPercentageAug = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Aug"]]?.Text.Trim(), out decimal aug) ? aug : (decimal?)null,
                                        ProductionAchievedPercentageSep = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Sep"]]?.Text.Trim(), out decimal sep) ? sep : (decimal?)null,
                                        ProductionAchievedPercentageOct = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Oct"]]?.Text.Trim(), out decimal oct) ? oct : (decimal?)null,
                                        ProductionAchievedPercentageNov = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Nov"]]?.Text.Trim(), out decimal nov) ? nov : (decimal?)null,
                                        ProductionAchievedPercentageDec = decimal.TryParse(worksheet.Cells[row, columnIndexes["Production Achieved % Dec"]]?.Text.Trim(), out decimal dec) ? dec : (decimal?)null,
                                        ProductivityYear = (int)excelImportDto.ProductivityYear,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    probations.Add(addProbation);
                                }
                                if (probations.Count > 0)
                                {
                                    await _context.ProbationReports.AddRangeAsync(probations);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("No valid probation report found in the Excel file.");
                                }
                            }
                            else if(excelImportDto.ExcelImportId == 19)
                            {
                                var probations = new List<ProbationTarget>();
                                var validDivisionIds = _context.DivisionMasters.Where(d => d.IsActive).Select(d => d.Id).ToHashSet();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var divisionName = worksheet.Cells[row, columnIndexes["EMP Division"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(divisionName))
                                    {
                                        continue;
                                    }
                                    var divisionId = await _context.DivisionMasters
                                        .Where(d => d.Name.ToLower() == divisionName.ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();
                                    if (divisionId == 0) throw new MessageNotFoundException($"Division '{divisionName}' not found.");
                                    var addProbation = new ProbationTarget
                                    {
                                        EmpId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim(),
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
                                        DivisionId = divisionId,
                                        Jan = decimal.TryParse(worksheet.Cells[row, columnIndexes["Jan"]]?.Text.Trim(), out decimal jan) ? jan : (decimal?)null,
                                        Feb = decimal.TryParse(worksheet.Cells[row, columnIndexes["Feb"]]?.Text.Trim(), out decimal feb) ? feb : (decimal?)null,
                                        Mar = decimal.TryParse(worksheet.Cells[row, columnIndexes["Mar"]]?.Text.Trim(), out decimal mar) ? mar : (decimal?)null,
                                        Apr = decimal.TryParse(worksheet.Cells[row, columnIndexes["Apr"]]?.Text.Trim(), out decimal apr) ? apr : (decimal?)null,
                                        May = decimal.TryParse(worksheet.Cells[row, columnIndexes["May"]]?.Text.Trim(), out decimal may) ? may : (decimal?)null,
                                        Jun = decimal.TryParse(worksheet.Cells[row, columnIndexes["Jun"]]?.Text.Trim(), out decimal jun) ? jun : (decimal?)null,
                                        Jul = decimal.TryParse(worksheet.Cells[row, columnIndexes["Jul"]]?.Text.Trim(), out decimal jul) ? jul : (decimal?)null,
                                        Aug = decimal.TryParse(worksheet.Cells[row, columnIndexes["Aug"]]?.Text.Trim(), out decimal aug) ? aug : (decimal?)null,
                                        Sep = decimal.TryParse(worksheet.Cells[row, columnIndexes["Sep"]]?.Text.Trim(), out decimal sep) ? sep : (decimal?)null,
                                        Oct = decimal.TryParse(worksheet.Cells[row, columnIndexes["Oct"]]?.Text.Trim(), out decimal oct) ? oct : (decimal?)null,
                                        Nov = decimal.TryParse(worksheet.Cells[row, columnIndexes["Nov"]]?.Text.Trim(), out decimal nov) ? nov : (decimal?)null,
                                        Dec = decimal.TryParse(worksheet.Cells[row, columnIndexes["Dec"]]?.Text.Trim(), out decimal dec) ? dec : (decimal?)null,
                                        ProductivityYear = (int)excelImportDto.ProductivityYear,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    probations.Add(addProbation);
                                }
                                if (probations.Count > 0)
                                {
                                    await _context.ProbationTargets.AddRangeAsync(probations);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("No valid probation report found in the Excel file.");
                                }
                            }
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw new Exception($"Error processing Excel file: {ex.Message}");
                        }
                    }
                }
            }
            return "Excel data imported successfully.";
        }
        catch (Exception ex)
        {
            throw new Exception($"Error processing the Excel file: {ex.Message}");
        }
    }

    DateTime? ConvertExcelDateTime(ExcelRangeBase cell)
    {
        if (double.TryParse(cell.Text, out var numericValue))
        {
            return DateTime.FromOADate(numericValue);
        }
        else if (DateTime.TryParse(cell.Text, out var parsedDateTime))
        {
            return parsedDateTime;
        }
        else if (DateTime.TryParseExact(
            cell.Text,
            new[] { "MM-dd-yy H:mm", "dd-MM-yyyy HH:mm:ss", "yyyy-MM-dd H:mm:ss", "dd/MM/yyyy H:mm:ss" },
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out var customParsedDateTime))
        {
            return customParsedDateTime;
        }
        return null;
    }
}