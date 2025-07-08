using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using AttendanceManagement.Infrastructure.Data;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Domain.Entities.Attendance;
namespace AttendanceManagement.Infrastructure.Infra
{
    public class AppraisalManagementInfra : IAppraisalManagementInfra
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly IEmailInfra _emailService;
        private readonly string _workspacePath;
        private readonly ILetterGenerationInfra _letterGenerationService;
        public AppraisalManagementInfra(AttendanceManagementSystemContext context, IEmailInfra emailService, IWebHostEnvironment env, ILetterGenerationInfra letterGenerationService)
        {
            _context = context;
            _emailService = emailService;
            _workspacePath = Path.Combine(env.ContentRootPath, "wwwroot\\UploadedExcel");
            if (!Directory.Exists(_workspacePath))
            {
                Directory.CreateDirectory(_workspacePath);
            }
            _letterGenerationService = letterGenerationService;
        }

        public async Task<List<object>> GetProductionEmployees(int appraisalId, int year, string quarter)
        {
            if (appraisalId == 1)
            {
                var grouped = await (
                    from staff in _context.StaffCreations
                    join division in _context.DivisionMasters on staff.DivisionId equals division.Id
                    join department in _context.DepartmentMasters on staff.DepartmentId equals department.Id
                    join manager in _context.StaffCreations on staff.ApprovalLevel1 equals manager.Id
                    join selected in _context.SelectedEmployeesForAppraisals.Where(s => s.AppraisalId == appraisalId) on staff.StaffId equals selected.EmployeeId into selectedJoin
                    from selected in selectedJoin.DefaultIfEmpty()
                    join per in _context.EmployeePerformanceReviews.Where(s => s.AppraisalId == appraisalId) on staff.StaffId equals per.EmpId into perJoin
                    from per in perJoin.DefaultIfEmpty()
                    join agm in _context.AgmApprovals on per.Id equals agm.EmployeePerformanceReviewId into agmJoin
                    from agm in agmJoin.DefaultIfEmpty()
                    where staff.IsActive == true
                          && division.IsActive
                          && department.IsActive
                          && !staff.IsNonProduction
                          && !(selected != null && selected.Year == year && selected.Quarter == quarter)
                    select new AppraisalDto
                    {
                        StaffId = staff.Id,
                        EmpId = staff.StaffId,
                        EmpName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                        Tenure = staff.Tenure,
                        ReportingManagers = $"{manager.FirstName}{(string.IsNullOrWhiteSpace(manager.LastName) ? "" : " " + manager.LastName)}",
                        Division = division.Name,
                        Department = department.Name,
                        IsCompleted = selected != null && selected.Year == year && selected.Quarter == quarter ? selected.IsCompleted : null
                    })
                    .ToListAsync();

                if (!grouped.Any())
                    throw new MessageNotFoundException("No employees found");

                return grouped.Cast<object>().ToList();
            }
            if (appraisalId == 2 || appraisalId == 3)
            {
                var grouped = await (
                    from staff in _context.StaffCreations
                    join division in _context.DivisionMasters on staff.DivisionId equals division.Id
                    join department in _context.DepartmentMasters on staff.DepartmentId equals department.Id
                    join manager in _context.StaffCreations on staff.ApprovalLevel1 equals manager.Id
                    join selected in _context.SelectedEmployeesForAppraisals.Where(s => s.AppraisalId == appraisalId) on staff.StaffId equals selected.EmployeeId into selectedJoin
                    from selected in selectedJoin.DefaultIfEmpty()
                    join per in _context.EmployeePerformanceReviews.Where(s => s.AppraisalId == appraisalId) on selected.EmployeeId equals per.EmpId into perJoin
                    from per in perJoin.DefaultIfEmpty()
                    join agm in _context.AgmApprovals on per.Id equals agm.EmployeePerformanceReviewId into agmJoin
                    from agm in agmJoin.DefaultIfEmpty()
                    where staff.IsActive == true && division.IsActive && department.IsActive && !staff.IsNonProduction && staff.OrganizationTypeId == 1
                          && (appraisalId == 2 ? staff.JoiningDate <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)) : true) && !(selected != null && selected.Year == year && selected.Quarter == quarter)
                    select new ProbationDto
                    {
                        StaffId = staff.Id,
                        EmpId = staff.StaffId,
                        EmpName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                        Tenure = staff.Tenure,
                        ReportingManagers = $"{manager.FirstName}{(string.IsNullOrWhiteSpace(manager.LastName) ? "" : " " + manager.LastName)}",
                        Division = division.Name,
                        Department = department.Name,
                        IsCompleted = selected != null && selected.Year == year && selected.Quarter == quarter ? selected.IsCompleted : null
                    })
                    .ToListAsync();
                if (grouped.Count == 0 || !grouped.Any()) throw new MessageNotFoundException("No employees found");
                return grouped.Cast<object>().ToList();
            }
            throw new MessageNotFoundException("Please choose a valid dropdown");
        }

        public async Task<string> MoveSelectedStaffToMis(SelectedEmployeesRequest selectedEmployeesRequest)
        {
            var employeeIds = selectedEmployeesRequest.SelectedRows.Select(x => x.EmpId).ToList();
            var alreadySelected = await _context.SelectedEmployeesForAppraisals
                .Where(x => employeeIds.Contains(x.EmployeeId) && x.AppraisalId == selectedEmployeesRequest.AppraisalId && !x.IsActive && x.IsCompleted == true
                            && (x.AppraisalId != 1 || (x.Year == selectedEmployeesRequest.Year && x.Quarter == selectedEmployeesRequest.Quarter)))
                .Select(x => x.EmployeeId)
                .ToListAsync();
            if (alreadySelected.Any())
            {
                throw new InvalidOperationException("Some selected employees are already moved to MIS");
            }
            var staffList = selectedEmployeesRequest.SelectedRows.Select(row => new SelectedEmployeesForAppraisal
            {
                AppraisalId = selectedEmployeesRequest.AppraisalId,
                EmployeeId = row.EmpId,
                EmployeeName = row.EmpName,
                TenureInYears = row.TenureInYears,
                ReportingManagers = row.ReportingManagers,
                Division = row.Division,
                Department = row.Department,
                Year = selectedEmployeesRequest.Year,
                Quarter = selectedEmployeesRequest.Quarter,
                IsCompleted = false,
                IsActive = true,
                CreatedBy = selectedEmployeesRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            }).ToList();
            await _context.SelectedEmployeesForAppraisals.AddRangeAsync(staffList);
            await _context.SaveChangesAsync();

            var apraisalDropDown = await _context.AppraisalSelectionDropDowns.FirstOrDefaultAsync(x => x.Id == selectedEmployeesRequest.AppraisalId && x.IsActive);
            if (apraisalDropDown == null) throw new MessageNotFoundException("Dropdown type not found");
            await _emailService.SendMailToMis(apraisalDropDown.Name, selectedEmployeesRequest.CreatedBy, staffList);
            return "Selected employees have been successfully moved to MIS";
        }

        public async Task<List<SelectedEmployeesResponseSelectedRows>> GetSelectedEmployees(int appraisalId, int year, string quarter)
        {
            var selectedEmployees = await (from staff in _context.SelectedEmployeesForAppraisals
                                           where staff.IsActive == true && staff.AppraisalId == appraisalId && staff.Year == year && staff.Quarter == quarter
                                           select new SelectedEmployeesResponseSelectedRows
                                           {
                                               EmpId = staff.EmployeeId,
                                               EmpName = staff.EmployeeName,
                                               TenureInYears = staff.TenureInYears,
                                               ReportingManagers = staff.ReportingManagers,
                                               Division = staff.Division,
                                               Department = staff.Department,
                                               Year = staff.Year,
                                               Quarter = staff.Quarter,
                                               IsCompleted = staff.IsCompleted
                                           }).ToListAsync();
            if (selectedEmployees == null || !selectedEmployees.Any())
            {
                throw new MessageNotFoundException("No employees found");
            }
            return selectedEmployees;
        }

