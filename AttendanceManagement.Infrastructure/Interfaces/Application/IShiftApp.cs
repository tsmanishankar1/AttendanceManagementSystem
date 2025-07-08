using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IShiftApp
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
