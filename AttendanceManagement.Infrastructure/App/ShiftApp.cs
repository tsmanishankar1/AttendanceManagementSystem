using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class ShiftApp : IShiftApp
    {
        private readonly IShiftInfra _shiftInfra;

        public ShiftApp(IShiftInfra shiftInfra)
        {
            _shiftInfra = shiftInfra;
        }

        public async Task<string> AssignShiftToStaffAsync(AssignShiftRequest assignShift)
            => await _shiftInfra.AssignShiftToStaffAsync(assignShift);

        public async Task AttendanceFreezeDate(int staffId, DateOnly date)
            => await _shiftInfra.AttendanceFreezeDate(staffId, date);

        public async Task<string> CreateRegularShiftAsync(RegularShiftRequest regularShift)
            => await _shiftInfra.CreateRegularShiftAsync(regularShift);

        public async Task<string> CreateShiftAsync(ShiftRequest newShift)
            => await _shiftInfra.CreateShiftAsync(newShift);

        public async Task<List<AssignedShiftResponse>> GetAllAssignedShifts(int approverId)
            => await _shiftInfra.GetAllAssignedShifts(approverId);

        public async Task<List<ShiftResponse>> GetAllShiftsAsync()
            => await _shiftInfra.GetAllShiftsAsync();

        public async Task<List<StaffsDto>> GetStaffByDivisionIdAsync(int divisionId)
            => await _shiftInfra.GetStaffByDivisionIdAsync(divisionId);

        public async Task<string> UpdateShiftAsync(UpdateShift updatedShift)
            => await _shiftInfra.UpdateShiftAsync(updatedShift);
    }
}
