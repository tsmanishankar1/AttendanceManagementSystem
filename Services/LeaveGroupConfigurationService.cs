using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttendanceManagement.Services
{
    public class LeaveGroupConfigurationService
    {
        private readonly AttendanceManagementSystemContext _context;

        public LeaveGroupConfigurationService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveGroupConfigurationDto>> GetAllConfigurations()
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

        public async Task<string> CreateConfigurations(LeaveGroupConfigurationRequest configurationRequeset)
        {
            var message = "Leave group configuration added successfully";
            var configuration = new LeaveGroupConfiguration
            {
                LeaveGroupConfigurationName = configurationRequeset.LeaveGroupConfigurationName,
                LeaveTypeId = configurationRequeset.LeaveTypeId,
                PaidLeave = configurationRequeset.PaidLeave,
                Accountable = configurationRequeset.Accountable,
                CarryForward = configurationRequeset.CarryForward,
                MaxAccountDays = configurationRequeset.MaxAccountDays,
                MaxAccountYears = configurationRequeset.MaxAccountYears,
                MaxDaysPerReq = configurationRequeset.MaxDaysPerReq,
                MinDaysPerReq = configurationRequeset.MinDaysPerReq,
                ProRata = configurationRequeset.ProRata,
                ElgInMonths = configurationRequeset.ElgInMonths,
                IsCalcToWorkingDays = configurationRequeset.IsCalcToWorkingDays,
                ClacToWorkingDays = configurationRequeset.ClacToWorkingDays,
                ConsiderWo = configurationRequeset.ConsiderWo,
                ConsiderPh = configurationRequeset.ConsiderPh,
                IsExcessEligibleAllowed = configurationRequeset.IsExcessEligibleAllowed,
                IsHalfDayApplicable = configurationRequeset.IsHalfDayApplicable,
                IsEncashmentAllowed = configurationRequeset.IsEncashmentAllowed,
                CreditFreq = configurationRequeset.CreditFreq,
                CreditDays = configurationRequeset.CreditDays,
                RoundOffTo = configurationRequeset.RoundOffTo,
                RoundOffValue = configurationRequeset.RoundOffValue,
                IsActive = configurationRequeset.IsActive,
                CreatedBy = configurationRequeset.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            _context.LeaveGroupConfigurations.Add(configuration);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateConfigurations(UpdateLeaveGroupConfiguration configuration)
        {
            var message = "Leave group configuration updated successfully";
            var existingTransaction = _context.LeaveGroupConfigurations.FirstOrDefault(t => t.Id == configuration.LeaveGroupConfigurationId);
            if (existingTransaction == null)
            {
                throw new MessageNotFoundException("Leave configuration group not found");
            }

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