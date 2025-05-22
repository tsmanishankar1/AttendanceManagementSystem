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
    private readonly string _workspacePath;
    public ExcelImportService(AttendanceManagementSystemContext context, IWebHostEnvironment env)
    {
        _context = context;
        _workspacePath = Path.Combine(env.ContentRootPath, "ExcelTemplates");
        if (!Directory.Exists(_workspacePath))
        {
            Directory.CreateDirectory(_workspacePath);
        }
    }

    /*    public async Task<byte[]> GetExcelTemplateBytes(int excelImportId)
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
    */

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
            var fileExtension = Path.GetExtension(excelImportDto.File.FileName);
            if (!string.Equals(fileExtension, ".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Please upload the correct Excel template File");
            }
            var expectedFileName = excelImportType.Name;
            var uploadedFileName = Path.GetFileNameWithoutExtension(excelImportDto.File.FileName);
            var pattern = $"^{Regex.Escape(expectedFileName)}( \\(\\d+\\))?$";
            if (!Regex.IsMatch(uploadedFileName, pattern, RegexOptions.IgnoreCase)) throw new ArgumentException("Please upload the correct Excel template file");
            var staffExists = await _context.StaffCreations.AnyAsync(s => s.Id == excelImportDto.CreatedBy && s.IsActive == true);
            if (!staffExists) throw new MessageNotFoundException($"Staff not found");
            using (var stream = new MemoryStream())
            {
                await excelImportDto.File.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null) throw new MessageNotFoundException("Worksheet not found in the uploaded file");
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
                            "AadharNo", "PanNo", "PassportNo", "DrivingLicense", "BankName", "BankAccountNo", "BankIfscCode", "BankBranch",
                            "HomeAddress", "FatherName", "EmergencyContactPerson1", "EmergencyContactPerson2", "EmergencyContactNo1", "EmergencyContactNo2", "MotherName",
                            "FatherAadharNo", "MotherAadharNo", "OrganizationType", "WorkingStatus", "ConfirmationDate", "PostalCode", "ApprovalLevel", "OfficialEmail"
                        };
                    }
                    else if (excelImportDto.ExcelImportId == 2)
                    {
                        requiredHeaders = new List<string> {"LeaveType", "StaffId", "TransactionFlag", "Month", "Year", "Remarks", "LeaveCount", "LeaveReason"};
                    }
                    else if (excelImportDto.ExcelImportId == 3)
                    {
                        requiredHeaders = new List<string> {"Name", "ShortName", "Phone", "Fax", "Email" };
                    }
                    else if (excelImportDto.ExcelImportId == 4 || excelImportDto.ExcelImportId == 5 || excelImportDto.ExcelImportId == 6)
                    {
                        requiredHeaders = new List<string> {"Name", "ShortName" };
                    }
                    else if (excelImportDto.ExcelImportId == 7)
                    {
                        requiredHeaders = new List<string> {"Name", "ShortName" };
                    }
                    else if (excelImportDto.ExcelImportId == 8)
                    {
                        requiredHeaders = new List<string> {"StaffId", "SelectPunch", "InPunch", "OutPunch", "Remarks", "ApplicationType" };
                    }
                    else if (excelImportDto.ExcelImportId == 9)
                    {
                        requiredHeaders = new List<string> {"StartTime", "EndTime", "StaffId", "Remarks", "PermissionDate", "PermissionType", "ApplicationType" };
                    }
                    else if (excelImportDto.ExcelImportId == 10)
                    {
                        requiredHeaders = new List<string> {"StaffId", "ResignationDate", "RelievingDate", "Status" };
                    }
                    else if (excelImportDto.ExcelImportId == 11)
                    {
                        requiredHeaders = new List<string> {"StaffId", "Shift", "FromDate", "ToDate" };
                    }
                    else if (excelImportDto.ExcelImportId == 12)
                    {
                        requiredHeaders = new List<string> {"ApplicationType", "FromDate", "ToDate", "Reason", "StaffId", "StartDuration", "EndDuration", "LeaveType", "TotalDays" };
                    }
                    else if (excelImportDto.ExcelImportId == 13)
                    {
                        requiredHeaders = new List<string> {"ApplicationType", "StartTime", "EndTime", "StartDate", "EndDate", "Reason", "StaffId", "StartDuration", "EndDuration", "TotalDays", "TotalHours" };
                    }
                    else if (excelImportDto.ExcelImportId == 14)
                    {
                        requiredHeaders = new List<string> {"ApplicationType", "FromTime", "ToTime", "FromDate", "ToDate", "Reason", "StartDuration", "EndDuration", "StaffId", "TotalDays", "TotalHours" };
                    }
                    else if (excelImportDto.ExcelImportId == 15)
                    {
                        requiredHeaders = new List<string> {"StaffId", "OTDate", "StartTime", "EndTime", "OTType" };
                    }
                    else if (excelImportDto.ExcelImportId == 16)
                    {
                        requiredHeaders = new List<string> {"ApplicationType", "StaffId", "TransactionDate", "BeforeShiftHours", "AfterShiftHours", "Remarks", "DurationHours" , "Shift"};
                    }
                    else if (excelImportDto.ExcelImportId == 17)
                    {
                        requiredHeaders = new List<string> {"StaffId", "VaccinatedDate", "VaccinationNumber", "IsExempted", "Comments", "SecondVaccinationDate" };
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
                    else if (excelImportDto.ExcelImportId == 20)
                    {
                        requiredHeaders = new List<string>
                        {
                            "Employee Name", "Group Name", "Display Name In Reports", "Date of Joining", "Employee Number", "Designation", "Department",
                            "Location", "Gender", "Date of Birth", "Father's/Mother's Name", "Spouse Name", "Address", "Email", "Phone No", "Bank Name",
                            "Account No", "IFSC Code", "PF Account No", "UAN No", "PAN No", "Aadhaar No", "ESI No", "Salary Effective From", "Basic_Actual",
                            "HRA_Actual", "CONVE_Actual", "MED_ALLOW_Actual", "SPL_ALLOW_Actual", "LOP_DAYS", "STD_DAYS", "WRK_DAYS", "PF_ADMIN",
                            "BASIC EARNED", "BASIC_ARRADJ", "HRA_EARNED", "HRA_ARRADJ", "CONVE_EARNED", "CONVE_ARRADJ", "MED_ALLOW_EARNED", "MED_ALLOW__ARRADJ",
                            "SPL_ALLOW_EARNED", "SPL_ALLOW__ARRADJ", "OTHER_ALL", "GROSS_EARN", "PF", "ESI", "LWF", "PT", "IT", "MED_CLAIM", "OTHER_DED",
                            "GROSS_DED", "NET_PAY"
                        };
                    }
                    else if(excelImportDto.ExcelImportId == 21)
                    {
                        requiredHeaders = new List<string>
                        {
                            "Emp ID", "Name", "EMP Division", "Prod %", "Prod Score", "Prod Grade", "Qual %", "Qual Score", "Qual Grade", "No of Abs", "Attd %",
                            "Attd Score", "Attd Grade", "Total Score", "Working months", "Score", "Final %", "Final Grade", "Comments"
                        };
                    }
                    else
                    {
                        throw new MessageNotFoundException("Excel import type not found");
                    }
                    var missingHeaders = requiredHeaders.Where(header => !headerRow.Contains(header)).ToList();
                    if (missingHeaders.Any())
                    {
                        throw new ArgumentException($"Missing headers: {string.Join(", ", missingHeaders)}");
                    }
                    var extraHeaders = headerRow.Except(requiredHeaders).ToList();
                    if (extraHeaders.Any())
                    {
                        throw new ArgumentException($"Contains unexpected headers: {string.Join(", ", extraHeaders)}");
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
                                    var branchName = worksheet.Cells[row, columnIndexes["BranchName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(branchName))
                                    {
                                        continue;
                                    }
                                    var branch = await _context.BranchMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == branchName.ToLower() && d.IsActive);
                                    if (branch == null)
                                    {
                                        continue;
                                    }
                                    var approvalLevel1 = worksheet.Cells[row, columnIndexes["ApprovalLevel1"]].Text.Trim();
                                    if (string.IsNullOrEmpty(approvalLevel1))
                                    {
                                        continue;
                                    }
                                    int numericPart1 = int.Parse(Regex.Match(approvalLevel1, @"\d+").Value);
                                    var approval1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == numericPart1 && s.IsActive == true);
                                    if (approval1 == null)
                                    {
                                        continue;
                                    }
                                    var approvalLevel2 = worksheet.Cells[row, columnIndexes["ApprovalLevel2"]]?.Text.Trim();
                                    if (string.IsNullOrEmpty(approvalLevel2))
                                    {
                                        continue;
                                    }
                                    int numericPart = int.Parse(Regex.Match(approvalLevel2, @"\d+").Value);
                                    var approval2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == numericPart && s.IsActive == true);
                                    if (approval2 == null)
                                    {
                                        continue;
                                    }
                                    var departmentName = worksheet.Cells[row, columnIndexes["DepartmentName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(departmentName))
                                    {
                                        continue;
                                    }
                                    var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == departmentName.ToLower() && d.IsActive);
                                    if (department == null) throw new MessageNotFoundException($"Department '{departmentName}' not found");
                                    var statusName = worksheet.Cells[row, columnIndexes["StatusName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(statusName))
                                    {
                                        continue;
                                    }
                                    var status = await _context.Statuses.FirstOrDefaultAsync(d => d.Name.ToLower() == statusName.ToLower() && d.IsActive);
                                    if (status == null) throw new MessageNotFoundException($"Status '{statusName}' not found");
                                    var divisionName = worksheet.Cells[row, columnIndexes["DivisionName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(divisionName))
                                    {
                                        continue;
                                    }
                                    var division = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == divisionName.ToLower() && d.IsActive);
                                    if (division == null) throw new MessageNotFoundException($"Division '{divisionName}' not found");
                                    var designationName = worksheet.Cells[row, columnIndexes["DesignationName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(designationName))
                                    {
                                        continue;
                                    }
                                    var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive);
                                    if (designation == null) throw new MessageNotFoundException($"Designation '{designationName}' not found");
                                    var gradeName = worksheet.Cells[row, columnIndexes["GradeName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(gradeName))
                                    {
                                        continue;
                                    }
                                    var grade = await _context.GradeMasters.FirstOrDefaultAsync(g => g.Name.ToLower() == gradeName.ToLower() && g.IsActive);
                                    if (grade == null) throw new MessageNotFoundException($"Grade '{gradeName}' not found");
                                    var organizationTypeName = worksheet.Cells[row, columnIndexes["OrganizationType"]].Text.Trim();
                                    if (string.IsNullOrEmpty(organizationTypeName))
                                    {
                                        continue;
                                    }
                                    var organizationType = await _context.OrganizationTypes.FirstOrDefaultAsync(g => g.ShortName.ToLower() == organizationTypeName.ToLower() && g.IsActive);
                                    if (organizationType == null) throw new MessageNotFoundException($"Organization '{organizationTypeName}' not found");
                                    var categoryName = worksheet.Cells[row, columnIndexes["CategoryName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(categoryName))
                                    {
                                        continue;
                                    }
                                    var category = await _context.CategoryMasters.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower() && c.IsActive);
                                    if (category == null) throw new MessageNotFoundException($"Category '{categoryName}' not found");
                                    var costCenterName = worksheet.Cells[row, columnIndexes["CostCenterName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(costCenterName))
                                    {
                                        continue;
                                    }
                                    var costCenter = await _context.CostCentreMasters.FirstOrDefaultAsync(c => c.Name.ToLower() == costCenterName.ToLower() && c.IsActive);
                                    if (costCenter == null) throw new MessageNotFoundException($"Cost Center '{costCenterName}' not found");
                                    var workStationName = worksheet.Cells[row, columnIndexes["WorkStationName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(workStationName))
                                    {
                                        continue;
                                    }
                                    var workStation = await _context.WorkstationMasters.FirstOrDefaultAsync(w => w.Name.ToLower() == workStationName.ToLower() && w.IsActive);
                                    if (workStation == null) throw new MessageNotFoundException($"Workstation '{workStationName}' not found");
                                    var leaveGroupName = worksheet.Cells[row, columnIndexes["LeaveGroupName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(leaveGroupName))
                                    {
                                        continue;
                                    }
                                    var leaveGroup = await _context.LeaveGroupConfigurations.FirstOrDefaultAsync(l => l.LeaveGroupConfigurationName.ToLower() == leaveGroupName.ToLower() && l.IsActive);
                                    if (leaveGroup == null) throw new MessageNotFoundException($"Leave Group '{leaveGroupName}' not found");
                                    var companyName = worksheet.Cells[row, columnIndexes["CompanyMasterName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(companyName))
                                    {
                                        continue;
                                    }
                                    var companyMaster = await _context.CompanyMasters.FirstOrDefaultAsync(c => c.Name.ToLower() == companyName.ToLower() && c.IsActive);
                                    if (companyMaster == null) throw new MessageNotFoundException($"Company '{companyName}' not found");
                                    var locationName = worksheet.Cells[row, columnIndexes["LocationMasterName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(locationName))
                                    {
                                        continue;
                                    }
                                    var locationMaster = await _context.LocationMasters.FirstOrDefaultAsync(l => l.Name.ToLower() == locationName.ToLower() && l.IsActive);
                                    if (locationMaster == null) throw new MessageNotFoundException($"Location '{locationName}' not found");
                                    var holidayCalendarName = worksheet.Cells[row, columnIndexes["HolidayCalendarName"]].Text.Trim();
                                    if (string.IsNullOrEmpty(holidayCalendarName))
                                    {
                                        continue;
                                    }
                                    var holidayCalendar = await _context.HolidayCalendarConfigurations.FirstOrDefaultAsync(h => h.Name.ToLower() == holidayCalendarName.ToLower() && h.IsActive);
                                    if (holidayCalendar == null) throw new MessageNotFoundException($"Holiday Calendar '{holidayCalendarName}' not found");
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
                                        BranchId = branch.Id,
                                        DepartmentId = department.Id,
                                        DivisionId = division.Id,
                                        Volume = worksheet.Cells[row, columnIndexes["Volume"]].Text.Trim(),
                                        DesignationId = designation.Id,
                                        GradeId = grade.Id,
                                        CategoryId = category.Id,
                                        CostCenterId = costCenter.Id,
                                        WorkStationId = workStation.Id,
                                        City = worksheet.Cells[row, columnIndexes["City"]].Text.Trim(),
                                        District = worksheet.Cells[row, columnIndexes["District"]].Text.Trim(),
                                        State = worksheet.Cells[row, columnIndexes["State"]].Text.Trim(),
                                        Country = worksheet.Cells[row, columnIndexes["Country"]].Text.Trim(),
                                        OtEligible = bool.TryParse(worksheet.Cells[row, columnIndexes["Oteligible"]].Text, out var otEligible) ? otEligible : false,
                                        ApprovalLevel1 = approval1.Id,
                                        ApprovalLevel2 = approval2.Id,
                                        AccessLevel = worksheet.Cells[row, columnIndexes["AccessLevel"]].Text.Trim(),
                                        PolicyGroup = worksheet.Cells[row, columnIndexes["PolicyGroup"]].Text.Trim(),
                                        WorkingDayPattern = worksheet.Cells[row, columnIndexes["WorkingDayPattern"]].Text.Trim(),
                                        Tenure = decimal.TryParse(worksheet.Cells[row, columnIndexes["Tenure"]].Text.Trim(), out decimal tenure) ? tenure : 0m,
                                        UanNumber = worksheet.Cells[row, columnIndexes["Uannumber"]]?.Text.Trim(),
                                        EsiNumber = worksheet.Cells[row, columnIndexes["EsiNumber"]]?.Text.Trim(),
                                        IsMobileAppEligible = (bool)(bool.TryParse(worksheet.Cells[row, columnIndexes["IsMobileAppEligible"]].Text, out var isMobileAppEligible) ? isMobileAppEligible : false),
                                        GeoStatus = worksheet.Cells[row, columnIndexes["GeoStatus"]].Text.Trim(),
                                        MiddleName = worksheet.Cells[row, columnIndexes["MiddleName"]]?.Text.Trim(),
                                        OfficialPhone = long.TryParse(worksheet.Cells[row, columnIndexes["OfficialPhone"]]?.Text, out var officialPhone) ? officialPhone : 0,
                                        PersonalLocation = worksheet.Cells[row, columnIndexes["PersonalLocation"]].Text.Trim(),
                                        PersonalEmail = worksheet.Cells[row, columnIndexes["PersonalEmail"]].Text.Trim(),
                                        LeaveGroupId = leaveGroup.Id,
                                        CompanyMasterId = companyMaster.Id,
                                        LocationMasterId = locationMaster.Id,
                                        HolidayCalendarId = holidayCalendar.Id,
                                        StatusId = status.Id,
                                        AadharNo = long.TryParse(worksheet.Cells[row, columnIndexes["AadharNo"]]?.Text, out var aadharNo) ? aadharNo : 0,
                                        PanNo = worksheet.Cells[row, columnIndexes["PanNo"]]?.Text.Trim(),
                                        PassportNo = worksheet.Cells[row, columnIndexes["PassportNo"]]?.Text.Trim(),
                                        DrivingLicense = worksheet.Cells[row, columnIndexes["DrivingLicense"]]?.Text.Trim(),
                                        BankName = worksheet.Cells[row, columnIndexes["BankName"]]?.Text.Trim(),
                                        BankAccountNo = long.TryParse(worksheet.Cells[row, columnIndexes["BankAccountNo"]]?.Text, out var bankAccountNo) ? bankAccountNo : 0,
                                        BankIfscCode = worksheet.Cells[row, columnIndexes["BankIfscCode"]]?.Text.Trim(),
                                        BankBranch = worksheet.Cells[row, columnIndexes["BankBranch"]]?.Text.Trim(),
                                        HomeAddress = worksheet.Cells[row, columnIndexes["HomeAddress"]].Text.Trim(),
                                        FatherName = worksheet.Cells[row, columnIndexes["FatherName"]].Text.Trim(),
                                        EmergencyContactPerson1 = worksheet.Cells[row, columnIndexes["EmergencyContactPerson1"]].Text.Trim(),
                                        EmergencyContactPerson2 = worksheet.Cells[row, columnIndexes["EmergencyContactPerson2"]]?.Text.Trim() ?? string.Empty,
                                        EmergencyContactNo1 = long.TryParse(worksheet.Cells[row, columnIndexes["EmergencyContactNo1"]].Text, out var emergencyContactNo1) ? emergencyContactNo1 : 0,
                                        EmergencyContactNo2 = long.TryParse(worksheet.Cells[row, columnIndexes["EmergencyContactNo2"]]?.Text, out var emergencyContactNo2) ? emergencyContactNo2 : 0,
                                        MotherName = worksheet.Cells[row, columnIndexes["MotherName"]].Text.Trim(),
                                        FatherAadharNo = long.TryParse(worksheet.Cells[row, columnIndexes["FatherAadharNo"]]?.Text, out var fatherAadharNo) ? fatherAadharNo : 0,
                                        MotherAadharNo = long.TryParse(worksheet.Cells[row, columnIndexes["MotherAadharNo"]]?.Text, out var motherAadharNo) ? motherAadharNo : 0,
                                        OrganizationTypeId = organizationType.Id,
                                        WorkingStatus = worksheet.Cells[row, columnIndexes["WorkingStatus"]].Text.Trim(),
                                        ConfirmationDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["ConfirmationDate"]]?.Text, out var confirmationDate) ? confirmationDate : (DateOnly?)null,
                                        PostalCode = int.TryParse(worksheet.Cells[row, columnIndexes["PostalCode"]].Text, out var postalCode) ? postalCode : 0,
                                        ApprovalLevel = worksheet.Cells[row, columnIndexes["ApprovalLevel"]].Text.Trim() ?? string.Empty,
                                        OfficialEmail = worksheet.Cells[row, columnIndexes["OfficialEmail"]]?.Text.Trim() ?? string.Empty,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        IsActive = true,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    staffCreations.Add(staffCreation);
                                }
                                if(staffCreations.Count > 0)
                                {
                                    await _context.StaffCreations.AddRangeAsync(staffCreations);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 2)
                            {
                                var individualLeaveCreditDebits = new List<IndividualLeaveCreditDebit>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var leaveTypeName = worksheet.Cells[row, columnIndexes["LeaveType"]].Text.Trim();
                                    if (string.IsNullOrEmpty(leaveTypeName)) throw new MessageNotFoundException($"Leave type {leaveTypeName} not found");
                                    var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(l => l.Name.ToLower() == leaveTypeName.ToLower() && l.IsActive);
                                    if (leaveType == null) throw new MessageNotFoundException($"Leave type '{leaveTypeName}' not found");
                                    var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    if (string.IsNullOrEmpty(staffCreationIdStr)) throw new MessageNotFoundException($"Staff {staffCreationIdStr} not found");
                                    var match = Regex.Match(staffCreationIdStr, @"([A-Za-z]+)(\d+)");
                                    if (!match.Success) throw new ArgumentException($"Invalid Staff Id format {staffCreationIdStr}");
                                    var shortName = match.Groups[1].Value;
                                    var staffId = int.Parse(match.Groups[2].Value);
                                    var staffCreation = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive == true);
                                    if (staffCreation == null) throw new MessageNotFoundException("Staff not found");
                                    var transactionFlagValue = worksheet.Cells[row, columnIndexes["TransactionFlag"]].Text.Trim().ToLower();
                                    var transactionFlag = transactionFlagValue == "true" || transactionFlagValue == "false";
                                    var leaveCount = decimal.TryParse(worksheet.Cells[row, columnIndexes["LeaveCount"]].Text, out var parsedLeaveCount) ? parsedLeaveCount : 0;
                                    if (leaveCount <= 0) throw new ArgumentException($"Invalid leave count {leaveCount}");
                                    var actualBalance = await _context.IndividualLeaveCreditDebits
                                        .Where(l => l.StaffCreationId == staffCreation.Id && l.LeaveTypeId == leaveType.Id && l.IsActive)
                                        .OrderByDescending(l => l.CreatedUtc)
                                        .Select(l => (decimal?)l.ActualBalance ?? 0)
                                        .FirstOrDefaultAsync();
                                    var availableBalance = await _context.IndividualLeaveCreditDebits
                                        .Where(l => l.StaffCreationId == staffCreation.Id && l.LeaveTypeId == leaveType.Id && l.IsActive)
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
                                        if (availableBalance < leaveCount) throw new ConflictException($"Insufficient leave balance for staff '{staffCreationIdStr}'");
                                        availableBalance -= leaveCount;
                                    }
                                    var individualLeaveCreditDebit = new IndividualLeaveCreditDebit
                                    {
                                        LeaveTypeId = leaveType.Id,
                                        StaffCreationId = staffCreation.Id,
                                        LeaveReason = worksheet.Cells[row, columnIndexes["LeaveReason"]].Text.Trim(),
                                        Remarks = worksheet.Cells[row, columnIndexes["Remarks"]].Text.Trim(),
                                        TransactionFlag = transactionFlag,
                                        LeaveCount = leaveCount,
                                        Month = worksheet.Cells[row, columnIndexes["Month"]].Text.Trim(),
                                        Year = int.TryParse(worksheet.Cells[row, columnIndexes["Year"]].Text, out var parsedYear) ? parsedYear : DateTime.UtcNow.Year,
                                        ActualBalance = actualBalance,
                                        AvailableBalance = availableBalance,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    individualLeaveCreditDebits.Add(individualLeaveCreditDebit);
                                }
                                if(individualLeaveCreditDebits.Count > 0)
                                {
                                    await _context.IndividualLeaveCreditDebits.AddRangeAsync(individualLeaveCreditDebits);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 3)
                            {
                                var departmentMasters = new List<DepartmentMaster>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var departmentMaster = new DepartmentMaster
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
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
                                if(departmentMasters.Count > 0)
                                {
                                    await _context.DepartmentMasters.AddRangeAsync(departmentMasters);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 4)
                            {
                                var designationMasters = new List<DesignationMaster>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var designationMaster = new DesignationMaster
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    designationMasters.Add(designationMaster);
                                }
                                if(designationMasters.Count > 0)
                                {
                                    await _context.DesignationMasters.AddRangeAsync(designationMasters);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 5)
                            {
                                var divisionMasters = new List<DivisionMaster>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var divisionMaster = new DivisionMaster
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    divisionMasters.Add(divisionMaster);
                                }
                                if(divisionMasters.Count > 0)
                                {
                                    await _context.DivisionMasters.AddRangeAsync(divisionMasters);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 6)
                            {
                                var costCentreMasters = new List<CostCentreMaster>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var costCentreMaster = new CostCentreMaster
                                    {
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
                                        ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    costCentreMasters.Add(costCentreMaster);
                                }
                                if(costCentreMasters.Count > 0)
                                {
                                    await _context.CostCentreMasters.AddRangeAsync(costCentreMasters);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
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
                                if(volumes.Count > 0)
                                {
                                    await _context.Volumes.AddRangeAsync(volumes);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 8)
                            {
                                var manualPunchRequisitions = new List<ManualPunchRequistion>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                                    if (string.IsNullOrEmpty(applicationTypeName))
                                    {
                                        throw new Exception($"Application Type is required");
                                    }
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name.ToLower() == applicationTypeName.ToLower() && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"Application Type '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            var staffExist = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                                            if (staffExist == null) throw new MessageNotFoundException("Staff not found");
                                            bool staffExistsInOrg = await _context.StaffCreations.AnyAsync(s => s.Id == staffExist.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff Id '{staffIdText}' not found");
                                            }
                                            staffId = staffExist.Id;
                                        }
                                        else
                                        {
                                            throw new ArgumentException($"Invalid Staff Id format '{staffIdText}'");
                                        }
                                    }
                                    DateTime? inPunch = DateTime.TryParse(worksheet.Cells[row, columnIndexes["InPunch"]]?.Text, out var parsedInPunch) ? parsedInPunch : null;
                                    DateTime? outPunch = DateTime.TryParse(worksheet.Cells[row, columnIndexes["OutPunch"]]?.Text, out var parsedOutPunch) ? parsedOutPunch : null;
                                    if (!inPunch.HasValue || !outPunch.HasValue)
                                    {
                                        throw new ArgumentException($"Invalid InPunch or OutPunch format");
                                    }
                                    var manualPunchRequisition = new ManualPunchRequistion
                                    {
                                        StaffId = staffId,
                                        SelectPunch = worksheet.Cells[row, columnIndexes["SelectPunch"]].Text.Trim(),
                                        InPunch = inPunch.Value,
                                        OutPunch = outPunch.Value,
                                        Remarks = worksheet.Cells[row, columnIndexes["Remarks"]].Text.Trim(),
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
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 9)
                            {
                                var commonPermissions = new List<CommonPermission>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"Application Type '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            var staffExist = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                                            if (staffExist == null) throw new MessageNotFoundException("Staff not found");
                                            bool staffExistsInOrg = await _context.StaffCreations.AnyAsync(s => s.Id == staffExist.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff Id '{staffIdText}' not found");
                                            }
                                            staffId = staffExist.Id;
                                        }
                                        else
                                        {
                                            throw new ArgumentException($"Invalid Staff Id format '{staffIdText}'");
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
                                        throw new ConflictException($"Permission for the date {permissionDate:yyyy-MM-dd} already exists.");
                                    }
                                    var permissionsThisMonth = await _context.CommonPermissions
                                        .Where(p => p.StaffId == staffId && p.PermissionDate >= startOfMonth && p.PermissionDate <= endOfMonth)
                                        .ToListAsync();
                                    var totalMinutesUsed = permissionsThisMonth.Sum(p => TimeSpan.Parse(p.TotalHours).TotalMinutes);
                                    var newRequestMinutes = (endTime - startTime).TotalMinutes;
                                    if (newRequestMinutes <= 0)
                                    {
                                        throw new ArgumentException("End time must be greater than start time.");
                                    }
                                    if (newRequestMinutes > 120)
                                    {
                                        throw new ArgumentException("Permission duration cannot exceed 2 hours");
                                    }
                                    if (totalMinutesUsed + newRequestMinutes > 120)
                                    {
                                        throw new InvalidOperationException($"Cumulative permission time for {monthName} cannot exceed 2 hours");
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
                                    var staffText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    if (string.IsNullOrEmpty(staffText))
                                    {
                                        continue;
                                    }
                                    if (string.IsNullOrEmpty(staffText) || !int.TryParse(Regex.Match(staffText, @"\d+").Value, out int staffId))
                                    {
                                        continue;
                                    }
                                    var existingStaff = _context.StaffCreations.FirstOrDefault(s => s.StaffId == staffText && s.IsActive == true);
                                    if (existingStaff == null)
                                    {
                                        continue;
                                    }
                                    var statusName = worksheet.Cells[row, columnIndexes["Status"]].Text.Trim();
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
                                if (staffCreations.Count > 0)
                                {
                                    _context.StaffCreations.UpdateRange(staffCreations);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 11)
                            {
                                var shiftMasters = new List<AssignShift>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            var staffExist = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                                            if (staffExist == null) throw new MessageNotFoundException("Staff not found");
                                            bool staffExistsInOrg = await _context.StaffCreations.AnyAsync(s => s.Id == staffExist.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff Id '{staffIdText}' not found");
                                            }
                                            staffId = staffExist.Id;
                                        }
                                        else
                                        {
                                            throw new ArgumentException($"Invalid Staff Id format '{staffIdText}'");
                                        }
                                    }
                                    var shift = worksheet.Cells[row, columnIndexes["Shift"]].Text.Trim();
                                    if (string.IsNullOrEmpty(shift))
                                    {
                                        continue;
                                    }
                                    var shiftId = await _context.Shifts
                                        .Where(d => d.Name.Trim().ToLower() == shift.Trim().ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();

                                    DateOnly fromDate;
                                    DateOnly toDate;
                                    if (!DateOnly.TryParse(worksheet.Cells[row, columnIndexes["FromDate"]].Text, out fromDate))
                                    {
                                        throw new FormatException($"Invalid FromDate");
                                    }
                                    if (!DateOnly.TryParse(worksheet.Cells[row, columnIndexes["ToDate"]].Text, out toDate))
                                    {
                                        throw new FormatException($"Invalid ToDate");
                                    }
                                    var shiftMaster = new AssignShift
                                    {
                                        StaffId = staffId ?? throw new Exception($"Staff Id is required"),
                                        ShiftId = shiftId,
                                        FromDate = fromDate,
                                        ToDate = toDate,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    shiftMasters.Add(shiftMaster);
                                }
                                if(shiftMasters.Count > 0)
                                {
                                    await _context.AssignShifts.AddRangeAsync(shiftMasters);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 12)
                            {
                                var leaveRequisitions = new List<LeaveRequisition>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"Application Type '{applicationTypeName}' not found");
                                    }
                                    var leaveTypeName = worksheet.Cells[row, columnIndexes["LeaveType"]].Text.Trim();
                                    if (string.IsNullOrEmpty(leaveTypeName)) throw new Exception($"Leave type name is required");
                                    var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(l => l.Name.ToLower() == leaveTypeName.ToLower() && l.IsActive);
                                    if (leaveType == null) throw new MessageNotFoundException($"Leave type '{leaveTypeName}' not found");
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            var staffExist = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                                            if (staffExist == null) throw new MessageNotFoundException("Staff not found");
                                            bool staffExistsInOrg = await _context.StaffCreations.AnyAsync(s => s.Id == staffExist.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff Id '{staffIdText}' not found");
                                            }
                                            staffId = staffExist.Id;
                                        }
                                        else
                                        {
                                            throw new ArgumentException($"Invalid Staff Id format '{staffIdText}'");
                                        }
                                    }
                                    bool isFromDateValid = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["FromDate"]].Text, out var fromDate);
                                    bool isToDateValid = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["ToDate"]].Text, out var toDate);
                                    if (!isFromDateValid)
                                    {
                                        throw new ArgumentException($"Invalid FromDate {isFromDateValid}");
                                    }
                                    toDate = isToDateValid ? toDate : fromDate;
                                    var leaveRequisition = new LeaveRequisition
                                    {
                                        ApplicationTypeId = applicationType.Id,
                                        StartDuration = worksheet.Cells[row, columnIndexes["StartDuration"]].Text.Trim(),
                                        EndDuration = worksheet.Cells[row, columnIndexes["EndDuration"]].Text.Trim(),
                                        Reason = worksheet.Cells[row, columnIndexes["Reason"]].Text.Trim(),
                                        LeaveTypeId = leaveType.Id,
                                        FromDate = fromDate,
                                        ToDate = toDate,
                                        TotalDays = decimal.TryParse(worksheet.Cells[row, columnIndexes["TotalDays"]].Text.Trim(), out decimal prodeScore) ? prodeScore : 0m,
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
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 13)
                            {
                                var onDutyRequisitions = new List<OnDutyRequisition>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"Application Type '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            var staffExist = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                                            if (staffExist == null) throw new MessageNotFoundException("Staff not found");
                                            bool staffExistsInOrg = await _context.StaffCreations.AnyAsync(s => s.Id == staffExist.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff Id '{staffIdText}' not found");
                                            }
                                            staffId = staffExist.Id;
                                        }
                                        else
                                        {
                                            throw new ArgumentException($"Invalid Staff Id format '{staffIdText}'");
                                        }
                                    }
                                    DateOnly? startDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["StartDate"]]?.Text, out var parsedStartDate) ? parsedStartDate : null;
                                    DateOnly? endDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["EndDate"]]?.Text, out var parsedEndDate) ? parsedEndDate : null;
                                    DateTime? startTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["StartTime"]]?.Text, out var parsedStartTime) ? parsedStartTime : null;
                                    DateTime? endTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["EndTime"]]?.Text, out var parsedEndTime) ? parsedEndTime : null;
/*                                    decimal? totalDays = null;
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
*/                                  var onDutyRequisition = new OnDutyRequisition
                                    {
                                        ApplicationTypeId = applicationType.Id,
                                        StartTime = startTime,
                                        EndTime = endTime,
                                        StartDate = startDate,
                                        EndDate = endDate,
                                        StartDuration = worksheet.Cells[row, columnIndexes["StartDuration"]].Text.Trim(),
                                        EndDuration = worksheet.Cells[row, columnIndexes["EndDuration"]]?.Text.Trim(),
                                        Reason = worksheet.Cells[row, columnIndexes["Reason"]].Text.Trim(),
                                        TotalDays = decimal.TryParse(worksheet.Cells[row, columnIndexes["TotalDays"]].Text.Trim(), out decimal prodeScore) ? prodeScore : 0m,
                                        TotalHours = worksheet.Cells[row, columnIndexes["TotalHours"]].Text.Trim(),
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
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 14)
                            {
                                var workFromHomes = new List<WorkFromHome>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"Application Type '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            var staffExist = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                                            if (staffExist == null) throw new MessageNotFoundException("Staff not found");
                                            bool staffExistsInOrg = await _context.StaffCreations.AnyAsync(s => s.Id == staffExist.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff Id '{staffIdText}' not found");
                                            }
                                            staffId = staffExist.Id;
                                        }
                                        else
                                        {
                                            throw new ArgumentException($"Invalid Staff Id format '{staffIdText}'");
                                        }
                                    }
                                    DateTime? fromTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["FromTime"]]?.Text, out var parsedFromTime) ? parsedFromTime : null;
                                    DateTime? toTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["ToTime"]]?.Text, out var parsedToTime) ? parsedToTime : null;
                                    DateOnly? fromDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["FromDate"]]?.Text, out var parsedFromDate) ? parsedFromDate : null;
                                    DateOnly? toDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["ToDate"]]?.Text, out var parsedToDate) ? parsedToDate : null;

