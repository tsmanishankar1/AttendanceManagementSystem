using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IShiftService
    {
        Task<List<StaffsDto>> GetStaffByDivisionIdAsync(int divisionId);
        Task<List<ShiftResponse>> GetAllShiftsAsync();
        Task<string> CreateShiftAsync(ShiftRequest newShift);
        Task<string> UpdateShiftAsync(UpdateShift updatedShift);
        Task<string> CreateRegularShiftAsync(RegularShiftRequest regularShift);
        Task<string> AssignShiftToStaffAsync(AssignShiftRequest assignShift);
        Task AttendanceFreezeDate(int staffId, DateOnly date);
        Task<List<AssignedShiftResponse>> GetAllAssignedShifts(int approverId);
    }
}
