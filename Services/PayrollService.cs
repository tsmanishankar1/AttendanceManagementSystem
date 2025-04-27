using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace AttendanceManagement.Services
{
    public class PayrollService
    {
        private readonly AttendanceManagementSystemContext _context;

        public PayrollService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<string> UploadPaySlip(IFormFile file, int createdBy)
        {
            var message = "Payslip uploaded successfully";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
                        throw new Exception($"StaffId {createdBy} not found in the database.");
                    }
                    var rowCount = worksheet.Dimension.Rows;
                    var paySlips = new List<PaySlip>();
                    var paySlipComponents = new List<PaySlipComponent>();
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            for (int row = 2; row <= rowCount; row++)
                            {
                                var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                if (string.IsNullOrEmpty(staffCreationIdStr)) throw new Exception($"Invalid or missing StaffId at row {row}.");
                                var match = Regex.Match(staffCreationIdStr, @"([A-Za-z]+)(\d+)");
                                if (!match.Success) throw new Exception($"Invalid StaffId format at row {row}.");
                                var shortName = match.Groups[1].Value;
                                var staffId = int.Parse(match.Groups[2].Value);
                                var organizationId = _context.OrganizationTypes
                                    .Where(o => o.ShortName.ToLower() == shortName.ToLower() && o.IsActive)
                                    .Select(o => o.Id)
                                    .FirstOrDefault();
                                if (organizationId == 0) throw new Exception($"Organization with short name '{shortName}' not found at row {row}.");
                                var staffCreationId = _context.StaffCreations
                                    .Where(s => (s.OrganizationTypeId == organizationId || s.Id == organizationId) && s.Id == staffId && s.IsActive == true)
                                    .Select(s => s.Id)
                                    .FirstOrDefault();
                                if (staffCreationId == 0) throw new Exception($"Staff with ID '{staffId}' not found at row {row}.");
                                var paySlip = new PaySlip
                                {
                                    StaffId = staffId,
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
                            await _context.PaySlips.AddRangeAsync(paySlips);
                            await _context.SaveChangesAsync();

                            for (int row = 2; row <= rowCount; row++)
                            {
                                var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                if (string.IsNullOrEmpty(staffCreationIdStr)) throw new Exception($"Invalid or missing StaffId at row {row}.");
                                var match = Regex.Match(staffCreationIdStr, @"([A-Za-z]+)(\d+)");
                                if (!match.Success) throw new Exception($"Invalid StaffId format at row {row}.");
                                var shortName = match.Groups[1].Value;
                                var staffId = int.Parse(match.Groups[2].Value);
                                var organizationId = _context.OrganizationTypes
                                    .Where(o => o.ShortName.ToLower() == shortName.ToLower() && o.IsActive)
                                    .Select(o => o.Id)
                                    .FirstOrDefault();
                                if (organizationId == 0) throw new Exception($"Organization with short name '{shortName}' not found at row {row}.");
                                var staffCreationId = _context.StaffCreations
                                    .Where(s => (s.OrganizationTypeId == organizationId || s.Id == organizationId) && s.Id == staffId && s.IsActive == true)
                                    .Select(s => s.Id)
                                    .FirstOrDefault();
                                if (staffCreationId == 0) throw new Exception($"Staff with ID '{staffId}' not found at row {row}.");
                                var paySlip = paySlips.FirstOrDefault(ps => ps.StaffId == staffCreationId && ps.IsActive);
                                if (paySlip == null) continue;
                                var componentType = worksheet.Cells[row, columnIndexes["ComponentType"]]?.Text.Trim();
                                var componentName = worksheet.Cells[row, columnIndexes["ComponentName"]]?.Text.Trim();
                                var amount = worksheet.Cells[row, columnIndexes["Amount"]]?.Text.Trim();
                                var isTaxableStr = worksheet.Cells[row, columnIndexes["IsTaxable"]]?.Text.Trim();
                                var remarks = worksheet.Cells[row, columnIndexes["Remarks"]]?.Text.Trim();
                                if (isTaxableStr == null) throw new Exception($"Invalid or missing IsTaxable value at row {row}.");
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
            return message;
        }

        public async Task<PayslipResponse> GetPaySlip(int staffId)
        {
            var paySlip = await (from payslip in _context.PaySlips
                                 join s in _context.StaffCreations on payslip.StaffId equals s.Id
                                 where payslip.StaffId == staffId && payslip.IsActive == true
                                 select new PayslipResponse
                                 {
                                     Id = payslip.Id,
                                     StaffId = payslip.StaffId,
                                     StaffCreationId = $"{_context.OrganizationTypes.Where(o => o.Id == s.OrganizationTypeId && o.IsActive).Select(o => o.ShortName).FirstOrDefault()}{s.Id}",
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
            return paySlip;
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
                        throw new Exception($"StaffId {createdBy} not found in the database.");
                    }
                    var rowCount = worksheet.Dimension.Rows;
                    var salaryStructures = new List<SalaryStructure>();
                    var salaryComponents = new List<SalaryComponent>();
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            for (int row = 2; row <= rowCount; row++)
                            {
                                var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                if (string.IsNullOrEmpty(staffCreationIdStr)) throw new Exception($"Invalid or missing StaffId at row {row}.");
                                var match = Regex.Match(staffCreationIdStr, @"([A-Za-z]+)(\d+)");
                                if (!match.Success) throw new Exception($"Invalid StaffId format at row {row}.");
                                var shortName = match.Groups[1].Value;
                                var staffId = int.Parse(match.Groups[2].Value);
                                var organizationId = _context.OrganizationTypes
                                    .Where(o => o.ShortName.ToLower() == shortName.ToLower() && o.IsActive)
                                    .Select(o => o.Id)
                                    .FirstOrDefault();
                                if (organizationId == 0) throw new Exception($"Organization with short name '{shortName}' not found at row {row}.");
                                var staffCreationId = _context.StaffCreations
                                    .Where(s => (s.OrganizationTypeId == organizationId || s.Id == organizationId) && s.Id == staffId && s.IsActive == true)
                                    .Select(s => s.Id)
                                    .FirstOrDefault();
                                if (staffCreationId == 0) throw new Exception($"Staff with ID '{staffId}' not found at row {row}.");
                                var salaryStructure = new SalaryStructure
                                {
                                    StaffId = staffId,
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
                            await _context.SalaryStructures.AddRangeAsync(salaryStructures);
                            await _context.SaveChangesAsync();

                            for (int row = 2; row <= rowCount; row++)
                            {
                                var staffCreationIdStr = worksheet.Cells[row, columnIndexes["StaffId"]]?.Text.Trim();
                                if (string.IsNullOrEmpty(staffCreationIdStr)) throw new Exception($"Invalid or missing StaffId at row {row}.");
                                var match = Regex.Match(staffCreationIdStr, @"([A-Za-z]+)(\d+)");
                                if (!match.Success) throw new Exception($"Invalid StaffId format at row {row}.");
                                var shortName = match.Groups[1].Value;
                                var staffId = int.Parse(match.Groups[2].Value);
                                var organizationId = _context.OrganizationTypes
                                    .Where(o => o.ShortName.ToLower() == shortName.ToLower() && o.IsActive)
                                    .Select(o => o.Id)
                                    .FirstOrDefault();
                                if (organizationId == 0) throw new Exception($"Organization with short name '{shortName}' not found at row {row}.");
                                var staffCreationId = _context.StaffCreations
                                    .Where(s => (s.OrganizationTypeId == organizationId || s.Id == organizationId) && s.Id == staffId && s.IsActive == true)
                                    .Select(s => s.Id)
                                    .FirstOrDefault();
                                if (staffCreationId == 0) throw new Exception($"Staff with ID '{staffId}' not found at row {row}.");
                                var salaryStructure = salaryStructures.FirstOrDefault(ps => ps.StaffId == staffCreationId);
                                if (salaryStructure == null) continue;
                                var componentType = worksheet.Cells[row, columnIndexes["ComponentType"]]?.Text.Trim();
                                var componentName = worksheet.Cells[row, columnIndexes["ComponentName"]]?.Text.Trim();
                                var amount = worksheet.Cells[row, columnIndexes["Amount"]]?.Text.Trim();
                                var isTaxableStr = worksheet.Cells[row, columnIndexes["IsTaxable"]]?.Text.Trim();
                                var remarks = worksheet.Cells[row, columnIndexes["Remarks"]]?.Text.Trim();
                                if (isTaxableStr == null) throw new Exception($"Invalid or missing IsTaxable value at row {row}.");
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
            return message;
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