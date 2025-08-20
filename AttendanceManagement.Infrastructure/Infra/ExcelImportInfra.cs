using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using LicenseContext = OfficeOpenXml.LicenseContext;
namespace AttendanceManagement.Infrastructure.Infra;
public class ExcelImportInfra : IExcelImportInfra
{
    private readonly AttendanceManagementSystemContext _context;
    private readonly string _workspacePath;
    private readonly string _excelWorkspacePath;
    public ExcelImportInfra(AttendanceManagementSystemContext context, IWebHostEnvironment env)
    {
        _context = context;
        _workspacePath = Path.Combine(env.ContentRootPath, "wwwroot\\ExcelTemplates");
        if (!Directory.Exists(_workspacePath))
        {
            Directory.CreateDirectory(_workspacePath);
        }
        _excelWorkspacePath = Path.Combine(env.ContentRootPath, "wwwroot\\UploadedExcel");
        if (!Directory.Exists(_excelWorkspacePath))
        {
            Directory.CreateDirectory(_excelWorkspacePath);
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

    public async Task<string> GetExcelTemplateFilePath(int excelImportId, int performanceId)
    {
        var excelTemplate = await _context.ExcelImports.FirstOrDefaultAsync(x => x.Id == excelImportId && x.IsActive == true);
        if (excelTemplate == null)
        {
            throw new MessageNotFoundException("Excel template not found");
        }
        string fileName;

        if (excelImportId == 21)
        {
            fileName = performanceId switch
            {
                1 => "Monthly Performance.xlsx",
                2 => "Quarterly Performance.xlsx",
                3 => "Yearly Performance.xlsx",
                _ => throw new ArgumentException("Invalid performance type ID")
            };
        }
        else
        {
            fileName = $"{excelTemplate.Name}.xlsx";
        }
        return Path.Combine(_workspacePath, fileName);
    }

    public async Task<ExcelImportResultDto> ImportExcelAsync(ExcelImportDto excelImportDto)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var excelImportType = await _context.ExcelImports.FirstOrDefaultAsync(e => e.Id == excelImportDto.ExcelImportId && e.IsActive);
        if (excelImportType == null) throw new MessageNotFoundException("Excel import type not found");
        var fileExtension = Path.GetExtension(excelImportDto.File.FileName);
        if (!string.Equals(fileExtension, ".xlsx", StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("Please upload the correct Excel template file");
        var staffExists = await _context.StaffCreations.AnyAsync(s => s.Id == excelImportDto.CreatedBy && s.IsActive == true);
        if (!staffExists) throw new MessageNotFoundException($"Staff not found");
        if (!string.Equals(fileExtension, ".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException("Please upload the correct Excel template File. Only .xlsx Excel files are supported");
        }
        string uploadFileName = $"{Path.GetFileNameWithoutExtension(excelImportDto.File.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
        string uploadFilePath = Path.Combine(_excelWorkspacePath, uploadFileName);
        using (var fileStream = new FileStream(uploadFilePath, FileMode.Create))
        {
            await excelImportDto.File.CopyToAsync(fileStream);
        }
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
                        "PolicyGroup", "WorkingDayPattern", "UanNumber", "EsiNumber", "IsMobileAppEligible", "GeoStatus", "MiddleName",
                        "OfficialPhone", "PersonalLocation", "PersonalEmail", "LeaveGroup", "Company", "Location", "HolidayCalendar", "Status",
                        "AadharNo", "PanNo", "PassportNo", "DrivingLicense", "BankName", "BankAccountNo", "BankIfscCode", "BankBranch",
                        "HomeAddress", "FatherName", "EmergencyContactPerson1", "EmergencyContactPerson2", "EmergencyContactNo1", "EmergencyContactNo2", "MotherName",
                        "FatherAadharNo", "MotherAadharNo", "OrganizationType", "WorkingStatus", "ConfirmationDate", "PostalCode", "ApprovalLevel", "OfficialEmail", "IsNonProduction"
                    };
                }
                else if (excelImportDto.ExcelImportId == 2)
                {
                    requiredHeaders = new List<string> { "LeaveType", "StaffId", "TransactionFlag", "Month", "Year", "Remarks", "LeaveCount", "LeaveReason" };
                }
                else if (excelImportDto.ExcelImportId == 3)
                {
                    requiredHeaders = new List<string> { "Name", "ShortName", "Phone", "Fax", "Email", "IsActive" };
                }
                else if (excelImportDto.ExcelImportId == 4 || excelImportDto.ExcelImportId == 5 || excelImportDto.ExcelImportId == 6 || excelImportDto.ExcelImportId == 7)
                {
                    requiredHeaders = new List<string> { "Name", "ShortName", "IsActive" };
                }
                else if (excelImportDto.ExcelImportId == 8)
                {
                    requiredHeaders = new List<string> { "StaffId", "SelectPunch", "InPunch", "OutPunch", "Remarks", "ApplicationType" };
                }
                else if (excelImportDto.ExcelImportId == 9)
                {
                    requiredHeaders = new List<string> { "StartTime", "EndTime", "StaffId", "Remarks", "PermissionDate", "PermissionType", "ApplicationType" };
                }
                else if (excelImportDto.ExcelImportId == 10)
                {
                    requiredHeaders = new List<string> { "StaffId", "ResignationDate", "RelievingDate", "Status" };
                }
                else if (excelImportDto.ExcelImportId == 11)
                {
                    requiredHeaders = new List<string> { "Name", "Short Name", "Start Time", "End Time", "Shift Type", "IsActive" };
                }
                else if (excelImportDto.ExcelImportId == 12)
                {
                    requiredHeaders = new List<string> { "ApplicationType", "FromDate", "ToDate", "Reason", "StaffId", "StartDuration", "EndDuration", "LeaveType", "TotalDays" };
                }
                else if (excelImportDto.ExcelImportId == 13)
                {
                    requiredHeaders = new List<string> { "ApplicationType", "StartTime", "EndTime", "StartDate", "EndDate", "Reason", "StaffId", "StartDuration", "EndDuration", "TotalDays", "TotalHours" };
                }
                else if (excelImportDto.ExcelImportId == 14)
                {
                    requiredHeaders = new List<string> { "ApplicationType", "FromTime", "ToTime", "FromDate", "ToDate", "Reason", "StartDuration", "EndDuration", "StaffId", "TotalDays", "TotalHours" };
                }
                else if (excelImportDto.ExcelImportId == 15)
                {
                    requiredHeaders = new List<string> { "StaffId", "OTDate", "StartTime", "EndTime", "OTType" };
                }
                else if (excelImportDto.ExcelImportId == 16)
                {
                    requiredHeaders = new List<string> { "ApplicationType", "StaffId", "TransactionDate", "BeforeShiftHours", "AfterShiftHours", "Remarks", "DurationHours", "Shift" };
                }
                else if (excelImportDto.ExcelImportId == 17)
                {
                    requiredHeaders = new List<string> { "StaffId", "VaccinatedDate", "VaccinationNumber", "IsExempted", "Comments", "SecondVaccinationDate" };
                }
                else if (excelImportDto.ExcelImportId == 18)
                {
                    requiredHeaders = new List<string>
                    {
                        "Emp ID", "Name", "Department", "Prod Score", "Prod %", "Prod Grade", "Quality Score", "Qual %", "No Of Absent",
                        "Attd Score", "Attd %", "Attd Grade", "Final Total", "Total Score", "Final Score %", "Final Grade", "Production Achieved % Jan",
                        "Production Achieved % Feb", "Production Achieved % Mar", "Production Achieved % Apr", "Production Achieved % May",
                        "Production Achieved % Jun", "Production Achieved % Jul", "Production Achieved % Aug", "Production Achieved % Sep",
                        "Production Achieved % Oct","Production Achieved % Nov", "Production Achieved % Dec", "No Of Months"
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
                else if (excelImportDto.ExcelImportId == 21)
                {
                    if (excelImportDto.PerformanceTypeId == 1)
                    {
                        requiredHeaders = new List<string>
                        {
                            "Employee Code", "Employee Name", "Designation", "Productivity Score", "Quality Score", "Present Score", "Total Score", "Productivity %", "Quality %",
                            "Present %", "Final %", "Grade", "Total Absents", "Reporting Head", "HR Comments"
                        };
                    }
                    else if (excelImportDto.PerformanceTypeId == 2)
                    {
                        requiredHeaders = new List<string>
                        {
                            "Employee Code", "Employee Name", "Designation", "Productivity %", "Quality %", "Present %", "Final %", "Grade", "Absent Days", "HR Comments"
                        };
                    }
                    else if (excelImportDto.PerformanceTypeId == 3)
                    {
                        requiredHeaders = new List<string>
                        {
                            "Employee Code", "Employee Name", "Designation", "Productivity %", "Quality %", "Present %", "Final %", "Grade", "Absent Days", "HR Comments"
                        };
                    }
                }
                else if (excelImportDto.ExcelImportId == 23)
                {
                    requiredHeaders = new List<string>
                    {
                        "EmployeeCode", "EmployeeName", "Designation", "RevisedDesignation", "Department", "AppraisalYear", "BasicCurrentPerAnnum", "BasicCurrentPerMonth", "BasicCurrentPerAnnumAfterApp",
                        "BasicCurrentPerMonthAfterApp", "HRAPerAnnum", "HRAPerMonth", "HRAPerAnnumAfterApp", "HRAPerMonthAfterApp", "ConveyancePerAnnum", "ConveyancePerMonth",
                        "ConveyancePerAnnumAfterApp", "ConveyancePerMonthAfterApp", "MedicalAllowancePerAnnum", "MedicalAllowancePerMonth", "MedicalAllowancePerAnnumAfterApp",
                        "MedicalAllowancePerMonthAfterApp", "SpecialAllowancePerAnnum", "SpecialAllowancePerMonth", "SpecialAllowancePerAnnumAfterApp", "SpecialAllowancePerMonthAfterApp",
                        "EmployerPFContributionPerAnnum", "EmployerPFContributionPerMonth", "EmployerPFContributionPerAnnumAfterApp", "EmployerPFContributionPerMonthAfterApp",
                        "EmployerESIContributionPerAnnum", "EmployerESIContributionPerMonth", "EmployerESIContributionPerAnnumAfterApp", "EmployerESIContributionPerMonthAfterApp",
                        "GroupPersonalAccidentPerAnnum", "GroupPersonalAccidentPerMonth", "GroupPersonalAccidentPerAnnumAfterApp", "GroupPersonalAccidentPerMonthAfterApp",
                        "EmployeePFContributionPerAnnum", "EmployeePFContributionPerMonth", "EmployeePFContributionPerAnnumAfterApp", "EmployeePFContributionPerMonthAfterApp",
                        "EmployeeESIContributionPerAnnum", "EmployeeESIContributionPerMonth", "EmployeeESIContributionPerAnnumAfterApp", "EmployeeESIContributionPerMonthAfterApp",
                        "ProfessionalTaxPerAnnum", "ProfessionalTaxPerMonth", "ProfessionalTaxPerAnnumAfterApp", "ProfessionalTaxPerMonthAfterApp", "EmployeeSalutation", "TotalAppraisal",
                        "GMCPerAnnum", "GMCPerMonth", "GMCPerAnnumAfterApp", "GMCPerMonthAfterApp", "EmployerGMCPerAnnum", "EmployerGMCPerMonth", "EmployerGMCPerAnnumAfterApp",
                        "EmployerGMCPerMonthAfterApp"
                    };
                }
                else if (excelImportDto.ExcelImportId == 24)
                {
                    requiredHeaders = new List<string>
                    {
                        "Date", "StaffId", "ShiftName"
                    };
                }
                else
                {
                    throw new MessageNotFoundException("Excel import type not found");
                }
                var missingHeaders = requiredHeaders.Where(header => !headerRow.Contains(header)).ToList();
                if (missingHeaders.Count == requiredHeaders.Count)
                {
                    throw new ArgumentException("Invalid Excel file");
                }
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
                var totalRecords = rowCount - 1;

                if (totalRecords <= 0)
                {
                    return new ExcelImportResultDto
                    {
                        TotalRecords = 0,
                        SuccessCount = 0,
                        ErrorCount = 0,
                        ErrorMessages = new List<string> { "File is empty" }
                    };
                }

                var result = new ExcelImportResultDto
                {
                    TotalRecords = totalRecords,
                    SuccessCount = 0,
                    ErrorCount = 0,
                    ErrorMessages = new List<string>()
                };
                if (excelImportDto.ExcelImportId == 1)
                {
                    var staffCreations = new List<StaffCreation>();
                    var errorLogs = new List<string>();
                    var validDepartmentIds = _context.DepartmentMasters.Where(d => d.IsActive).Select(d => d.Id).ToHashSet();
                    var staffIdsInExcel = new HashSet<string>();
                    var existingStaffIds = await _context.StaffCreations.Where(s => s.IsActive == true).Select(s => s.StaffId).ToHashSetAsync();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var staffId = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (existingStaffIds.Contains(staffId))
                        {
                            errorLogs.Add($"StaffId '{staffId}' already exists at row {row}");
                            continue;
                        }
                        if (!staffIdsInExcel.Add(staffId))
                        {
                            errorLogs.Add($"Duplicate StaffId '{staffId}' found in Excel at row {row}");
                            continue;
                        }
                        var branchName = worksheet.Cells[row, columnIndexes["Branch"]].Text.Trim();
                        if (string.IsNullOrEmpty(branchName))
                        {
                            errorLogs.Add($"Branch is empty at {row}");
                            continue;
                        }
                        var branch = await _context.BranchMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == branchName.ToLower() && d.IsActive);
                        if (branch == null)
                        {
                            errorLogs.Add($"Branch '{branchName}' not found at {row}");
                            continue;
                        }
                        var approvalLevel1 = worksheet.Cells[row, columnIndexes["ApprovalLevel1"]].Text.Trim();
                        if (string.IsNullOrEmpty(approvalLevel1))
                        {
                            errorLogs.Add($"Approval Level1 is empty at {row}");
                            continue;
                        }
                        var approval1 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == approvalLevel1 && s.IsActive == true);
                        if (approval1 == null)
                        {
                            errorLogs.Add($"Approval Level1 '{approvalLevel1}' not found at {row}");
                            continue;
                        }
                        var approvalLevel2 = worksheet.Cells[row, columnIndexes["ApprovalLevel2"]]?.Text.Trim();
                        StaffCreation? approval2 = null;
                        if (!string.IsNullOrWhiteSpace(approvalLevel2))
                        {
                            approval2 = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == approvalLevel2 && s.IsActive == true);
                            if (approval2 == null)
                            {
                                errorLogs.Add($"Approval Level2 '{approvalLevel2}' not found at {row}");
                                continue;
                            }
                        }
                        var departmentName = worksheet.Cells[row, columnIndexes["Department"]].Text.Trim();
                        if (string.IsNullOrEmpty(departmentName))
                        {
                            errorLogs.Add($"Department is empty at {row}");
                            continue;
                        }
                        var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == departmentName.ToLower() && d.IsActive);
                        if (department == null)
                        {
                            errorLogs.Add($"Department '{departmentName}' not found at row {row}");
                            continue;
                        }
                        var statusName = worksheet.Cells[row, columnIndexes["Status"]].Text.Trim();
                        if (string.IsNullOrEmpty(statusName))
                        {
                            errorLogs.Add($"Status is empty at {row}");
                            continue;
                        }
                        var status = await _context.Statuses.FirstOrDefaultAsync(d => d.Name.ToLower() == statusName.ToLower() && d.IsActive);
                        if (status == null)
                        {
                            errorLogs.Add($"Status '{statusName}' not found at row {row}");
                            continue;
                        }
                        var divisionName = worksheet.Cells[row, columnIndexes["Division"]].Text.Trim();
                        if (string.IsNullOrEmpty(divisionName))
                        {
                            errorLogs.Add($"Division is empty at {row}");
                            continue;
                        }
                        var division = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == divisionName.ToLower() && d.IsActive);
                        if (division == null)
                        {
                            errorLogs.Add($"Division '{divisionName}' not found at row {row}");
                            continue;
                        }
                        var designationName = worksheet.Cells[row, columnIndexes["Designation"]].Text.Trim();
                        if (string.IsNullOrEmpty(designationName))
                        {
                            errorLogs.Add($"Designation is empty at {row}");
                            continue;
                        }
                        var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive);
                        if (designation == null)
                        {
                            errorLogs.Add($"Designation '{designationName}' not found at row {row}");
                            continue;
                        }
                        var gradeName = worksheet.Cells[row, columnIndexes["Grade"]]?.Text.Trim();
                        GradeMaster? grade = null;
                        if (!string.IsNullOrWhiteSpace(gradeName))
                        {
                            grade = await _context.GradeMasters.FirstOrDefaultAsync(g => g.Name.ToLower() == gradeName.ToLower() && g.IsActive);
                            if (grade == null)
                            {
                                errorLogs.Add($"Grade '{gradeName}' not found at row {row}");
                                continue;
                            }
                        }
                        var organizationTypeName = worksheet.Cells[row, columnIndexes["OrganizationType"]].Text.Trim();
                        if (string.IsNullOrEmpty(organizationTypeName))
                        {
                            errorLogs.Add($"Organization type is empty at {row}");
                            continue;
                        }
                        var organizationType = await _context.OrganizationTypes.FirstOrDefaultAsync(g => g.Name.ToLower() == organizationTypeName.ToLower() && g.IsActive);
                        if (organizationType == null)
                        {
                            errorLogs.Add($"Organization type '{organizationTypeName}' not found at row {row}");
                            continue;
                        }
                        var categoryName = worksheet.Cells[row, columnIndexes["Category"]].Text.Trim();
                        if (string.IsNullOrEmpty(categoryName))
                        {
                            errorLogs.Add($"Category is empty at {row}");
                            continue;
                        }
                        var category = await _context.CategoryMasters.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower() && c.IsActive);
                        if (category == null)
                        {
                            errorLogs.Add($"Category '{categoryName}' not found at row {row}");
                            continue;
                        }
                        var costCenterName = worksheet.Cells[row, columnIndexes["CostCenter"]]?.Text.Trim();
                        CostCentreMaster? costCenter = null;
                        if (!string.IsNullOrWhiteSpace(costCenterName))
                        {
                            costCenter = await _context.CostCentreMasters.FirstOrDefaultAsync(c => c.Name.ToLower() == costCenterName.ToLower() && c.IsActive);
                            if (costCenter == null)
                            {
                                errorLogs.Add($"Cost Center '{costCenterName}' not found at row {row}");
                                continue;
                            }
                        }
                        var workStationName = worksheet.Cells[row, columnIndexes["WorkStation"]].Text.Trim();
                        if (string.IsNullOrEmpty(workStationName))
                        {
                            errorLogs.Add($"Workstation is empty at {row}");
                            continue;
                        }
                        var workStation = await _context.WorkstationMasters.FirstOrDefaultAsync(w => w.Name.ToLower() == workStationName.ToLower() && w.IsActive);
                        if (workStation == null)
                        {
                            errorLogs.Add($"Workstation '{workStationName}' not found at row {row}");
                            continue;
                        }
                        var leaveGroupName = worksheet.Cells[row, columnIndexes["LeaveGroup"]].Text.Trim();
                        if (string.IsNullOrEmpty(leaveGroupName))
                        {
                            errorLogs.Add($"Leave group is empty at {row}");
                            continue;
                        }
                        var leaveGroup = await _context.LeaveGroups.FirstOrDefaultAsync(l => l.Name.ToLower() == leaveGroupName.ToLower() && l.IsActive);
                        if (leaveGroup == null)
                        {
                            errorLogs.Add($"Leave Group '{leaveGroupName}' not found at row {row}");
                            continue;
                        }
                        var companyName = worksheet.Cells[row, columnIndexes["Company"]].Text.Trim();
                        if (string.IsNullOrEmpty(companyName))
                        {
                            errorLogs.Add($"Company is empty at {row}");
                            continue;
                        }
                        var companyMaster = await _context.CompanyMasters.FirstOrDefaultAsync(c => c.Name.ToLower() == companyName.ToLower() && c.IsActive);
                        if (companyMaster == null)
                        {
                            errorLogs.Add($"Company '{companyName}' not found at row {row}");
                            continue;
                        }
                        var locationName = worksheet.Cells[row, columnIndexes["Location"]].Text.Trim();
                        if (string.IsNullOrEmpty(locationName))
                        {
                            errorLogs.Add($"Location is empty at {row}");
                            continue;
                        }
                        var locationMaster = await _context.LocationMasters.FirstOrDefaultAsync(l => l.Name.ToLower() == locationName.ToLower() && l.IsActive);
                        if (locationMaster == null)
                        {
                            errorLogs.Add($"Location '{locationName}' not found at row {row}");
                            continue;
                        }
                        var holidayCalendarName = worksheet.Cells[row, columnIndexes["HolidayCalendar"]].Text.Trim();
                        if (string.IsNullOrEmpty(holidayCalendarName))
                        {
                            errorLogs.Add($"Holiday Calander is empty at {row}");
                            continue;
                        }
                        var holidayCalendar = await _context.HolidayCalendarConfigurations.FirstOrDefaultAsync(h => h.Name.ToLower() == holidayCalendarName.ToLower() && h.IsActive);
                        if (holidayCalendar == null)
                        {
                            errorLogs.Add($"Holiday Calendar '{holidayCalendarName}' not found at row {row}");
                            continue;
                        }
                        var staffCreation = new StaffCreation
                        {
                            CardCode = worksheet.Cells[row, columnIndexes["CardCode"]].Text.Trim(),
                            StaffId = staffId,
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
                            GradeId = grade?.Id,
                            CategoryId = category.Id,
                            CostCenterId = costCenter?.Id,
                            WorkStationId = workStation.Id,
                            City = worksheet.Cells[row, columnIndexes["City"]].Text.Trim(),
                            District = worksheet.Cells[row, columnIndexes["District"]].Text.Trim(),
                            State = worksheet.Cells[row, columnIndexes["State"]].Text.Trim(),
                            Country = worksheet.Cells[row, columnIndexes["Country"]].Text.Trim(),
                            OtEligible = bool.TryParse(worksheet.Cells[row, columnIndexes["OtEligible"]].Text, out var otEligible) ? otEligible : false,
                            ApprovalLevel1 = approval1.Id,
                            ApprovalLevel2 = approval2?.Id,
                            AccessLevel = worksheet.Cells[row, columnIndexes["AccessLevel"]].Text.Trim(),
                            PolicyGroup = worksheet.Cells[row, columnIndexes["PolicyGroup"]].Text.Trim(),
                            WorkingDayPattern = worksheet.Cells[row, columnIndexes["WorkingDayPattern"]].Text.Trim(),
                            UanNumber = worksheet.Cells[row, columnIndexes["UanNumber"]]?.Text.Trim(),
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
                            IsNonProduction = bool.TryParse(worksheet.Cells[row, columnIndexes["IsNonProduction"]].Text?.Trim(), out bool isNonProduction),
                            CreatedBy = excelImportDto.CreatedBy,
                            IsActive = true,
                            CreatedUtc = DateTime.UtcNow
                        };
                        staffCreations.Add(staffCreation);
                    }
                    if (staffCreations.Any())
                    {
                        await _context.StaffCreations.AddRangeAsync(staffCreations);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = staffCreations.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 2)
                {
                    var individualLeaveCreditDebits = new List<IndividualLeaveCreditDebit>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var leaveTypeName = worksheet.Cells[row, columnIndexes["LeaveType"]].Text.Trim();
                        if (string.IsNullOrEmpty(leaveTypeName))
                        {
                            errorLogs.Add($"Leave type is empty at {row}");
                            continue;
                        }
                        var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(l => l.Name.ToLower() == leaveTypeName.ToLower() && l.IsActive);
                        if (leaveType == null)
                        {
                            errorLogs.Add($"Leave type '{leaveTypeName}' not found at row {row}");
                            continue;
                        }
                        var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffCreationIdStr))
                        {
                            errorLogs.Add($"Staff is empty at {row}");
                            continue;
                        }
                        var staffCreation = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffCreationIdStr && s.IsActive == true);
                        if (staffCreation == null)
                        {
                            errorLogs.Add($"Staff '{staffCreationIdStr}' not found at row {row}");
                            continue;
                        }
                        var value = worksheet.Cells[row, columnIndexes["TransactionFlag"]].Text?.Trim().ToLower();
                        bool transactionFlag;
                        if (value == "credit")
                        {
                            transactionFlag = true;
                        }
                        else if (value == "debit")
                        {
                            transactionFlag = false;
                        }
                        else
                        {
                            errorLogs.Add($"Invalid transaction flag at row {row}");
                            continue;
                        }
                        var leaveCount = decimal.TryParse(worksheet.Cells[row, columnIndexes["LeaveCount"]].Text, out var parsedLeaveCount) ? parsedLeaveCount : 0;
                        if (leaveCount <= 0)
                        {
                            errorLogs.Add($"Invalid leave count '{leaveCount}' at row {row}");
                            continue;
                        }
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
                            if (availableBalance < leaveCount)
                            {
                                errorLogs.Add($"Insufficient leave balance for staff '{staffCreationIdStr}' at row {row}");
                                continue;
                            }
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
                    if (individualLeaveCreditDebits.Any())
                    {
                        await _context.IndividualLeaveCreditDebits.AddRangeAsync(individualLeaveCreditDebits);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = individualLeaveCreditDebits.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 3)
                {
                    var departmentMasters = new List<DepartmentMaster>();
                    var errorLogs = new List<string>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var departmentName = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim();
                        if (string.IsNullOrEmpty(departmentName))
                        {
                            errorLogs.Add($"Row {row}: Department name is required.");
                            continue;
                        }
                        if (departmentName.Length > 50)
                        {
                            errorLogs.Add($"Row {row}: Department name is too long (max 100 chars).");
                            continue;
                        }
                        if (_context.DepartmentMasters.Any(d => d.Name == departmentName))
                        {
                            errorLogs.Add($"Row {row}: Department name '{departmentName}' already exists.");
                            continue;
                        }

                        var isActiveText = worksheet.Cells[row, columnIndexes["IsActive"]].Text.Trim();
                        if (string.IsNullOrEmpty(isActiveText))
                        {
                            errorLogs.Add($"Row {row}: IsActive is required.");
                            continue;
                        }
                        if (!bool.TryParse(isActiveText, out var isActive))
                        {
                            errorLogs.Add($"Row {row}: Invalid IsActive value '{isActiveText}'. Must be true/false.");
                            continue;
                        }

                        var shortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim();
                        if (shortName.Length > 50)
                        {
                            errorLogs.Add($"Row {row}: ShortName is too long (max 50 chars).");
                            continue;
                        }

                        var phoneText = worksheet.Cells[row, columnIndexes["Phone"]].Text.Trim();
                        long phone = 0;
                        if (!string.IsNullOrEmpty(phoneText) && !long.TryParse(phoneText, out phone))
                        {
                            errorLogs.Add($"Row {row}: Phone '{phoneText}' is not a valid number.");
                            continue;
                        }

                        var email = worksheet.Cells[row, columnIndexes["Email"]].Text.Trim();
                        if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                        {
                            errorLogs.Add($"Row {row}: Invalid email format '{email}'.");
                            continue;
                        }

                        if (_context.DepartmentMasters.Any(d => d.Email == email))
                        {
                            errorLogs.Add($"Row {row}: Department name '{departmentName}' already exists.");
                            continue;
                        }

                        var departmentMaster = new DepartmentMaster
                        {
                            Name = departmentName,
                            ShortName = shortName,
                            Phone = phone,
                            Fax = worksheet.Cells[row, columnIndexes["Fax"]].Text.Trim(),
                            Email = email,
                            IsActive = isActive,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };

                        departmentMasters.Add(departmentMaster);
                    }

                    if (departmentMasters.Any())
                    {
                        await _context.DepartmentMasters.AddRangeAsync(departmentMasters);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = departmentMasters.Count;
                    }

                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 4)
                {
                    var designationMasters = new List<DesignationMaster>();
                    var errorLogs = new List<string>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var designationName = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim();
                        if (string.IsNullOrEmpty(designationName))
                        {
                            errorLogs.Add($"Row {row}: Designation name is required.");
                            continue;
                        }
                        if (designationName.Length > 50)
                        {
                            errorLogs.Add($"Row {row}: Designation name is too long (max 50 chars).");
                            continue;
                        }

                        if (await _context.DesignationMasters.AnyAsync(d => d.Name.ToLower() == designationName.ToLower()))
                        {
                            errorLogs.Add($"Row {row}: Designation '{designationName}' already exists.");
                            continue;
                        }

                        var isActiveText = worksheet.Cells[row, columnIndexes["IsActive"]].Text.Trim();
                        if (string.IsNullOrEmpty(isActiveText))
                        {
                            errorLogs.Add($"Row {row}: IsActive is required.");
                            continue;
                        }
                        if (!bool.TryParse(isActiveText, out var isActive))
                        {
                            errorLogs.Add($"Row {row}: Invalid IsActive value '{isActiveText}'. Must be true/false.");
                            continue;
                        }

                        var shortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim();
                        if (shortName.Length > 50)
                        {
                            errorLogs.Add($"Row {row}: ShortName is too long (max 50 chars).");
                            continue;
                        }

                        var designationMaster = new DesignationMaster
                        {
                            Name = designationName,
                            ShortName = shortName,
                            IsActive = isActive,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };

                        designationMasters.Add(designationMaster);
                    }

                    if (designationMasters.Any())
                    {
                        await _context.DesignationMasters.AddRangeAsync(designationMasters);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = designationMasters.Count;
                    }

                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 5)
                {
                    var divisionMasters = new List<DivisionMaster>();
                    var errorLogs = new List<string>();
                    var excelDivisionNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var divisionName = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim();
                        if (string.IsNullOrEmpty(divisionName))
                        {
                            errorLogs.Add($"Row {row}: Division name is required.");
                            continue;
                        }
                        if (divisionName.Length > 50)
                        {
                            errorLogs.Add($"Row {row}: Division name too long (max 50 chars).");
                            continue;
                        }

                        if (!excelDivisionNames.Add(divisionName))
                        {
                            errorLogs.Add($"Row {row}: Duplicate division '{divisionName}' found in Excel.");
                            continue;
                        }

                        if (await _context.DivisionMasters.AnyAsync(d => d.Name.ToLower() == divisionName.ToLower()))
                        {
                            errorLogs.Add($"Row {row}: Division '{divisionName}' already exists");
                            continue;
                        }

                        var divisionShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim();
                        if (string.IsNullOrEmpty(divisionShortName))
                        {
                            errorLogs.Add($"Row {row}: Division short name is required.");
                            continue;
                        }
                        if (divisionShortName.Length > 20)
                        {
                            errorLogs.Add($"Row {row}: Short name too long (max 20 chars).");
                            continue;
                        }

                        var isActiveText = worksheet.Cells[row, columnIndexes["IsActive"]].Text.Trim();
                        if (string.IsNullOrEmpty(isActiveText))
                        {
                            errorLogs.Add($"Row {row}: IsActive is required.");
                            continue;
                        }
                        if (!bool.TryParse(isActiveText, out var isActive))
                        {
                            errorLogs.Add($"Row {row}: Invalid IsActive value '{isActiveText}'. Must be true/false.");
                            continue;
                        }

                        var divisionMaster = new DivisionMaster
                        {
                            Name = divisionName,
                            ShortName = divisionShortName,
                            IsActive = isActive,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };

                        divisionMasters.Add(divisionMaster);
                    }

                    if (divisionMasters.Any())
                    {
                        await _context.DivisionMasters.AddRangeAsync(divisionMasters);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = divisionMasters.Count;
                    }

                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 6)
                {
                    var costCentreMasters = new List<CostCentreMaster>();
                    var errorLogs = new List<string>();
                    var excelCostCentreNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var costcentreName = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim();
                        if (string.IsNullOrEmpty(costcentreName))
                        {
                            errorLogs.Add($"Row {row}: Cost centre name is required.");
                            continue;
                        }
                        if (costcentreName.Length > 50)
                        {
                            errorLogs.Add($"Row {row}: Cost centre name too long (max 50 chars).");
                            continue;
                        }

                        if (!excelCostCentreNames.Add(costcentreName))
                        {
                            errorLogs.Add($"Row {row}: Duplicate cost centre '{costcentreName}' found in Excel.");
                            continue;
                        }

                        if (await _context.CostCentreMasters.AnyAsync(c => c.Name.ToLower() == costcentreName.ToLower()))
                        {
                            errorLogs.Add($"Row {row}: Cost centre '{costcentreName}' already exists");
                            continue;
                        }

                        var costcentreShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim();
                        if (string.IsNullOrEmpty(costcentreShortName))
                        {
                            errorLogs.Add($"Row {row}: Cost centre short name is required.");
                            continue;
                        }
                        if (costcentreShortName.Length > 5)
                        {
                            errorLogs.Add($"Row {row}: Short name too long (max 5 chars).");
                            continue;
                        }

                        var isActiveText = worksheet.Cells[row, columnIndexes["IsActive"]].Text.Trim();
                        if (string.IsNullOrEmpty(isActiveText))
                        {
                            errorLogs.Add($"Row {row}: IsActive is required.");
                            continue;
                        }
                        if (!bool.TryParse(isActiveText, out var isActive))
                        {
                            errorLogs.Add($"Row {row}: Invalid IsActive value '{isActiveText}'. Must be true/false.");
                            continue;
                        }

                        var costCentreMaster = new CostCentreMaster
                        {
                            Name = costcentreName,
                            ShortName = costcentreShortName,
                            IsActive = isActive,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };

                        costCentreMasters.Add(costCentreMaster);
                    }

                    if (costCentreMasters.Any())
                    {
                        await _context.CostCentreMasters.AddRangeAsync(costCentreMasters);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = costCentreMasters.Count;
                    }

                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 7)
                {
                    var volumes = new List<Volume>();
                    var errorLogs = new List<string>();
                    var excelVolumeNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var volumeName = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim();
                        if (string.IsNullOrEmpty(volumeName))
                        {
                            errorLogs.Add($"Row {row}: Volume name is required.");
                            continue;
                        }
                        if (volumeName.Length > 100)
                        {
                            errorLogs.Add($"Row {row}: Volume name too long (max 100 chars).");
                            continue;
                        }

                        if (!excelVolumeNames.Add(volumeName))
                        {
                            errorLogs.Add($"Row {row}: Duplicate volume '{volumeName}' found in Excel.");
                            continue;
                        }

                        if (await _context.Volumes.AnyAsync(v => v.Name.ToLower() == volumeName.ToLower()))
                        {
                            errorLogs.Add($"Row {row}: Volume '{volumeName}' already exists");
                            continue;
                        }

                        var volumeShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim();
                        if (string.IsNullOrEmpty(volumeShortName))
                        {
                            errorLogs.Add($"Row {row}: Volume short name is required.");
                            continue;
                        }
                        if (volumeShortName.Length > 20)
                        {
                            errorLogs.Add($"Row {row}: Volume short name too long (max 20 chars).");
                            continue;
                        }

                        var isActiveText = worksheet.Cells[row, columnIndexes["IsActive"]].Text.Trim();
                        if (string.IsNullOrEmpty(isActiveText))
                        {
                            errorLogs.Add($"Row {row}: IsActive is required.");
                            continue;
                        }
                        if (!bool.TryParse(isActiveText, out var isActive))
                        {
                            errorLogs.Add($"Row {row}: Invalid IsActive value '{isActiveText}'. Must be true/false.");
                            continue;
                        }

                        var volume = new Volume
                        {
                            Name = volumeName,
                            ShortName = volumeShortName,
                            IsActive = isActive,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };

                        volumes.Add(volume);
                    }

                    if (volumes.Any())
                    {
                        await _context.Volumes.AddRangeAsync(volumes);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = volumes.Count;
                    }

                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 8)
                {
                    var manualPunchRequisitions = new List<ManualPunchRequistion>();
                    var errorLogs = new List<string>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                        if (string.IsNullOrEmpty(applicationTypeName))
                        {
                            errorLogs.Add($"Row {row}: Application Type is required.");
                            continue;
                        }
                        var applicationType = await _context.ApplicationTypes
                            .FirstOrDefaultAsync(a => a.Name.ToLower() == applicationTypeName.ToLower() && a.IsActive);
                        if (applicationType == null)
                        {
                            errorLogs.Add($"Row {row}: Application Type '{applicationTypeName}' not found or inactive.");
                            continue;
                        }

                        var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffIdText))
                        {
                            errorLogs.Add($"Row {row}: Staff ID is required.");
                            continue;
                        }
                        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                        if (staff == null)
                        {
                            errorLogs.Add($"Row {row}: Staff '{staffIdText}' not found or inactive.");
                            continue;
                        }

                        var selectedPunch = worksheet.Cells[row, columnIndexes["SelectPunch"]].Text.Trim();
                        if (string.IsNullOrEmpty(selectedPunch))
                        {
                            errorLogs.Add($"Row {row}: SelectPunch is required.");
                            continue;
                        }
                        var validPunches = new[] { "in", "out", "in & out" };
                        if (!validPunches.Contains(selectedPunch.ToLower()))
                        {
                            errorLogs.Add($"Row {row}: Invalid SelectPunch '{selectedPunch}'. Allowed values: In, Out, In & Out.");
                            continue;
                        }

                        DateTime? inPunch = DateTime.TryParse(worksheet.Cells[row, columnIndexes["InPunch"]]?.Text, out var parsedInPunch) ? parsedInPunch : null;
                        DateTime? outPunch = DateTime.TryParse(worksheet.Cells[row, columnIndexes["OutPunch"]]?.Text, out var parsedOutPunch) ? parsedOutPunch : null;

                        if (selectedPunch.Equals("In", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!inPunch.HasValue)
                            {
                                errorLogs.Add($"Row {row}: InPunch is required when SelectPunch = 'In'.");
                                continue;
                            }
                        }
                        else if (selectedPunch.Equals("Out", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!outPunch.HasValue)
                            {
                                errorLogs.Add($"Row {row}: OutPunch is required when SelectPunch = 'Out'.");
                                continue;
                            }
                        }
                        else if (selectedPunch.Equals("In & Out", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!inPunch.HasValue || !outPunch.HasValue)
                            {
                                errorLogs.Add($"Row {row}: Both InPunch and OutPunch are required when SelectPunch = 'In & Out'.");
                                continue;
                            }
                            if (inPunch >= outPunch)
                            {
                                errorLogs.Add($"Row {row}: InPunch must be earlier than OutPunch.");
                                continue;
                            }
                        }

                        var remarks = worksheet.Cells[row, columnIndexes["Remarks"]].Text.Trim();
                        if (!string.IsNullOrEmpty(remarks) && remarks.Length > 255)
                        {
                            errorLogs.Add($"Row {row}: Remarks too long (max 255 characters).");
                            continue;
                        }

                        var manualPunchRequisition = new ManualPunchRequistion
                        {
                            StaffId = staff.Id,
                            SelectPunch = selectedPunch,
                            InPunch = inPunch,
                            OutPunch = outPunch,
                            Remarks = remarks,
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow,
                            ApplicationTypeId = applicationType.Id
                        };

                        manualPunchRequisitions.Add(manualPunchRequisition);
                    }

                    if (manualPunchRequisitions.Any())
                    {
                        await _context.ManualPunchRequistions.AddRangeAsync(manualPunchRequisitions);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = manualPunchRequisitions.Count;
                    }

                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 9)
                {
                    var commonPermissions = new List<CommonPermission>();
                    var errorLogs = new List<string>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                        if (string.IsNullOrEmpty(applicationTypeName))
                        {
                            errorLogs.Add($"Row {row}: Application Type is required.");
                            continue;
                        }
                        var applicationType = await _context.ApplicationTypes
                            .FirstOrDefaultAsync(a => a.Name.ToLower() == applicationTypeName.ToLower() && a.IsActive);
                        if (applicationType == null)
                        {
                            errorLogs.Add($"Row {row}: Application Type '{applicationTypeName}' not found or inactive.");
                            continue;
                        }

                        var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffIdText))
                        {
                            errorLogs.Add($"Row {row}: Staff ID is required.");
                            continue;
                        }
                        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                        if (staff == null)
                        {
                            errorLogs.Add($"Row {row}: Staff '{staffIdText}' not found or inactive.");
                            continue;
                        }

                        var permissionDateText = worksheet.Cells[row, columnIndexes["PermissionDate"]].Text.Trim();
                        if (!DateOnly.TryParse(permissionDateText, out var permissionDate))
                        {
                            errorLogs.Add($"Row {row}: Invalid PermissionDate '{permissionDateText}'.");
                            continue;
                        }

                        var startTimeText = worksheet.Cells[row, columnIndexes["StartTime"]].Text.Trim();
                        var endTimeText = worksheet.Cells[row, columnIndexes["EndTime"]].Text.Trim();

                        if (!TimeOnly.TryParse(startTimeText, out var startTime))
                        {
                            errorLogs.Add($"Row {row}: Invalid StartTime '{startTimeText}'.");
                            continue;
                        }
                        if (!TimeOnly.TryParse(endTimeText, out var endTime))
                        {
                            errorLogs.Add($"Row {row}: Invalid EndTime '{endTimeText}'.");
                            continue;
                        }

                        var newRequestMinutes = (endTime - startTime).TotalMinutes;
                        if (newRequestMinutes <= 0)
                        {
                            errorLogs.Add($"Row {row}: EndTime must be greater than StartTime.");
                            continue;
                        }
                        if (newRequestMinutes > 120)
                        {
                            errorLogs.Add($"Row {row}: Permission duration cannot exceed 2 hours.");
                            continue;
                        }

                        var existingPermissionOnDate = await _context.CommonPermissions
                            .AnyAsync(p => p.StaffId == staff.Id && p.PermissionDate == permissionDate);
                        if (existingPermissionOnDate)
                        {
                            errorLogs.Add($"Row {row}: Permission already exists on {permissionDate:yyyy-MM-dd}.");
                            continue;
                        }

                        var startOfMonth = new DateOnly(permissionDate.Year, permissionDate.Month, 1);
                        var endOfMonth = new DateOnly(permissionDate.Year, permissionDate.Month,
                            DateTime.DaysInMonth(permissionDate.Year, permissionDate.Month));
                        var permissionsThisMonth = await _context.CommonPermissions
                            .Where(p => p.StaffId == staff.Id && p.PermissionDate >= startOfMonth && p.PermissionDate <= endOfMonth)
                            .ToListAsync();

                        var totalMinutesUsed = permissionsThisMonth.Sum(p => TimeSpan.Parse(p.TotalHours).TotalMinutes);
                        if (totalMinutesUsed + newRequestMinutes > 120)
                        {
                            errorLogs.Add($"Row {row}: Monthly limit exceeded. {totalMinutesUsed + newRequestMinutes} minutes > 120 minutes.");
                            continue;
                        }

                        var permissionType = worksheet.Cells[row, columnIndexes["PermissionType"]].Text.Trim();
                        if (string.IsNullOrEmpty(permissionType))
                        {
                            errorLogs.Add($"Row {row}: PermissionType is required.");
                            continue;
                        }

                        var remarks = worksheet.Cells[row, columnIndexes["Remarks"]].Text.Trim();
                        if (!string.IsNullOrEmpty(remarks) && remarks.Length > 255)
                        {
                            errorLogs.Add($"Row {row}: Remarks too long (max 255 chars).");
                            continue;
                        }

                        var totalHours = (endTime - startTime).ToString("hh\\:mm");
                        var commonPermission = new CommonPermission
                        {
                            PermissionType = permissionType,
                            StartTime = startTime,
                            EndTime = endTime,
                            TotalHours = totalHours,
                            StaffId = staff.Id,
                            PermissionDate = permissionDate,
                            ApplicationTypeId = applicationType.Id,
                            Remarks = remarks,
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        commonPermissions.Add(commonPermission);
                    }

                    if (commonPermissions.Any())
                    {
                        await _context.CommonPermissions.AddRangeAsync(commonPermissions);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = commonPermissions.Count;
                    }

                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 10)
                {
                    var staffCreations = new List<StaffCreation>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var staffText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffText))
                        {
                            errorLogs.Add($"Staff is empty at {row}");
                            continue;
                        }
                        var existingStaff = _context.StaffCreations.FirstOrDefault(s => s.StaffId == staffText && s.IsActive == true);
                        if (existingStaff == null)
                        {
                            errorLogs.Add($"Staff '{staffText}' not found at {row}");
                            continue;
                        }
                        var statusName = worksheet.Cells[row, columnIndexes["Status"]].Text.Trim();
                        if (string.IsNullOrEmpty(statusName))
                        {
                            errorLogs.Add($"Status is empty at {row}");
                            continue;
                        }
                        var statusId = await _context.Statuses
                            .Where(d => d.Name.Trim().ToLower() == statusName.Trim().ToLower() && d.IsActive)
                            .Select(d => d.Id)
                            .FirstOrDefaultAsync();
                        if (statusId == 0)
                        {
                            errorLogs.Add($"Status '{statusName}' not found at {row}");
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
                        await _context.StaffCreations.AddRangeAsync(staffCreations);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = staffCreations.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 11)
                {
                    var shiftMasters = new List<Shift>();
                    var errorLogs = new List<string>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var shiftTypeName = worksheet.Cells[row, columnIndexes["Shift Type"]].Text.Trim();
                        if (string.IsNullOrEmpty(shiftTypeName))
                        {
                            errorLogs.Add($"Row {row}: Shift Type is required.");
                            continue;
                        }
                        var shiftType = await _context.ShiftTypeDropDowns
                            .FirstOrDefaultAsync(s => s.Name.ToLower() == shiftTypeName.ToLower() && s.IsActive);
                        if (shiftType == null)
                        {
                            errorLogs.Add($"Row {row}: Shift Type '{shiftTypeName}' not found or inactive.");
                            continue;
                        }

                        var name = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim();
                        if (string.IsNullOrEmpty(name))
                        {
                            errorLogs.Add($"Row {row}: Name is required.");
                            continue;
                        }
                        if (name.Length > 100)
                        {
                            errorLogs.Add($"Row {row}: Name '{name}' exceeds 100 characters.");
                            continue;
                        }
                        var existingName = await _context.Shifts.AnyAsync(s => s.Name.ToLower() == name.ToLower() && s.IsActive);
                        if (existingName)
                        {
                            errorLogs.Add($"Row {row}: Shift name '{name}' already exists.");
                            continue;
                        }

                        var shortName = worksheet.Cells[row, columnIndexes["Short Name"]].Text.Trim();
                        if (string.IsNullOrEmpty(shortName))
                        {
                            errorLogs.Add($"Row {row}: Short Name is required.");
                            continue;
                        }
                        if (shortName.Length > 5)
                        {
                            errorLogs.Add($"Row {row}: Short Name '{shortName}' exceeds 5 characters.");
                            continue;
                        }

                        var startTimeText = worksheet.Cells[row, columnIndexes["Start Time"]].Text.Trim();
                        var endTimeText = worksheet.Cells[row, columnIndexes["End Time"]].Text.Trim();

                        if (!TimeOnly.TryParse(startTimeText, out var startTime))
                        {
                            errorLogs.Add($"Row {row}: Invalid Start Time '{startTimeText}'.");
                            continue;
                        }
                        if (!TimeOnly.TryParse(endTimeText, out var endTime))
                        {
                            errorLogs.Add($"Row {row}: Invalid End Time '{endTimeText}'.");
                            continue;
                        }
                        var isActiveText = worksheet.Cells[row, columnIndexes["IsActive"]].Text.Trim();
                        if (string.IsNullOrEmpty(isActiveText))
                        {
                            errorLogs.Add($"Row {row}: IsActive is required.");
                            continue;
                        }
                        if (!bool.TryParse(isActiveText, out var isActive))
                        {
                            errorLogs.Add($"Row {row}: Invalid IsActive value '{isActiveText}' (must be True/False).");
                            continue;
                        }

                        var shiftMaster = new Shift
                        {
                            Name = name,
                            ShortName = shortName,
                            StartTime = startTime.ToString("HH:mm"),
                            EndTime = endTime.ToString("HH:mm"),
                            ShiftTypeId = shiftType.Id,
                            IsActive = isActive,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        shiftMasters.Add(shiftMaster);
                    }

                    if (shiftMasters.Any())
                    {
                        await _context.Shifts.AddRangeAsync(shiftMasters);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = shiftMasters.Count;
                    }

                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 12)
                {
                    var leaveRequisitions = new List<LeaveRequisition>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                        var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                        if (applicationType == null)
                        {
                            errorLogs.Add($"Application Type '{applicationTypeName}' not found at row {row}");
                            continue;
                        }
                        var leaveTypeName = worksheet.Cells[row, columnIndexes["LeaveType"]].Text.Trim();
                        if (string.IsNullOrEmpty(leaveTypeName))
                        {
                            errorLogs.Add($"Leave type is empty at row {row}");
                            continue;
                        }
                        var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(l => l.Name.ToLower() == leaveTypeName.ToLower() && l.IsActive);
                        if (leaveType == null)
                        {
                            errorLogs.Add($"Leave type '{leaveTypeName}' not found at row {row}");
                            continue;
                        }
                        var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffIdText))
                        {
                            errorLogs.Add($"Staff is empty at row {row}");
                            continue;
                        }
                        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                        if (staff == null)
                        {
                            errorLogs.Add($"Staff '{staffIdText}' not found at row {row}");
                            continue;
                        }
                        bool isFromDateValid = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["FromDate"]].Text, out var fromDate);
                        bool isToDateValid = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["ToDate"]].Text, out var toDate);
                        if (!isFromDateValid)
                        {
                            errorLogs.Add($"Invalid FromDate '{isFromDateValid}' at row {row}");
                            continue;
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
                            StaffId = staff.Id,
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        leaveRequisitions.Add(leaveRequisition);
                    }
                    if (leaveRequisitions.Any())
                    {
                        await _context.LeaveRequisitions.AddRangeAsync(leaveRequisitions);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = leaveRequisitions.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 13)
                {
                    var onDutyRequisitions = new List<OnDutyRequisition>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                        var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                        if (applicationType == null)
                        {
                            errorLogs.Add($"Application Type '{applicationTypeName}' not found at row {row}");
                            continue;
                        }
                        var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffIdText))
                        {
                            errorLogs.Add($"Staff is empty at row {row}");
                            continue;
                        }
                        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                        if (staff == null)
                        {
                            errorLogs.Add($"Staff '{staffIdText}' not found at row {row}");
                            continue;
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
                        */
                        var onDutyRequisition = new OnDutyRequisition
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
                            StaffId = staff.Id,
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        onDutyRequisitions.Add(onDutyRequisition);
                    }
                    if (onDutyRequisitions.Any())
                    {
                        await _context.OnDutyRequisitions.AddRangeAsync(onDutyRequisitions);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = onDutyRequisitions.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 14)
                {
                    var workFromHomes = new List<WorkFromHome>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                        var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name == applicationTypeName && a.IsActive);
                        if (applicationType == null)
                        {
                            errorLogs.Add($"Application Type '{applicationTypeName}' not found at row {row}");
                            continue;
                        }
                        var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffIdText))
                        {
                            errorLogs.Add($"Staff is empty at row {row}");
                            continue;
                        }
                        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                        if (staff == null)
                        {
                            errorLogs.Add($"Staff '{staffIdText}' not found at row {row}");
                            continue;
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
                        */
                        var workFrom = new WorkFromHome
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
                            StaffId = staff.Id,
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        workFromHomes.Add(workFrom);
                    }
                    if (workFromHomes.Any())
                    {
                        await _context.WorkFromHomes.AddRangeAsync(workFromHomes);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = workFromHomes.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 15)
                {
                    var onDutyOvertimes = new List<OnDutyOvertime>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffIdText))
                        {
                            errorLogs.Add($"Staff is empty at row {row}");
                            continue;
                        }
                        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                        if (staff == null)
                        {
                            errorLogs.Add($"Staff '{staffIdText}' not found at row {row}");
                            continue;
                        }
                        DateOnly? otDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["OTDate"]].Text, out var parsedOtDate) ? parsedOtDate : null;
                        if (!otDate.HasValue)
                        {
                            errorLogs.Add($"Invalid OT Date is empty at row {row}");
                            continue;
                        }
                        DateTime? startTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["StartTime"]]?.Text, out var parsedStartTime) ? parsedStartTime : null;
                        DateTime? endTime = DateTime.TryParse(worksheet.Cells[row, columnIndexes["EndTime"]]?.Text, out var parsedEndTime) ? parsedEndTime : null;
                        string otType = worksheet.Cells[row, columnIndexes["OTType"]].Text.Trim();
                        if (string.IsNullOrEmpty(otType))
                        {
                            errorLogs.Add($"OT Type is empty at row {row}");
                            continue;
                        }
                        var onDutyOvertime = new OnDutyOvertime
                        {
                            StaffId = staff.Id,
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
                    if (onDutyOvertimes.Any())
                    {
                        await _context.OnDutyOvertimes.AddRangeAsync(onDutyOvertimes);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = onDutyOvertimes.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 16)
                {
                    var shiftExtensions = new List<ShiftExtension>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var applicationTypeName = worksheet.Cells[row, columnIndexes["ApplicationType"]].Text.Trim();
                        if (string.IsNullOrEmpty(applicationTypeName))
                        {
                            errorLogs.Add($"Application Type is empty at row {row}");
                            continue;
                        }
                        var applicationType = await _context.ApplicationTypes.FirstOrDefaultAsync(a => a.Name.ToLower() == applicationTypeName.ToLower() && a.IsActive);
                        if (applicationType == null)
                        {
                            errorLogs.Add($"Application Type '{applicationTypeName}' not found at row {row}");
                            continue;
                        }
                        var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffIdText))
                        {
                            errorLogs.Add($"Staff is empty at row {row}");
                            continue;
                        }
                        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                        if (staff == null)
                        {
                            errorLogs.Add($"Staff '{staffIdText}' not found at {row}");
                            continue;
                        }
                        var shift = worksheet.Cells[row, columnIndexes["Shift"]].Text.Trim();
                        if (string.IsNullOrEmpty(shift))
                        {
                            errorLogs.Add($"Shift type is empty at row {row}");
                            continue;
                        }
                        var shiftId = await _context.Shifts
                            .Where(d => d.Name.Trim().ToLower() == shift.Trim().ToLower() && d.IsActive)
                            .Select(d => d.Id)
                            .FirstOrDefaultAsync();
                        DateOnly? transactionDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["TransactionDate"]].Text, out var parsedDate) ? parsedDate : null;
                        if (!transactionDate.HasValue)
                        {
                            errorLogs.Add($"Invalid TransactionDate at row {row}");
                            continue;
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
                            StaffId = staff.Id,
                            ShiftId = shiftId
                        };
                        shiftExtensions.Add(shiftExtension);
                    }
                    if (shiftExtensions.Any())
                    {
                        await _context.ShiftExtensions.AddRangeAsync(shiftExtensions);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = shiftExtensions.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 17)
                {
                    var staffVaccinations = new List<StaffVaccination>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var staffIdText = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        if (string.IsNullOrEmpty(staffIdText))
                        {
                            errorLogs.Add($"StaffId is empty at row {row}");
                            continue;
                        }
                        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffIdText && s.IsActive == true);
                        if (staff == null)
                        {
                            errorLogs.Add($"StaffId '{staffIdText}' not found at row {row}");
                            continue;
                        }
                        DateOnly? vaccinatedDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["VaccinatedDate"]]?.Text, out var parsedVaccinatedDate) ? parsedVaccinatedDate : null;
                        if (!vaccinatedDate.HasValue)
                        {
                            errorLogs.Add($"Invalid VaccinatedDate at row {row}");
                            continue;
                        }
                        DateTime? secondVaccinatedDate = DateTime.TryParse(worksheet.Cells[row, columnIndexes["SecondVaccinationDate"]]?.Text, out var parsedSecondVaccinatedDate) ? parsedSecondVaccinatedDate : null;
                        if (!vaccinatedDate.HasValue)
                        {
                            errorLogs.Add($"Invalid SecondVaccinatedDate at row {row}");
                            continue;
                        }
                        if (!int.TryParse(worksheet.Cells[row, columnIndexes["VaccinationNumber"]]?.Text, out int vaccinationNumber))
                        {
                            errorLogs.Add($"Invalid VaccinationNumber '{vaccinationNumber}' at row {row}");
                            continue;
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
                            errorLogs.Add($"Invalid IsExempted value '{isExemptedText}' at row {row}");
                            continue;
                        }
                        string? comments = worksheet.Cells[row, columnIndexes["Comments"]]?.Text?.Trim();
                        var staffVaccination = new StaffVaccination
                        {
                            StaffId = staff.Id,
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
                    if (staffVaccinations.Any())
                    {
                        await _context.StaffVaccinations.AddRangeAsync(staffVaccinations);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = staffVaccinations.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 18)
                {
                    var probations = new List<ProbationReport>();
                    var errorLogs = new List<string>();
                    var validDepartmentIds = _context.DepartmentMasters.Where(d => d.IsActive).Select(d => d.Id).ToHashSet();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var employeeId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim();
                        var staffId = await _context.StaffCreations.AnyAsync(s => s.IsActive == true && s.StaffId == employeeId);
                        if (!staffId)
                        {
                            errorLogs.Add($"StaffId '{employeeId}' not found at row {row}");
                            continue;
                        }
                        var employeeName = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim();
                        var employee = _context.StaffCreations.Where(s => s.IsActive == true)
                            .AsEnumerable()
                            .FirstOrDefault(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}" == employeeName);
                        if (employee == null)
                        {
                            errorLogs.Add($"Staff '{employeeName}' not found at row {row}");
                            continue;

                        }
                        var departmentName = worksheet.Cells[row, columnIndexes["Department"]].Text.Trim();
                        if (string.IsNullOrEmpty(departmentName))
                        {
                            errorLogs.Add($"Department is empty at row {row}");
                            continue;
                        }
                        var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == departmentName.ToLower() && d.IsActive);
                        if (department == null)
                        {
                            errorLogs.Add($"Department '{department}' not found at row {row}");
                            continue;
                        }
                        var addProbation = new ProbationReport
                        {
                            EmpId = employeeId,
                            Name = employeeName,
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
                            ProductivityYear = excelImportDto.Year ?? 0,
                            Month = excelImportDto.Month ?? 0,
                            NumberOfMonths = int.TryParse(worksheet.Cells[row, columnIndexes["No Of Months"]].Text.Trim(), out int numberOfMonths) ? numberOfMonths : (int?)0,
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        probations.Add(addProbation);
                    }
                    if (probations.Any())
                    {
                        await _context.ProbationReports.AddRangeAsync(probations);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = probations.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 19)
                {
                    var probations = new List<ProbationTarget>();
                    var errorLogs = new List<string>();
                    var validDivisionIds = _context.DivisionMasters.Where(d => d.IsActive).Select(d => d.Id).ToHashSet();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var employeeId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim();
                        var staffId = await _context.StaffCreations.AnyAsync(s => s.IsActive == true && s.StaffId == employeeId);
                        if (!staffId)
                        {
                            errorLogs.Add($"StaffId '{employeeId}' not found at row {row}");
                            continue;
                        }
                        var employeeName = worksheet.Cells[row, columnIndexes["Name"]].Text.Trim();
                        var employee = _context.StaffCreations.Where(s => s.IsActive == true)
                            .AsEnumerable()
                            .FirstOrDefault(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}" == employeeName);
                        if (employee == null)
                        {
                            errorLogs.Add($"Staff '{employeeName}' not found at row {row}");
                            continue;
                        }
                        var divisionName = worksheet.Cells[row, columnIndexes["EMP Division"]].Text.Trim();
                        if (string.IsNullOrEmpty(divisionName))
                        {
                            errorLogs.Add($"Division is empty at row {row}");
                            continue;
                        }
                        var division = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == divisionName.ToLower() && d.IsActive);
                        if (division == null)
                        {
                            errorLogs.Add($"Division '{division}' is empty at row {row}");
                            continue;
                        }
                        var addProbation = new ProbationTarget
                        {
                            EmpId = employeeId,
                            Name = employeeName,
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
                            ProductivityYear = excelImportDto.Year ?? 0,
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        probations.Add(addProbation);
                    }
                    if (probations.Any())
                    {
                        await _context.ProbationTargets.AddRangeAsync(probations);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = probations.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 20)
                {
                    var paySheets = new List<PaySheet>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var employeeName = worksheet.Cells[row, columnIndexes["Employee Name"]].Text.Trim();
                        var employee = _context.StaffCreations.Where(s => s.IsActive == true)
                            .AsEnumerable()
                            .FirstOrDefault(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}" == employeeName);
                        if (employee == null)
                        {
                            errorLogs.Add($"Staff '{employeeName}' not found at row {row}");
                            continue;
                        }
                        var employeeId = employee.StaffId;
                        var designationName = worksheet.Cells[row, columnIndexes["Designation"]].Text.Trim();
                        if (string.IsNullOrEmpty(designationName))
                        {
                            errorLogs.Add($"Designation is empty at row {row}");
                            continue;
                        }
                        var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive);
                        if (designation == null)
                        {
                            errorLogs.Add($"Designation '{designation}' not found at row {row}");
                            continue;
                        }
                        var departmentName = worksheet.Cells[row, columnIndexes["Department"]].Text.Trim();
                        if (string.IsNullOrEmpty(designationName))
                        {
                            errorLogs.Add($"Department is empty at row {row}");
                            continue;
                        }
                        var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == departmentName.ToLower() && d.IsActive);
                        if (department == null)
                        {
                            errorLogs.Add($"Department '{department}' not found at row {row}");
                            continue;
                        }
                        DateOnly dateOfJoining = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["Date of Joining"]].Text, out var parsedDate) ? parsedDate : DateOnly.MinValue;
                        if (dateOfJoining == DateOnly.MinValue)
                        {
                            errorLogs.Add($"Invalid Date of Joining: '{dateOfJoining}' at row {row}");
                            continue;
                        }
                        DateOnly dateOfBirth = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["Date of Birth"]].Text, out var parsedDate1) ? parsedDate1 : DateOnly.MinValue;
                        if (dateOfBirth == DateOnly.MinValue)
                        {
                            errorLogs.Add($"Invalid Date of Birth: '{dateOfBirth}' at row {row}");
                            continue;
                        }
                        DateOnly salaryEffectiveFrom = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["Salary Effective From"]].Text, out var parsedDate2) ? parsedDate2 : DateOnly.MinValue;
                        if (salaryEffectiveFrom == DateOnly.MinValue)
                        {
                            errorLogs.Add($"Invalid Salary Effective From: '{salaryEffectiveFrom}' at row {row}");
                            continue;
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
                            Year = excelImportDto.Year ?? 0,
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        paySheets.Add(addPaySheet);
                    }
                    if (paySheets.Any())
                    {
                        await _context.PaySheets.AddRangeAsync(paySheets);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = paySheets.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 21)
                {
                    if (excelImportDto.PerformanceTypeId == 1)
                    {
                        var monthlyPerformances = new List<MonthlyPerformance>();
                        var errorLogs = new List<string>();
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var employeeId = worksheet.Cells[row, columnIndexes["Employee Code"]].Text.Trim();
                            var staffId = await _context.StaffCreations.AnyAsync(s => s.IsActive == true && s.StaffId == employeeId);
                            if (!staffId)
                            {
                                errorLogs.Add($"StaffId '{employeeId}' not found at row {row}");
                                continue;
                            }
                            var employeeName = worksheet.Cells[row, columnIndexes["Employee Name"]].Text.Trim();
                            var employee = _context.StaffCreations.Where(s => s.IsActive == true)
                                .AsEnumerable()
                                .FirstOrDefault(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}" == employeeName);
                            if (employee == null)
                            {
                                errorLogs.Add($"Staff '{employeeName}' not found at row {row}");
                                continue;

                            }
                            var designationName = worksheet.Cells[row, columnIndexes["Designation"]].Text.Trim();
                            if (string.IsNullOrEmpty(designationName))
                            {
                                errorLogs.Add($"Designation is empty at row {row}");
                                continue;
                            }
                            var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive);
                            if (designation == null)
                            {
                                errorLogs.Add($"Designation '{designationName}' not found at row {row}");
                                continue;
                            }
                            var addMonthlyPerformance = new MonthlyPerformance
                            {
                                EmployeeCode = employeeId,
                                EmployeeName = employeeName,
                                Designation = designationName,
                                ProductivityScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Productivity Score"]].Text.Trim(), out decimal basicEarned) ? basicEarned : 0m,
                                QualityScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Quality Score"]].Text.Trim(), out decimal basicArradj) ? basicArradj : 0m,
                                PresentScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Present Score"]].Text.Trim(), out decimal presentScore) ? presentScore : 0m,
                                TotalScore = decimal.TryParse(worksheet.Cells[row, columnIndexes["Total Score"]].Text.Trim(), out decimal hraArradj) ? hraArradj : 0m,
                                ProductivityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Productivity %"]].Text.Trim(), out decimal convEarned) ? convEarned : 0m,
                                QualityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Quality %"]].Text.Trim(), out decimal qualPer) ? qualPer : 0m,
                                PresentPercentage = int.TryParse(worksheet.Cells[row, columnIndexes["Present %"]].Text, out var postalCode) ? postalCode : 0,
                                FinalPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final %"]].Text.Trim(), out decimal medAllowArradj) ? medAllowArradj : 0m,
                                Grade = worksheet.Cells[row, columnIndexes["Grade"]].Text.Trim(),
                                TotalAbsents = decimal.TryParse(worksheet.Cells[row, columnIndexes["Total Absents"]].Text.Trim(), out decimal totalAbs) ? totalAbs : 0m,
                                ReportingHead = worksheet.Cells[row, columnIndexes["Reporting Head"]].Text.Trim(),
                                HrComments = worksheet.Cells[row, columnIndexes["HR Comments"]].Text.Trim(),
                                PerformanceTypeId = excelImportDto.PerformanceTypeId ?? 0,
                                Month = excelImportDto.Month ?? 0,
                                Year = excelImportDto.Year ?? 0,
                                IsActive = true,
                                CreatedBy = excelImportDto.CreatedBy,
                                CreatedUtc = DateTime.UtcNow
                            };
                            monthlyPerformances.Add(addMonthlyPerformance);
                        }
                        if (monthlyPerformances.Any())
                        {
                            await _context.MonthlyPerformances.AddRangeAsync(monthlyPerformances);
                            await _context.SaveChangesAsync();
                            result.SuccessCount = monthlyPerformances.Count;
                        }
                        result.ErrorCount = result.TotalRecords - result.SuccessCount;
                        result.ErrorMessages = errorLogs;
                        return result;
                    }
                    else if (excelImportDto.PerformanceTypeId == 2)
                    {
                        var quarterlyPerformances = new List<QuarterlyPerformance>();
                        var errorLogs = new List<string>();
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var employeeId = worksheet.Cells[row, columnIndexes["Employee Code"]].Text.Trim();
                            var staffId = await _context.StaffCreations.AnyAsync(s => s.IsActive == true && s.StaffId == employeeId);
                            if (!staffId)
                            {
                                errorLogs.Add($"StaffId '{employeeId}' not found at row {row}");
                                continue;
                            }
                            var employeeName = worksheet.Cells[row, columnIndexes["Employee Name"]].Text.Trim();
                            var employee = _context.StaffCreations.Where(s => s.IsActive == true)
                                .AsEnumerable()
                                .FirstOrDefault(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}" == employeeName);
                            if (employee == null)
                            {
                                errorLogs.Add($"Staff '{employeeName}' not found at row {row}");
                                continue;
                            }
                            var designationName = worksheet.Cells[row, columnIndexes["Designation"]].Text.Trim();
                            if (string.IsNullOrEmpty(designationName))
                            {
                                errorLogs.Add($"Designation is empty at {row}");
                                continue;
                            }
                            var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive);
                            if (designation == null)
                            {
                                errorLogs.Add($"Designation '{designationName}' not found at row {row}");
                                continue;
                            }
                            var addQuarterlyPerformance = new QuarterlyPerformance
                            {
                                EmployeeCode = employeeId,
                                EmployeeName = employeeName,
                                Designation = designationName,
                                ProductivityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Productivity %"]].Text.Trim(), out decimal convEarned) ? convEarned : 0m,
                                QualityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Quality %"]].Text.Trim(), out decimal qualPer) ? qualPer : 0m,
                                PresentPercentage = int.TryParse(worksheet.Cells[row, columnIndexes["Present %"]].Text, out var postalCode) ? postalCode : 0,
                                FinalPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final %"]].Text.Trim(), out decimal medAllowArradj) ? medAllowArradj : 0m,
                                Grade = worksheet.Cells[row, columnIndexes["Grade"]].Text.Trim(),
                                AbsentDays = decimal.TryParse(worksheet.Cells[row, columnIndexes["Absent Days"]].Text.Trim(), out decimal basicEarned) ? basicEarned : 0m,
                                HrComments = worksheet.Cells[row, columnIndexes["HR Comments"]].Text.Trim(),
                                PerformanceTypeId = excelImportDto.PerformanceTypeId ?? 0,
                                Quarter = excelImportDto.QuarterType ?? string.Empty,
                                Year = excelImportDto.Year ?? 0,
                                IsActive = true,
                                CreatedBy = excelImportDto.CreatedBy,
                                CreatedUtc = DateTime.UtcNow
                            };
                            quarterlyPerformances.Add(addQuarterlyPerformance);
                        }
                        if (quarterlyPerformances.Any())
                        {
                            await _context.QuarterlyPerformances.AddRangeAsync(quarterlyPerformances);
                            await _context.SaveChangesAsync();
                            result.SuccessCount = quarterlyPerformances.Count;
                        }
                        result.ErrorCount = result.TotalRecords - result.SuccessCount;
                        result.ErrorMessages = errorLogs;
                        return result;
                    }
                    else if (excelImportDto.PerformanceTypeId == 3)
                    {
                        var quarterlyPerformances = new List<YearlyPerformance>();
                        var errorLogs = new List<string>();
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var employeeId = worksheet.Cells[row, columnIndexes["Employee Code"]].Text.Trim();
                            var staffId = await _context.StaffCreations.AnyAsync(s => s.IsActive == true && s.StaffId == employeeId);
                            if (!staffId)
                            {
                                errorLogs.Add($"StaffId '{employeeId}' not found at row {row}");
                                continue;
                            }
                            var employeeName = worksheet.Cells[row, columnIndexes["Employee Name"]].Text.Trim();
                            var employee = _context.StaffCreations.Where(s => s.IsActive == true)
                                .AsEnumerable()
                                .FirstOrDefault(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}" == employeeName);
                            if (employee == null)
                            {
                                errorLogs.Add($"Staff '{employeeName}' not found at row {row}");
                                continue;
                            }
                            var designationName = worksheet.Cells[row, columnIndexes["Designation"]].Text.Trim();
                            if (string.IsNullOrEmpty(designationName))
                            {
                                errorLogs.Add($"Designation is empty at {row}");
                                continue;
                            }
                            var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive);
                            if (designation == null)
                            {
                                errorLogs.Add($"Designation '{designationName}' not found at row {row}");
                                continue;
                            }
                            var addQuarterlyPerformance = new YearlyPerformance
                            {
                                EmployeeCode = employeeId,
                                EmployeeName = employeeName,
                                Designation = designationName,
                                ProductivityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Productivity %"]].Text.Trim(), out decimal convEarned) ? convEarned : 0m,
                                QualityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Quality %"]].Text.Trim(), out decimal qualPer) ? qualPer : 0m,
                                PresentPercentage = int.TryParse(worksheet.Cells[row, columnIndexes["Present %"]].Text, out var postalCode) ? postalCode : 0,
                                FinalPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final %"]].Text.Trim(), out decimal medAllowArradj) ? medAllowArradj : 0m,
                                Grade = worksheet.Cells[row, columnIndexes["Grade"]].Text.Trim(),
                                AbsentDays = decimal.TryParse(worksheet.Cells[row, columnIndexes["Absent Days"]].Text.Trim(), out decimal basicEarned) ? basicEarned : 0m,
                                HrComments = worksheet.Cells[row, columnIndexes["HR Comments"]].Text.Trim(),
                                PerformanceTypeId = excelImportDto.PerformanceTypeId ?? 0,
                                Year = excelImportDto.Year ?? 0,
                                IsActive = true,
                                CreatedBy = excelImportDto.CreatedBy,
                                CreatedUtc = DateTime.UtcNow
                            };
                            quarterlyPerformances.Add(addQuarterlyPerformance);
                        }
                        if (quarterlyPerformances.Any())
                        {
                            await _context.YearlyPerformances.AddRangeAsync(quarterlyPerformances);
                            await _context.SaveChangesAsync();
                            result.SuccessCount = quarterlyPerformances.Count;
                        }
                        result.ErrorCount = result.TotalRecords - result.SuccessCount;
                        result.ErrorMessages = errorLogs;
                        return result;
                    }
                }
                else if (excelImportDto.ExcelImportId == 23)
                {
                    var appraisals = new List<EmployeeAppraisalSheet>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var employeeId = worksheet.Cells[row, columnIndexes["EmployeeCode"]].Text.Trim();
                        var staffId = await _context.StaffCreations.AnyAsync(s => s.IsActive == true && s.StaffId == employeeId);
                        if (!staffId)
                        {
                            errorLogs.Add($"StaffId '{employeeId}' not found at row {row}");
                            continue;
                        }
                        var employeeName = worksheet.Cells[row, columnIndexes["EmployeeName"]].Text.Trim();
                        var employee = _context.StaffCreations.Where(s => s.IsActive == true)
                            .AsEnumerable()
                            .FirstOrDefault(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}" == employeeName);
                        if (employee == null)
                        {
                            errorLogs.Add($"Staff '{employeeName}' not found at row {row}");
                            continue;
                        }
                        var appraisalYear = int.TryParse(worksheet.Cells[row, columnIndexes["AppraisalYear"]].Text, out var postalCode) ? postalCode : 0;
                        if (appraisals.Any(a => a.EmployeeCode == employeeId && a.AppraisalYear == appraisalYear))
                        {
                            errorLogs.Add($"Duplicate entry for EmployeeCode '{employeeId}' with AppraisalYear '{appraisalYear}' at row {row}");
                            continue;
                        }
                        var existsInDb = await _context.EmployeeAppraisalSheets.AnyAsync(a => a.EmployeeCode == employeeId && a.AppraisalYear == appraisalYear && a.IsActive);
                        if (existsInDb)
                        {
                            errorLogs.Add($"Appraisal for EmployeeCode '{employeeId}' already exists for AppraisalYear {appraisalYear} (row {row})");
                            continue;
                        }
                        var designationName = worksheet.Cells[row, columnIndexes["Designation"]].Text.Trim();
                        if (string.IsNullOrEmpty(designationName))
                        {
                            errorLogs.Add($"Designation is empty at {row}");
                            continue;
                        }
                        var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == designationName.ToLower() && d.IsActive);
                        if (designation == null)
                        {
                            errorLogs.Add($"Designation '{designationName}' not found at row {row}");
                            continue;
                        }
                        var revisedDesignationName = worksheet.Cells[row, columnIndexes["RevisedDesignation"]].Text.Trim();
                        var revisedDesignation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == revisedDesignationName.ToLower() && d.IsActive);
                        if (revisedDesignation == null)
                        {
                            errorLogs.Add($"Revised Designation '{revisedDesignationName}' not found at row {row}");
                            continue;
                        }
                        var departmentName = worksheet.Cells[row, columnIndexes["Department"]].Text.Trim();
                        if (string.IsNullOrEmpty(departmentName))
                        {
                            errorLogs.Add($"Department is empty at {row}");
                            continue;
                        }
                        var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Name.ToLower() == departmentName.ToLower() && d.IsActive);
                        if (department == null)
                        {
                            errorLogs.Add($"Department '{departmentName}' not found at row {row}");
                            continue;
                        }
                        var appraisal = new EmployeeAppraisalSheet
                        {
                            EmployeeCode = employeeId,
                            EmployeeName = employeeName,
                            Designation = designationName,
                            RevisedDesignation = revisedDesignationName,
                            Department = departmentName,
                            AppraisalYear = appraisalYear,
                            BasicCurrentPerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["BasicCurrentPerAnnum"]].Text),
                            BasicCurrentPerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["BasicCurrentPerMonth"]].Text),
                            BasicCurrentPerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["BasicCurrentPerAnnumAfterApp"]].Text),
                            BasicCurrentPerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["BasicCurrentPerMonthAfterApp"]].Text),
                            HraperAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["HRAPerAnnum"]].Text),
                            HraperMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["HRAPerMonth"]].Text),
                            HraperAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["HRAPerAnnumAfterApp"]].Text),
                            HraperMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["HRAPerMonthAfterApp"]].Text),
                            ConveyancePerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["ConveyancePerAnnum"]].Text),
                            ConveyancePerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["ConveyancePerMonth"]].Text),
                            ConveyancePerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["ConveyancePerAnnumAfterApp"]].Text),
                            ConveyancePerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["ConveyancePerMonthAfterApp"]].Text),
                            MedicalAllowancePerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["MedicalAllowancePerAnnum"]].Text),
                            MedicalAllowancePerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["MedicalAllowancePerMonth"]].Text),
                            MedicalAllowancePerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["MedicalAllowancePerAnnumAfterApp"]].Text),
                            MedicalAllowancePerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["MedicalAllowancePerMonthAfterApp"]].Text),
                            SpecialAllowancePerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["SpecialAllowancePerAnnum"]].Text),
                            SpecialAllowancePerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["SpecialAllowancePerMonth"]].Text),
                            SpecialAllowancePerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["SpecialAllowancePerAnnumAfterApp"]].Text),
                            SpecialAllowancePerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["SpecialAllowancePerMonthAfterApp"]].Text),
                            EmployerPfcontributionPerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerPFContributionPerAnnum"]].Text),
                            EmployerPfcontributionPerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerPFContributionPerMonth"]].Text),
                            EmployerPfcontributionPerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerPFContributionPerAnnumAfterApp"]].Text),
                            EmployerPfcontributionPerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerPFContributionPerMonthAfterApp"]].Text),
                            EmployerEsicontributionPerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerESIContributionPerAnnum"]].Text),
                            EmployerEsicontributionPerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerESIContributionPerMonth"]].Text),
                            EmployerEsicontributionPerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerESIContributionPerAnnumAfterApp"]].Text),
                            EmployerEsicontributionPerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerESIContributionPerMonthAfterApp"]].Text),
                            GroupPersonalAccidentPerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["GroupPersonalAccidentPerAnnum"]].Text),
                            GroupPersonalAccidentPerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["GroupPersonalAccidentPerMonth"]].Text),
                            GroupPersonalAccidentPerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["GroupPersonalAccidentPerAnnumAfterApp"]].Text),
                            GroupPersonalAccidentPerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["GroupPersonalAccidentPerMonthAfterApp"]].Text),
                            EmployeePfcontributionPerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployeePFContributionPerAnnum"]].Text),
                            EmployeePfcontributionPerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployeePFContributionPerMonth"]].Text),
                            EmployeePfcontributionPerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployeePFContributionPerAnnumAfterApp"]].Text),
                            EmployeePfcontributionPerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployeePFContributionPerMonthAfterApp"]].Text),
                            EmployeeEsicontributionPerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployeeESIContributionPerAnnum"]].Text),
                            EmployeeEsicontributionPerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployeeESIContributionPerMonth"]].Text),
                            EmployeeEsicontributionPerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployeeESIContributionPerAnnumAfterApp"]].Text),
                            EmployeeEsicontributionPerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployeeESIContributionPerMonthAfterApp"]].Text),
                            ProfessionalTaxPerAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["ProfessionalTaxPerAnnum"]].Text),
                            ProfessionalTaxPerMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["ProfessionalTaxPerMonth"]].Text),
                            ProfessionalTaxPerAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["ProfessionalTaxPerAnnumAfterApp"]].Text),
                            ProfessionalTaxPerMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["ProfessionalTaxPerMonthAfterApp"]].Text),
                            EmployeeSalutation = worksheet.Cells[row, columnIndexes["EmployeeSalutation"]].Text.Trim(),
                            TotalAppraisal = ParseDecimal(worksheet.Cells[row, columnIndexes["TotalAppraisal"]].Text),
                            GmcperAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["GMCPerAnnum"]].Text),
                            GmcperMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["GMCPerMonth"]].Text),
                            GmcperAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["GMCPerAnnumAfterApp"]].Text),
                            GmcperMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["GMCPerMonthAfterApp"]].Text),
                            EmployerGmcperAnnum = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerGMCPerAnnum"]].Text),
                            EmployerGmcperMonth = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerGMCPerMonth"]].Text),
                            EmployerGmcperAnnumAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerGMCPerAnnumAfterApp"]].Text),
                            EmployerGmcperMonthAfterApp = ParseDecimal(worksheet.Cells[row, columnIndexes["EmployerGMCPerMonthAfterApp"]].Text),
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        appraisals.Add(appraisal);
                    }
                    if (appraisals.Any())
                    {
                        await _context.EmployeeAppraisalSheets.AddRangeAsync(appraisals);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = appraisals.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
                else if (excelImportDto.ExcelImportId == 24)
                {
                    var assignShifts = new List<AssignShift>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var employeeId = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                        var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == employeeId && s.IsActive == true);
                        if (staff == null)
                        {
                            errorLogs.Add($"StaffId '{employeeId}' not found at row {row}");
                            continue;
                        }
                        var staffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
                        var rawShift = worksheet.Cells[row, columnIndexes["ShiftName"]].Text?.Trim();
                        var shiftName = rawShift?.Split('(')[0].Trim().ToLower();
                        var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.Name.Trim() == shiftName && s.IsActive);
                        if (shift == null)
                        {
                            errorLogs.Add($"Shift '{rawShift}' not found at row {row}");
                            continue;
                        }
                        var dateText = worksheet.Cells[row, columnIndexes["Date"]]?.Text?.Trim();
                        if (string.IsNullOrWhiteSpace(dateText) || !DateOnly.TryParse(dateText, out var confirmationDate))
                        {
                            errorLogs.Add($"Date is required and must be valid in row {row}");
                            continue;
                        }
                        var hasUnfreezed = await _context.AttendanceRecords.AnyAsync(f => f.IsFreezed == true && f.StaffId == staff.Id && f.AttendanceDate == confirmationDate);
                        if (hasUnfreezed)
                        {
                            errorLogs.Add($"Shift cannot be assign attendance records are frozen at row {row}");
                            continue;
                        }
                        var existingAssignedShift = await _context.AssignShifts
                            .Where(a => a.FromDate == confirmationDate &&
                                        a.StaffId == staff.Id &&
                                        a.IsActive)
                            .ToListAsync();
                        if (existingAssignedShift.Count > 0)
                        {
                            errorLogs.Add($"Shift already assigned for staff '{staffName}' at row {row}");
                            continue;
                        }
                        var shiftAssign = new AssignShift
                        {
                            FromDate = confirmationDate,
                            ShiftId = shift.Id,
                            StaffId = staff.Id,
                            IsActive = true,
                            CreatedBy = excelImportDto.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        await _context.AssignShifts.AddAsync(shiftAssign);
                        assignShifts.Add(shiftAssign);
                    }
                    if (assignShifts.Any())
                    {
                        await _context.AssignShifts.AddRangeAsync(assignShifts);
                        await _context.SaveChangesAsync();
                        result.SuccessCount = assignShifts.Count;
                    }
                    result.ErrorCount = result.TotalRecords - result.SuccessCount;
                    result.ErrorMessages = errorLogs;
                    return result;
                }
            }
        }
        return new ExcelImportResultDto
        {
            TotalRecords = 0,
            SuccessCount = 0,
            ErrorCount = 0,
            ErrorMessages = new List<string> { $"Invalid ExcelImportId: {excelImportDto.ExcelImportId}" }
        };
    }

    private decimal ParseDecimal(string value)
    {
        return decimal.TryParse(value.Trim(), out var result) ? result : 0m;
    }
}