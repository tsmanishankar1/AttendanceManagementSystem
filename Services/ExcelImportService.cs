using AttendanceManagement.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LicenseContext = OfficeOpenXml.LicenseContext;
using OfficeOpenXml.Drawing;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using AttendanceManagement.Input_Models;

public class ExcelImportService
{
    private readonly AttendanceManagementSystemContext _context;

    public ExcelImportService(AttendanceManagementSystemContext context)
    {
        _context = context;
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
                    if (excelImportId == 3)
                    {
                        requiredHeaders = new List<string> { "FullName", "ShortName", "Phone", "Fax", "Email" };
                    }
                    else if (excelImportId == 2)
                    {
                        requiredHeaders = new List<string> {
                            "LeaveTypeName", "StaffCreationId", "TransactionFlag", "Month", "Year", "Remarks", "LeaveCount", "ActualBalance",
                            "AvailableBalance", "LeaveReason" };
                    }
                    else if (excelImportId == 4 || excelImportId == 5 || excelImportId == 6)
                    {
                        requiredHeaders = new List<string> { "FullName", "ShortName" };
                    }
                    else if (excelImportId == 9)
                    {
                        requiredHeaders = new List<string> {
                            "StartTime", "EndTime", "TotalHours", "StaffCreationName", "Remarks", "PermissionDate", "PermissionType", "Status" };
                    }
                    else if (excelImportId == 1 || excelImportId == 10)
                    {
                        requiredHeaders = new List<string>{
                            "CardCode", "Title", "FirstName", "LastName", "ShortName", "MiddleName", "Dob", "Gender", "BloodGroup", "MaritalStatus", "MarriageDate",
                            "JoiningDate", "Confirmed", "BranchName", "DepartmentName", "DivisionName", "DesignationName", "GradeName", "CategoryName",
                            "CostCenterName", "WorkStationName", "City", "District", "State","Country", "Oteligible", "ApprovalLevel1", "Hide", "Status", "VolumeId",
                            "AccessLevel", "PolicyGroup", "WorkingDayPattern", "Tenure", "Uannumber", "EsiNumber", "IsMobileAppEligible", "GeoStatus", "PersonalPhone",
                            "OfficialPhone", "PersonalLocation", "PersonalEmail", "OfficialEmail", "LeaveGroupName", "CompanyMasterName", "LocationMasterName", "HolidayCalendarName",
                            "ApprovalLevel2"
                        };
                    }
                    else if (excelImportId == 11)
                    {
                        requiredHeaders = new List<string> { "ShiftName", "StartTime", "EndTime", "IsActive" };
                    }
                    else
                    {
                        throw new Exception("Invalid ExcelImportId. Only 1 or 2 or 3 or 4 or 5 or 6 or 9 or 10 or 11 are valid.");
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
                    if (excelImportId == 3)
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
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow,
                                UpdatedBy = createdBy
                            };

                            departmentMasters.Add(departmentMaster);
                        }

