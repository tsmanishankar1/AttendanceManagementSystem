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

    public ExcelImportService(AttendanceManagementSystemContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<string> GenerateExcelTemplateUrl(int excelImportId)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new Exception("HttpContext is null.");
        }

        var excelTemplate = await _context.ExcelImports
            .FirstOrDefaultAsync(x => x.Id == excelImportId && x.IsActive == true);

        if (excelTemplate == null)
        {
            throw new MessageNotFoundException("Excel template not found");
        }

        string fileName = $"{excelTemplate.Name}.xlsx";
        var workspacePath = _configuration.GetSection("Excel").GetValue<string>("ExcelTemplatesPath");

        if (string.IsNullOrEmpty(workspacePath))
        {
            throw new Exception("Workspace path is not configured.");
        }

        string filePath = Path.Combine(workspacePath, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            throw new MessageNotFoundException("Excel template not found in workspace");
        }

        string baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        string fileUrl = $"{baseUrl}/ExcelTemplates/{fileName}";

        return fileUrl;
    }

    public async Task<string> ImportExcelAsync(int excelImportId, int createdBy, IFormFile file)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        try
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        throw new Exception("Worksheet not found in the uploaded file.");
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns]
                                    .Select(cell => cell.Text.Trim()).ToList();
                    var requiredHeaders = new List<string>();
                    if (excelImportId == 1)
                    {
                        requiredHeaders = new List<string>
                        {
                            "CardCode", "Title", "FirstName", "LastName", "ShortName", "Gender", "Hide", "BloodGroup", "MaritalStatus", "Dob", "MarriageDate",
                            "PersonalPhone", "JoiningDate", "Confirmed", "Branch", "Department", "Division", "Volume", "Designation", "Grade", "Category",
                            "CostCenter", "WorkStation", "City", "District", "State","Country", "OtEligible", "ApprovalLevel1", "ApprovalLevel2", "AccessLevel",
                            "PolicyGroup", "WorkingDayPattern", "Tenure", "UanNumber", "EsiNumber", "IsMobileAppEligible", "GeoStatus", "MiddleName",
                            "OfficialPhone", "PersonalLocation", "PersonalEmail", "LeaveGroup", "Company", "Location", "HolidayCalendar", "Status",
                            "AadharNo", "PanNo", "PassportNo", "DrivingLicense", "BankName", "BankAccountNo", "BankIfscCode", "BankBranch", "Qualification",
                            "HomeAddress", "FatherName", "EmergencyContactPerson1", "EmergencyContactPerson2", "EmergencyContactNo1", "EmergencyContactNo2", "MotherName",
                            "FatherAadharNo", "MotherAadharNo", "OrganizationType", "WorkingStatus", "ConfirmationDate", "PostalCode", "ApprovalLevel", "OfficialEmail"
                        };
                    }
                    else if (excelImportId == 2)
                    {
                        requiredHeaders = new List<string> {"LeaveTypeName", "StaffCreationId", "TransactionFlag", "Month", "Year", "Remarks", "LeaveCount", "LeaveReason"};
                    }
                    else if (excelImportId == 3)
                    {
                        requiredHeaders = new List<string> {"FullName", "ShortName", "Phone", "Fax", "Email" };
                    }
                    else if (excelImportId == 4 || excelImportId == 5 || excelImportId == 6)
                    {
                        requiredHeaders = new List<string> {"FullName", "ShortName" };
                    }
                    else if (excelImportId == 7)
                    {
                        requiredHeaders = new List<string> {"Name", "ShortName" };
                    }
                    else if (excelImportId == 8)
                    {
                        requiredHeaders = new List<string> {"StaffId", "SelectPunch", "InPunch", "OutPunch", "Remarks", "ApplicationTypeName" };
                    }
                    else if (excelImportId == 9)
                    {
                        requiredHeaders = new List<string> {"StartTime", "EndTime", "StaffId", "Remarks", "PermissionDate", "PermissionType", "ApplicationTypeName" };
                    }
                    else if (excelImportId == 10)
                    {
                        requiredHeaders = new List<string> {"StaffId", "ResignationDate", "RelievingDate", "Status" };
                    }
                    else if (excelImportId == 11)
                    {
                        requiredHeaders = new List<string> {"ShiftName", "StartTime", "EndTime", "ShortName" };
                    }
                    else if (excelImportId == 12)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "FromDate", "ToDate", "Reason", "StaffId", "StartDuration", "EndDuration", "LeaveTypeName" };
                    }
                    else if (excelImportId == 13)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "StartTime", "EndTime", "StartDate", "EndDate", "Reason", "StaffId", "StartDuration", "EndDuration" };
                    }
                    else if (excelImportId == 14)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "FromTime", "ToTime", "FromDate", "ToDate", "Reason", "StartDuration", "EndDuration", "StaffId" };
                    }
                    else if (excelImportId == 15)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "StaffId", "OTDate", "StartTime", "EndTime", "OTType" };
                    }
                    else if (excelImportId == 16)
                    {
                        requiredHeaders = new List<string> {"ApplicationTypeName", "StaffId", "TransactionDate", "BeforeShiftHours", "AfterShiftHours", "Remarks", "DurationHours" };
                    }
                    else if (excelImportId == 17)
                    {
                        requiredHeaders = new List<string> {"StaffId", "VaccinatedDate", "VaccinationNumber", "IsExempted", "Comments" };
                    }
                    else
                    {
                        throw new Exception("Invalid ExcelImportId. Only 1 or 2 or 3 or 4 or 5 or 6 or 9 or 10 or 11 or 12 or 13 or 14 or 15 or 16 or 17 are valid.");
                    }
                    var missingHeaders = requiredHeaders.Where(header => !headerRow.Contains(header)).ToList();
                    if (missingHeaders.Any())
                    {
                        throw new Exception($"Invalid Excel file for ExcelImportId {excelImportId}. Missing headers: {string.Join(", ", missingHeaders)}");
                    }
                    var extraHeaders = headerRow.Except(requiredHeaders).ToList();
                    if (extraHeaders.Any())
                    {
                        throw new Exception($"Invalid Excel file for ExcelImportId {excelImportId}. Contains unexpected headers: {string.Join(", ", extraHeaders)}");
                    }
                    var columnIndexes = requiredHeaders.ToDictionary(
                        header => header,
                        header => headerRow.IndexOf(header) + 1
                    );
                    var staffExists = await _context.StaffCreations
                        .AnyAsync(s => s.Id == createdBy);

                    if (!staffExists)
                    {
                        throw new Exception($"StaffId {createdBy} not found in the database.");
                    }
                    var rowCount = worksheet.Dimension.Rows;
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            if (excelImportId == 1)
                            {
                                var staffCreations = new List<StaffCreation>();
                                var validDepartmentIds = _context.DepartmentMasters.Select(d => d.Id).ToHashSet();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var branchName = worksheet.Cells[row, columnIndexes["BranchName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(branchName))
                                    {
                                        Console.WriteLine($"Invalid or missing BranchId at row {row}. Skipping...");
                                        continue;
                                    }
                                    var branchId = _context.BranchMasters
                                        .Where(d => d.FullName.ToLower() == branchName.ToLower())
                                        .Select(d => d.Id)
                                        .FirstOrDefault();
                                    if (branchId == 0)
                                    {
                                        Console.WriteLine($"Branch '{branchName}' not found in the database. Skipping row {row}...");
                                        continue;
                                    }
                                    var approvalLevel1 = worksheet.Cells[row, columnIndexes["ApprovalLevel1"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(approvalLevel1))
                                    {
                                        Console.WriteLine($"Invalid or missing ApprovalLevel1Id at row {row}. Skipping...");
                                        continue;
                                    }
                                    int numericPart1 = int.Parse(Regex.Match(approvalLevel1, @"\d+").Value);
                                    var approvalId1 = (from staff in _context.StaffCreations
                                                       where staff.Id == numericPart1 && staff.IsActive == true
                                                       select staff.Id).FirstOrDefault();

                                    if (approvalId1 == 0)
                                    {
                                        Console.WriteLine($"Approval1 '{approvalLevel1}' not found in the database. Skipping row {row}...");
                                        continue;
                                    }
                                    var approvalLevel2 = worksheet.Cells[row, columnIndexes["ApprovalLevel2"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(approvalLevel2))
                                    {
                                        Console.WriteLine($"Invalid or missing ApprovalLevel1Id at row {row}. Skipping...");
                                        continue;
                                    }
                                    int numericPart = int.Parse(Regex.Match(approvalLevel2, @"\d+").Value);
                                    var approvalId2 = (from staff in _context.StaffCreations
                                                       where staff.Id == numericPart1 && staff.IsActive == true
                                                       select staff.Id).FirstOrDefault();

                                    if (approvalId2 == 0)
                                    {
                                        Console.WriteLine($"Approval2 '{approvalLevel2}' not found in the database. Skipping row {row}...");
                                        continue;
                                    }
                                    var departmentName = worksheet.Cells[row, columnIndexes["DepartmentName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(departmentName))
                                    {
                                        Console.WriteLine($"Invalid Department name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var departmentId = _context.DepartmentMasters
                                        .Where(d => d.FullName.ToLower() == departmentName.ToLower())
                                        .Select(d => d.Id)
                                        .FirstOrDefault();
                                    if (departmentId == 0)
                                        throw new Exception($"Department '{departmentName}' not found in the database.");

                                    var statusName = worksheet.Cells[row, columnIndexes["StatusName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(statusName))
                                    {
                                        Console.WriteLine($"Invalid Status name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var statusId = _context.Statuses
                                        .Where(d => d.Name.ToLower() == statusName.ToLower())
                                        .Select(d => d.Id)
                                        .FirstOrDefault();
                                    if (statusId == 0)
                                        throw new Exception($"Status '{statusName}' not found in the database.");
                                    var divisionName = worksheet.Cells[row, columnIndexes["DivisionName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(divisionName))
                                    {
                                        Console.WriteLine($"Invalid Division name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var divisionId = _context.DivisionMasters
                                        .Where(d => d.FullName.ToLower() == divisionName.ToLower())
                                        .Select(d => d.Id)
                                        .FirstOrDefault();
                                    if (divisionId == 0)
                                        throw new Exception($"Division '{divisionName}' not found in the database.");

                                    var designationName = worksheet.Cells[row, columnIndexes["DesignationName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(designationName))
                                    {
                                        Console.WriteLine($"Invalid Designation name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var designationId = _context.DesignationMasters
                                        .Where(d => d.FullName.ToLower() == designationName.ToLower())
                                        .Select(d => d.Id)
                                        .FirstOrDefault();
                                    if (designationId == 0)
                                        throw new Exception($"Designation '{designationName}' not found in the database.");

                                    var gradeName = worksheet.Cells[row, columnIndexes["GradeName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(gradeName))
                                    {
                                        Console.WriteLine($"Invalid Grade name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var gradeId = _context.GradeMasters
                                        .Where(g => g.FullName.ToLower() == gradeName.ToLower())
                                        .Select(g => g.Id)
                                        .FirstOrDefault();
                                    if (gradeId == 0)
                                        throw new Exception($"Grade '{gradeName}' not found in the database.");

                                    var organizationTypeName = worksheet.Cells[row, columnIndexes["OrganizationType"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(organizationTypeName))
                                    {
                                        Console.WriteLine($"Invalid organization type name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var organizationTypeId = _context.OrganizationTypes
                                        .Where(g => g.ShortName.ToLower() == organizationTypeName.ToLower())
                                        .Select(g => g.Id)
                                        .FirstOrDefault();

                                    if (organizationTypeId == 0)
                                        throw new Exception($"Organization '{organizationTypeName}' not found in the database.");

                                    var categoryName = worksheet.Cells[row, columnIndexes["CategoryName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(categoryName))
                                    {
                                        Console.WriteLine($"Invalid Category name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var categoryId = _context.CategoryMasters
                                        .Where(c => c.FullName.ToLower() == categoryName.ToLower())
                                        .Select(c => c.Id)
                                        .FirstOrDefault();
                                    if (categoryId == 0)
                                        throw new Exception($"Category '{categoryName}' not found in the database.");

                                    var costCenterName = worksheet.Cells[row, columnIndexes["CostCenterName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(costCenterName))
                                    {
                                        Console.WriteLine($"Invalid Cost Center name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var costCenterId = _context.CostCentreMasters
                                        .Where(c => c.FullName.ToLower() == costCenterName.ToLower())
                                        .Select(c => c.Id)
                                        .FirstOrDefault();
                                    if (costCenterId == 0)
                                        throw new Exception($"Cost Center '{costCenterName}' not found in the database.");

                                    var workStationName = worksheet.Cells[row, columnIndexes["WorkStationName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(workStationName))
                                    {
                                        Console.WriteLine($"Invalid Workstation name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var workStationId = _context.WorkstationMasters
                                        .Where(w => w.FullName.ToLower() == workStationName.ToLower())
                                        .Select(w => w.Id)
                                        .FirstOrDefault();
                                    if (workStationId == 0)
                                        throw new Exception($"Workstation '{workStationName}' not found in the database.");

                                    var leaveGroupName = worksheet.Cells[row, columnIndexes["LeaveGroupName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(leaveGroupName))
                                    {
                                        Console.WriteLine($"Invalid Leave Group name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var leaveGroupId = _context.LeaveGroupConfigurations
                                        .Where(l => l.LeaveGroupConfigurationName.ToLower() == leaveGroupName.ToLower())
                                        .Select(l => l.Id)
                                        .FirstOrDefault();
                                    if (leaveGroupId == 0)
                                        throw new Exception($"Leave Group '{leaveGroupName}' not found in the database.");

                                    var companyName = worksheet.Cells[row, columnIndexes["CompanyMasterName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(companyName))
                                    {
                                        Console.WriteLine($"Invalid Company name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var companyMasterId = _context.CompanyMasters
                                        .Where(c => c.FullName.ToLower() == companyName.ToLower())
                                        .Select(c => c.Id)
                                        .FirstOrDefault();
                                    if (companyMasterId == 0)
                                        throw new Exception($"Company '{companyName}' not found in the database.");

                                    var locationName = worksheet.Cells[row, columnIndexes["LocationMasterName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(locationName))
                                    {
                                        Console.WriteLine($"Invalid Location name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var locationMasterId = _context.LocationMasters
                                        .Where(l => l.FullName.ToLower() == locationName.ToLower())
                                        .Select(l => l.Id)
                                        .FirstOrDefault();
                                    if (locationMasterId == 0)
                                        throw new Exception($"Location '{locationName}' not found in the database.");

                                    var holidayCalendarName = worksheet.Cells[row, columnIndexes["HolidayCalendarName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(holidayCalendarName))
                                    {
                                        Console.WriteLine($"Invalid Holiday Calendar name at row {row}. Skipping...");
                                        continue;
                                    }
                                    var holidayCalendarId = _context.HolidayCalendarConfigurations
                                        .Where(h => h.GroupName.ToLower() == holidayCalendarName.ToLower())
                                        .Select(h => h.Id)
                                        .FirstOrDefault();
                                    if (holidayCalendarId == 0)
                                        throw new Exception($"Holiday Calendar '{holidayCalendarName}' not found in the database.");

                                    var staffCreation = new StaffCreation
                                    {
                                        CardCode = worksheet.Cells[row, columnIndexes["CardCode"]].Text.Trim(),
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
                                        Qualification = worksheet.Cells[row, columnIndexes["Qualification"]]?.Text.Trim() ?? string.Empty,
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
                                        CreatedBy = createdBy,
                                        IsActive = true,
                                        CreatedUtc = DateTime.UtcNow
                                    };

                                    staffCreations.Add(staffCreation);
                                }
                                await _context.StaffCreations.AddRangeAsync(staffCreations);
                            }
                            else if (excelImportId == 2)
                            {
                                var individualLeaveCreditDebits = new List<IndividualLeaveCreditDebit>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var leaveTypeName = worksheet.Cells[row, columnIndexes["LeaveTypeName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(leaveTypeName))
                                        throw new Exception($"Leave type name is required at row {row}.");

                                    var leaveTypeId = _context.LeaveTypes
                                        .Where(l => l.Name.ToLower() == leaveTypeName.ToLower())
                                        .Select(l => l.Id)
                                        .FirstOrDefault();

                                    if (leaveTypeId == 0)
                                        throw new Exception($"Leave type '{leaveTypeName}' not found in the database at row {row}.");
                                    var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffCreationId"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(staffCreationIdStr))
                                        throw new Exception($"Invalid or missing StaffCreationId at row {row}.");
                                    var match = Regex.Match(staffCreationIdStr, @"([A-Za-z]+)(\d+)");
                                    if (!match.Success)
                                        throw new Exception($"Invalid StaffCreationId format at row {row}.");
                                    var shortName = match.Groups[1].Value;
                                    var staffId = int.Parse(match.Groups[2].Value);
                                    var organizationId = _context.OrganizationTypes
                                        .Where(o => o.ShortName.ToLower() == shortName.ToLower())
                                        .Select(o => o.Id)
                                        .FirstOrDefault();
                                    if (organizationId == 0)
                                        throw new Exception($"Organization with short name '{shortName}' not found at row {row}.");
                                    var staffCreationId = _context.StaffCreations
                                        .Where(s => (s.OrganizationTypeId == organizationId || s.Id == organizationId) && s.Id == staffId)
                                        .Select(s => s.Id)
                                        .FirstOrDefault();
                                    var transactionFlagValue = worksheet.Cells[row, columnIndexes["TransactionFlag"]]?.Text.Trim().ToLower();
                                    var transactionFlag = transactionFlagValue == "1" || transactionFlagValue == "true";
                                    var leaveCount = decimal.TryParse(worksheet.Cells[row, columnIndexes["LeaveCount"]]?.Text, out var parsedLeaveCount) ? parsedLeaveCount : 0;
                                    if (leaveCount <= 0)
                                        throw new Exception($"Invalid leave count at row {row}.");
                                    var actualBalance = _context.IndividualLeaveCreditDebits
                                        .Where(l => l.StaffCreationId == staffCreationId && l.LeaveTypeId == leaveTypeId)
                                        .OrderByDescending(l => l.CreatedUtc)
                                        .Select(l => (decimal?)l.ActualBalance ?? 0)
                                        .FirstOrDefault();
                                    var availableBalance = _context.IndividualLeaveCreditDebits
                                        .Where(l => l.StaffCreationId == staffCreationId && l.LeaveTypeId == leaveTypeId)
                                        .OrderByDescending(l => l.CreatedUtc)
                                        .Select(l => (decimal?)l.AvailableBalance ?? 0)
                                        .FirstOrDefault();
                                    if (transactionFlag)
                                    {
                                        actualBalance += leaveCount;
                                        availableBalance += leaveCount;
                                    }
                                    else
                                    {
                                        if (availableBalance < leaveCount)
                                            throw new Exception($"Insufficient available balance for StaffCreationId '{staffCreationId}' at row {row}.");

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
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow,
                                        UpdatedUtc = DateTime.UtcNow,
                                        UpdatedBy = createdBy
                                    };

                                    individualLeaveCreditDebits.Add(individualLeaveCreditDebit);
                                }
                                await _context.IndividualLeaveCreditDebits.AddRangeAsync(individualLeaveCreditDebits);
                            }
                            else if (excelImportId == 3)
                            {
                                var departmentMasters = new List<DepartmentMaster>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var departmentMaster = new DepartmentMaster
                                    {
                                        FullName = worksheet.Cells[row, columnIndexes["FullName"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        Phone = long.TryParse(worksheet.Cells[row, columnIndexes["Phone"]].Text, out var phone) ? phone : 0,
                                        Fax = worksheet.Cells[row, columnIndexes["Fax"]].Text.Trim(),
                                        Email = worksheet.Cells[row, columnIndexes["Email"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };

                                    departmentMasters.Add(departmentMaster);
                                }

                                await _context.DepartmentMasters.AddRangeAsync(departmentMasters);
                            }
                            else if (excelImportId == 4)
                            {
                                var designationMasters = new List<DesignationMaster>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var designationMaster = new DesignationMaster
                                    {
                                        FullName = worksheet.Cells[row, columnIndexes["FullName"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };

                                    designationMasters.Add(designationMaster);
                                }

                                await _context.DesignationMasters.AddRangeAsync(designationMasters);
                            }
                            else if (excelImportId == 5)
                            {
                                var divisionMasters = new List<DivisionMaster>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var divisionMaster = new DivisionMaster
                                    {
                                        FullName = worksheet.Cells[row, columnIndexes["FullName"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };

                                    divisionMasters.Add(divisionMaster);
                                }

                                await _context.DivisionMasters.AddRangeAsync(divisionMasters);
                            }
                            else if (excelImportId == 6)
                            {
                                var costCentreMasters = new List<CostCentreMaster>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var costCentreMaster = new CostCentreMaster
                                    {
                                        FullName = worksheet.Cells[row, columnIndexes["FullName"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    costCentreMasters.Add(costCentreMaster);
                                }
                                await _context.CostCentreMasters.AddRangeAsync(costCentreMasters);
                            }
                            else if (excelImportId == 7)
                            {
                                var volumes = new List<Volume>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var volume = new Volume
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    volumes.Add(volume);
                                }
                                await _context.Volumes.AddRangeAsync(volumes);
                            }
                            else if (excelImportId == 8)
                            {
                                var manualPunchRequisitions = new List<ManualPunchRequistion>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text?.Trim();
                                    if (string.IsNullOrEmpty(applicationTypeName))
                                    {
                                        throw new Exception($"ERROR: ApplicationTypeName is required at row {row}.");
                                    }
                                    var applicationType = await _context.ApplicationTypes
                                        .FirstOrDefaultAsync(a => a.ApplicationTypeName.ToLower() == applicationTypeName.ToLower());

                                    if (applicationType == null)
                                    {
                                        throw new Exception($"ERROR: ApplicationType '{applicationTypeName}' not found in the database at row {row}.");
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
                                            var organizationType = await _context.OrganizationTypes
                                                .FirstOrDefaultAsync(o => o.ShortName == organizationCode);
                                            if (organizationType == null)
                                            {
                                                throw new Exception($"ERROR: Organization code '{organizationCode}' not found in OrganizationTypes table at row {row}.");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id);

                                            if (!staffExistsInOrg)
                                            {
                                                throw new Exception($"ERROR: Staff ID '{parsedStaffId}' not found or does not belong to Organization Type '{organizationCode}' at row {row}.");
                                            }

                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"ERROR: Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    DateTime? inPunch = DateTime.TryParse(worksheet.Cells[row, columnIndexes["InPunch"]]?.Text, out var parsedInPunch) ? parsedInPunch : null;
                                    DateTime? outPunch = DateTime.TryParse(worksheet.Cells[row, columnIndexes["OutPunch"]]?.Text, out var parsedOutPunch) ? parsedOutPunch : null;

                                    if (!inPunch.HasValue || !outPunch.HasValue)
                                    {
                                        throw new Exception($"ERROR: Invalid InPunch or OutPunch format at row {row}.");
                                    }

                                    var manualPunchRequisition = new ManualPunchRequistion
                                    {
                                        StaffId = staffId,
                                        SelectPunch = worksheet.Cells[row, columnIndexes["SelectPunch"]]?.Text?.Trim() ?? string.Empty,
                                        InPunch = inPunch.Value,
                                        OutPunch = outPunch.Value,
                                        Remarks = worksheet.Cells[row, columnIndexes["Remarks"]]?.Text?.Trim() ?? string.Empty,
                                        IsActive = true,
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow,
                                        UpdatedUtc = DateTime.UtcNow,
                                        UpdatedBy = createdBy,
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
                                    throw new Exception("No valid manual punch requisitions found in the Excel file.");
                                }
                            }
                            else if (excelImportId == 9)
                            {
                                var commonPermissions = new List<CommonPermission>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text.Trim();
                                    var applicationType = await _context.ApplicationTypes
                                        .FirstOrDefaultAsync(a => a.ApplicationTypeName == applicationTypeName);

                                    if (applicationType == null)
                                    {
                                        throw new Exception($"Invalid ApplicationTypeName '{applicationTypeName}' at row {row}. It does not exist in the database.");
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
                                                throw new Exception($"ERROR: Organization code '{organizationCode}' not found in OrganizationTypes table at row {row}.");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id);

                                            if (!staffExistsInOrg)
                                            {
                                                throw new Exception($"ERROR: Staff ID '{parsedStaffId}' not found or does not belong to Organization Type '{organizationCode}' at row {row}.");
                                            }

                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"ERROR: Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
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
                                    var existingPermissionOnDate = await _context.CommonPermissions
                                        .AnyAsync(p => p.StaffId == staffId && p.PermissionDate == permissionDate);
                                    if (existingPermissionOnDate)
                                    {
                                        throw new Exception($"Permission for the date {permissionDate:yyyy-MM-dd} already exists.");
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
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };

                                    commonPermissions.Add(commonPermission);
                                }
                                await _context.CommonPermissions.AddRangeAsync(commonPermissions);
                            }
                            else if (excelImportId == 10)
                            {
                                var staffCreations = new List<StaffCreation>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var staffText = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();

                                    if (string.IsNullOrEmpty(staffText))
                                    {
                                        Console.WriteLine($"Invalid or missing StaffId at row {row}. Skipping...");
                                        continue;
                                    }

                                    if (string.IsNullOrEmpty(staffText) || !int.TryParse(Regex.Match(staffText, @"\d+").Value, out int staffId))
                                    {
                                        Console.WriteLine($"Invalid or missing StaffId at row {row}. Skipping...");
                                        continue;
                                    }

                                    var existingStaff = _context.StaffCreations
                                        .FirstOrDefault(s => s.Id == staffId && s.IsActive == true);

                                    if (existingStaff == null)
                                    {
                                        Console.WriteLine($"Staff with ID {staffText} not found or inactive at row {row}. Skipping...");
                                        continue;
                                    }

                                    var statusName = worksheet.Cells[row, columnIndexes["Status"]]?.Text.Trim();

                                    if (string.IsNullOrEmpty(statusName))
                                    {
                                        Console.WriteLine($"Invalid Status name at row {row}. Skipping...");
                                        continue;
                                    }

                                    var statusId = _context.Statuses
                                        .Where(d => d.Name.Trim().ToLower() == statusName.Trim().ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefault();

                                    if (statusId == 0)
                                    {
                                        Console.WriteLine($"Status '{statusName}' not found in the database at row {row}. Skipping...");
                                        continue;
                                    }

                                    var resignationDateText = worksheet.Cells[row, columnIndexes["ResignationDate"]]?.Text;
                                    var relievingDateText = worksheet.Cells[row, columnIndexes["RelievingDate"]]?.Text;

                                    DateOnly? resignationDate = DateOnly.TryParse(resignationDateText, out var parsedResignationDate) ? parsedResignationDate : (DateOnly?)null;
                                    DateOnly? relievingDate = DateOnly.TryParse(relievingDateText, out var parsedRelievingDate) ? parsedRelievingDate : (DateOnly?)null;

                                    existingStaff.ResignationDate = resignationDate;
                                    existingStaff.RelievingDate = relievingDate;
                                    existingStaff.StatusId = statusId;
                                    existingStaff.UpdatedBy = createdBy;
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
                                    Console.WriteLine("No valid staff records found for update.");
                                }
                            }
                            else if (excelImportId == 11)
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
                                        ShiftName = worksheet.Cells[row, columnIndexes["ShiftName"]].Text.Trim(),
                                        StartTime = startTime,
                                        ShortName = shortName,
                                        EndTime = endTime,
                                        IsActive = true,
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };

                                    shiftMasters.Add(shiftMaster);
                                }

                                await _context.Shifts.AddRangeAsync(shiftMasters);
                            }
                            else if (excelImportId == 12)
                            {
                                var leaveRequisitions = new List<LeaveRequisition>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text.Trim();
                                    var applicationType = await _context.ApplicationTypes
                                        .FirstOrDefaultAsync(a => a.ApplicationTypeName == applicationTypeName);
                                    if (applicationType == null)
                                    {
                                        throw new Exception($"Invalid ApplicationTypeName '{applicationTypeName}' at row {row}. It does not exist in the database.");
                                    }
                                    var leaveTypeName = worksheet.Cells[row, columnIndexes["LeaveTypeName"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(leaveTypeName))
                                        throw new Exception($"Leave type name is required at row {row}.");
                                    var leaveTypeId = _context.LeaveTypes
                                        .Where(l => l.Name.ToLower() == leaveTypeName.ToLower())
                                        .Select(l => l.Id)
                                        .FirstOrDefault();

                                    if (leaveTypeId == 0)
                                        throw new Exception($"Leave type '{leaveTypeName}' not found in the database at row {row}.");

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
                                                throw new Exception($"ERROR: Organization code '{organizationCode}' not found in OrganizationTypes table at row {row}.");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new Exception($"ERROR: Staff ID '{parsedStaffId}' not found or does not belong to Organization Type '{organizationCode}' at row {row}.");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"ERROR: Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
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
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    leaveRequisitions.Add(leaveRequisition);
                                }
                                if (leaveRequisitions.Count > 0)
                                {
                                    await _context.LeaveRequisitions.AddRangeAsync(leaveRequisitions);
                                }
                            }
                            else if (excelImportId == 13)
                            {
                                var onDutyRequisitions = new List<OnDutyRequisition>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text.Trim();
                                    var applicationType = await _context.ApplicationTypes
                                        .FirstOrDefaultAsync(a => a.ApplicationTypeName == applicationTypeName);
                                    if (applicationType == null)
                                    {
                                        throw new Exception($"Invalid ApplicationTypeName '{applicationTypeName}' at row {row}. It does not exist in the database.");
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
                                                throw new Exception($"ERROR: Organization code '{organizationCode}' not found in OrganizationTypes table at row {row}.");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new Exception($"ERROR: Staff ID '{parsedStaffId}' not found or does not belong to Organization Type '{organizationCode}' at row {row}.");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"ERROR: Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
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
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    onDutyRequisitions.Add(onDutyRequisition);
                                }
                                if (onDutyRequisitions.Count > 0)
                                {
                                    await _context.OnDutyRequisitions.AddRangeAsync(onDutyRequisitions);
                                }
                            }
                            else if (excelImportId == 14)
                            {
                                var workFromHomes = new List<WorkFromHome>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text.Trim();
                                    var applicationType = await _context.ApplicationTypes
                                        .FirstOrDefaultAsync(a => a.ApplicationTypeName == applicationTypeName);

                                    if (applicationType == null)
                                    {
                                        throw new Exception($"Invalid ApplicationTypeName '{applicationTypeName}' at row {row}. It does not exist in the database.");
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
                                                throw new Exception($"ERROR: Organization code '{organizationCode}' not found in OrganizationTypes table at row {row}.");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id);

                                            if (!staffExistsInOrg)
                                            {
                                                throw new Exception($"ERROR: Staff ID '{parsedStaffId}' not found or does not belong to Organization Type '{organizationCode}' at row {row}.");
                                            }

                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"ERROR: Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
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
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };

                                    workFromHomes.Add(workFrom);
                                }

                                if (workFromHomes.Count > 0)
                                {
                                    await _context.WorkFromHomes.AddRangeAsync(workFromHomes);
                                }
                            }
                            else if (excelImportId == 15)
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

                                            var organizationType = await _context.OrganizationTypes
                                                .FirstOrDefaultAsync(o => o.ShortName == organizationCode);
                                            if (organizationType == null)
                                            {
                                                throw new Exception($"ERROR: Organization code '{organizationCode}' not found in OrganizationTypes table at row {row}.");
                                            }

                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new Exception($"ERROR: Staff ID '{parsedStaffId}' not found or does not belong to Organization Type '{organizationCode}' at row {row}.");
                                            }
                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"ERROR: Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    DateOnly? otDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["OTDate"]]?.Text, out var parsedOtDate) ? parsedOtDate : null;
                                    if (!otDate.HasValue)
                                    {
                                        throw new Exception($"ERROR: Invalid or missing OTDate at row {row}.");
                                    }

                                    DateTime? startTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["StartTime"]]?.Text, out var parsedStartTime) ? parsedStartTime : null;
                                    DateTime? endTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["EndTime"]]?.Text, out var parsedEndTime) ? parsedEndTime : null;

                                    string otType = worksheet.Cells[row, columnIndexes["OTType"]]?.Text.Trim() ?? string.Empty;
                                    if (string.IsNullOrEmpty(otType))
                                    {
                                        throw new Exception($"ERROR: OTType is required at row {row}.");
                                    }

                                    var onDutyOvertime = new OnDutyOvertime
                                    {
                                        StaffId = staffId ?? throw new Exception($"ERROR: Staff ID is required at row {row}."),
                                        Otdate = otDate.Value,
                                        StartTime = startTime,
                                        EndTime = endTime,
                                        Ottype = otType,
                                        IsActive = true,
                                        CreatedBy = createdBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };

                                    onDutyOvertimes.Add(onDutyOvertime);
                                }
                                if (onDutyOvertimes.Count > 0)
                                {
                                    await _context.OnDutyOvertimes.AddRangeAsync(onDutyOvertimes);
                                }
                            }
                            else if (excelImportId == 16)
                            {
                                var shiftExtensions = new List<ShiftExtension>();

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationTypeName"]]?.Text?.Trim();
                                    if (string.IsNullOrEmpty(applicationTypeName))
                                    {
                                        throw new Exception($"ERROR: ApplicationTypeName is required at row {row}.");
                                    }
                                    var applicationType = await _context.ApplicationTypes
                                        .FirstOrDefaultAsync(a => a.ApplicationTypeName.ToLower() == applicationTypeName.ToLower());

                                    if (applicationType == null)
                                    {
                                        throw new Exception($"ERROR: ApplicationType '{applicationTypeName}' not found in the database at row {row}.");
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
                                            var organizationType = await _context.OrganizationTypes
                                                .FirstOrDefaultAsync(o => o.ShortName == organizationCode);
                                            if (organizationType == null)
                                            {
                                                throw new Exception($"ERROR: Organization code '{organizationCode}' not found in OrganizationTypes table at row {row}.");
                                            }
                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id);

                                            if (!staffExistsInOrg)
                                            {
                                                throw new Exception($"ERROR: Staff ID '{parsedStaffId}' not found or does not belong to Organization Type '{organizationCode}' at row {row}.");
                                            }

                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"ERROR: Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }

                                    DateOnly? transactionDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["TransactionDate"]]?.Text, out var parsedDate) ? parsedDate : null;
                                    if (!transactionDate.HasValue)
                                    {
                                        throw new Exception($"ERROR: Invalid or missing TransactionDate at row {row}.");
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
                                        CreatedBy = createdBy,
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
                                    throw new Exception("No valid shift extensions found in the Excel file.");
                                }
                            }
                            else if (excelImportId == 17)
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

                                            var organizationType = await _context.OrganizationTypes
                                                .FirstOrDefaultAsync(o => o.ShortName == organizationCode);
                                            if (organizationType == null)
                                            {
                                                throw new Exception($"ERROR: Organization code '{organizationCode}' not found in OrganizationTypes table at row {row}.");
                                            }

                                            bool staffExistsInOrg = await _context.StaffCreations
                                                .AnyAsync(s => s.Id == parsedStaffId && s.OrganizationTypeId == organizationType.Id);

                                            if (!staffExistsInOrg)
                                            {
                                                throw new Exception($"ERROR: Staff ID '{parsedStaffId}' not found or does not belong to Organization Type '{organizationCode}' at row {row}.");
                                            }

                                            staffId = parsedStaffId;
                                        }
                                        else
                                        {
                                            throw new Exception($"ERROR: Invalid Staff ID format '{staffIdText}' at row {row}. Expected format: OrganizationCode + NumericId (e.g., VL42).");
                                        }
                                    }
                                    DateOnly? vaccinatedDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["VaccinatedDate"]]?.Text, out var parsedVaccinatedDate) ? parsedVaccinatedDate : null;
                                    if (!vaccinatedDate.HasValue)
                                    {
                                        throw new Exception($"ERROR: Invalid or missing VaccinatedDate at row {row}.");
                                    }

                                    if (!int.TryParse(worksheet.Cells[row, columnIndexes["VaccinationNumber"]]?.Text, out int vaccinationNumber))
                                    {
                                        throw new Exception($"ERROR: Invalid or missing VaccinationNumber at row {row}.");
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
                                        throw new Exception($"ERROR: Invalid IsExempted value '{isExemptedText}' at row {row}. Expected 'Yes' or 'No'.");
                                    }
                                    string? comments = worksheet.Cells[row, columnIndexes["Comments"]]?.Text?.Trim();

                                    var staffVaccination = new StaffVaccination
                                    {
                                        StaffId = staffId ?? throw new Exception($"ERROR: Staff ID is required at row {row}."),
                                        VaccinatedDate = vaccinatedDate,
                                        VaccinationNumber = vaccinationNumber,
                                        IsExempted = isExempted,
                                        Comments = comments,
                                        IsActive = true,
                                        CreatedBy = createdBy,
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
                                    throw new Exception("No valid staff vaccinations found in the Excel file.");
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
