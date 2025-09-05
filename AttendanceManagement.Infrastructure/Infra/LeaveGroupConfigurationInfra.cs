using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class LeaveGroupConfigurationInfra : ILeaveGroupConfigurationInfra
    {
        private readonly AttendanceManagementSystemContext _context;

        public LeaveGroupConfigurationInfra(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveGroupConfigurationDto>> GetAllConfigurations()
        {
            var allLeave = await (from leaveGroup in _context.LeaveGroupConfigurations
                                  join leaveType in _context.LeaveTypes
                                  on leaveGroup.LeaveTypeId equals leaveType.Id
                                  select new LeaveGroupConfigurationDto
                                  {
                                      LeaveGroupConfigurationId = leaveGroup.Id,
                                      LeaveGroupConfigurationName = leaveGroup.LeaveGroupConfigurationName,
                                      LeaveTypeId = leaveGroup.LeaveTypeId,
                                      LeaveTypeName = leaveType.Name,
                                      PaidLeave = leaveGroup.PaidLeave,
                                      Accountable = leaveGroup.Accountable,
                                      CarryForward = leaveGroup.CarryForward,
                                      MaxAccountDays = leaveGroup.MaxAccountDays,
                                      MaxAccountYears = leaveGroup.MaxAccountYears,
                                      MaxDaysPerReq = leaveGroup.MaxDaysPerReq,
                                      MinDaysPerReq = leaveGroup.MinDaysPerReq,
                                      ProRata = leaveGroup.ProRata,
                                      ElgInMonths = leaveGroup.ElgInMonths,
                                      IsCalcToWorkingDays = leaveGroup.IsCalcToWorkingDays,
                                      ClacToWorkingDays = leaveGroup.ClacToWorkingDays,
                                      ConsiderWo = leaveGroup.ConsiderWo,
                                      ConsiderPh = leaveGroup.ConsiderPh,
                                      IsExcessEligibleAllowed = leaveGroup.IsExcessEligibleAllowed,
                                      IsHalfDayApplicable = leaveGroup.IsHalfDayApplicable,
                                      IsEncashmentAllowed = leaveGroup.IsEncashmentAllowed,
                                      CreditFreq = leaveGroup.CreditFreq,
                                      CreditDays = leaveGroup.CreditDays,
                                      RoundOffTo = leaveGroup.RoundOffTo,
                                      RoundOffValue = leaveGroup.RoundOffValue,
                                      IsActive = leaveGroup.IsActive,
                                      CreatedBy = leaveGroup.CreatedBy
                                  })
                                  .ToListAsync();
            if (allLeave.Count == 0)
            {
                throw new MessageNotFoundException("No leave group configurations found");
            }
            return allLeave;
        }

        public async Task<LeaveGroupConfigurationDto> GetConfigurationDetailsById(int leaveGroupConfigurationId)
        {
            var allLeave = await (from leaveGroup in _context.LeaveGroupConfigurations
                            join leaveType in _context.LeaveTypes
                            on leaveGroup.LeaveTypeId equals leaveType.Id
                            where leaveGroup.Id == leaveGroupConfigurationId
                            select new LeaveGroupConfigurationDto
                            {
                                LeaveGroupConfigurationId = leaveGroup.Id,
                                LeaveGroupConfigurationName = leaveGroup.LeaveGroupConfigurationName,
                                LeaveTypeId = leaveGroup.LeaveTypeId,
                                LeaveTypeName = leaveType.Name,
                                PaidLeave = leaveGroup.PaidLeave,
                                Accountable = leaveGroup.Accountable,
                                CarryForward = leaveGroup.CarryForward,
                                MaxAccountDays = leaveGroup.MaxAccountDays,
                                MaxAccountYears = leaveGroup.MaxAccountYears,
                                MaxDaysPerReq = leaveGroup.MaxDaysPerReq,
                                MinDaysPerReq = leaveGroup.MinDaysPerReq,
                                ProRata = leaveGroup.ProRata,
                                ElgInMonths = leaveGroup.ElgInMonths,
                                IsCalcToWorkingDays = leaveGroup.IsCalcToWorkingDays,
                                ClacToWorkingDays = leaveGroup.ClacToWorkingDays,
                                ConsiderWo = leaveGroup.ConsiderWo,
                                ConsiderPh = leaveGroup.ConsiderPh,
                                IsExcessEligibleAllowed = leaveGroup.IsExcessEligibleAllowed,
                                IsHalfDayApplicable = leaveGroup.IsHalfDayApplicable,
                                IsEncashmentAllowed = leaveGroup.IsEncashmentAllowed,
                                CreditFreq = leaveGroup.CreditFreq,
                                CreditDays = leaveGroup.CreditDays,
                                RoundOffTo = leaveGroup.RoundOffTo,
                                RoundOffValue = leaveGroup.RoundOffValue,
                                IsActive = leaveGroup.IsActive,
                                CreatedBy = leaveGroup.CreatedBy
                            })
                            .FirstOrDefaultAsync();
            if (allLeave == null)
            {
                throw new MessageNotFoundException("Leave group configuration not found");
            }

            return allLeave;
        }

        public async Task<string> CreateConfigurations(LeaveGroupConfigurationRequest configurationRequest)
        {
            var message = "Leave group configuration added successfully";
            var leaveType = await _context.LeaveTypes.AnyAsync(h => h.Id == configurationRequest.LeaveTypeId && h.IsActive);
            if (!leaveType) throw new MessageNotFoundException("Leave type not found");
            var duplicateConfiguration = await _context.LeaveGroupConfigurations
                .AnyAsync(c => c.LeaveGroupConfigurationName.ToLower() == configurationRequest.LeaveGroupConfigurationName.ToLower()
                               && c.LeaveTypeId == configurationRequest.LeaveTypeId
                               && c.IsActive);
            if (duplicateConfiguration)
                throw new ConflictException("Leave group configuration name already exists for this leave type");
            var configuration = new LeaveGroupConfiguration
            {
                LeaveGroupConfigurationName = configurationRequest.LeaveGroupConfigurationName,
                LeaveTypeId = configurationRequest.LeaveTypeId,
                PaidLeave = configurationRequest.PaidLeave,
                Accountable =configurationRequest.Accountable,
                CarryForward = configurationRequest.CarryForward,
                MaxAccountDays = configurationRequest.MaxAccountDays,
                MaxAccountYears = configurationRequest.MaxAccountYears,
                MaxDaysPerReq = configurationRequest.MaxDaysPerReq,
                MinDaysPerReq = configurationRequest.MinDaysPerReq,
                ProRata = configurationRequest.ProRata,
                ElgInMonths = configurationRequest.ElgInMonths,
                IsCalcToWorkingDays = configurationRequest.IsCalcToWorkingDays,
                ClacToWorkingDays = configurationRequest.ClacToWorkingDays,
                ConsiderWo = configurationRequest.ConsiderWo,
                ConsiderPh = configurationRequest.ConsiderPh,
                IsExcessEligibleAllowed = configurationRequest.IsExcessEligibleAllowed,
                IsHalfDayApplicable = configurationRequest.IsHalfDayApplicable,
                IsEncashmentAllowed = configurationRequest.IsEncashmentAllowed,
                CreditFreq = configurationRequest.CreditFreq,
                CreditDays = configurationRequest.CreditDays,
                RoundOffTo = configurationRequest.RoundOffTo,
                RoundOffValue = configurationRequest.RoundOffValue,
                IsActive = configurationRequest.IsActive,
                CreatedBy = configurationRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.LeaveGroupConfigurations.AddAsync(configuration);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateConfigurations(UpdateLeaveGroupConfiguration configuration)
        {
            var message = "Leave group configuration updated successfully";
            var leaveType = await _context.LeaveTypes.AnyAsync(h => h.Id == configuration.LeaveTypeId && h.IsActive);
            if (!leaveType) throw new MessageNotFoundException("Leave type not found");
            var existingTransaction = await _context.LeaveGroupConfigurations.FirstOrDefaultAsync(t => t.Id == configuration.LeaveGroupConfigurationId);
            if (existingTransaction == null)
            {
                throw new MessageNotFoundException("Leave configuration group not found");
            }
            var duplicateConfiguration = await _context.LeaveGroupConfigurations
            .AnyAsync(c => c.LeaveGroupConfigurationName.ToLower() == configuration.LeaveGroupConfigurationName.ToLower()
                      && c.LeaveTypeId == configuration.LeaveTypeId
                      && c.Id != configuration.LeaveGroupConfigurationId
                      && c.IsActive);
            if (duplicateConfiguration)
                throw new ConflictException("Leave group configuration name already exists for this leave type");
            existingTransaction.LeaveGroupConfigurationName = configuration.LeaveGroupConfigurationName ?? existingTransaction.LeaveGroupConfigurationName;
            existingTransaction.LeaveTypeId = configuration.LeaveTypeId ?? existingTransaction.LeaveTypeId;
            existingTransaction.PaidLeave = configuration.PaidLeave ?? existingTransaction.PaidLeave;
            existingTransaction.Accountable = configuration.Accountable ?? existingTransaction.Accountable;
            existingTransaction.CarryForward = configuration.CarryForward ?? existingTransaction.CarryForward;
            existingTransaction.MaxAccountDays = configuration.MaxAccountDays ?? existingTransaction.MaxAccountDays;
            existingTransaction.MaxAccountYears = configuration.MaxAccountYears ?? existingTransaction.MaxAccountYears;
            existingTransaction.MaxDaysPerReq = configuration.MaxDaysPerReq ?? existingTransaction.MaxDaysPerReq;
            existingTransaction.MinDaysPerReq = configuration.MinDaysPerReq ?? existingTransaction.MinDaysPerReq;
            existingTransaction.ProRata = configuration.ProRata ?? existingTransaction.ProRata;
            existingTransaction.ElgInMonths = configuration.ElgInMonths ?? existingTransaction.ElgInMonths;
            existingTransaction.IsCalcToWorkingDays = configuration.IsCalcToWorkingDays ?? existingTransaction.IsCalcToWorkingDays;
            existingTransaction.ClacToWorkingDays = configuration.ClacToWorkingDays ?? existingTransaction.ClacToWorkingDays;
            existingTransaction.ConsiderWo = configuration.ConsiderWo ?? existingTransaction.ConsiderWo;
            existingTransaction.ConsiderPh = configuration.ConsiderPh ?? existingTransaction.ConsiderPh;
            existingTransaction.IsExcessEligibleAllowed = configuration.IsExcessEligibleAllowed ?? existingTransaction.IsExcessEligibleAllowed;
            existingTransaction.IsHalfDayApplicable = configuration.IsHalfDayApplicable ?? existingTransaction.IsHalfDayApplicable;
            existingTransaction.IsEncashmentAllowed = configuration.IsEncashmentAllowed ?? existingTransaction.IsEncashmentAllowed;
            existingTransaction.CreditFreq = configuration.CreditFreq ?? existingTransaction.CreditFreq;
            existingTransaction.CreditDays = configuration.CreditDays ?? existingTransaction.CreditDays;
            existingTransaction.RoundOffTo = configuration.RoundOffTo ?? existingTransaction.RoundOffTo;
            existingTransaction.RoundOffValue = configuration.RoundOffValue ?? existingTransaction.RoundOffValue;
            existingTransaction.IsActive = configuration.IsActive;
            existingTransaction.UpdatedBy = configuration.UpdatedBy;
            existingTransaction.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }
    }
}