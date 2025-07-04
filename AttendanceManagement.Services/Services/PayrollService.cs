using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AttendanceManagement.Services.Interface;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AttendanceManagement.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly string _excelWorkspacePath;
        private readonly string _workspacePath;
        private readonly ILetterGeneration _letterGenerationService;
        public PayrollService(AttendanceManagementSystemContext context, IWebHostEnvironment env, ILetterGeneration letterGenerationService)
        {
            _context = context;
            _excelWorkspacePath = Path.Combine(env.ContentRootPath, "wwwroot\\UploadedExcel");
            if (!Directory.Exists(_excelWorkspacePath))
            {
                Directory.CreateDirectory(_excelWorkspacePath);
            }
            _workspacePath = Path.Combine(env.ContentRootPath, "wwwroot\\GeneratedLetters\\PaySlip");
            if (!Directory.Exists(_workspacePath))
            {
                Directory.CreateDirectory(_workspacePath);
            }
            _letterGenerationService = letterGenerationService;
        }

        public async Task<string> UploadPaySlip(IFormFile file, int createdBy)
        {
            var message = "Payslip uploaded successfully";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string uploadFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            string uploadFilePath = Path.Combine(_excelWorkspacePath, uploadFileName);
            using (var fileStream = new FileStream(uploadFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null) throw new Exception("Worksheet not found in the uploaded file.");
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(cell => cell.Text.Trim()).ToList();
                    var requiredHeaders = new List<string>
                    {
                        "StaffId", "Basic", "HRA", "DA", "OtherAllowance", "SpecialAllowance", "Conveyance",
                        "TDS", "ESIC", "PF", "PT", "OTHours", "OTPerHour", "TotalOT", "LOPPerDay", "AbsentDays",
                        "LWOPDays", "TotalLOP", "IsFreezed", "ESICEmployerContribution", "PFEmployerContribution",
                        "SalaryMonth", "SalaryYear", "ComponentType", "ComponentName", "Amount", "IsTaxable", "Remarks"
                    };

                    var missingHeaders = requiredHeaders.Where(header => !headerRow.Contains(header)).ToList();
                    if (missingHeaders.Count == requiredHeaders.Count)
                    {
                        throw new ArgumentException("Invalid Excel file");
                    }
                    if (missingHeaders.Any())
                    {
                        throw new Exception($"Invalid Excel file. Missing headers: {string.Join(", ", missingHeaders)}");
                    }
                    var columnIndexes = requiredHeaders.ToDictionary(
                        header => header,
                        header => headerRow.IndexOf(header) + 1
                    );
                    var rowCount = worksheet.Dimension.Rows;
                    var paySlips = new List<PaySlip>();
                    var paySlipComponents = new List<PaySlipComponent>();
                    var errorLogs = new List<string>();
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                            if (string.IsNullOrEmpty(staffCreationIdStr))
                            {
                                errorLogs.Add($"Staff is empty at {row}");
                                continue;
                            }
                            var staffCreation = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffCreationIdStr && s.IsActive == true);
                            if (staffCreation == null)
                            {
                                errorLogs.Add($"Staff '{staffCreationIdStr}' not found at {row}");
                                continue;
                            }
                            var paySlip = new PaySlip
                            {
                                StaffId = staffCreation.Id,
                                Basic = ParseDecimal(worksheet.Cells[row, columnIndexes["Basic"]].Text, row, "Basic"),
                                Hra = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["HRA"]].Text),
                                Da = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["DA"]].Text),
                                OtherAllowance = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["OtherAllowance"]].Text),
                                SpecialAllowance = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["SpecialAllowance"]].Text),
                                Conveyance = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["Conveyance"]].Text),
                                Tds = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["TDS"]].Text),
                                Esic = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["ESIC"]].Text),
                                Pf = ParseDecimal(worksheet.Cells[row, columnIndexes["PF"]].Text, row, "PF"),
                                Pt = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["PT"]].Text),
                                Othours = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["OTHours"]].Text),
                                OtperHour = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["OTPerHour"]].Text),
                                TotalOt = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["TotalOT"]].Text),
                                LopperDay = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["LOPPerDay"]].Text),
                                AbsentDays = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["AbsentDays"]].Text),
                                Lwopdays = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["LWOPDays"]].Text),
                                TotalLop = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["TotalLOP"]].Text),
                                IsFreezed = ParseBool(worksheet.Cells[row, columnIndexes["IsFreezed"]].Text, row, "IsFreezed"),
                                EsicemployerContribution = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["ESICEmployerContribution"]].Text),
                                PfemployerContribution = ParseDecimal(worksheet.Cells[row, columnIndexes["PFEmployerContribution"]].Text, row, "PFEmployerContribution"),
                                SalaryMonth = worksheet.Cells[row, columnIndexes["SalaryMonth"]].Text,
                                SalaryYear = ParseInt(worksheet.Cells[row, columnIndexes["SalaryYear"]].Text, row, "SalaryYear"),
                                IsActive = true,
                                CreatedBy = createdBy,
                                CreatedUtc = DateTime.UtcNow
                            };
                            paySlips.Add(paySlip);
                        }
                        if (paySlips.Any())
                        {
                            await _context.PaySlips.AddRangeAsync(paySlips);
                            await _context.SaveChangesAsync();
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

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                            if (string.IsNullOrEmpty(staffCreationIdStr))
                            {
                                errorLogs.Add($"Staff is empty at {row}");
                                continue;
                            }
                            var staffCreation = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffCreationIdStr && s.IsActive == true);
                            if (staffCreation == null)
                            {
                                errorLogs.Add($"Staff '{staffCreationIdStr}' not found at {row}");
                                continue;
                            }
                            var paySlip = paySlips.FirstOrDefault(ps => ps.StaffId == staffCreation.Id && ps.IsActive);
                            if (paySlip == null) continue;
                            var componentType = worksheet.Cells[row, columnIndexes["ComponentType"]]?.Text.Trim();
                            var componentName = worksheet.Cells[row, columnIndexes["ComponentName"]]?.Text.Trim();
                            var amount = worksheet.Cells[row, columnIndexes["Amount"]]?.Text.Trim();
                            var isTaxableStr = worksheet.Cells[row, columnIndexes["IsTaxable"]]?.Text.Trim();
                            var remarks = worksheet.Cells[row, columnIndexes["Remarks"]]?.Text.Trim();
                            if (string.IsNullOrEmpty(isTaxableStr))
                            {
                                errorLogs.Add($"IsTaxable is empty at {row}");
                                continue;
                            }
                            if (!string.IsNullOrEmpty(componentType) && !string.IsNullOrEmpty(componentName) && !string.IsNullOrEmpty(amount))
                            {
                                var paySlipComponent = new PaySlipComponent
                                {
                                    PaySlipId = paySlip.Id,
                                    StaffId = paySlip.StaffId,
                                    ComponentType = componentType,
                                    ComponentName = componentName,
                                    Amount = amount,
                                    IsTaxable = ParseBool(isTaxableStr, row, "IsTaxable"),
                                    Remarks = remarks,
                                    IsActive = true,
                                    CreatedBy = createdBy,
                                    CreatedUtc = DateTime.UtcNow
                                };
                                paySlipComponents.Add(paySlipComponent);
                            }
                        }
                        if (paySlipComponents.Any())
                        {
                            await _context.PaySlipComponents.AddRangeAsync(paySlipComponents);
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
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                }
            }
            return message;
        }

        public async Task<string> GeneratePaySlip(GeneratePaySheetRequest paySlipGenerate)
        {
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == paySlipGenerate.StaffId && s.IsActive == true);
            if (staff == null) throw new MessageNotFoundException("Staff not found");
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(paySlipGenerate.Month);
            string fileName = $"Payslip_{staff.StaffId}_{monthName}_{paySlipGenerate.Year}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            var paySlip = await (from payslip in _context.PaySlips
                                 join s in _context.StaffCreations on payslip.StaffId equals s.Id
                                 where payslip.StaffId == paySlipGenerate.StaffId && payslip.SalaryMonth == monthName
                                 && payslip.SalaryYear == paySlipGenerate.Year && payslip.IsActive == true
                                 select new PayslipResponse
                                 {
                                     Id = payslip.Id,
                                     StaffId = payslip.StaffId,
                                     StaffCreationId = s.StaffId,
                                     StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                     Basic = payslip.Basic,
                                     Hra = payslip.Hra,
                                     Da = payslip.Da,
                                     OtherAllowance = payslip.OtherAllowance,
                                     SpecialAllowance = payslip.SpecialAllowance,
                                     Conveyance = payslip.Conveyance,
                                     Tds = payslip.Tds,
                                     Esic = payslip.Esic,
                                     Pf = payslip.Pf,
                                     Pt = payslip.Pt,
                                     Othours = payslip.Othours,
                                     OtperHour = payslip.OtperHour,
                                     TotalOt = payslip.TotalOt,
                                     LopperDay = payslip.LopperDay,
                                     TotalLop = payslip.TotalLop,
                                     Lwopdays = payslip.Lwopdays,
                                     AbsentDays = payslip.AbsentDays,
                                     IsFreezed = payslip.IsFreezed,
                                     EsicemployerContribution = payslip.EsicemployerContribution,
                                     PfemployerContribution = payslip.PfemployerContribution,
                                     SalaryMonth = payslip.SalaryMonth,
                                     SalaryYear = payslip.SalaryYear,
                                     CreatedBy = payslip.CreatedBy,
                                     PaySlipComponents = payslip.PaySlipComponents
                                    .Select(pc => new PaySlipComponentResponse
                                    {
                                        Id = pc.Id,
                                        ComponentType = pc.ComponentType,
                                        ComponentName = pc.ComponentName,
                                        Amount = pc.Amount,
                                        IsTaxable = pc.IsTaxable,
                                        Remarks = pc.Remarks
                                    }).ToList()
                                 })
                                 .FirstOrDefaultAsync();
            if (paySlip == null)
            {
                throw new MessageNotFoundException("Payslip not found");
            }

            var file = _letterGenerationService.GeneratePaySlip(paySlip, fileName);
            byte[] pdfBytes = System.IO.File.ReadAllBytes(file);
            string base64Pdf = Convert.ToBase64String(pdfBytes);
            var letterGeneration = new LetterGeneration
            {
                LetterPath = file,
                LetterContent = Convert.FromBase64String(base64Pdf),
                StaffCreationId = staff.Id,
                FileName = fileName,
                CreatedBy = paySlipGenerate.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = true
            };
            await _context.LetterGenerations.AddAsync(letterGeneration);
            await _context.SaveChangesAsync();

            return file;
        }

        public async Task<string> GeneratePaySheet(GeneratePaySheetRequest generatePaySheetRequest)
        {
            var staffs = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == generatePaySheetRequest.StaffId && s.IsActive == true);
            if (staffs == null) throw new MessageNotFoundException("Staff not found");
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(generatePaySheetRequest.Month);
            string fileName = $"Paysheet_{staffs.StaffId}_{monthName}_{generatePaySheetRequest.Year}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            var paySheet = await (from pay in _context.PaySheets
                                  join staff in _context.StaffCreations on pay.StaffId equals staff.StaffId
                                  join designation in _context.DesignationMasters on pay.DesignationId equals designation.Id
                                  join department in _context.DepartmentMasters on pay.DepartmentId equals department.Id
                                  where staff.IsActive == true && designation.IsActive && department.IsActive && pay.IsActive
                                  && pay.StaffId == staffs.StaffId && pay.Month == generatePaySheetRequest.Month && pay.Year == generatePaySheetRequest.Year
                                  select new PaysheetResponse
                                  {
                                      StaffId = staff.StaffId,
                                      EmployeeName = pay.EmployeeName,
                                      GroupName = pay.GroupName,
                                      DisplayNameInReports = pay.DisplayNameInReports,
                                      Month = monthName,
                                      Year = generatePaySheetRequest.Year,
                                      DateOfJoining = pay.DateOfJoining,
                                      EmployeeNumber = pay.EmployeeNumber,
                                      Designation = designation.Name,
                                      Department = department.Name,
                                      Location = pay.Location,
                                      Gender = pay.Gender,
                                      DateOfBirth = pay.DateOfBirth,
                                      FatherOrMotherName = pay.FatherOrMotherName,
                                      SpouseName = pay.SpouseName,
                                      Address = pay.Address,
                                      Email = pay.Email,
                                      PhoneNo = pay.PhoneNo,
                                      BankName = pay.BankName,
                                      AccountNo = pay.AccountNo,
                                      IfscCode = pay.IfscCode,
                                      PfAccountNo = pay.PfAccountNo,
                                      Uan = pay.Uan,
                                      Pan = pay.Pan,
                                      AadhaarNo = pay.AadhaarNo,
                                      EsiNo = pay.EsiNo,
                                      SalaryEffectiveFrom = pay.SalaryEffectiveFrom,
                                      BasicActual = pay.BasicActual,
                                      HraActual = pay.HraActual,
                                      ConveActual = pay.ConveActual,
                                      MedAllowActual = pay.MedAllowActual,
                                      SplAllowActual = pay.SplAllowActual,
                                      LopDays = pay.LopDays,
                                      StdDays = pay.StdDays,
                                      WrkDays = pay.WrkDays,
                                      PfAdmin = pay.PfAdmin,
                                      BasicEarned = pay.BasicEarned,
                                      BasicArradj = pay.BasicArradj,
                                      HraEarned = pay.HraEarned,
                                      HraArradj = pay.HraArradj,
                                      ConveEarned = pay.ConveEarned,
                                      ConveArradj = pay.ConveArradj,
                                      MedAllowEarned = pay.MedAllowEarned,
                                      MedAllowArradj = pay.MedAllowArradj,
                                      SplAllowEarned = pay.SplAllowEarned,
                                      SplAllowArradj = pay.SplAllowArradj,
                                      OtherAll = pay.OtherAll,
                                      GrossEarn = pay.GrossEarn,
                                      Pf = pay.Pf,
                                      Esi = pay.Esi,
                                      Lwf = pay.Lwf,
                                      Pt = pay.Pt,
                                      It = pay.It,
                                      MedClaim = pay.MedClaim,
                                      OtherDed = pay.OtherDed,
                                      GrossDed = pay.GrossDed,
                                      NetPay = pay.NetPay
                                  })
                                  .FirstOrDefaultAsync();
            if (paySheet == null) throw new MessageNotFoundException("Paysheet not found");

            var file = _letterGenerationService.GeneratePaysheetPdf(paySheet, fileName);
            byte[] pdfBytes = System.IO.File.ReadAllBytes(file);
            string base64Pdf = Convert.ToBase64String(pdfBytes);
            var letterGeneration = new LetterGeneration
            {
                LetterPath = file,
                LetterContent = Convert.FromBase64String(base64Pdf),
                StaffCreationId = staffs.Id,
                FileName = fileName,
                CreatedBy = generatePaySheetRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = true
            };
            await _context.LetterGenerations.AddAsync(letterGeneration);
            await _context.SaveChangesAsync();

            return file;
        }

        public async Task<List<PayslipResponse>> GetAllPaySlip()
        {
            var paySlip = await (from payslip in _context.PaySlips
                                 join s in _context.StaffCreations on payslip.StaffId equals s.Id
                                 where payslip.IsActive == true
                                 select new PayslipResponse
                                 {
                                     Id = payslip.Id,
                                     StaffId = payslip.StaffId,
                                     StaffCreationId = s.StaffId,
                                     StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                     Basic = payslip.Basic,
                                     Hra = payslip.Hra,
                                     Da = payslip.Da,
                                     OtherAllowance = payslip.OtherAllowance,
                                     SpecialAllowance = payslip.SpecialAllowance,
                                     Conveyance = payslip.Conveyance,
                                     Tds = payslip.Tds,
                                     Esic = payslip.Esic,
                                     Pf = payslip.Pf,
                                     Pt = payslip.Pt,
                                     Othours = payslip.Othours,
                                     OtperHour = payslip.OtperHour,
                                     TotalOt = payslip.TotalOt,
                                     LopperDay = payslip.LopperDay,
                                     TotalLop = payslip.TotalLop,
                                     Lwopdays = payslip.Lwopdays,
                                     AbsentDays = payslip.AbsentDays,
                                     IsFreezed = payslip.IsFreezed,
                                     EsicemployerContribution = payslip.EsicemployerContribution,
                                     PfemployerContribution = payslip.PfemployerContribution,
                                     SalaryMonth = payslip.SalaryMonth,
                                     SalaryYear = payslip.SalaryYear,
                                     CreatedBy = payslip.CreatedBy,
                                     PaySlipComponents = payslip.PaySlipComponents
                                    .Select(pc => new PaySlipComponentResponse
                                    {
                                        Id = pc.Id,
                                        ComponentType = pc.ComponentType,
                                        ComponentName = pc.ComponentName,
                                        Amount = pc.Amount,
                                        IsTaxable = pc.IsTaxable,
                                        Remarks = pc.Remarks
                                    }).ToList()
                                 })
                                 .ToListAsync();
            if (paySlip.Count == 0)
            {
                throw new MessageNotFoundException("Payslip not found");
            }
            return paySlip;
        }

        public async Task<string> UploadSalaryStructure(IFormFile file, int createdBy)
        {
            var message = "Salary structure uploaded successfully";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string uploadFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            string uploadFilePath = Path.Combine(_excelWorkspacePath, uploadFileName);
            using (var fileStream = new FileStream(uploadFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null) throw new Exception("Worksheet not found in the uploaded file.");
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(cell => cell.Text.Trim()).ToList();
                    var requiredHeaders = new List<string>
                    {
                        "StaffId", "Basic", "HRA", "DA", "OtherAllowance", "SpecialAllowance", "Conveyance",
                        "TDSApplicable", "TDS", "ESICApplicable", "ESIC", "ESICEmployerContribution", "PFApplicable",
                        "PF", "PFEmployerContribution", "PTApplicable", "PT", "OTApplicable", "OTPerHour", "LOPApplicable",
                        "LOP", "IsLOPFixed", "IsPFFloating", "IsESICFloating", "IsPTFloating", "SalaryStructureYear",
                        "ComponentType", "ComponentName", "Amount", "IsTaxable", "Remarks"
                    };
                    var missingHeaders = requiredHeaders.Where(header => !headerRow.Contains(header)).ToList();
                    if (missingHeaders.Count == requiredHeaders.Count)
                    {
                        throw new ArgumentException("Invalid Excel file");
                    }
                    if (missingHeaders.Any())
                    {
                        throw new Exception($"Invalid Excel file. Missing headers: {string.Join(", ", missingHeaders)}");
                    }
                    var columnIndexes = requiredHeaders.ToDictionary(
                        header => header,
                        header => headerRow.IndexOf(header) + 1
                    );
                    var staffExists = await _context.StaffCreations.AnyAsync(s => s.Id == createdBy && s.IsActive == true);
                    if (!staffExists)
                    {
                        throw new MessageNotFoundException($"Staff not found");
                    }
                    var rowCount = worksheet.Dimension.Rows;
                    var salaryStructures = new List<SalaryStructure>();
                    var salaryComponents = new List<SalaryComponent>();
                    var errorLogs = new List<string>();
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]].Text.Trim();
                            if (string.IsNullOrEmpty(staffCreationIdStr))
                            {
                                errorLogs.Add($"Staff is empty at {row}");
                                continue;
                            }
                            var staffCreation = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffCreationIdStr && s.IsActive == true);
                            if (staffCreation == null)
                            {
                                errorLogs.Add($"Staff '{staffCreationIdStr}' not found at {row}");
                                continue;
                            }
                            var salaryStructure = new SalaryStructure
                            {
                                StaffId = staffCreation.Id,
                                Basic = ParseDecimal(worksheet.Cells[row, columnIndexes["Basic"]].Text, row, "Basic"),
                                Hra = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["HRA"]].Text),
                                Da = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["DA"]].Text),
                                OtherAllowance = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["OtherAllowance"]].Text),
                                SpecialAllowance = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["SpecialAllowance"]].Text),
                                Conveyance = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["Conveyance"]].Text),
                                Tdsapplicable = ParseBool(worksheet.Cells[row, columnIndexes["TDSApplicable"]].Text, row, "TDSApplicable"),
                                Tds = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["TDS"]].Text),
                                Esicapplicable = ParseBool(worksheet.Cells[row, columnIndexes["ESICApplicable"]].Text, row, "ESICApplicable"),
                                Esic = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["ESIC"]].Text),
                                EsicemployerContribution = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["ESICEmployerContribution"]].Text),
                                Pfapplicable = ParseBool(worksheet.Cells[row, columnIndexes["PFApplicable"]].Text, row, "PFApplicable"),
                                Pf = ParseDecimal(worksheet.Cells[row, columnIndexes["PF"]].Text, row, "PF"),
                                PfemployerContribution = ParseDecimal(worksheet.Cells[row, columnIndexes["PFEmployerContribution"]].Text, row, "PFEmployerContribution"),
                                Ptapplicable = ParseBool(worksheet.Cells[row, columnIndexes["PTApplicable"]].Text, row, "PTApplicable"),
                                Pt = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["PT"]].Text),
                                Otapplicable = ParseBool(worksheet.Cells[row, columnIndexes["OTApplicable"]].Text, row, "OTApplicable"),
                                OtperHour = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["OTPerHour"]].Text),
                                Lopapplicable = ParseBool(worksheet.Cells[row, columnIndexes["LOPApplicable"]].Text, row, "LOPApplicable"),
                                Lop = ParseNullableDecimal(worksheet.Cells[row, columnIndexes["LOP"]].Text),
                                IsLopfixed = ParseBool(worksheet.Cells[row, columnIndexes["IsLOPFixed"]].Text, row, "IsLOPFixed"),
                                IsPffloating = ParseBool(worksheet.Cells[row, columnIndexes["IsPFFloating"]].Text, row, "IsPFFloating"),
                                IsEsicfloating = ParseBool(worksheet.Cells[row, columnIndexes["IsESICFloating"]].Text, row, "IsESICFloating"),
                                IsPtfloating = ParseBool(worksheet.Cells[row, columnIndexes["IsPTFloating"]].Text, row, "IsPTFloating"),
                                SalaryStructureYear = ParseInt(worksheet.Cells[row, columnIndexes["SalaryStructureYear"]].Text, row, "SalaryStructureYear"),
                                IsActive = true,
                                CreatedBy = createdBy,
                                CreatedUtc = DateTime.UtcNow
                            };
                            salaryStructures.Add(salaryStructure);
                        }
                        if (salaryStructures.Any())
                        {
                            await _context.SalaryStructures.AddRangeAsync(salaryStructures);
                            await _context.SaveChangesAsync();
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

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                            if (string.IsNullOrEmpty(staffCreationIdStr))
                            {
                                errorLogs.Add($"Staff is empty at {row}");
                                continue;
                            }
                            var staffCreation = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == staffCreationIdStr && s.IsActive == true);
                            if (staffCreation == null)
                            {
                                errorLogs.Add($"Staff '{staffCreationIdStr}' not found at {row}");
                                continue;
                            }
                            var salaryStructure = salaryStructures.FirstOrDefault(ps => ps.StaffId == staffCreation.Id);
                            if (salaryStructure == null) continue;
                            var componentType = worksheet.Cells[row, columnIndexes["ComponentType"]]?.Text.Trim();
                            var componentName = worksheet.Cells[row, columnIndexes["ComponentName"]]?.Text.Trim();
                            var amount = worksheet.Cells[row, columnIndexes["Amount"]]?.Text.Trim();
                            var isTaxableStr = worksheet.Cells[row, columnIndexes["IsTaxable"]]?.Text.Trim();
                            var remarks = worksheet.Cells[row, columnIndexes["Remarks"]]?.Text.Trim();
                            if (isTaxableStr == null)
                            {
                                errorLogs.Add($"IsTaxable is empty at {row}");
                                continue;
                            }
                            if (!string.IsNullOrEmpty(componentType) && !string.IsNullOrEmpty(componentName) && !string.IsNullOrEmpty(amount))
                            {
                                var salaryComponent = new SalaryComponent
                                {
                                    SalaryStructureId = salaryStructure.Id,
                                    StaffId = salaryStructure.StaffId,
                                    ComponentType = componentType,
                                    ComponentName = componentName,
                                    Amount = amount,
                                    IsTaxable = ParseBool(isTaxableStr, row, "IsTaxable"),
                                    Remarks = remarks,
                                    IsActive = true,
                                    CreatedBy = createdBy,
                                    CreatedUtc = DateTime.UtcNow
                                };
                                salaryComponents.Add(salaryComponent);
                            }
                        }
                        if (salaryComponents.Any())
                        {
                            await _context.SalaryComponents.AddRangeAsync(salaryComponents);
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
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                }
            }
            return message;
        }

        public async Task<(Stream PdfStream, string FileName)> ViewPayslip(int staffId, int month, int year)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive == true);
            if(staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            string filePrefix = $"Payslip_{staff.StaffId}_{monthName}_{year}_";
            var letterGeneration = await _context.LetterGenerations
                 .Where(x => x.FileName.StartsWith(filePrefix) && x.IsActive)
                 .OrderByDescending(x => x.CreatedUtc)
                 .FirstOrDefaultAsync();

            if (letterGeneration == null)
            {
                throw new FileNotFoundException("Payslip not found.");
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

        public async Task<string> DownloadPayslip(int staffId, int month, int year)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            string filePrefix = $"Payslip_{staff.StaffId}_{monthName}_{year}_";
            var paySlip = await _context.LetterGenerations
                .Where(x => x.FileName.StartsWith(filePrefix) && x.IsActive)
                .OrderByDescending(x => x.CreatedUtc)
                .FirstOrDefaultAsync();

            if (paySlip == null)
            {
                throw new MessageNotFoundException("Payslip not found");
            }
            string file = paySlip.FileName ?? string.Empty;
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new MessageNotFoundException("File is empty");
            }
            return Path.Combine(_workspacePath, file);
        }

        public async Task<SalaryStructureResponse> GetSalaryStructure(int staffId)
        {
            var salaryStructure = await (from salary in _context.SalaryStructures
                                         join s in _context.StaffCreations on salary.StaffId equals s.Id
                                         where salary.StaffId == staffId && salary.IsActive == true
                                         select new SalaryStructureResponse
                                         {
                                             Id = salary.Id,
                                             StaffId = salary.StaffId,
                                             StaffCreationId = s.StaffId,
                                             StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                             Basic = salary.Basic,
                                             Hra = salary.Hra,
                                             Da = salary.Da,
                                             OtherAllowance = salary.OtherAllowance,
                                             SpecialAllowance = salary.SpecialAllowance,
                                             Conveyance = salary.Conveyance,
                                             Tdsapplicable = salary.Tdsapplicable,
                                             Tds = salary.Tds,
                                             Esicapplicable = salary.Esicapplicable,
                                             Esic = salary.Esic,
                                             EsicemployerContribution = salary.EsicemployerContribution,
                                             Pfapplicable = salary.Pfapplicable,
                                             Pf = salary.Pf,
                                             PfemployerContribution = salary.PfemployerContribution,
                                             Ptapplicable = salary.Ptapplicable,
                                             Pt = salary.Pt,
                                             Otapplicable = salary.Otapplicable,
                                             OtperHour = salary.OtperHour,
                                             Lopapplicable = salary.Lopapplicable,
                                             Lop = salary.Lop,
                                             IsLopfixed = salary.IsLopfixed,
                                             IsPffloating = salary.IsPffloating,
                                             IsEsicfloating = salary.IsEsicfloating,
                                             IsPtfloating = salary.IsPtfloating,
                                             SalaryStructureYear = salary.SalaryStructureYear,
                                             CreatedBy = salary.CreatedBy,
                                             SalaryComponents = salary.SalaryComponents
                                            .Select(pc => new PaySlipComponentResponse
                                            {
                                                Id = pc.Id,
                                                ComponentType = pc.ComponentType,
                                                ComponentName = pc.ComponentName,
                                                Amount = pc.Amount,
                                                IsTaxable = pc.IsTaxable,
                                                Remarks = pc.Remarks
                                            }).ToList()
                                         })
                                         .FirstOrDefaultAsync();
            if (salaryStructure == null)
            {
                throw new MessageNotFoundException("Salary structure not found");
            }
            return salaryStructure;
        }

        public async Task<List<SalaryStructureResponse>> GetAllSalaryStructure()
        {
            var salaryStructure = await (from salary in _context.SalaryStructures
                                         join s in _context.StaffCreations on salary.StaffId equals s.Id
                                         where salary.IsActive == true
                                         select new SalaryStructureResponse
                                         {
                                             Id = salary.Id,
                                             StaffId = salary.StaffId,
                                             StaffCreationId = s.StaffId,
                                             StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                             Basic = salary.Basic,
                                             Hra = salary.Hra,
                                             Da = salary.Da,
                                             OtherAllowance = salary.OtherAllowance,
                                             SpecialAllowance = salary.SpecialAllowance,
                                             Conveyance = salary.Conveyance,
                                             Tdsapplicable = salary.Tdsapplicable,
                                             Tds = salary.Tds,
                                             Esicapplicable = salary.Esicapplicable,
                                             Esic = salary.Esic,
                                             EsicemployerContribution = salary.EsicemployerContribution,
                                             Pfapplicable = salary.Pfapplicable,
                                             Pf = salary.Pf,
                                             PfemployerContribution = salary.PfemployerContribution,
                                             Ptapplicable = salary.Ptapplicable,
                                             Pt = salary.Pt,
                                             Otapplicable = salary.Otapplicable,
                                             OtperHour = salary.OtperHour,
                                             Lopapplicable = salary.Lopapplicable,
                                             Lop = salary.Lop,
                                             IsLopfixed = salary.IsLopfixed,
                                             IsPffloating = salary.IsPffloating,
                                             IsEsicfloating = salary.IsEsicfloating,
                                             IsPtfloating = salary.IsPtfloating,
                                             SalaryStructureYear = salary.SalaryStructureYear,
                                             CreatedBy = salary.CreatedBy,
                                             SalaryComponents = salary.SalaryComponents
                                            .Select(pc => new PaySlipComponentResponse
                                            {
                                                Id = pc.Id,
                                                ComponentType = pc.ComponentType,
                                                ComponentName = pc.ComponentName,
                                                Amount = pc.Amount,
                                                IsTaxable = pc.IsTaxable,
                                                Remarks = pc.Remarks
                                            }).ToList()
                                         })
                                         .ToListAsync();
            if (salaryStructure.Count == 0)
            {
                throw new MessageNotFoundException("Salary structure not found");
            }
            return salaryStructure;
        }

        private decimal ParseDecimal(string value, int row, string columnName)
        {
            if (decimal.TryParse(value, out decimal result))
                return result;
            throw new Exception($"Invalid value '{value}' in column '{columnName}' at row {row}.");
        }

        private decimal? ParseNullableDecimal(string value)
        {
            return decimal.TryParse(value, out decimal result) ? result : (decimal?)null;
        }

        private int ParseInt(string value, int row, string columnName)
        {
            if (int.TryParse(value, out int result))
                return result;
            throw new Exception($"Invalid integer '{value}' in column '{columnName}' at row {row}.");
        }

        private bool ParseBool(string value, int row, string columnName)
        {
            return value.ToLower() == "true" || value == "1";
        }
    }
}