/*        public async Task<byte[]> DownloadSelectedEmployeesExcel(int appraisalId)
        {
            var employees = await GetSelectedEmployees(appraisalId);
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Selected Employees");
            worksheet.Cells[1, 1].Value = "Emp ID";
            worksheet.Cells[1, 2].Value = "Emp Name";
            worksheet.Cells[1, 3].Value = "Tenure in years";
            worksheet.Cells[1, 4].Value = "Reporting Managers";
            worksheet.Cells[1, 5].Value = "Division";
            worksheet.Cells[1, 6].Value = "Department";

            for (int i = 0; i < employees.Count; i++)
            {
                var emp = employees[i];
                worksheet.Cells[i + 2, 1].Value = emp.EmpId;
                worksheet.Cells[i + 2, 2].Value = emp.EmpName;
                worksheet.Cells[i + 2, 3].Value = emp.TenureInYears;
                worksheet.Cells[i + 2, 4].Value = emp.ReportingManagers;
                worksheet.Cells[i + 2, 5].Value = emp.Division;
                worksheet.Cells[i + 2, 6].Value = emp.Department;
            }
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
        }
*/
        public async Task<string> MisUploadSheet(UploadMisSheetRequest uploadMisSheetRequest)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == uploadMisSheetRequest.CreatedBy && s.IsActive == true);
            if (staff == null) throw new MessageNotFoundException("Staff not found");
            var name = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
            var appraisal = await _context.AppraisalSelectionDropDowns.FirstOrDefaultAsync(a => a.Id == uploadMisSheetRequest.AppraisalId && a.IsActive);
            if (appraisal == null) throw new MessageNotFoundException("Drop down type not found");
            var fileExtension = Path.GetExtension(uploadMisSheetRequest.File.FileName);
            if (!string.Equals(fileExtension, ".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Please upload the correct Excel template File");
            }
            string uploadFileName = $"{Path.GetFileNameWithoutExtension(uploadMisSheetRequest.File.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            string uploadFilePath = Path.Combine(_workspacePath, uploadFileName);
            using (var fileStream = new FileStream(uploadFilePath, FileMode.Create))
            {
                await uploadMisSheetRequest.File.CopyToAsync(fileStream);
            }
            using (var stream = new MemoryStream())
            {
                await uploadMisSheetRequest.File.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null) throw new MessageNotFoundException("Worksheet not found in the uploaded file");
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(cell => cell.Text.Trim()).ToList();
                    var requiredHeaders = new List<string>();
                    requiredHeaders = new List<string> { "Emp ID", "Emp Name", "Tenure in years", "Reporting Managers", "Division", "Department", "Productivity %", "Quality %", "Present %", "Final %", "Grade", "Absent Days", "HR Comments" };
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
                    var employeePerformanceReviews = new List<EmployeePerformanceReview>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var empId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim();
                        if (string.IsNullOrEmpty(empId))
                        {
                            errorLogs.Add($"Staff Id is empty at {row}");
                            continue;
                        }
                        var staffs = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == empId && s.IsActive == true);
                        if (staffs == null)
                        {
                            errorLogs.Add($"Staff Id '{empId}' not found at row {row}");
                            continue;
                        }
                        var empName = worksheet.Cells[row, columnIndexes["Emp Name"]].Text.Trim();
                        if (string.IsNullOrEmpty(empId))
                        {
                            errorLogs.Add($"Staff Name is empty at {row}");
                            continue;
                        }
                        var tenureYears = decimal.TryParse(worksheet.Cells[row, columnIndexes["Tenure in years"]].Text.Trim(), out var tenure) ? tenure : 0;
                        var reportingManager = worksheet.Cells[row, columnIndexes["Reporting Managers"]].Text.Trim();
                        if (string.IsNullOrEmpty(empId))
                        {
                            errorLogs.Add($"Reporting Manager is empty at {row}");
                            continue;
                        }
                        var division = worksheet.Cells[row, columnIndexes["Division"]].Text.Trim();
                        if (string.IsNullOrEmpty(empId))
                        {
                            errorLogs.Add($"Division is empty at {row}");
                            continue;
                        }
                        var divisionName = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Name == division && d.IsActive);
                        if (divisionName == null)
                        {
                            errorLogs.Add($"Division {division} not found at {row}");
                        }
                        var department = worksheet.Cells[row, columnIndexes["Department"]].Text.Trim();
                        if (string.IsNullOrEmpty(empId))
                        {
                            errorLogs.Add($"Department is empty at {row}");
                            continue;
                        }
                        var departmentName = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Name == department && d.IsActive);
                        if (departmentName == null)
                        {
                            errorLogs.Add($"Department {department} not found at {row}");
                        }
                        var productivityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Productivity %"]].Text.Trim(), out var productivity) ? productivity : 0;
                        var qualityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Quality %"]].Text.Trim(), out var quality) ? quality : 0;
                        var presentPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Present %"]].Text.Trim(), out var present) ? present : 0;
                        var finalPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final %"]].Text.Trim(), out var final) ? final : 0;
                        var grade = worksheet.Cells[row, columnIndexes["Grade"]].Text.Trim();
                        var absentDays = int.TryParse(worksheet.Cells[row, columnIndexes["Absent Days"]].Text.Trim(), out var absent) ? absent : 0;
                        var hrComments = worksheet.Cells[row, columnIndexes["HR Comments"]].Text.Trim();
                        var employeeReview = new EmployeePerformanceReview
                        {
                            EmpId = empId,
                            EmpName = empName,
                            TenureInYears = tenureYears,
                            ReportingManagers = reportingManager,
                            Division = division,
                            Department = department,
                            ProductivityPercentage = productivityPercentage,
                            QualityPercentage = qualityPercentage,
                            PresentPercentage = presentPercentage,
                            FinalPercentage = finalPercentage,
                            Grade = grade,
                            AbsentDays = absentDays,
                            HrComments = hrComments,
                            AppraisalId = uploadMisSheetRequest.AppraisalId,
                            Year = uploadMisSheetRequest.Year,
                            Quarter = uploadMisSheetRequest.Quarter,
                            IsCompleted = false,
                            IsActive = true,
                            CreatedBy = uploadMisSheetRequest.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        employeePerformanceReviews.Add(employeeReview);
                    }
                    if (employeePerformanceReviews.Count > 0)
                    {
                        var empIds = employeePerformanceReviews.Select(x => x.EmpId).Distinct().ToList();
                        var alreadyUploaded = await _context.EmployeePerformanceReviews
                            .Where(x =>
                                empIds.Contains(x.EmpId) &&
                                x.AppraisalId == uploadMisSheetRequest.AppraisalId &&
                                x.Year == uploadMisSheetRequest.Year &&
                                x.Quarter == uploadMisSheetRequest.Quarter)
                            .Select(x => x.EmpId)
                            .Distinct()
                            .ToListAsync();
                        var rowsToProcess = employeePerformanceReviews.Where(r => !alreadyUploaded.Contains(r.EmpId)).ToList();
                        if (!rowsToProcess.Any())
                        {
                            throw new ConflictException($"All selected staff have already been uploaded for appraisal year {uploadMisSheetRequest.Year} and {uploadMisSheetRequest.Quarter}");
                        }
                        var newRecords = employeePerformanceReviews
                            .Where(x => !alreadyUploaded.Contains(x.EmpId))
                            .ToList();
                        var selectedEmployeesToUpdate = await _context.SelectedEmployeesForAppraisals
                            .Where(x => newRecords.Select(e => e.EmpId).Contains(x.EmployeeId)
                                        && x.AppraisalId == uploadMisSheetRequest.AppraisalId
                                        && x.IsActive)
                            .ToListAsync();
                        foreach (var selected in selectedEmployeesToUpdate)
                        {
                            selected.IsCompleted = true;
                            selected.IsActive = false;
                            selected.UpdatedBy = uploadMisSheetRequest.CreatedBy;
                            selected.UpdatedUtc = DateTime.UtcNow;
                        }
                        await _context.EmployeePerformanceReviews.AddRangeAsync(rowsToProcess);
                        await _context.SaveChangesAsync();

                        await _emailService.SendMisUploadNotificationToHr(uploadMisSheetRequest.CreatedBy, name, appraisal.Name);

                        if (alreadyUploaded.Any())
                        {
                            throw new ConflictException($"The following staff have already been uploaded for appraisal year {uploadMisSheetRequest.Year} and {uploadMisSheetRequest.Quarter}: " + string.Join(", ", alreadyUploaded));
                        }
                    }
                    else
                    {
                        if (errorLogs.Any())
                        {
                            throw new InvalidOperationException("Some records could not be processed: " + string.Join(", ", errorLogs));
                        }
                        else
                        {
                            throw new MessageNotFoundException("File is empty");
                        }
                    }
                }
            }
            return "Excel data imported successfully";
        }

        public async Task<List<PerformanceReviewResponse>> GetSelectedEmployeeReview(int appraisalId, int year, string quarter)
        {
            var selectedEmployees = await (from staff in _context.EmployeePerformanceReviews
                                           where staff.IsActive == true && staff.AppraisalId == appraisalId && staff.Year == year && staff.Quarter == quarter
                                           select new PerformanceReviewResponse
                                           {
                                               Id = staff.Id,
                                               EmpId = staff.EmpId,
                                               EmpName = staff.EmpName,
                                               TenureInYears = staff.TenureInYears,
                                               ReportingManagers = staff.ReportingManagers,
                                               Division = staff.Division,
                                               Department = staff.Department,
                                               ProductivityPercentage = staff.ProductivityPercentage,
                                               QualityPercentage = staff.QualityPercentage,
                                               PresentPercentage = staff.PresentPercentage,
                                               FinalPercentage = staff.FinalPercentage,
                                               Grade = staff.Grade,
                                               AbsentDays = staff.AbsentDays,
                                               Year = staff.Year,
                                               Quarter = staff.Quarter,
                                               IsCompleted = staff.IsCompleted
                                           }).ToListAsync();
            if (selectedEmployees == null || !selectedEmployees.Any())
            {
                throw new MessageNotFoundException("No employees found");
            }
            return selectedEmployees;
        }

        public async Task<List<AgmDetails>> GetAllAgm()
        {
            var allAgm = await (from agm in _context.StaffCreations
                                where agm.IsActive == true && agm.DesignationId == 65
                                select new AgmDetails
                                {
                                    Id = agm.Id,
                                    Name = $"{agm.FirstName}{(string.IsNullOrWhiteSpace(agm.LastName) ? "" : " " + agm.LastName)}"
                                })
                                .ToListAsync();
            if(allAgm == null || !allAgm.Any())
            {
                throw new MessageNotFoundException("No Agm found");
            }
            return allAgm;
        }

        public async Task<string> MoveToAgmApproval(AgmApprovalTab agmApprovalRequest)
        {
            var message = "Selected employees have been successfully moved to AGM approval";
            var selectedRows = agmApprovalRequest.SelectedRows;
            var selectedReviewIds = selectedRows.Select(r => r.Id).ToList();
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == agmApprovalRequest.AgmId && s.IsActive == true);
            if(staff == null) throw new MessageNotFoundException("AGM not found");
            var name = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
            var existingApprovalReviewIds = await _context.AgmApprovals
                 .Where(x =>
                     selectedReviewIds.Contains(x.EmployeePerformanceReviewId) &&
                     x.Year == agmApprovalRequest.Year &&
                     x.Quarter == agmApprovalRequest.Quarter)
                 .Select(x => x.EmployeePerformanceReviewId)
                 .ToListAsync();
            var rowsToProcess = selectedRows.Where(r => !existingApprovalReviewIds.Contains(r.Id)).ToList();
            if (!rowsToProcess.Any())
            {
                throw new InvalidOperationException("All selected employees have already been approved or rejected");
            }
            var newReviewIds = selectedReviewIds.Except(existingApprovalReviewIds).ToList();
            var newApprovals = newReviewIds.Select(id => new AgmApproval
            {
                EmployeePerformanceReviewId = id,
                AgmId = staff.Id,
                IsAgmApproved = false,
                Year = agmApprovalRequest.Year,
                Quarter = agmApprovalRequest.Quarter,
                IsActive = true,
                CreatedBy = agmApprovalRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            }).ToList();

            await _context.AgmApprovals.AddRangeAsync(newApprovals);
            var relatedReviews = await _context.EmployeePerformanceReviews
                .Where(x => newReviewIds.Contains(x.Id) && x.IsActive)
                .ToListAsync();
            foreach (var review in relatedReviews)
            {
                review.IsCompleted = true;
                review.IsActive = false;
                review.UpdatedBy = agmApprovalRequest.CreatedBy;
                review.UpdatedUtc = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            await _emailService.SendAgmApprovalNotification(agmApprovalRequest.CreatedBy, staff.OfficialEmail, name);

            if (existingApprovalReviewIds.Any())
            {
                throw new InvalidOperationException("Some selected employees are already moved to AGM approval");
            }

            return message;
        }

        public async Task<List<PerformanceReviewResponse>> GetSelectedEmployeeAgmApproval(int appraisalId, int year, string quarter)
        {
            var selectedEmployees = await (from staff in _context.AgmApprovals
                                           join per in _context.EmployeePerformanceReviews on staff.EmployeePerformanceReviewId equals per.Id
                                           where staff.IsActive == true && per.AppraisalId == appraisalId && per.Year == year && per.Quarter == quarter
                                           select new PerformanceReviewResponse
                                           {
                                               Id = staff.Id,
                                               EmpId = per.EmpId,
                                               EmpName = per.EmpName,
                                               TenureInYears = per.TenureInYears,
                                               ReportingManagers = per.ReportingManagers,
                                               Division = per.Division,
                                               Department = per.Department,
                                               ProductivityPercentage = per.ProductivityPercentage,
                                               QualityPercentage = per.QualityPercentage,
                                               PresentPercentage = per.PresentPercentage,
                                               FinalPercentage = per.FinalPercentage,
                                               Grade = per.Grade,
                                               AbsentDays = per.AbsentDays,
                                               Year = per.Year,
                                               Quarter = per.Quarter,
                                               IsCompleted = staff.IsAgmApproved
                                           }).ToListAsync();
            if (selectedEmployees == null || !selectedEmployees.Any())
            {
                throw new MessageNotFoundException("No employees found");
            }
            return selectedEmployees;
        }

        public async Task<string> AgmApproval(AgmApprovalRequest agmApprovalRequest)
        {
            var message = "AGM Approval has been successfully submitted";
            var selectedRows = agmApprovalRequest.SelectedRows;
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == agmApprovalRequest.ApprovedBy && s.IsActive == true);
            if (staff == null) throw new MessageNotFoundException("Staff not found");
            var name = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
            var selectedIds = selectedRows.Select(r => r.Id).ToList();
            var existingApprovals = await _context.AgmApprovals
                .Where(a => selectedIds.Contains(a.EmployeePerformanceReviewId) && a.Year == agmApprovalRequest.Year && a.Quarter == agmApprovalRequest.Quarter && a.IsActive && (a.IsAgmApproved == true || a.IsAgmApproved == false))
                .ToListAsync();
            var alreadyApprovedIds = existingApprovals.Select(a => a.EmployeePerformanceReviewId).ToHashSet();
            var rowsToProcess = selectedRows.Where(r => !alreadyApprovedIds.Contains(r.Id)).ToList();
            if (!rowsToProcess.Any())
            {
                throw new InvalidOperationException("All selected employees have already been approved or rejected");
            }
            foreach ( var row in rowsToProcess)
            {
                var employeeReview = await _context.AgmApprovals.FirstOrDefaultAsync(x => x.Id == row.Id && x.IsActive);
                if (employeeReview == null) throw new MessageNotFoundException($"Employee report not found");
                employeeReview.IsAgmApproved = agmApprovalRequest.IsApproved;
                employeeReview.Year = agmApprovalRequest.Year;
                employeeReview.Quarter = agmApprovalRequest.Quarter;
                employeeReview.IsActive = false;
                employeeReview.UpdatedBy = agmApprovalRequest.ApprovedBy;
                employeeReview.UpdatedUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            await _emailService.SendHrApprovalNotification(agmApprovalRequest.ApprovedBy, name);
            if (existingApprovals.Any())
            {
                throw new InvalidOperationException("Some selected employees have already been approved or rejected");
            }
            return message;
        }

        public async Task<string> GenerateAppraisalLetter(int createdBy, int year)
        {
            var yearAppraisalData = await _context.EmployeeAppraisalSheets.Where(s => s.AppraisalYear == year).ToListAsync();
            if (!yearAppraisalData.Any())
            {
                throw new MessageNotFoundException("No appraisal data found for the selected year");
            }
            if (yearAppraisalData.All(s => !s.IsActive))
            {
                throw new ConflictException("Appraisal data for the selected year is already generated");
            }
            var resultFilePaths = new List<string>();
            var appraisalSheets = yearAppraisalData.Where(s => s.IsActive).Distinct().ToList();
            foreach (var appraisalSheet in appraisalSheets)
            {
                var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == appraisalSheet.EmployeeCode && s.IsActive == true);
                if (staff == null) continue;
                string fileName = !string.IsNullOrWhiteSpace(appraisalSheet.RevisedDesignation)
                    ? $"Appraisal_Letter_With_DC_{appraisalSheet.EmployeeCode}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf"
                    : $"Appraisal_Letter_Without_DC_{appraisalSheet.EmployeeCode}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
                var designation = !string.IsNullOrWhiteSpace(appraisalSheet.RevisedDesignation) ? appraisalSheet.RevisedDesignation : appraisalSheet.Designation;
                var designationName = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.IsActive && d.Name == designation);
                if(designationName == null) continue;
                if (!string.IsNullOrWhiteSpace(appraisalSheet.RevisedDesignation))
                {
                    staff.DesignationId = designationName.Id;
                    await _context.SaveChangesAsync();
                }
                var appraisal = new AppraisalAnnexureResponse
                {
                    EmployeeCode = appraisalSheet.EmployeeCode,
                    EmployeeName = appraisalSheet.EmployeeName,
                    Designation = designation,
                    IsDesignationChange = !string.IsNullOrWhiteSpace(appraisalSheet.RevisedDesignation) ? true : false,
                    EmployeeSalutation = appraisalSheet.EmployeeSalutation,
                    Department = appraisalSheet.Department,
                    AppraisalYear = appraisalSheet.AppraisalYear,
                    BasicCurrentPerAnnum = appraisalSheet.BasicCurrentPerAnnum,
                    BasicCurrentPerMonth = appraisalSheet.BasicCurrentPerMonth,
                    BasicCurrentPerAnnumAfterApp = appraisalSheet.BasicCurrentPerAnnumAfterApp,
                    BasicCurrentPerMonthAfterApp = appraisalSheet.BasicCurrentPerMonthAfterApp,
                    HraperAnnum = appraisalSheet.HraperAnnum,
                    HraperMonth = appraisalSheet.HraperMonth,
                    HraperAnnumAfterApp = appraisalSheet.HraperAnnumAfterApp,
                    HraperMonthAfterApp = appraisalSheet.HraperMonthAfterApp,
                    ConveyancePerAnnum = appraisalSheet.ConveyancePerAnnum,
                    ConveyancePerMonth = appraisalSheet.ConveyancePerMonth,
                    ConveyancePerAnnumAfterApp = appraisalSheet.ConveyancePerAnnumAfterApp,
                    ConveyancePerMonthAfterApp = appraisalSheet.ConveyancePerMonthAfterApp,
                    MedicalAllowancePerAnnum = appraisalSheet.MedicalAllowancePerAnnum,
                    MedicalAllowancePerMonth = appraisalSheet.MedicalAllowancePerMonth,
                    MedicalAllowancePerAnnumAfterApp = appraisalSheet.MedicalAllowancePerAnnumAfterApp,
                    MedicalAllowancePerMonthAfterApp = appraisalSheet.MedicalAllowancePerMonthAfterApp,
                    SpecialAllowancePerAnnum = appraisalSheet.SpecialAllowancePerAnnum,
                    SpecialAllowancePerMonth = appraisalSheet.SpecialAllowancePerMonth,
                    SpecialAllowancePerAnnumAfterApp = appraisalSheet.SpecialAllowancePerAnnumAfterApp,
                    SpecialAllowancePerMonthAfterApp = appraisalSheet.SpecialAllowancePerMonthAfterApp,
                    GrossMonthCurrent = appraisalSheet.BasicCurrentPerMonth + (appraisalSheet.HraperMonth ?? 0) + (appraisalSheet.ConveyancePerMonth ?? 0)
                                    + (appraisalSheet.MedicalAllowancePerMonth ?? 0) + (appraisalSheet.SpecialAllowancePerMonth ?? 0),
                    GrossPerAnnumCurrent = appraisalSheet.BasicCurrentPerAnnum + (appraisalSheet.HraperAnnum ?? 0) + (appraisalSheet.ConveyancePerAnnum ?? 0)
                                    + (appraisalSheet.MedicalAllowancePerAnnum ?? 0) + (appraisalSheet.SpecialAllowancePerAnnum ?? 0),
                    GrossMonthAfterApp = appraisalSheet.BasicCurrentPerMonthAfterApp + (appraisalSheet.HraperMonthAfterApp ?? 0) + (appraisalSheet.ConveyancePerMonthAfterApp ?? 0)
                                    + (appraisalSheet.MedicalAllowancePerMonthAfterApp ?? 0) + (appraisalSheet.SpecialAllowancePerMonthAfterApp ?? 0),
                    GrossPerAnnumAfterApp = appraisalSheet.BasicCurrentPerAnnumAfterApp + (appraisalSheet.HraperAnnumAfterApp ?? 0) + (appraisalSheet.ConveyancePerAnnumAfterApp ?? 0)
                                    + (appraisalSheet.MedicalAllowancePerAnnumAfterApp ?? 0) + (appraisalSheet.SpecialAllowancePerAnnumAfterApp ?? 0),
                    EmployerPfcontributionPerAnnum = appraisalSheet.EmployerPfcontributionPerAnnum,
                    EmployerPfcontributionPerMonth = appraisalSheet.EmployerPfcontributionPerMonth,
                    EmployerPfcontributionPerAnnumAfterApp = appraisalSheet.EmployerPfcontributionPerAnnumAfterApp,
                    EmployerPfcontributionPerMonthAfterApp = appraisalSheet.EmployerPfcontributionPerMonthAfterApp,
                    EmployerEsicontributionPerAnnum = appraisalSheet.EmployerEsicontributionPerAnnum,
                    EmployerEsicontributionPerMonth = appraisalSheet.EmployerEsicontributionPerMonth,
                    EmployerEsicontributionPerAnnumAfterApp = appraisalSheet.EmployerEsicontributionPerAnnumAfterApp,
                    EmployerEsicontributionPerMonthAfterApp = appraisalSheet.EmployerEsicontributionPerMonthAfterApp,
                    EmployerGmcperAnnum = appraisalSheet.EmployerGmcperAnnum,
                    EmployerGmcperMonth = appraisalSheet.EmployerGmcperMonth,
                    EmployerGmcperAnnumAfterApp = appraisalSheet.EmployerGmcperAnnumAfterApp,
                    EmployerGmcperMonthAfterApp = appraisalSheet.EmployerGmcperMonthAfterApp,
                    GroupPersonalAccidentPerAnnum = appraisalSheet.GroupPersonalAccidentPerAnnum,
                    GroupPersonalAccidentPerMonth = appraisalSheet.GroupPersonalAccidentPerMonth,
                    GroupPersonalAccidentPerAnnumAfterApp = appraisalSheet.GroupPersonalAccidentPerAnnumAfterApp,
                    GroupPersonalAccidentPerMonthAfterApp = appraisalSheet.GroupPersonalAccidentPerMonthAfterApp,
                    CtcMonthCurrent = appraisalSheet.BasicCurrentPerMonth + (appraisalSheet.HraperMonth ?? 0) + (appraisalSheet.ConveyancePerMonth ?? 0)
                                    + (appraisalSheet.MedicalAllowancePerMonth ?? 0) + (appraisalSheet.SpecialAllowancePerMonth ?? 0) + (appraisalSheet.EmployerPfcontributionPerMonth ?? 0)
                                    + (appraisalSheet.EmployerEsicontributionPerMonth ?? 0) + (appraisalSheet.EmployerGmcperMonth ?? 0) + (appraisalSheet.GroupPersonalAccidentPerMonth ?? 0),
                    CtcPerAnnumCurrent = appraisalSheet.BasicCurrentPerAnnum + (appraisalSheet.HraperAnnum ?? 0) + (appraisalSheet.ConveyancePerAnnum ?? 0)
                                    + (appraisalSheet.MedicalAllowancePerAnnum ?? 0) + (appraisalSheet.SpecialAllowancePerAnnum ?? 0) + (appraisalSheet.EmployerPfcontributionPerAnnum ?? 0)
                                    + (appraisalSheet.EmployerEsicontributionPerAnnum ?? 0) + (appraisalSheet.EmployerGmcperAnnum ?? 0) + (appraisalSheet.GroupPersonalAccidentPerAnnum ?? 0),
                    CtcMonthAfterApp = appraisalSheet.BasicCurrentPerMonthAfterApp + (appraisalSheet.HraperMonthAfterApp ?? 0) + (appraisalSheet.ConveyancePerMonthAfterApp ?? 0)
                                    + (appraisalSheet.MedicalAllowancePerMonthAfterApp ?? 0) + (appraisalSheet.SpecialAllowancePerMonthAfterApp ?? 0) + (appraisalSheet.EmployerPfcontributionPerMonthAfterApp ?? 0)
                                    + (appraisalSheet.EmployerEsicontributionPerMonthAfterApp ?? 0) + (appraisalSheet.EmployerGmcperMonthAfterApp ?? 0) + (appraisalSheet.GroupPersonalAccidentPerMonthAfterApp ?? 0),
                    CtcPerAnnumAfterApp = appraisalSheet.BasicCurrentPerAnnumAfterApp + (appraisalSheet.HraperAnnumAfterApp ?? 0) + (appraisalSheet.ConveyancePerAnnumAfterApp ?? 0)
                                    + (appraisalSheet.MedicalAllowancePerAnnumAfterApp ?? 0) + (appraisalSheet.SpecialAllowancePerAnnumAfterApp ?? 0) + (appraisalSheet.EmployerPfcontributionPerAnnumAfterApp ?? 0)
                                    + (appraisalSheet.EmployerEsicontributionPerAnnumAfterApp ?? 0) + (appraisalSheet.EmployerGmcperAnnumAfterApp ?? 0) + (appraisalSheet.GroupPersonalAccidentPerAnnumAfterApp ?? 0),
                    EmployeePfcontributionPerAnnum = appraisalSheet.EmployeePfcontributionPerAnnum,
                    EmployeePfcontributionPerMonth = appraisalSheet.EmployeePfcontributionPerMonth,
                    EmployeePfcontributionPerAnnumAfterApp = appraisalSheet.EmployeePfcontributionPerAnnumAfterApp,
                    EmployeePfcontributionPerMonthAfterApp = appraisalSheet.EmployeePfcontributionPerMonthAfterApp,
                    EmployeeEsicontributionPerAnnum = appraisalSheet.EmployeeEsicontributionPerAnnum,
                    EmployeeEsicontributionPerMonth = appraisalSheet.EmployeeEsicontributionPerMonth,
                    EmployeeEsicontributionPerAnnumAfterApp = appraisalSheet.EmployeeEsicontributionPerAnnumAfterApp,
                    EmployeeEsicontributionPerMonthAfterApp = appraisalSheet.EmployeeEsicontributionPerMonthAfterApp,
                    ProfessionalTaxPerAnnum = appraisalSheet.ProfessionalTaxPerAnnum,
                    ProfessionalTaxPerMonth = appraisalSheet.ProfessionalTaxPerMonth,
                    ProfessionalTaxPerAnnumAfterApp = appraisalSheet.ProfessionalTaxPerAnnumAfterApp,
                    ProfessionalTaxPerMonthAfterApp = appraisalSheet.ProfessionalTaxPerMonthAfterApp,
                    GmcperAnnum = appraisalSheet.GmcperAnnum,
                    GmcperMonth = appraisalSheet.GmcperMonth,
                    GmcperAnnumAfterApp = appraisalSheet.GmcperAnnumAfterApp,
                    GmcperMonthAfterApp = appraisalSheet.GmcperMonthAfterApp,
                    NetTakeHomeMonthCurrent = (appraisalSheet.BasicCurrentPerMonth + (appraisalSheet.HraperMonth ?? 0) + (appraisalSheet.ConveyancePerMonth ?? 0)
                                   + (appraisalSheet.MedicalAllowancePerMonth ?? 0) + (appraisalSheet.SpecialAllowancePerMonth ?? 0)) - ((appraisalSheet.EmployeePfcontributionPerMonth ?? 0)
                                   + (appraisalSheet.EmployeeEsicontributionPerMonth ?? 0) + (appraisalSheet.ProfessionalTaxPerMonth ?? 0) + (appraisalSheet.GmcperMonth ?? 0)),
                    NetTakeHomePerAnnumCurrent = (appraisalSheet.BasicCurrentPerAnnum + (appraisalSheet.HraperAnnum ?? 0) + (appraisalSheet.ConveyancePerAnnum ?? 0)
                                   + (appraisalSheet.MedicalAllowancePerAnnum ?? 0) + (appraisalSheet.SpecialAllowancePerAnnum ?? 0)) - ((appraisalSheet.EmployeePfcontributionPerAnnum ?? 0)
                                   + (appraisalSheet.EmployeeEsicontributionPerAnnum ?? 0) + (appraisalSheet.ProfessionalTaxPerAnnum ?? 0) + (appraisalSheet.GmcperAnnum ?? 0)),
                    NetTakeHomeMonthAfterApp = (appraisalSheet.BasicCurrentPerMonthAfterApp + (appraisalSheet.HraperMonthAfterApp ?? 0) + (appraisalSheet.ConveyancePerMonthAfterApp ?? 0)
                                   + (appraisalSheet.MedicalAllowancePerMonthAfterApp ?? 0) + (appraisalSheet.SpecialAllowancePerMonthAfterApp ?? 0)) - ((appraisalSheet.EmployeePfcontributionPerMonthAfterApp ?? 0)
                                   + (appraisalSheet.EmployeeEsicontributionPerMonthAfterApp ?? 0) + (appraisalSheet.ProfessionalTaxPerMonthAfterApp ?? 0) + (appraisalSheet.GmcperMonthAfterApp ?? 0)),
                    NetTakeHomePerAnnumAfterApp = (appraisalSheet.BasicCurrentPerAnnumAfterApp + (appraisalSheet.HraperAnnumAfterApp ?? 0) + (appraisalSheet.ConveyancePerAnnumAfterApp ?? 0)
                                   + (appraisalSheet.MedicalAllowancePerAnnumAfterApp ?? 0) + (appraisalSheet.SpecialAllowancePerAnnumAfterApp ?? 0)) - ((appraisalSheet.EmployeePfcontributionPerAnnumAfterApp ?? 0)
                                   + (appraisalSheet.EmployeeEsicontributionPerAnnumAfterApp ?? 0) + (appraisalSheet.ProfessionalTaxPerAnnumAfterApp ?? 0) + (appraisalSheet.GmcperAnnumAfterApp ?? 0)),
                    TotalAppraisal = appraisalSheet.TotalAppraisal
                };
                if (appraisal == null) throw new MessageNotFoundException("Appraisal sheet not found");
                var file = _letterGenerationService.GenerateAppraisalLetterPdf(appraisal, fileName);
                byte[] pdfBytes = System.IO.File.ReadAllBytes(file);
                string base64Pdf = Convert.ToBase64String(pdfBytes);
                appraisalSheet.IsActive = false; 
                var letterGeneration = new LetterGeneration
                {
                    LetterPath = file,
                    LetterContent = Convert.FromBase64String(base64Pdf),
                    StaffCreationId = staff.Id,
                    FileName = fileName,
                    CreatedBy = createdBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsActive = true
                };
                await _context.LetterGenerations.AddAsync(letterGeneration);
                await _context.SaveChangesAsync();

                await _emailService.SendAppraisalEmailAsync(staff.PersonalEmail, appraisal.EmployeeName, pdfBytes, fileName, createdBy);
                appraisalSheet.IsActive = false;
                appraisalSheet.UpdatedBy = createdBy;
                appraisalSheet.UpdatedUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return "Appraisal letter generated and email sent successfully";
        }

        public async Task<(Stream PdfStream, string FileName)> ViewAppraisalLetter(int staffId)
        {
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            string filePrefix = $"Appraisal_Letter_Without_DC_{staff.StaffId}_" ?? $"Appraisal_Letter_With_DC_{staff.StaffId}";
            var letterGeneration = await _context.LetterGenerations
                 .Where(x => x.FileName.StartsWith(filePrefix) && x.IsActive)
                 .OrderByDescending(x => x.CreatedUtc)
                 .FirstOrDefaultAsync();
            if (letterGeneration == null)
            {
                throw new FileNotFoundException("Appraisal letter not found");
            }
            var filePath = letterGeneration.LetterPath;
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("PDF file not found at the specified path");
            }
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = Path.GetFileName(filePath);
            return (stream, fileName);
        }

        public async Task<string> DownloadAppraisalLetter(int staffId)
        {
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            string filePrefix = $"Appraisal_Letter_Without_DC_{staff.StaffId}_" ?? $"Appraisal_Letter_With_DC_{staff.StaffId}";
            var letterGeneration = await _context.LetterGenerations
                .Where(x => x.FileName.StartsWith(filePrefix) && x.IsActive)
                .OrderByDescending(x => x.CreatedUtc)
                .FirstOrDefaultAsync();

            if (letterGeneration == null)
            {
                throw new MessageNotFoundException("Appraisal letter not found");
            }
            string file = letterGeneration.FileName ?? string.Empty;
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new MessageNotFoundException("File is empty");
            }
            return Path.Combine(_workspacePath, file);
        }

        public async Task<string> AcceptAppraisalLetter(LetterAcceptance letterAcceptance)
        {
            var letter = await _context.LetterGenerations.FirstOrDefaultAsync(x => x.Id == letterAcceptance.Id && x.IsActive);
            if (letter == null) throw new MessageNotFoundException("Appraisal letter not found");
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(x => x.Id == letterAcceptance.AcceptedBy && x.IsActive == true);
            if(staff == null) throw new MessageNotFoundException("Staff not found");
            var empId = staff.StaffId;
            var empName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
            var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Id == staff.DepartmentId && d.IsActive);
            if (department == null) throw new MessageNotFoundException("Department not found");
            var division = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Id == staff.DivisionId && d.IsActive);
            if (division == null) throw new MessageNotFoundException("Division not found");
            var latestAcceptance = await _context.EmployeeAcceptances
                .Where(x => x.FileId == letterAcceptance.Id && x.EmpId == empId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            if (latestAcceptance != null)
            {
                if (latestAcceptance.IsAccepted) throw new InvalidOperationException("You have already accepted");
                else throw new InvalidOperationException("You have already rejected");
            }
            var appraisal = new EmployeeAcceptance
            {
                EmpId = empId,
                EmpName = empName,
                Department = department.Name,
                Division = division.Name,
                FileId = letterAcceptance.Id,
                IsAccepted = letterAcceptance.IsAccepted,
                IsActive = true,
                CreatedBy = letterAcceptance.AcceptedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.AddAsync(appraisal);
            await _context.SaveChangesAsync();
            return letterAcceptance.IsAccepted ? "Appraisal letter has been successfully accepted" : "Appraisal letter has been successfully rejected";
        }

        public async Task<List<LetterAcceptanceResponse>> GetAcceptedEmployees(int year)
        {
            var allAcceptances = await _context.EmployeeAcceptances
                .Where(emp => emp.IsActive && emp.CreatedUtc.Year == year)
                .ToListAsync();

            var acceptedEmployees = allAcceptances
                .GroupBy(emp => emp.EmpId)
                .Select(g => g.OrderByDescending(e => e.Id).First())
                .Select(emp => new LetterAcceptanceResponse
                {
                    EmpId = emp.EmpId,
                    EmpName = emp.EmpName,
                    Division = emp.Division,
                    Department = emp.Department,
                    IsAccepted = emp.IsAccepted
                })
                .ToList(); if (acceptedEmployees == null || !acceptedEmployees.Any())
                {
                    throw new MessageNotFoundException("No employees found");
                }
            return acceptedEmployees;
        }

        /*        public async Task<byte[]> DownloadEmployeesPerformanceReportExcel(int appraisalId)
                {
                    var employees = await GetSelectedEmployeeReview(appraisalId);
                    using var package = new ExcelPackage();
                    var worksheet = package.Workbook.Worksheets.Add("Employees Performance Report");
                    worksheet.Cells[1, 1].Value = "Emp ID";
                    worksheet.Cells[1, 2].Value = "Emp Name";
                    worksheet.Cells[1, 3].Value = "Tenure in years";
                    worksheet.Cells[1, 4].Value = "Reporting Managers";
                    worksheet.Cells[1, 5].Value = "Division";
                    worksheet.Cells[1, 6].Value = "Department";
                    worksheet.Cells[1, 7].Value = "Productivity %";
                    worksheet.Cells[1, 8].Value = "Quality %";
                    worksheet.Cells[1, 9].Value = "Present %";
                    worksheet.Cells[1, 10].Value = "Final %";
                    worksheet.Cells[1, 11].Value = "Grade";
                    worksheet.Cells[1, 12].Value = "Absent Days";

                    for (int i = 0; i < employees.Count; i++)
                    {
                        var emp = employees[i];
                        worksheet.Cells[i + 2, 1].Value = emp.EmpId;
                        worksheet.Cells[i + 2, 2].Value = emp.EmpName;
                        worksheet.Cells[i + 2, 3].Value = emp.TenureInYears;
                        worksheet.Cells[i + 2, 4].Value = emp.ReportingManagers;
                        worksheet.Cells[i + 2, 5].Value = emp.Division;
                        worksheet.Cells[i + 2, 6].Value = emp.Department;
                        worksheet.Cells[i + 2, 7].Value = $"{emp.ProductivityPercentage}%";
                        worksheet.Cells[i + 2, 8].Value = $"{emp.QualityPercentage}%";
                        worksheet.Cells[i + 2, 9].Value = $"{emp.PresentPercentage}%";
                        worksheet.Cells[i + 2, 10].Value = $"{emp.FinalPercentage}%";
                        worksheet.Cells[i + 2, 11].Value = emp.Grade;
                        worksheet.Cells[i + 2, 12].Value = emp.AbsentDays;
                    }
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    return package.GetAsByteArray();
                }
        */

        public async Task<List<object>> GetNonProductionEmployees(int appraisalId, int year, string quarter)
        {
            if (appraisalId == 1)
            {
                var grouped = await (
                    from staff in _context.StaffCreations
                    join division in _context.DivisionMasters on staff.DivisionId equals division.Id
                    join department in _context.DepartmentMasters on staff.DepartmentId equals department.Id
                    join manager in _context.StaffCreations on staff.ApprovalLevel1 equals manager.Id
                    join selected in _context.SelectedNonProductionEmployees.Where(s => s.AppraisalId == appraisalId)
                        on staff.StaffId equals selected.EmployeeId into selectedJoin
                    from selected in selectedJoin.DefaultIfEmpty()
                    join per in _context.NonProductionEmployeePerformanceReviews.Where(s => s.AppraisalId == appraisalId)
                        on staff.StaffId equals per.EmpId into perJoin
                    from per in perJoin.DefaultIfEmpty()
                    where staff.IsActive == true
                          && division.IsActive
                          && department.IsActive
                          && staff.IsNonProduction
                          && !(selected != null && selected.Year == year && selected.Quarter == quarter)
                    select new AppraisalDto
                    {
                        StaffId = staff.Id,
                        EmpId = staff.StaffId,
                        EmpName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                        Tenure = staff.Tenure,
                        ReportingManagers = $"{manager.FirstName}{(string.IsNullOrWhiteSpace(manager.LastName) ? "" : " " + manager.LastName)}",
                        Division = division.Name,
                        Department = department.Name,
                        IsCompleted = selected != null && selected.Year == year && selected.Quarter == quarter ? selected.IsCompleted : null
                    })
                    .ToListAsync();

                if (!grouped.Any())
                    throw new MessageNotFoundException("No employees found");

                return grouped.Cast<object>().ToList();
            }
            if (appraisalId == 2 || appraisalId == 3)
            {
                var grouped = await (
                    from staff in _context.StaffCreations
                    join division in _context.DivisionMasters on staff.DivisionId equals division.Id
                    join department in _context.DepartmentMasters on staff.DepartmentId equals department.Id
                    join manager in _context.StaffCreations on staff.ApprovalLevel1 equals manager.Id
                    join selected in _context.SelectedNonProductionEmployees.Where(s => s.AppraisalId == appraisalId) on staff.StaffId equals selected.EmployeeId into selectedJoin
                    from selected in selectedJoin.DefaultIfEmpty()
                    join per in _context.NonProductionEmployeePerformanceReviews.Where(s => s.AppraisalId == appraisalId) on selected.EmployeeId equals per.EmpId into perJoin
                    from per in perJoin.DefaultIfEmpty()
                    where staff.IsActive == true && division.IsActive && department.IsActive && staff.IsNonProduction && staff.OrganizationTypeId == 1
                          && (appraisalId == 2 ? staff.JoiningDate <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)) : true) && !(selected != null && selected.Year == year && selected.Quarter == quarter)
                    select new ProbationDto
                    {
                        StaffId = staff.Id,
                        EmpId = staff.StaffId,
                        EmpName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                        Tenure = staff.Tenure,
                        ReportingManagers = $"{manager.FirstName}{(string.IsNullOrWhiteSpace(manager.LastName) ? "" : " " + manager.LastName)}",
                        Division = division.Name,
                        Department = department.Name,
                        IsCompleted = selected != null && selected.Year == year && selected.Quarter == quarter ? selected.IsCompleted : null
                    })
                    .ToListAsync();
                if (grouped.Count == 0 || !grouped.Any()) throw new MessageNotFoundException("No employees found");
                return grouped.Cast<object>().ToList();
            }
            throw new MessageNotFoundException("Please choose a valid dropdown");
        }

        public async Task<string> MoveSelectedStaff(SelectedEmployeesRequest selectedEmployeesRequest)
        {
            var employeeIds = selectedEmployeesRequest.SelectedRows.Select(x => x.EmpId).ToList();
            var alreadySelected = await _context.SelectedNonProductionEmployees
                .Where(x => employeeIds.Contains(x.EmployeeId) && x.AppraisalId == selectedEmployeesRequest.AppraisalId && !x.IsActive && x.IsCompleted == true
                            && (x.AppraisalId != 1 || (x.Year == selectedEmployeesRequest.Year && x.Quarter == selectedEmployeesRequest.Quarter)))
                .Select(x => x.EmployeeId)
                .ToListAsync();
            var rowsToInsert = selectedEmployeesRequest.SelectedRows
            .Where(row => !alreadySelected.Contains(row.EmpId))
            .ToList();
            if (!rowsToInsert.Any())
            {
                throw new InvalidOperationException("All selected employees have already been moved.");
            }
            var staffList = rowsToInsert.Select(row => new SelectedNonProductionEmployee
            {
                AppraisalId = selectedEmployeesRequest.AppraisalId,
                EmployeeId = row.EmpId,
                EmployeeName = row.EmpName,
                TenureInYears = row.TenureInYears,
                ReportingManagers = row.ReportingManagers,
                Division = row.Division,
                Department = row.Department,
                Year = selectedEmployeesRequest.Year,
                Quarter = selectedEmployeesRequest.Quarter,
                IsCompleted = false,
                IsActive = true,
                CreatedBy = selectedEmployeesRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            }).ToList();
            await _context.SelectedNonProductionEmployees.AddRangeAsync(staffList);
            await _context.SaveChangesAsync();

            if (alreadySelected.Any())
            {
                throw new InvalidOperationException("Some selected employees are already moved");
            }

            return "Selected employees have been successfully moved";
        }

        public async Task<List<SelectedEmployeesResponseSelectedRows>> GetSelectedNonProductionEmployees(int appraisalId, int year, string quarter)
        {
            var selectedEmployees = await (from staff in _context.SelectedNonProductionEmployees
                                           where staff.IsCompleted == false && staff.AppraisalId == appraisalId && staff.Year == year && staff.Quarter == quarter
                                           select new SelectedEmployeesResponseSelectedRows
                                           {
                                               EmpId = staff.EmployeeId,
                                               EmpName = staff.EmployeeName,
                                               TenureInYears = staff.TenureInYears,
                                               ReportingManagers = staff.ReportingManagers,
                                               Division = staff.Division,
                                               Department = staff.Department,
                                               Year = staff.Year,
                                               Quarter = staff.Quarter,
                                               IsCompleted = staff.IsCompleted
                                           }).ToListAsync();
            if (selectedEmployees == null || !selectedEmployees.Any())
            {
                throw new MessageNotFoundException("No employees found");
            }
            return selectedEmployees;
        }

        public async Task<string> CreateKra(KraDto kraDto)
        {
            var selectedRows = kraDto.SelectedRows;
            foreach (var item in selectedRows)
            {
                foreach (var staffId in item.StaffId)
                {
                    var kra = new Goal
                    {
                        Kra = item.Kra,
                        Weightage = item.Weightage,
                        EvaluationPeriod = item.Quarter,
                        Year = item.Year,
                        Quarter = item.Quarter,
                        StaffId = staffId,
                        AppraisalId = item.AppraisalId,
                        IsActive = true,
                        CreatedBy = kraDto.CreatedBy,
                        CreatedUtc = DateTime.UtcNow
                    };

                    await _context.Goals.AddAsync(kra);
                }
            }
            await _context.SaveChangesAsync();
            return "KRA submitted successfully";
        }

        public async Task<List<KraResponse>> GetKra(int createdBy, int appraisalId, int year, string quarter)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == createdBy && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";
            var getKra = await (from kra in _context.Goals
                                join s in _context.StaffCreations on kra.CreatedBy equals s.Id
                                where kra.Year == year && kra.Quarter == quarter && kra.AppraisalId == appraisalId && (isSuperAdmin || kra.CreatedBy == createdBy || kra.StaffId == createdBy)
                                select new KraResponse
                                {
                                    Id = kra.Id,
                                    Kra = kra.Kra,
                                    Weightage = kra.Weightage,
                                    Year = kra.Year,
                                    Quarter = kra.Quarter,
                                    CreatedBy = kra.CreatedBy
                                })
                                .ToListAsync();

            if (getKra.Count == 0) throw new MessageNotFoundException("No kra found");
            return getKra;
        }

        public async Task<string> CreateSelfEvaluation(SelfEvaluationRequest selfEvaluationRequest)
        {
            var selectedRows = selfEvaluationRequest.SelectedRows;
            foreach(var item in selectedRows)
            {
                string? savedFilePath = null;
                if (item.AttachmentsSelf != null && item.AttachmentsSelf.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "KraAttachments", "KraSelfAttachments");
                    Directory.CreateDirectory(uploadsFolder);
                    var originalFileName = Path.GetFileNameWithoutExtension(item.AttachmentsSelf.FileName);
                    var extension = Path.GetExtension(item.AttachmentsSelf.FileName);
                    var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    var uniqueFileName = $"{originalFileName}_{timestamp}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.AttachmentsSelf.CopyToAsync(stream);
                    }
                    savedFilePath = Path.Combine("KraAttachments", "KraSelfAttachments", uniqueFileName);
                }
                var selfEvaluation = new KraSelfReview
                {
                    GoalId = item.GoalId,
                    SelfEvaluationScale = item.SelfEvaluationScale,
                    SelfScore = item.SelfScore,
                    SelfEvaluationComments = item.SelfEvaluationComments,
                    AttachmentsSelf = savedFilePath,
                    AppraisalId = item.AppraisalId,
                    IsActive = true,
                    CreatedBy = selfEvaluationRequest.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                await _context.KraSelfReviews.AddAsync(selfEvaluation);
            }
            await _context.SaveChangesAsync();
            return "Self Kra submitted successfully";
        }

        public async Task<List<SelfEvaluationResponse>> GetSelfEvaluation(int approverId, int appraisalId, int year, string quarter)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == approverId && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";
            var isApprovalLevel1 = await _context.StaffCreations.AnyAsync(s => s.ApprovalLevel1 == approverId && s.IsActive == true);
            var isApprovalLevel2 = await _context.StaffCreations.AnyAsync(s => s.ApprovalLevel2 == approverId && s.IsActive == true);

            var getSelfEvaluation = await (from selfEvaluation in _context.KraSelfReviews
                                           join goal in _context.Goals on selfEvaluation.GoalId equals goal.Id
                                           join s in _context.StaffCreations on selfEvaluation.CreatedBy equals s.Id
                                           where selfEvaluation.AppraisalId == appraisalId && goal.Year == year && goal.Quarter == quarter && (isSuperAdmin || selfEvaluation.CreatedBy == approverId ||                         // Direct creator
                                                 (isApprovalLevel1 && s.ApprovalLevel1 == approverId) || (isApprovalLevel2 && s.ApprovalLevel2 == approverId))
                                           select new SelfEvaluationResponse
                                           {
                                               Id = selfEvaluation.Id,
                                               GoalId = selfEvaluation.GoalId,
                                               SelfEvaluationScale = selfEvaluation.SelfEvaluationScale,
                                               SelfScore = selfEvaluation.SelfScore,
                                               SelfEvaluationComments = selfEvaluation.SelfEvaluationComments,
                                               AttachmentsSelf = selfEvaluation.AttachmentsSelf,
                                               StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                               Year = goal.Year,
                                               Quarter = goal.Quarter,
                                               CreatedBy = selfEvaluation.CreatedBy
                                           })
                                           .ToListAsync();
            if (getSelfEvaluation.Count == 0) throw new MessageNotFoundException("No staff evaluation found");
            return getSelfEvaluation;
        }
        public async Task<string> CreateManagerEvaluation(ManagerEvaluationRequest managerEvaluationRequest)
        {
            var selectedRows = managerEvaluationRequest.SelectedRows;
            foreach (var item in selectedRows)
            {
                string? savedFilePath = null;
                if (item.AttachmentsManager != null && item.AttachmentsManager.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "KraAttachments", "KraManagerAttachments");
                    Directory.CreateDirectory(uploadsFolder);
                    var originalFileName = Path.GetFileNameWithoutExtension(item.AttachmentsManager.FileName);
                    var extension = Path.GetExtension(item.AttachmentsManager.FileName);
                    var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    var uniqueFileName = $"{originalFileName}_{timestamp}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.AttachmentsManager.CopyToAsync(stream);
                    }
                    savedFilePath = Path.Combine("KraAttachments", "KraManagerAttachments", uniqueFileName);
                }
                var managerEvaluation = new KraManagerReview
                {
                    KraSelfReviewId = item.KraSelfReviewId,
                    ManagerEvaluationScale = item.ManagerEvaluationScale,
                    ManagerScore = item.ManagerScore,
                    ManagerEvaluationComments = item.ManagerEvaluationComments,
                    AttachmentsManager = savedFilePath,
                    IsCompleted = true,
                    AppraisalId = item.AppraisalId,
                    IsActive = true,
                    CreatedBy = managerEvaluationRequest.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                await _context.KraManagerReviews.AddAsync(managerEvaluation);
            }
            await _context.SaveChangesAsync();
            return "Self Kra submitted successfully";
        }

        public async Task<List<ManagerEvaluationResponse>> GetManagerEvaluation(int createdBy, int appraisalId, int year, string quarter)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == createdBy && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";
            var getManagerEvaluation = await (from managerEvaluation in _context.KraManagerReviews
                                              join selfKra in _context.KraSelfReviews on managerEvaluation.KraSelfReviewId equals selfKra.Id
                                              join goal in _context.Goals on selfKra.GoalId equals goal.Id
                                              join s in _context.StaffCreations on managerEvaluation.CreatedBy equals s.Id
                                              where managerEvaluation.AppraisalId == appraisalId && goal.Year == year && goal.Quarter == quarter && (isSuperAdmin || managerEvaluation.CreatedBy == createdBy || goal.StaffId == createdBy)
                                              select new ManagerEvaluationResponse
                                              {
                                                  Id = managerEvaluation.Id,
                                                  KraSelfReviewId = managerEvaluation.KraSelfReviewId,
                                                  ManagerEvaluationScale = managerEvaluation.ManagerEvaluationScale,
                                                  ManagerScore = managerEvaluation.ManagerScore,
                                                  ManagerEvaluationComments = managerEvaluation.ManagerEvaluationComments,
                                                  AttachmentsManager = managerEvaluation.AttachmentsManager,
                                                  IsCompleted = managerEvaluation.IsCompleted,
                                                  ManagerName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                                  Year = goal.Year,
                                                  Quarter = goal.Quarter,
                                                  CreatedBy = managerEvaluation.CreatedBy
                                              })
                                           .ToListAsync();
            if (getManagerEvaluation.Count == 0) throw new MessageNotFoundException("No manager evaluation found");
            return getManagerEvaluation;
        }

        public async Task<object> GetFinalAverageManagerScore(int createdBy, int appraisalId, int year, string? quarter)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == createdBy && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();

            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN" || approver == "SITE HR";

            var managerEvaluations = await (
                from managerEvaluation in _context.KraManagerReviews
                join selfEvaluation in _context.KraSelfReviews on managerEvaluation.KraSelfReviewId equals selfEvaluation.Id
                join goal in _context.Goals on selfEvaluation.GoalId equals goal.Id
                join staff in _context.StaffCreations on selfEvaluation.CreatedBy equals staff.Id
                join manager in _context.StaffCreations on managerEvaluation.CreatedBy equals manager.Id
                where managerEvaluation.IsActive
                      && managerEvaluation.AppraisalId == appraisalId
                      && goal.Year == year
                      && (string.IsNullOrEmpty(quarter) || goal.Quarter == quarter)
                      && (isSuperAdmin || managerEvaluation.CreatedBy == createdBy || goal.StaffId == createdBy)
                select new
                {
                    EmpId = staff.StaffId,
                    EmpName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                    ManagerKraScore = managerEvaluation.ManagerScore,
                    ReportingManagerId = manager.StaffId,
                    ReportingManagerName = $"{manager.FirstName}{(string.IsNullOrWhiteSpace(manager.LastName) ? "" : " " + manager.LastName)}",
                    Year = goal.Year,
                    Quarter = goal.Quarter
                }).ToListAsync();

            if (!managerEvaluations.Any())
                throw new MessageNotFoundException("No manager evaluation found");

            var staffWiseScores = managerEvaluations
                .GroupBy(e => new { e.EmpId, e.EmpName })
                .Select(group => new
                {
                    EmpId = group.Key.EmpId,
                    EmpName = group.Key.EmpName,
                    AverageManagerScore = group.Average(x => x.ManagerKraScore),
                    Evaluations = group.ToList()
                })
                .ToList();

            return staffWiseScores;
        }

        public async Task<string> HrUploadSheet(UploadMisSheetRequest uploadMisSheetRequest)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == uploadMisSheetRequest.CreatedBy && s.IsActive == true);
            if (staff == null) throw new MessageNotFoundException("Staff not found");
            var name = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
            var appraisal = await _context.AppraisalSelectionDropDowns.FirstOrDefaultAsync(a => a.Id == uploadMisSheetRequest.AppraisalId && a.IsActive);
            if (appraisal == null) throw new MessageNotFoundException("Drop down type not found");
            var fileExtension = Path.GetExtension(uploadMisSheetRequest.File.FileName);
            if (!string.Equals(fileExtension, ".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Please upload the correct Excel template File");
            }
            string uploadFileName = $"{Path.GetFileNameWithoutExtension(uploadMisSheetRequest.File.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            string uploadFilePath = Path.Combine(_workspacePath, uploadFileName);
            using (var fileStream = new FileStream(uploadFilePath, FileMode.Create))
            {
                await uploadMisSheetRequest.File.CopyToAsync(fileStream);
            }
            using (var stream = new MemoryStream())
            {
                await uploadMisSheetRequest.File.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null) throw new MessageNotFoundException("Worksheet not found in the uploaded file");
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(cell => cell.Text.Trim()).ToList();
                    var requiredHeaders = new List<string>();
                    requiredHeaders = new List<string> { "Emp ID", "Emp Name", "Tenure in years", "Reporting Managers", "Division", "Department", "Final Average KRA Grade", "Absent Days", "HR Comments" };
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
                    var employeePerformanceReviews = new List<NonProductionEmployeePerformanceReview>();
                    var errorLogs = new List<string>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var empId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim();
                        if (string.IsNullOrEmpty(empId))
                        {
                            errorLogs.Add($"Staff Id is empty at {row}");
                            continue;
                        }
                        var staffs = await _context.StaffCreations.FirstOrDefaultAsync(d => d.StaffId == empId && d.IsActive == true);
                        if (staffs == null)
                        {
                            errorLogs.Add($"Staff Id '{empId}' not found at row {row}");
                            continue;
                        }
                        var empName = worksheet.Cells[row, columnIndexes["Emp Name"]].Text.Trim();
                        if (string.IsNullOrEmpty(empName))
                        {
                            errorLogs.Add($"Staff Name is empty at {row}");
                            continue;
                        }
                        var tenureYears = decimal.TryParse(worksheet.Cells[row, columnIndexes["Tenure in years"]].Text.Trim(), out var tenure) ? tenure : 0;
                        var reportingManager = worksheet.Cells[row, columnIndexes["Reporting Managers"]].Text.Trim();
                        if (string.IsNullOrEmpty(reportingManager))
                        {
                            errorLogs.Add($"Reporting Manager is empty at {row}");
                            continue;
                        }
                        var division = worksheet.Cells[row, columnIndexes["Division"]].Text.Trim();
                        if (string.IsNullOrEmpty(division))
                        {
                            errorLogs.Add($"Division is empty at {row}");
                            continue;
                        }
                        var divisionName = await _context.DivisionMasters.FirstOrDefaultAsync(d => d.Name == division && d.IsActive);
                        if (divisionName == null)
                        {
                            errorLogs.Add($"Division '{divisionName}' not found at row {row}");
                            continue;
                        }
                        var department = worksheet.Cells[row, columnIndexes["Department"]].Text.Trim();
                        if (string.IsNullOrEmpty(department))
                        {
                            errorLogs.Add($"Department is empty at {row}");
                            continue;
                        }
                        var departmentName = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Name == department && d.IsActive);
                        if (departmentName == null)
                        {
                            errorLogs.Add($"Department '{department}' not found at row {row}");
                            continue;
                        }
                        var cellText = worksheet.Cells[row, columnIndexes["Final Average KRA Grade"]].Text.Trim();
                        if (string.IsNullOrEmpty(cellText))
                        {
                            errorLogs.Add($"Final Average KRA is empty at {row}");
                            continue;
                        }
                        var finalAverageKraGrade = decimal.TryParse(cellText, out var kra) ? kra : 0;
                        var absentDays = int.TryParse(worksheet.Cells[row, columnIndexes["Absent Days"]].Text.Trim(), out var absent) ? absent : 0;
                        var hrComments = worksheet.Cells[row, columnIndexes["HR Comments"]].Text.Trim();
                        var employeeReview = new NonProductionEmployeePerformanceReview
                        {
                            EmpId = empId,
                            EmpName = empName,
                            TenureInYears = tenureYears,
                            ReportingManagers = reportingManager,
                            Division = division,
                            Department = department,
                            FinalAverageKraGrade = finalAverageKraGrade,
                            AbsentDays = absentDays,
                            HrComments = hrComments,
                            AppraisalId = uploadMisSheetRequest.AppraisalId,
                            Year = uploadMisSheetRequest.Year,
                            Quarter = uploadMisSheetRequest.Quarter,
                            IsCompleted = true,
                            IsActive = true,
                            CreatedBy = uploadMisSheetRequest.CreatedBy,
                            CreatedUtc = DateTime.UtcNow
                        };
                        employeePerformanceReviews.Add(employeeReview);
                    }
                    if (employeePerformanceReviews.Count > 0)
                    {
                        var selectedEmpIds = employeePerformanceReviews.Select(x => x.EmpId).Distinct().ToList();
                        var alreadyUploaded = await _context.NonProductionEmployeePerformanceReviews
                            .Where(x =>
                                selectedEmpIds.Contains(x.EmpId) &&
                                x.AppraisalId == uploadMisSheetRequest.AppraisalId &&
                                x.Year == uploadMisSheetRequest.Year &&
                                x.Quarter == uploadMisSheetRequest.Quarter)
                            .Select(x => x.EmpId)
                            .Distinct()
                            .ToListAsync();
                        var rowsToProcess = employeePerformanceReviews.Where(r => !alreadyUploaded.Contains(r.EmpId)).ToList();
                        if (!rowsToProcess.Any())
                        {
                            throw new ConflictException($"All selected staff have already been uploaded for appraisal year {uploadMisSheetRequest.Year} and {uploadMisSheetRequest.Quarter}");
                        }
                        var newRecords = employeePerformanceReviews
                            .Where(x => !alreadyUploaded.Contains(x.EmpId))
                            .ToList();
                        var selectedEmployeesToUpdate = await _context.SelectedNonProductionEmployees
                            .Where(x => newRecords.Select(e => e.EmpId).Contains(x.EmployeeId)
                                        && x.AppraisalId == uploadMisSheetRequest.AppraisalId
                                        && x.IsActive)
                            .ToListAsync();
                        foreach (var selected in selectedEmployeesToUpdate)
                        {
                            selected.IsCompleted = true;
                            selected.IsActive = false;
                            selected.UpdatedBy = uploadMisSheetRequest.CreatedBy;
                            selected.UpdatedUtc = DateTime.UtcNow;
                        }
                        await _context.NonProductionEmployeePerformanceReviews.AddRangeAsync(rowsToProcess);
                        await _context.SaveChangesAsync();

                        if (alreadyUploaded.Any())
                        {
                            throw new ConflictException($"The following staff have already been uploaded for appraisal year {uploadMisSheetRequest.Year} and {uploadMisSheetRequest.Quarter}: " + string.Join(", ", alreadyUploaded));
                        }
                    }
                    else
                    {
                        if (errorLogs.Any())
                        {
                            throw new InvalidOperationException("Some records could not be processed: " + string.Join(", ", errorLogs));
                        }
                        else
                        {
                            throw new MessageNotFoundException("File is empty");
                        }
                    }
                }
            }
            return "Excel data imported successfully";
        }

        public async Task<List<HrUploadResponse>> GetHrUploadedSheet(int appraisalId, int year, string quarter)
        {
            var response = await (from hr in _context.NonProductionEmployeePerformanceReviews
                                  join ap in _context.AppraisalSelectionDropDowns on hr.AppraisalId equals ap.Id
                                  where hr.Year == year && hr.Quarter == quarter && ap.IsActive && hr.AppraisalId == appraisalId
                                  select new HrUploadResponse
                                  {
                                      Id = hr.Id,
                                      EmpId = hr.EmpId,
                                      EmpName = hr.EmpName,
                                      TenureInYears = hr.TenureInYears,
                                      ReportingManagers = hr.ReportingManagers,
                                      Division = hr.Division,
                                      Department = hr.Department,
                                      FinalAverageKraGrade = hr.FinalAverageKraGrade,
                                      AbsentDays = hr.AbsentDays,
                                      HrComments = hr.HrComments,
                                      AppraisalType = ap.Name,
                                      IsCompleted = hr.IsCompleted,
                                      Year = hr.Year,
                                      Quarter = hr.Quarter,
                                      CreatedBy = hr.CreatedBy
                                  })
                                  .ToListAsync();
            if (response.Count == 0) throw new MessageNotFoundException("Hr uploaded sheet not found");
            return response;
        }
    }
}