/*                                    string? totalHours = null;
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
*/                                  var workFrom = new WorkFromHome
                                    {
                                        ApplicationTypeId = applicationType.Id,
                                        FromTime = fromTime,
                                        ToTime = toTime,
                                        FromDate = fromDate,
                                        ToDate = toDate,
                                        Reason = worksheet.Cells[row, columnIndexes["Reason"]].Text.Trim(),
                                        TotalDays = decimal.TryParse(worksheet.Cells[row, columnIndexes["TotalDays"]].Text.Trim(), out decimal prodeScore) ? prodeScore : 0m,
                                        TotalHours = worksheet.Cells[row, columnIndexes["TotalHours"]].Text.Trim(),
                                        StartDuration = worksheet.Cells[row, columnIndexes["StartDuration"]].Text.Trim(),
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
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 15)
                            {
                                var onDutyOvertimes = new List<OnDutyOvertime>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            var staffExist = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                                            if (staffExist == null) throw new MessageNotFoundException("Staff not found");
                                            bool staffExistsInOrg = await _context.StaffCreations.AnyAsync(s => s.Id == staffExist.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff Id '{staffIdText}' not found");
                                            }
                                            staffId = staffExist.Id;
                                        }
                                        else
                                        {
                                            throw new ArgumentException($"Invalid Staff Id format '{staffIdText}'");
                                        }
                                    }
                                    DateOnly? otDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["OTDate"]].Text, out var parsedOtDate) ? parsedOtDate : null;
                                    if (!otDate.HasValue)
                                    {
                                        throw new ArgumentException($"Invalid OT Date {otDate}");
                                    }
                                    DateTime? startTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["StartTime"]]?.Text, out var parsedStartTime) ? parsedStartTime : null;
                                    DateTime? endTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["EndTime"]]?.Text, out var parsedEndTime) ? parsedEndTime : null;
                                    string otType = worksheet.Cells[row, columnIndexes["OTType"]].Text.Trim();
                                    if (string.IsNullOrEmpty(otType))
                                    {
                                        throw new Exception($"OT Type is required");
                                    }
                                    var onDutyOvertime = new OnDutyOvertime
                                    {
                                        StaffId = staffId ?? throw new Exception($"Staff Id is required"),
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
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 16)
                            {
                                var shiftExtensions = new List<ShiftExtension>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                                    if (string.IsNullOrEmpty(applicationTypeName))
                                    {
                                        throw new Exception($"Application Type is required");
                                    }
                                    var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name.ToLower() == applicationTypeName.ToLower() && a.IsActive);
                                    if (applicationType == null)
                                    {
                                        throw new MessageNotFoundException($"Application Type '{applicationTypeName}' not found");
                                    }
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            var staffExist = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                                            if (staffExist == null) throw new MessageNotFoundException("Staff not found");
                                            bool staffExistsInOrg = await _context.StaffCreations.AnyAsync(s => s.Id == staffExist.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff Id '{staffIdText}' not found");
                                            }
                                            staffId = staffExist.Id;
                                        }
                                        else
                                        {
                                            throw new ArgumentException($"Invalid Staff Id format '{staffIdText}'");
                                        }
                                    }
                                    var shift = worksheet.Cells[row, columnIndexes["Shift"]].Text.Trim();
                                    if (string.IsNullOrEmpty(shift))
                                    {
                                        continue;
                                    }
                                    var shiftId = await _context.Shifts
                                        .Where(d => d.Name.Trim().ToLower() == shift.Trim().ToLower() && d.IsActive)
                                        .Select(d => d.Id)
                                        .FirstOrDefaultAsync();
                                    DateOnly? transactionDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["TransactionDate"]].Text, out var parsedDate) ? parsedDate : null;
                                    if (!transactionDate.HasValue)
                                    {
                                        throw new ArgumentException($"Invalid TransactionDate at row {row}.");
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
                                        Remarks = worksheet.Cells[row, columnIndexes["Remarks"]].Text.Trim(),
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow,
                                        StaffId = staffId,
                                        ShiftId = shiftId
                                    };
                                    shiftExtensions.Add(shiftExtension);
                                }
                                if (shiftExtensions.Count > 0)
                                {
                                    await _context.ShiftExtensions.AddRangeAsync(shiftExtensions);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if (excelImportDto.ExcelImportId == 17)
                            {
                                var staffVaccinations = new List<StaffVaccination>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                                    int? staffId = null;
                                    if (!string.IsNullOrEmpty(staffIdText))
                                    {
                                        var match = System.Text.RegularExpressions.Regex.Match(staffIdText, @"^([A-Za-z]+)(\d+)$");
                                        if (match.Success)
                                        {
                                            var staffExist = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                                            if (staffExist == null) throw new MessageNotFoundException("Staff not found");
                                            bool staffExistsInOrg = await _context.StaffCreations.AnyAsync(s => s.Id == staffExist.Id && s.IsActive == true);
                                            if (!staffExistsInOrg)
                                            {
                                                throw new MessageNotFoundException($"Staff Id '{staffIdText}' not found");
                                            }
                                            staffId = staffExist.Id;
                                        }
                                        else
                                        {
                                            throw new ArgumentException($"Invalid Staff Id format '{staffIdText}'");
                                        }
                                    }
                                    DateOnly? vaccinatedDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["VaccinatedDate"]]?.Text, out var parsedVaccinatedDate) ? parsedVaccinatedDate : null;
                                    if (!vaccinatedDate.HasValue)
                                    {
                                        throw new ArgumentException($"Invalid VaccinatedDate '{vaccinatedDate}'");
                                    }
                                    DateTime? secondVaccinatedDate = DateTime.TryParse(worksheet.Cells[row, columnIndexes["SecondVaccinationDate"]]?.Text, out var parsedSecondVaccinatedDate) ? parsedSecondVaccinatedDate : null;
                                    if (!vaccinatedDate.HasValue)
                                    {
                                        throw new ArgumentException($"Invalid SecondVaccinatedDate '{secondVaccinatedDate}'");
                                    }
                                    if (!int.TryParse(worksheet.Cells[row, columnIndexes["VaccinationNumber"]]?.Text, out int vaccinationNumber))
                                    {
                                        throw new ArgumentException($"Invalid VaccinationNumber '{vaccinationNumber}'");
                                    }
                                    bool isExempted = false;
                                    var isExemptedText = worksheet.Cells[row, columnIndexes["IsExempted"]].Text.Trim().ToLower();
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
                                        throw new ArgumentException($"Invalid IsExempted value '{isExemptedText}'");
                                    }
                                    string? comments = worksheet.Cells[row, columnIndexes["Comments"]]?.Text?.Trim();
                                    var staffVaccination = new StaffVaccination
                                    {
                                        StaffId = staffId ?? throw new Exception($"Staff Id is required"),
                                        VaccinatedDate = vaccinatedDate,
                                        VaccinationNumber = vaccinationNumber,
                                        SecondVaccinationDate = secondVaccinatedDate,
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
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if(excelImportDto.ExcelImportId == 18)
                            {
                                var probations = new List<ProbationReport>();
                                var validDepartmentIds = _context.DepartmentMasters.Where(d => d.IsActive).Select(d => d.Id).ToHashSet();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var departmentName = worksheet.Cells[row, columnIndexes["Department"]].Text.Trim();
                                    if (string.IsNullOrEmpty(departmentName))
                                    {
                                        continue;
                                    }
                                    var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == departmentName.ToLower() && d.IsActive);
                                    if (department == null) throw new MessageNotFoundException($"Department '{departmentName}' not found.");
                                    var addProbation = new ProbationReport
                                    {
                                        EmpId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim(),
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
                                        DepartmentId = department.Id,
                                        ProdScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Prod Score"]].Text.Trim(), out decimal prodeScore) ? prodeScore : 0m,
                                        ProdPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Prod %"]].Text.Trim(), out decimal prodPercentage) ? prodPercentage : 0m,
                                        ProdGrade = worksheet.Cells[row, columnIndexes["Prod Grade"]].Text.Trim(),
                                        QualityScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Quality Score"]].Text.Trim(), out decimal qualityScore) ? qualityScore : 0m,
                                        QualityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Qual %"]].Text.Trim(), out decimal qualityPercentage) ? qualityPercentage : 0m,
                                        NoOfAbsent = decimal.TryParse(worksheet.Cells[row, columnIndexes["No Of Absent"]].Text.Trim(), out decimal noOfAbsent) ? noOfAbsent : 0m,
                                        AttendanceScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Attd Score"]].Text.Trim(), out decimal attendanceScore) ? attendanceScore : 0m,
                                        AttendancePercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Attd %"]].Text.Trim(), out decimal attendancePercentage) ? attendancePercentage : 0m,
                                        AttendanceGrade = worksheet.Cells[row, columnIndexes["Attd Grade"]].Text.Trim(),
                                        FinalTotal = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final Total"]].Text.Trim(), out decimal finalTotal) ? finalTotal : 0m,
                                        TotalScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Total Score"]].Text.Trim(), out decimal totalScore) ? totalScore : 0m,
                                        FinalScorePercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final Score %"]].Text.Trim(), out decimal finalScorePercentage) ? finalScorePercentage : 0m,
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
                                        ProductivityYear = excelImportDto.ProductivityYear ?? 0,
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
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if(excelImportDto.ExcelImportId == 19)
                            {
                                var probations = new List<ProbationTarget>();
                                var validDivisionIds = _context.DivisionMasters.Where(d => d.IsActive).Select(d => d.Id).ToHashSet();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var divisionName = worksheet.Cells[row, columnIndexes["EMP Division"]].Text.Trim();
                                    if (string.IsNullOrEmpty(divisionName))
                                    {
                                        continue;
                                    }
                                    var division = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == divisionName.ToLower() && d.IsActive);
                                    if (division == null) throw new MessageNotFoundException($"Division '{divisionName}' not found.");
                                    var addProbation = new ProbationTarget
                                    {
                                        EmpId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim(),
                                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim(),
                                        DivisionId = division.Id,
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
                                        ProductivityYear = excelImportDto.ProductivityYear ?? 0,
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
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if(excelImportDto.ExcelImportId == 20)
                            {
                                var paySheets = new List<PaySheet>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var employeeName = worksheet.Cells[row, columnIndexes["Employee Name"]].Text.Trim();
                                    var employee = _context.StaffCreations.Where(s => s.IsActive == true)
                                        .AsEnumerable()
                                        .FirstOrDefault(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}" == employeeName);
                                    if (employee == null) throw new MessageNotFoundException($"Staff {employeeName} not found");
                                    var employeeId = employee.StaffId;
                                    var designationName = worksheet.Cells[row, columnIndexes["Designation"]].Text.Trim();
                                    if (string.IsNullOrEmpty(designationName))
                                    {
                                        continue;
                                    }
                                    var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive);
                                    if (designation == null) throw new MessageNotFoundException($"Designation '{designationName}' not found");
                                    var departmentName = worksheet.Cells[row, columnIndexes["Department"]].Text.Trim();
                                    if (string.IsNullOrEmpty(designationName))
                                    {
                                        continue;
                                    }
                                    var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == departmentName.ToLower() && d.IsActive);
                                    if (department == null) throw new MessageNotFoundException($"Department '{departmentName}' not found");
                                    DateOnly dateOfJoining = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["Date of Joining"]].Text, out var parsedDate) ? parsedDate : DateOnly.MinValue;
                                    if (dateOfJoining == DateOnly.MinValue)
                                    {
                                        throw new ArgumentException($"Invalid Date of Joining: '{dateOfJoining}'");
                                    }
                                    DateOnly dateOfBirth = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["Date of Birth"]].Text, out var parsedDate1) ? parsedDate1 : DateOnly.MinValue;
                                    if (dateOfBirth == DateOnly.MinValue)
                                    {
                                        throw new ArgumentException($"Invalid Date of Birth: '{dateOfBirth}'");
                                    }
                                    DateOnly salaryEffectiveFrom = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["Salary Effective From"]].Text, out var parsedDate2) ? parsedDate2 : DateOnly.MinValue;
                                    if (salaryEffectiveFrom == DateOnly.MinValue)
                                    {
                                        throw new ArgumentException($"Invalid Salary Effective From: '{salaryEffectiveFrom}'");
                                    }
                                    var addPaySheet = new PaySheet
                                    {
                                        EmployeeName = employeeName,
                                        StaffId = employeeId,
                                        GroupName = worksheet.Cells[row, columnIndexes["Group Name"]].Text.Trim(),
                                        DisplayNameInReports = worksheet.Cells[row, columnIndexes["Display Name In Reports"]].Text.Trim(),
                                        DateOfJoining = dateOfJoining,
                                        EmployeeNumber = int.TryParse(worksheet.Cells[row, columnIndexes["Employee Number"]].Text, out var postalCode) ? postalCode : 0,
                                        DesignationId = designation.Id,
                                        DepartmentId = department.Id,
                                        Location = worksheet.Cells[row, columnIndexes["Location"]].Text.Trim(),
                                        Gender = worksheet.Cells[row, columnIndexes["Gender"]].Text.Trim(),
                                        DateOfBirth = dateOfBirth,
                                        FatherOrMotherName = worksheet.Cells[row, columnIndexes["Father's/Mother's Name"]].Text.Trim(),
                                        SpouseName = worksheet.Cells[row, columnIndexes["Spouse Name"]]?.Text?.Trim(),
                                        Address = worksheet.Cells[row, columnIndexes["Address"]].Text.Trim(),
                                        Email = worksheet.Cells[row, columnIndexes["Email"]].Text.Trim(),
                                        PhoneNo = long.TryParse(worksheet.Cells[row, columnIndexes["Phone No"]].Text, out var personalPhone) ? personalPhone : 0,
                                        BankName = worksheet.Cells[row, columnIndexes["Bank Name"]].Text.Trim(),
                                        AccountNo = long.TryParse(worksheet.Cells[row, columnIndexes["Account No"]].Text, out var account) ? account : 0,
                                        IfscCode = worksheet.Cells[row, columnIndexes["IFSC Code"]].Text.Trim(),
                                        PfAccountNo = worksheet.Cells[row, columnIndexes["PF Account No"]].Text.Trim(),
                                        Uan = long.TryParse(worksheet.Cells[row, columnIndexes["UAN No"]].Text, out var uan) ? uan : 0,
                                        Pan = worksheet.Cells[row, columnIndexes["PAN No"]].Text.Trim(),
                                        AadhaarNo = long.TryParse(worksheet.Cells[row, columnIndexes["Aadhaar No"]].Text, out var aadhar) ? aadhar : 0,
                                        EsiNo = long.TryParse(worksheet.Cells[row, columnIndexes["ESI No"]].Text, out var esiNo) ? esiNo : 0,
                                        SalaryEffectiveFrom = salaryEffectiveFrom,
                                        BasicActual = decimal.TryParse(worksheet.Cells[row, columnIndexes["Basic_Actual"]].Text.Trim(), out decimal basic) ? basic : 0m,
                                        HraActual = decimal.TryParse(worksheet.Cells[row, columnIndexes["HRA_Actual"]].Text.Trim(), out decimal hra) ? hra : 0m,
                                        ConveActual = decimal.TryParse(worksheet.Cells[row, columnIndexes["CONVE_Actual"]].Text.Trim(), out decimal conv) ? conv : 0m,
                                        MedAllowActual = decimal.TryParse(worksheet.Cells[row, columnIndexes["MED_ALLOW_Actual"]].Text.Trim(), out decimal med) ? med : 0m,
                                        SplAllowActual = decimal.TryParse(worksheet.Cells[row, columnIndexes["SPL_ALLOW_Actual"]].Text.Trim(), out decimal spl) ? spl : 0m,
                                        LopDays = decimal.TryParse(worksheet.Cells[row, columnIndexes["LOP_DAYS"]].Text.Trim(), out decimal lop) ? lop : 0m,
                                        StdDays = decimal.TryParse(worksheet.Cells[row, columnIndexes["STD_DAYS"]].Text.Trim(), out decimal std) ? std : 0m,
                                        WrkDays = decimal.TryParse(worksheet.Cells[row, columnIndexes["WRK_DAYS"]].Text.Trim(), out decimal wrk) ? wrk : 0m,
                                        PfAdmin = worksheet.Cells[row, columnIndexes["PF_ADMIN"]].Text.Trim(),
                                        BasicEarned = decimal.TryParse(worksheet.Cells[row, columnIndexes["BASIC EARNED"]].Text.Trim(), out decimal basicEarned) ? basicEarned : 0m,
                                        BasicArradj = decimal.TryParse(worksheet.Cells[row, columnIndexes["BASIC_ARRADJ"]].Text.Trim(), out decimal basicArradj) ? basicArradj : 0m,
                                        HraEarned = decimal.TryParse(worksheet.Cells[row, columnIndexes["HRA_EARNED"]].Text.Trim(), out decimal hraEarned) ? hraEarned : 0m,
                                        HraArradj = decimal.TryParse(worksheet.Cells[row, columnIndexes["HRA_ARRADJ"]].Text.Trim(), out decimal hraArradj) ? hraArradj : 0m,
                                        ConveEarned = decimal.TryParse(worksheet.Cells[row, columnIndexes["CONVE_EARNED"]].Text.Trim(), out decimal convEarned) ? convEarned : 0m,
                                        ConveArradj = decimal.TryParse(worksheet.Cells[row, columnIndexes["CONVE_ARRADJ"]].Text.Trim(), out decimal convArradj) ? convArradj : 0m,
                                        MedAllowEarned = decimal.TryParse(worksheet.Cells[row, columnIndexes["MED_ALLOW_EARNED"]].Text.Trim(), out decimal medAllowEarned) ? medAllowEarned : 0m,
                                        MedAllowArradj = decimal.TryParse(worksheet.Cells[row, columnIndexes["MED_ALLOW__ARRADJ"]].Text.Trim(), out decimal medAllowArradj) ? medAllowArradj : 0m,
                                        SplAllowEarned = decimal.TryParse(worksheet.Cells[row, columnIndexes["SPL_ALLOW_EARNED"]].Text.Trim(), out decimal splAllowEarned) ? splAllowEarned : 0m,
                                        SplAllowArradj = decimal.TryParse(worksheet.Cells[row, columnIndexes["SPL_ALLOW__ARRADJ"]].Text.Trim(), out decimal splAllowArradj) ? splAllowArradj : 0m,
                                        OtherAll = decimal.TryParse(worksheet.Cells[row, columnIndexes["OTHER_ALL"]].Text.Trim(), out decimal otherAll) ? otherAll : 0m,
                                        GrossEarn = decimal.TryParse(worksheet.Cells[row, columnIndexes["GROSS_EARN"]].Text.Trim(), out decimal grossEarn) ? grossEarn : 0m,
                                        Pf = decimal.TryParse(worksheet.Cells[row, columnIndexes["PF"]].Text.Trim(), out decimal pf) ? pf : 0m,
                                        Esi = decimal.TryParse(worksheet.Cells[row, columnIndexes["ESI"]].Text.Trim(), out decimal esi) ? esi : 0m,
                                        Lwf = decimal.TryParse(worksheet.Cells[row, columnIndexes["LWF"]].Text.Trim(), out decimal lwf) ? lwf : 0m,
                                        Pt = decimal.TryParse(worksheet.Cells[row, columnIndexes["PT"]].Text.Trim(), out decimal pt) ? pt : 0m,
                                        It = decimal.TryParse(worksheet.Cells[row, columnIndexes["IT"]].Text.Trim(), out decimal it) ? it : 0m,
                                        MedClaim = decimal.TryParse(worksheet.Cells[row, columnIndexes["MED_CLAIM"]].Text.Trim(), out decimal medClaim) ? medClaim : 0m,
                                        OtherDed = decimal.TryParse(worksheet.Cells[row, columnIndexes["OTHER_DED"]].Text.Trim(), out decimal otherDed) ? otherDed : 0m,
                                        GrossDed = decimal.TryParse(worksheet.Cells[row, columnIndexes["GROSS_DED"]].Text.Trim(), out decimal grossDed) ? grossDed : 0m,
                                        NetPay = decimal.TryParse(worksheet.Cells[row, columnIndexes["NET_PAY"]].Text.Trim(), out decimal netPay) ? netPay : 0m,
                                        Month = excelImportDto.Month ?? 0,
                                        Year = excelImportDto.ProductivityYear ?? 0,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    paySheets.Add(addPaySheet);
                                }
                                if(paySheets.Count > 0)
                                {
                                    await _context.PaySheets.AddRangeAsync(paySheets);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                            }
                            else if(excelImportDto.ExcelImportId == 21)
                            {
                                var performances = new List<PerformanceReport>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var employeeId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim();
                                    var employeeName = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim();
                                    var employee = _context.StaffCreations.Where(s => s.IsActive == true)
                                        .AsEnumerable()
                                        .FirstOrDefault(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}" == employeeName);
                                    if (employee == null) throw new MessageNotFoundException($"Staff {employeeName} not found");
                                    var designationName = worksheet.Cells[row, columnIndexes["EMP Division"]].Text.Trim();
                                    if (string.IsNullOrEmpty(designationName))
                                    {
                                        continue;
                                    }
                                    var designation = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive);
                                    if (designation == null) throw new MessageNotFoundException($"Division '{designationName}' not found");
                                    var addPerformance = new PerformanceReport
                                    {
                                        EmpId = employeeId,
                                        Name = employeeName,
                                        EmpDivisionId = designation.Id,
                                        ProdPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Prod %"]].Text.Trim(), out decimal basicEarned) ? basicEarned : 0m,
                                        ProdScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Prod Score"]].Text.Trim(), out decimal basicArradj) ? basicArradj : 0m,
                                        ProdGrade = worksheet.Cells[row, columnIndexes["Prod Grade"]].Text.Trim(),
                                        QualityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Qual %"]].Text.Trim(), out decimal hraArradj) ? hraArradj : 0m,
                                        QualityScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Qual Score"]].Text.Trim(), out decimal convEarned) ? convEarned : 0m,
                                        QualityGrade = worksheet.Cells[row, columnIndexes["Qual Grade"]].Text.Trim(),
                                        NoOfAbsents = int.TryParse(worksheet.Cells[row, columnIndexes["No of Abs"]].Text, out var postalCode) ? postalCode : 0,
                                        AttendancePercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Attd %"]].Text.Trim(), out decimal medAllowArradj) ? medAllowArradj : 0m,
                                        AttendanceScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Attd Score"]].Text.Trim(), out decimal splAllowEarned) ? splAllowEarned : 0m,
                                        AttendanceGrade = worksheet.Cells[row, columnIndexes["Attd Grade"]].Text.Trim(),
                                        TotalScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Total Score"]].Text.Trim(), out decimal medAllowArra) ? medAllowArra : 0m,
                                        WorkingMonths = int.TryParse(worksheet.Cells[row, columnIndexes["Working months"]].Text.Trim(), out var postal) ? postal : 0,
                                        Score = decimal.TryParse(worksheet.Cells[row, columnIndexes["Score"]].Text.Trim(), out var score) ? score : 0,
                                        FinalPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final %"]].Text.Trim(), out var final) ? final : 0,
                                        FinalGrade = worksheet.Cells[row, columnIndexes["Final Grade"]].Text.Trim(),
                                        Comments = worksheet.Cells[row, columnIndexes["Comments"]].Text.Trim(),
                                        PerformanceTypeId = excelImportDto.PerformanceTypeId ?? 0,
                                        IsActive = true,
                                        CreatedBy = excelImportDto.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow
                                    };
                                    performances.Add(addPerformance);
                                }
                                if (performances.Count > 0)
                                {
                                    await _context.PerformanceReports.AddRangeAsync(performances);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }

                            }
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            return "Excel data imported successfully.";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}