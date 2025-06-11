using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Azure;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AttendanceManagement.Services
{
    public class AppraisalManagementService
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly EmailService _emailService;
        private readonly string _workspacePath;
        private readonly LetterGenerationService _letterGenerationService;
        public AppraisalManagementService(AttendanceManagementSystemContext context, EmailService emailService, IWebHostEnvironment env, LetterGenerationService letterGenerationService)
        {
            _context = context;
            _emailService = emailService;
            _workspacePath = Path.Combine(env.ContentRootPath, "wwwroot\\GeneratedLetters\\AppraisalLetter");
            if (!Directory.Exists(_workspacePath))
            {
                Directory.CreateDirectory(_workspacePath);
            }
            _letterGenerationService = letterGenerationService;
        }

        public async Task<List<object>> GetProductionEmployees(int appraisalId)
        {
            var currentYear = DateTime.UtcNow.Year;
            if (appraisalId == 1)
            {
                var grouped = await (
                            from staff in _context.StaffCreations
                            join designation in _context.DivisionMasters on staff.DesignationId equals designation.Id
                            join department in _context.DepartmentMasters on staff.DepartmentId equals department.Id
                            join manager in _context.StaffCreations on staff.ApprovalLevel1 equals manager.Id
                            join selected in _context.SelectedEmployeesForAppraisals.Where(s => s.AppraisalId == appraisalId) on staff.StaffId equals selected.EmployeeId into selectedJoin
                            from selected in selectedJoin.DefaultIfEmpty()
                            join per in _context.EmployeePerformanceReviews.Where(s => s.AppraisalId == appraisalId) on selected.EmployeeId equals per.EmpId into perJoin
                            from per in perJoin.DefaultIfEmpty()
                            join agm in _context.AgmApprovals on per.Id equals agm.EmployeePerformanceReviewId into agmJoin
                            from agm in agmJoin.DefaultIfEmpty()
                            where staff.IsActive == true && designation.IsActive && department.IsActive && !staff.IsNonProduction
                            select new
                            {
                                staff.Id,
                                staff.StaffId,
                                StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                                staff.Tenure,
                                ReportingManager = $"{manager.FirstName}{(string.IsNullOrWhiteSpace(manager.LastName) ? "" : " " + manager.LastName)}",
                                Division = designation.Name,
                                Department = department.Name,
                                SelectedCreatedUtc = selected != null ? selected.CreatedUtc : (DateTime?)null,
                                PerformanceReviewId = per != null ? per.Id : (int?)null,
                                PerformanceReviewCreatedUtc = per != null ? per.CreatedUtc : (DateTime?)null,
                                AgmIsApproved = agm != null ? agm.IsAgmApproved : (bool?)null,
                                AgmCreatedUtc = agm != null ? agm.CreatedUtc : (DateTime?)null
                            })
                            .ToListAsync();

                var result = grouped
                    .GroupBy(x => x.Id)
                    .Select(g =>
                    {
                        var first = g.First();
                        var agmApproval = g.FirstOrDefault(x => x.AgmCreatedUtc.HasValue && x.AgmCreatedUtc.Value.Year == currentYear && x.AgmIsApproved == true);
                        var perfReview = g.FirstOrDefault(x => x.PerformanceReviewCreatedUtc.HasValue && x.PerformanceReviewCreatedUtc.Value.Year == currentYear);
                        var selectedEmp = g.FirstOrDefault(x => x.SelectedCreatedUtc.HasValue && x.SelectedCreatedUtc.Value.Year == currentYear);
                        bool? isCompleted = null;
                        if (agmApproval != null)
                        {
                            isCompleted = true;
                        }
                        else if (perfReview != null || selectedEmp != null)
                        {
                            isCompleted = false;
                        }
                        return new AppraisalDto
                        {
                            StaffId = g.Key,
                            EmpId = first.StaffId,
                            EmpName = first.StaffName,
                            Tenure = first.Tenure,
                            ReportingManagers = first.ReportingManager,
                            Division = first.Division,
                            Department = first.Department,
                            IsCompleted = isCompleted
                        };
                    })
                    .ToList();
                if (result == null || !result.Any())
                {
                    throw new MessageNotFoundException("No employees found");
                }
                return result.Cast<object>().ToList();
            }
            else if (appraisalId == 2)
            {
                var grouped = await (
                            from staff in _context.StaffCreations
                            join designation in _context.DivisionMasters on staff.DesignationId equals designation.Id
                            join department in _context.DepartmentMasters on staff.DepartmentId equals department.Id
                            join manager in _context.StaffCreations on staff.ApprovalLevel1 equals manager.Id
                            join selected in _context.SelectedEmployeesForAppraisals.Where(s => s.AppraisalId == appraisalId) on staff.StaffId equals selected.EmployeeId into selectedJoin
                            from selected in selectedJoin.DefaultIfEmpty()
                            join per in _context.EmployeePerformanceReviews.Where(s => s.AppraisalId == appraisalId) on selected.EmployeeId equals per.EmpId into perJoin
                            from per in perJoin.DefaultIfEmpty()
                            join agm in _context.AgmApprovals on per.Id equals agm.EmployeePerformanceReviewId into agmJoin
                            from agm in agmJoin.DefaultIfEmpty()
                            where staff.IsActive == true
                                && designation.IsActive
                                && department.IsActive
                                && !staff.IsNonProduction
                                && staff.JoiningDate <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1))
                                && staff.OrganizationTypeId == 1
                            select new
                            {
                                staff.Id,
                                staff.StaffId,
                                StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                                staff.Tenure,
                                ReportingManager = $"{manager.FirstName}{(string.IsNullOrWhiteSpace(manager.LastName) ? "" : " " + manager.LastName)}",
                                Division = designation.Name,
                                Department = department.Name,
                                SelectedCreatedUtc = selected != null ? selected.CreatedUtc : (DateTime?)null,
                                PerformanceReviewId = per != null ? per.Id : (int?)null,
                                PerformanceReviewCreatedUtc = per != null ? per.CreatedUtc : (DateTime?)null,
                                AgmIsApproved = agm != null ? agm.IsAgmApproved : (bool?)null,
                                AgmCreatedUtc = agm != null ? agm.CreatedUtc : (DateTime?)null
                            }
                        ).ToListAsync();

                var result = grouped
                    .GroupBy(x => x.Id)
                    .Select(g =>
                    {
                        var first = g.First();
                        var agmApproval = g.FirstOrDefault(x => x.AgmCreatedUtc.HasValue && x.AgmCreatedUtc.Value.Year == currentYear && x.AgmIsApproved == true);
                        var perfReview = g.FirstOrDefault(x => x.PerformanceReviewCreatedUtc.HasValue && x.PerformanceReviewCreatedUtc.Value.Year == currentYear);
                        var selectedEmp = g.FirstOrDefault(x => x.SelectedCreatedUtc.HasValue && x.SelectedCreatedUtc.Value.Year == currentYear);
                        bool? isCompleted = null;
                        if (agmApproval != null)
                        {
                            isCompleted = true;
                        }
                        else if (perfReview != null || selectedEmp != null)
                        {
                            isCompleted = false;
                        }
                        return new ProbationDto
                        {
                            StaffId = g.Key,
                            EmpId = first.StaffId,
                            EmpName = first.StaffName,
                            Tenure = first.Tenure,
                            ReportingManagers = first.ReportingManager,
                            Division = first.Division,
                            Department = first.Department,
                            IsCompleted = isCompleted
                        };
                    })
                    .ToList();
                if (result == null || !result.Any())
                {
                    throw new MessageNotFoundException("No employees found");
                }
                return result.Cast<object>().ToList();
            }
            else if (appraisalId == 3)
            {
                var grouped = await (
                            from staff in _context.StaffCreations
                            join designation in _context.DivisionMasters on staff.DesignationId equals designation.Id
                            join department in _context.DepartmentMasters on staff.DepartmentId equals department.Id
                            join manager in _context.StaffCreations on staff.ApprovalLevel1 equals manager.Id
                            join selected in _context.SelectedEmployeesForAppraisals.Where(s => s.AppraisalId == appraisalId) on staff.StaffId equals selected.EmployeeId into selectedJoin
                            from selected in selectedJoin.DefaultIfEmpty()
                            join per in _context.EmployeePerformanceReviews.Where(s => s.AppraisalId == appraisalId) on selected.EmployeeId equals per.EmpId into perJoin
                            from per in perJoin.DefaultIfEmpty()
                            join agm in _context.AgmApprovals on per.Id equals agm.EmployeePerformanceReviewId into agmJoin
                            from agm in agmJoin.DefaultIfEmpty()
                            where staff.IsActive == true
                                && designation.IsActive
                                && department.IsActive
                                && !staff.IsNonProduction
                                && staff.OrganizationTypeId == 1
                            select new
                            {
                                staff.Id,
                                staff.StaffId,
                                StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                                staff.Tenure,
                                ReportingManager = $"{manager.FirstName}{(string.IsNullOrWhiteSpace(manager.LastName) ? "" : " " + manager.LastName)}",
                                Division = designation.Name,
                                Department = department.Name,
                                SelectedCreatedUtc = selected != null ? selected.CreatedUtc : (DateTime?)null,
                                PerformanceReviewCreatedUtc = per != null ? per.CreatedUtc : (DateTime?)null,
                                AgmIsApproved = agm != null ? agm.IsAgmApproved : (bool?)null,
                                AgmCreatedUtc = agm != null ? agm.CreatedUtc : (DateTime?)null
                            }
                        ).ToListAsync();

                var result = grouped
                    .GroupBy(x => x.Id)
                    .Select(g =>
                    {
                        var first = g.First();
                        var agmApproval = g.FirstOrDefault(x => x.AgmCreatedUtc.HasValue && x.AgmCreatedUtc.Value.Year == currentYear && x.AgmIsApproved == true);
                        var perfReview = g.FirstOrDefault(x => x.PerformanceReviewCreatedUtc.HasValue && x.PerformanceReviewCreatedUtc.Value.Year == currentYear);
                        var selectedEmp = g.FirstOrDefault(x => x.SelectedCreatedUtc.HasValue && x.SelectedCreatedUtc.Value.Year == currentYear);
                        bool? isCompleted = null;
                        if (agmApproval != null)
                        {
                            isCompleted = true;
                        }
                        else if (perfReview != null || selectedEmp != null)
                        {
                            isCompleted = false;
                        }
                        return new ProbationDto
                        {
                            StaffId = g.Key,
                            EmpId = first.StaffId,
                            EmpName = first.StaffName,
                            Tenure = first.Tenure,
                            ReportingManagers = first.ReportingManager,
                            Division = first.Division,
                            Department = first.Department,
                            IsCompleted = isCompleted
                        };
                    })
                    .ToList();
                if (result == null || !result.Any())
                {
                    throw new MessageNotFoundException("No employees found");
                }
                return result.Cast<object>().ToList();
            }
            throw new MessageNotFoundException("Please choose a valid dropdown");
        }

        public async Task<string> MoveSelectedStaffToMis(SelectedEmployeesRequest selectedEmployeesRequest)
        {
            var employeeIds = selectedEmployeesRequest.SelectedRows.Select(x => x.EmpId).ToList();
            var currentYear = DateTime.UtcNow.Year;
            var alreadySelected = await _context.SelectedEmployeesForAppraisals
                .Where(x => employeeIds.Contains(x.EmployeeId)
                            && x.AppraisalId == selectedEmployeesRequest.AppraisalId
                            && !x.IsActive
                            && x.IsCompleted == true
                            && (
                                x.AppraisalId != 1 ||                   
                                x.CreatedUtc.Year == currentYear          
                            ))
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

        public async Task<List<SelectedEmployeesResponseSelectedRows>> GetSelectedEmployees(int appraisalId)
        {
            var selectedEmployees = await (from staff in _context.SelectedEmployeesForAppraisals
                                           where staff.IsActive == true && staff.AppraisalId == appraisalId
                                           select new SelectedEmployeesResponseSelectedRows
                                           {
                                               EmpId = staff.EmployeeId,
                                               EmpName = staff.EmployeeName,
                                               TenureInYears = staff.TenureInYears,
                                               ReportingManagers = staff.ReportingManagers,
                                               Division = staff.Division,
                                               Department = staff.Department,
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
            try
            {
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
                using (var stream = new MemoryStream())
                {
                    await uploadMisSheetRequest.File.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null) throw new MessageNotFoundException("Worksheet not found in the uploaded file");
                        var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(cell => cell.Text.Trim()).ToList();
                        var requiredHeaders = new List<string>();
                        requiredHeaders = new List<string> { "Emp ID", "Emp Name", "Tenure in years", "Reporting Managers", "Division", "Department", "Productivity %", "Quality %", "Present %", "Final %", "Grade", "Absent Days" };
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
                                var employeePerformanceReviews = new List<EmployeePerformanceReview>();
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var empId = worksheet.Cells[row, columnIndexes["Emp ID"]].Text.Trim();
                                    var empName = worksheet.Cells[row, columnIndexes["Emp Name"]].Text.Trim();
                                    var tenureYears = decimal.TryParse(worksheet.Cells[row, columnIndexes["Tenure in years"]].Text.Trim(), out var tenure) ? tenure : 0;
                                    var reportingManager = worksheet.Cells[row, columnIndexes["Reporting Managers"]].Text.Trim();
                                    var division = worksheet.Cells[row, columnIndexes["Division"]].Text.Trim();
                                    var department = worksheet.Cells[row, columnIndexes["Department"]].Text.Trim();
                                    var productivityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Productivity %"]].Text.Trim(), out var productivity) ? productivity : 0;
                                    var qualityPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Quality %"]].Text.Trim(), out var quality) ? quality : 0;
                                    var presentPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Present %"]].Text.Trim(), out var present) ? present : 0;
                                    var finalPercentage = decimal.TryParse(worksheet.Cells[row, columnIndexes["Final %"]].Text.Trim(), out var final) ? final : 0;
                                    var grade = worksheet.Cells[row, columnIndexes["Grade"]].Text.Trim();
                                    var absentDays = int.TryParse(worksheet.Cells[row, columnIndexes["Absent Days"]].Text.Trim(), out var absent) ? absent : 0;
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
                                        AppraisalId = uploadMisSheetRequest.AppraisalId,
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
                                    var existingRecords = await _context.EmployeePerformanceReviews
                                        .Where(x => empIds.Contains(x.EmpId) && x.AppraisalId == uploadMisSheetRequest.AppraisalId && x.IsActive)
                                        .ToListAsync();
                                    foreach (var record in existingRecords)
                                    {
                                        record.IsActive = false;
                                        record.UpdatedBy = uploadMisSheetRequest.CreatedBy;
                                        record.UpdatedUtc = DateTime.UtcNow;
                                    }
                                    var selectedEmpIds = employeePerformanceReviews.Select(x => x.EmpId).Distinct().ToList();
                                    var selectedEmployeesToUpdate = await _context.SelectedEmployeesForAppraisals
                                        .Where(x => selectedEmpIds.Contains(x.EmployeeId)
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
                                    await _context.EmployeePerformanceReviews.AddRangeAsync(employeePerformanceReviews);
                                }
                                else
                                {
                                    throw new MessageNotFoundException("File is empty");
                                }
                                await _context.SaveChangesAsync();
                                await transaction.CommitAsync();
                                await _emailService.SendMisUploadNotificationToHr(uploadMisSheetRequest.CreatedBy, name, appraisal.Name);
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                throw new Exception(ex.Message);
                            }
                        }
                    }
                }
                return "Excel data imported successfully";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PerformanceReviewResponse>> GetSelectedEmployeeReview(int appraisalId)
        {
            var selectedEmployees = await (from staff in _context.EmployeePerformanceReviews
                                           where staff.IsActive == true && staff.AppraisalId == appraisalId
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
            var currentYear = DateTime.UtcNow.Year;
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == agmApprovalRequest.AgmId && s.IsActive == true);
            if(staff == null) throw new MessageNotFoundException("AGM not found");
            var name = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
            var selectedReviewIds = selectedRows.Select(r => r.Id).ToList();

            var alreadyApproved = await _context.AgmApprovals
                .Where(x => selectedReviewIds.Contains(x.EmployeePerformanceReviewId)
                            && !x.IsActive
                            && x.CreatedUtc.Year == currentYear)
                .Select(x => x.EmployeePerformanceReviewId)
                .ToListAsync();
            if (alreadyApproved.Any())
            {
                throw new InvalidOperationException("Some selected employees are already moved to AGM");
            }
            var existingActiveApprovals = await _context.AgmApprovals
                .Where(x => selectedReviewIds.Contains(x.EmployeePerformanceReviewId)
                            && x.IsActive
                            && x.CreatedUtc.Year == currentYear)
                .ToListAsync();
            foreach (var existingApproval in existingActiveApprovals)
            {
                existingApproval.IsActive = false;
                existingApproval.UpdatedBy = agmApprovalRequest.CreatedBy;
                existingApproval.UpdatedUtc = DateTime.UtcNow;
            }
            if (existingActiveApprovals.Any())
            {
                await _context.SaveChangesAsync();
            }
            foreach (var row in selectedRows)
            {
                var employeeReview = new AgmApproval
                {
                    EmployeePerformanceReviewId = row.Id,
                    AgmId = staff.Id,
                    IsAgmApproved = false,
                    IsActive = true,
                    CreatedBy = agmApprovalRequest.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                await _context.AddAsync(employeeReview);
                await _context.SaveChangesAsync();
                var review = await _context.EmployeePerformanceReviews.Where(x => x.Id == employeeReview.EmployeePerformanceReviewId && x.IsActive).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                if (review != null)
                {
                    review.IsCompleted = true;
                    review.IsActive = false;
                    review.UpdatedBy = agmApprovalRequest.CreatedBy;
                    review.UpdatedUtc = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            await _emailService.SendAgmApprovalNotification(agmApprovalRequest.CreatedBy, staff.OfficialEmail, name);
            return message;
        }

        public async Task<List<PerformanceReviewResponse>> GetSelectedEmployeeAgmApproval(int appraisalId)
        {
            var selectedEmployees = await (from staff in _context.AgmApprovals
                                           join per in _context.EmployeePerformanceReviews on staff.EmployeePerformanceReviewId equals per.Id
                                           where staff.IsActive == true && per.AppraisalId == appraisalId
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
            foreach ( var row in selectedRows)
            {
                var employeeReview = await _context.AgmApprovals.FirstOrDefaultAsync(x => x.Id == row.Id && x.IsActive);
                if (employeeReview == null) throw new MessageNotFoundException($"Employee report not found");
                employeeReview.IsAgmApproved = agmApprovalRequest.IsApproved;
                employeeReview.IsActive = false;
                employeeReview.UpdatedBy = agmApprovalRequest.ApprovedBy;
                employeeReview.UpdatedUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            await _emailService.SendHrApprovalNotification(agmApprovalRequest.ApprovedBy, name);
            return message;
        }

        public async Task<string> GenerateAppraisalLetter(int createdBy)
        {
            var appraisalSheets = await _context.EmployeeAppraisalSheets.Where(s => s.AppraisalYear == DateTime.UtcNow.Year && s.IsActive == true).Distinct().ToListAsync();
            if (appraisalSheets == null || !appraisalSheets.Any()) throw new MessageNotFoundException("No appraisal data found for the current year");
            var resultFilePaths = new List<string>();
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
            }
            return "Appraisal letter generated and email sent successfully";
        }

        public async Task<(Stream PdfStream, string FileName)> ViewAppraisalLetter(int staffId, int fileId)
        {
            var letterGeneration = await _context.LetterGenerations.FirstOrDefaultAsync(x => x.StaffCreationId == staffId && x.Id == fileId && x.IsActive == true);
            if (letterGeneration == null)
            {
                throw new FileNotFoundException("Letter generation record not found");
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

        public async Task<string> DownloadAppraisalLetter(int staffId, int fileId)
        {
            var appraisalLetter = await _context.LetterGenerations.FirstOrDefaultAsync(x => x.StaffCreationId == staffId && x.Id == fileId && x.IsActive == true);
            if (appraisalLetter == null)
            {
                throw new MessageNotFoundException("Appraisal letter not found");
            }
            string file = appraisalLetter.FileName ?? string.Empty;
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new MessageNotFoundException("File name is empty");
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
            var currentYear = DateTime.UtcNow.Year;
            var latestAcceptance = await _context.EmployeeAcceptances
                .Where(x => x.EmpId == empId && x.IsActive && x.CreatedUtc.Year == currentYear && x.IsAccepted)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            if (latestAcceptance != null)
            {
                throw new InvalidOperationException("You have already accepted");
            }
            var appraisal = new EmployeeAcceptance
            {
                EmpId = empId,
                EmpName = empName,
                Department = department.Name,
                Division = division.Name,
                IsAccepted = letterAcceptance.IsAccepted,
                IsActive = true,
                CreatedBy = letterAcceptance.AcceptedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.AddAsync(appraisal);
            await _context.SaveChangesAsync();
            return "Appraisal letter has been successfully accepted";
        }

        public async Task<List<LetterAcceptanceResponse>> GetAcceptedEmployees()
        {
            var currentYear = DateTime.UtcNow.Year;
            var acceptedEmployees = await (from emp in _context.EmployeeAcceptances
                                           where emp.IsActive == true && emp.CreatedUtc.Year == currentYear
                                           select new LetterAcceptanceResponse
                                           {
                                               EmpId = emp.EmpId,
                                               EmpName = emp.EmpName,
                                               Division = emp.Division,
                                               Department = emp.Department,
                                               IsAccepted = emp.IsAccepted
                                           }).ToListAsync();
            if (acceptedEmployees == null || !acceptedEmployees.Any())
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
      }
}