                        await _context.DepartmentMasters.AddRangeAsync(departmentMasters);
                        await _context.SaveChangesAsync();
                    }
                    else if (excelImportId == 11)
                    {
                        var shiftMasters = new List<Shift>();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var startTimeCell = worksheet.Cells[row, columnIndexes["StartTime"]];
                            var endTimeCell = worksheet.Cells[row, columnIndexes["EndTime"]];
                            var isActiveText = worksheet.Cells[row, columnIndexes["IsActive"]].Text.Trim();

                            var startTime = ConvertExcelDateTime(startTimeCell).ToString()
                             ?? throw new Exception($"Unable to parse StartTime. Raw value: '{startTimeCell.Text}'");

                            var endTime = ConvertExcelDateTime(endTimeCell).ToString()
                                ?? throw new Exception($"Unable to parse EndTime. Raw value: '{endTimeCell.Text}'");

                            var shiftMaster = new Shift
                            {
                                ShiftName = worksheet.Cells[row, columnIndexes["ShiftName"]].Text.Trim(),
                                StartTime = startTime,
                                EndTime = endTime,
                                IsActive = bool.TryParse(isActiveText, out var isActive) ? isActive : false,
                                CreatedBy = createdBy,
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow,
                                UpdatedBy = createdBy
                            };

                            shiftMasters.Add(shiftMaster);
                        }

                        await _context.Shifts.AddRangeAsync(shiftMasters);
                        await _context.SaveChangesAsync();
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
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow,
                                UpdatedBy = createdBy
                            };

                            designationMasters.Add(designationMaster);
                        }

                        await _context.DesignationMasters.AddRangeAsync(designationMasters);
                        await _context.SaveChangesAsync();
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
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow,
                                UpdatedBy = createdBy
                            };

                            divisionMasters.Add(divisionMaster);
                        }

                        await _context.DivisionMasters.AddRangeAsync(divisionMasters);
                        await _context.SaveChangesAsync();
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
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow,
                                UpdatedBy = createdBy
                            };

                            costCentreMasters.Add(costCentreMaster);
                        }

                        await _context.CostCentreMasters.AddRangeAsync(costCentreMasters);
                        await _context.SaveChangesAsync();
                    }
                    else if (excelImportId == 9)
                    {
                        var commonPermissions = new List<CommonPermission>();
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var staffCreationName = worksheet.Cells[row, columnIndexes["StaffCreationName"]]?.Text.Trim();
                            if (string.IsNullOrEmpty(staffCreationName))
                            {
                                continue;
                            }
                            var staffCreationId = _context.StaffCreations
                                .Where(s => (s.FirstName + " " + s.LastName).ToLower() == staffCreationName.ToLower())
                                .Select(s => s.Id)
                                .FirstOrDefault();

                            if (staffCreationId == 0)
                                throw new Exception($"Staff name '{staffCreationName}' not found in the database.");
                            var startTimeText = worksheet.Cells[row, columnIndexes["StartTime"]].Text.Trim();
                            var endTimeText = worksheet.Cells[row, columnIndexes["EndTime"]].Text.Trim();
                            var permissionDateText = worksheet.Cells[row, columnIndexes["PermissionDate"]].Text.Trim();

                            var startTime = TimeOnly.Parse(startTimeText);
                            var endTime = TimeOnly.Parse(endTimeText);
                            var permissionDate = DateOnly.Parse(permissionDateText);
                            var totalHours = (endTime - startTime).ToString("hh\\:mm");
                            var statusText = worksheet.Cells[row, columnIndexes["Status"]].Text.Trim();
                            bool status = statusText.Equals("true", StringComparison.OrdinalIgnoreCase) || statusText.Equals("yes", StringComparison.OrdinalIgnoreCase);

                            var commonPermission = new CommonPermission
                            {
                                PermissionType = worksheet.Cells[row, columnIndexes["PermissionType"]].Text.Trim(),
                                StartTime = startTime,
                                EndTime = endTime,
                                TotalHours = totalHours,
                                StaffId = staffCreationId,
                                PermissionDate = permissionDate,
                                Status = status,
                                Remarks = worksheet.Cells[row, columnIndexes["Remarks"]].Text.Trim(),
                                IsActive = true,
                                CreatedBy = createdBy,
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow,
                                UpdatedBy = createdBy
                            };

                            commonPermissions.Add(commonPermission);
                        }
                        await _context.CommonPermissions.AddRangeAsync(commonPermissions);
                        await _context.SaveChangesAsync();
                    }

                    else if (excelImportId == 1)
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
                                BranchId = branchId,
                                DepartmentId = departmentId,
                                DivisionId = divisionId,
                                DesignationId = designationId,
                                GradeId = gradeId,
                                CategoryId = categoryId,
                                CostCenterId = costCenterId,
                                WorkStationId = workStationId,
                                LeaveGroupId = leaveGroupId,
                                CompanyMasterId = companyMasterId,
                                LocationMasterId = locationMasterId,
                                HolidayCalendarId = holidayCalendarId,
                                CardCode = worksheet.Cells[row, columnIndexes["CardCode"]].Text.Trim(),
                                Title = worksheet.Cells[row, columnIndexes["Title"]].Text.Trim(),
                                FirstName = worksheet.Cells[row, columnIndexes["FirstName"]].Text.Trim(),
                                LastName = worksheet.Cells[row, columnIndexes["LastName"]].Text.Trim(),
                                ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                MiddleName = worksheet.Cells[row, columnIndexes["MiddleName"]].Text.Trim(),
                                StatusId = statusId,
                                Gender = worksheet.Cells[row, columnIndexes["Gender"]].Text.Trim(),
                                Hide = bool.TryParse(worksheet.Cells[row, columnIndexes["Hide"]].Text, out var hide) ? hide : false,
                                BloodGroup = worksheet.Cells[row, columnIndexes["BloodGroup"]].Text.Trim(),
                                //ProfilePhoto = ConvertImageToBase64(package, worksheet, row, columnIndexes["ProfilePhoto"]),
                                //ProfilePhoto = worksheet.Cells[row, columnIndexes["ProfilePhoto"]].Text.Trim(),
                                MaritalStatus = worksheet.Cells[row, columnIndexes["MaritalStatus"]].Text.Trim(),
                                Dob = (DateOnly)(DateOnly.TryParse(worksheet.Cells[row, columnIndexes["Dob"]].Text, out var dob) ? dob : default),
                                MarriageDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["MarriageDate"]].Text, out var marriageDate) ? marriageDate : (DateOnly?)null,
                                PersonalPhone = long.TryParse(worksheet.Cells[row, columnIndexes["PersonalPhone"]].Text, out var personalPhone) ? personalPhone : 0,
                                JoiningDate = (DateOnly)(DateOnly.TryParse(worksheet.Cells[row, columnIndexes["JoiningDate"]].Text, out var joiningDate) ? joiningDate : default),
                                Confirmed = bool.TryParse(worksheet.Cells[row, columnIndexes["Confirmed"]].Text, out var confirmed) ? confirmed : false,
                                City = worksheet.Cells[row, columnIndexes["City"]].Text.Trim(),
                                District = worksheet.Cells[row, columnIndexes["District"]].Text.Trim(),
                                State = worksheet.Cells[row, columnIndexes["State"]].Text.Trim(),
                                Country = worksheet.Cells[row, columnIndexes["Country"]].Text.Trim(),
                                OtEligible = bool.TryParse(worksheet.Cells[row, columnIndexes["Oteligible"]].Text, out var otEligible) ? otEligible : false,
                                ApprovalLevel1 = (int)(int.TryParse(worksheet.Cells[row, columnIndexes["ApprovalLevel1"]]?.Text, out var approvalLevel1) ? approvalLevel1 : default),
                                AccessLevel = worksheet.Cells[row, columnIndexes["AccessLevel"]]?.Text?.Trim() ?? string.Empty,
                                PolicyGroup = worksheet.Cells[row, columnIndexes["PolicyGroup"]]?.Text?.Trim() ?? string.Empty,
                                WorkingDayPattern = worksheet.Cells[row, columnIndexes["WorkingDayPattern"]]?.Text?.Trim() ?? string.Empty,
                                Tenure = decimal.TryParse(worksheet.Cells[row, columnIndexes["Tenure"]]?.Text.Trim(), out decimal tenure) ? tenure : 0m,
                                UanNumber = worksheet.Cells[row, columnIndexes["Uannumber"]]?.Text.Trim(),
                                EsiNumber = worksheet.Cells[row, columnIndexes["EsiNumber"]]?.Text.Trim(),
                                IsMobileAppEligible = (bool)(bool.TryParse(worksheet.Cells[row, columnIndexes["IsMobileAppEligible"]]?.Text, out var isMobileAppEligible) ? isMobileAppEligible : false),
                                GeoStatus = worksheet.Cells[row, columnIndexes["GeoStatus"]]?.Text.Trim() ?? string.Empty,
                                UpdatedBy = createdBy,
                                UpdatedUtc = DateTime.UtcNow,
                                OfficialPhone = long.TryParse(worksheet.Cells[row, columnIndexes["OfficialPhone"]]?.Text, out var officialPhone) ? officialPhone : 0,
                                PersonalLocation = worksheet.Cells[row, columnIndexes["PersonalLocation"]]?.Text.Trim() ?? string.Empty,
                                PersonalEmail = worksheet.Cells[row, columnIndexes["PersonalEmail"]].Text.Trim(),
                                OfficialEmail = worksheet.Cells[row, columnIndexes["OfficialEmail"]]?.Text.Trim(),
                                ApprovalLevel2 = int.TryParse(worksheet.Cells[row, columnIndexes["ApprovalLevel2"]]?.Text, out var approvalLevel2) ? approvalLevel2 : (int?)null,
                                CreatedBy = createdBy,
                                IsActive = true,
                                CreatedUtc = DateTime.UtcNow
                            };

                            staffCreations.Add(staffCreation);
                        }

                        await _context.StaffCreations.AddRangeAsync(staffCreations);
                        await _context.SaveChangesAsync();
                    }
                    else if (excelImportId == 10)
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

                            var staffCreation = new StaffCreation
                            {
                                BranchId = branchId,
                                DepartmentId = departmentId,
                                DivisionId = divisionId,
                                DesignationId = designationId,
                                GradeId = gradeId,
                                CategoryId = categoryId,
                                CostCenterId = costCenterId,
                                WorkStationId = workStationId,
                                LeaveGroupId = leaveGroupId,
                                CompanyMasterId = companyMasterId,
                                LocationMasterId = locationMasterId,
                                HolidayCalendarId = holidayCalendarId,
                                CardCode = worksheet.Cells[row, columnIndexes["CardCode"]].Text.Trim(),
                                Title = worksheet.Cells[row, columnIndexes["Title"]].Text.Trim(),
                                FirstName = worksheet.Cells[row, columnIndexes["FirstName"]].Text.Trim(),
                                LastName = worksheet.Cells[row, columnIndexes["LastName"]].Text.Trim(),
                                ShortName = worksheet.Cells[row, columnIndexes["ShortName"]].Text.Trim(),
                                MiddleName = worksheet.Cells[row, columnIndexes["MiddleName"]].Text.Trim(),
                                StatusId = statusId,
                                Gender = worksheet.Cells[row, columnIndexes["Gender"]].Text.Trim(),
                                Hide = bool.TryParse(worksheet.Cells[row, columnIndexes["Hide"]].Text, out var hide) ? hide : false,
                                BloodGroup = worksheet.Cells[row, columnIndexes["BloodGroup"]].Text.Trim(),
                                //ProfilePhoto = ConvertImageToBase64(package, worksheet, row, columnIndexes["ProfilePhoto"]),
                                //ProfilePhoto = worksheet.Cells[row, columnIndexes["ProfilePhoto"]].Text.Trim(),
                                MaritalStatus = worksheet.Cells[row, columnIndexes["MaritalStatus"]].Text.Trim(),
                                Dob = (DateOnly)(DateOnly.TryParse(worksheet.Cells[row, columnIndexes["Dob"]].Text, out var dob) ? dob : default),
                                MarriageDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["MarriageDate"]].Text, out var marriageDate) ? marriageDate : (DateOnly?)null,
                                PersonalPhone = long.TryParse(worksheet.Cells[row, columnIndexes["PersonalPhone"]].Text, out var personalPhone) ? personalPhone : 0,
                                JoiningDate = DateOnly.TryParse(worksheet.Cells[row, columnIndexes["JoiningDate"]].Text, out var joiningDate) ? joiningDate : DateOnly.MinValue,
                                Confirmed = bool.TryParse(worksheet.Cells[row, columnIndexes["Confirmed"]].Text, out var confirmed) ? confirmed : false,
                                City = worksheet.Cells[row, columnIndexes["City"]].Text.Trim(),
                                District = worksheet.Cells[row, columnIndexes["District"]].Text.Trim(),
                                State = worksheet.Cells[row, columnIndexes["State"]].Text.Trim(),
                                Country = worksheet.Cells[row, columnIndexes["Country"]].Text.Trim(),
                                OtEligible = bool.TryParse(worksheet.Cells[row, columnIndexes["Oteligible"]].Text, out var otEligible) ? otEligible : false,
                                ApprovalLevel1 = (int)(int.TryParse(worksheet.Cells[row, columnIndexes["ApprovalLevel1"]]?.Text, out var approvalLevel1) ? approvalLevel1 : 0),
                                AccessLevel = worksheet.Cells[row, columnIndexes["AccessLevel"]]?.Text.Trim() ?? string.Empty,
                                PolicyGroup = worksheet.Cells[row, columnIndexes["PolicyGroup"]]?.Text.Trim() ?? string.Empty,
                                WorkingDayPattern = worksheet.Cells[row, columnIndexes["WorkingDayPattern"]]?.Text.Trim() ?? string.Empty,
                                Tenure = decimal.TryParse(worksheet.Cells[row, columnIndexes["Tenure"]]?.Text.Trim(), out decimal tenure) ? tenure : 0m,
                                UanNumber = worksheet.Cells[row, columnIndexes["Uannumber"]]?.Text.Trim(),
                                EsiNumber = worksheet.Cells[row, columnIndexes["EsiNumber"]]?.Text.Trim(),
                                IsMobileAppEligible = (bool)(bool.TryParse(worksheet.Cells[row, columnIndexes["IsMobileAppEligible"]]?.Text, out var isMobileAppEligible) ? isMobileAppEligible : false),
                                GeoStatus = worksheet.Cells[row, columnIndexes["GeoStatus"]]?.Text.Trim() ?? string.Empty,
                                UpdatedBy = createdBy,
                                UpdatedUtc = DateTime.UtcNow,
                                OfficialPhone = (long)(long.TryParse(worksheet.Cells[row, columnIndexes["OfficialPhone"]]?.Text, out var officialPhone) ? officialPhone : 0),
                                PersonalLocation = worksheet.Cells[row, columnIndexes["PersonalLocation"]]?.Text.Trim() ?? string.Empty,
                                PersonalEmail = worksheet.Cells[row, columnIndexes["PersonalEmail"]].Text.Trim(),
                                OfficialEmail = worksheet.Cells[row, columnIndexes["OfficialEmail"]]?.Text.Trim(),
                                ApprovalLevel2 = int.TryParse(worksheet.Cells[row, columnIndexes["ApprovalLevel2"]]?.Text, out var approvalLevel2) ? approvalLevel2 : (int?)null,
                                CreatedBy = createdBy,
                                IsActive = false,
                                CreatedUtc = DateTime.UtcNow
                            };

                            staffCreations.Add(staffCreation);
                        }

                        await _context.StaffCreations.AddRangeAsync(staffCreations);
                        await _context.SaveChangesAsync();
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
                            if (string.IsNullOrEmpty(staffCreationIdStr) || !int.TryParse(staffCreationIdStr, out var staffCreationId))
                                throw new Exception($"Invalid or missing StaffCreationId at row {row}.");
                            var staffExits = _context.StaffCreations.Any(s => s.Id == staffCreationId);
                            if (!staffExists)
                                throw new Exception($"StaffCreationId '{staffCreationId}' not found in the database at row {row}.");
                            var transactionFlagValue = worksheet.Cells[row, columnIndexes["TransactionFlag"]]?.Text.Trim().ToLower();
                            var transactionFlag = transactionFlagValue == "1" || transactionFlagValue == "true";
                            var leaveCount = decimal.TryParse(worksheet.Cells[row, columnIndexes["LeaveCount"]]?.Text, out var parsedLeaveCount) ? parsedLeaveCount : 0;
                            if (leaveCount <= 0)
                                throw new Exception($"Invalid leave count at row {row}.");
                            var actualBalance = _context.IndividualLeaveCreditDebits
                                .Where(l => l.StaffCreationId == staffCreationId && l.LeaveTypeId == leaveTypeId)
                                .OrderByDescending(l => l.CreatedUtc)
                                .Select(l => l.ActualBalance ?? 0)
                                .FirstOrDefault();

                            var availableBalance = _context.IndividualLeaveCreditDebits
                                .Where(l => l.StaffCreationId == staffCreationId && l.LeaveTypeId == leaveTypeId)
                                .OrderByDescending(l => l.CreatedUtc)
                                .Select(l => l.AvailableBalance ?? 0)
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
                        await _context.SaveChangesAsync();
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
    //private string ConvertImageToBase64(ExcelPackage package, ExcelWorksheet worksheet, int row, int columnIndex)
    //{
    //    var base64ProfilePhoto = worksheet.Cells[row, columnIndex].Text.Trim();
    //    if (string.IsNullOrEmpty(base64ProfilePhoto))
    //        return null;

    //    var base64Data = base64ProfilePhoto.Split(',')[1];
    //    byte[] profilePhotoData = Convert.FromBase64String(base64Data);
    //    return Convert.ToBase64String(profilePhotoData);
    //}
}
