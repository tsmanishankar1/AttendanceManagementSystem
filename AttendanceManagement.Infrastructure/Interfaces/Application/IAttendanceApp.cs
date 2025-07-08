using AttendanceManagement.Application.Dtos.Atrak;
using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IAttendanceApp
    {
        Task<SmaxTransactionResponse?> GetCheckInCheckOutAsync(int staffId);
        Task<string> AddGraceTimeAndBreakTime(AttendanceGraceTimeCalcRequest request);
        Task<List<AttendanceGraceTimeCalcResponse>> GetGraceTimeAndBreakTime();
        Task<string> UpdateGraceTimeAndBreakTime(UpdateAttendanceGraceTimeCalc request);
        Task<List<AttendanceRecordResponse>> AttendanceRecords();
        Task<List<StaffInfoDto>> GetAllStaffsByDepartmentAndDivision(GetStaffByDepartmentDivision staff);
        Task<List<AttendanceRecordDto>> GetAttendanceRecords(AttendanceStatusResponse attendanceStatus);
        Task<string> FreezeAttendanceRecords(AttendanceFreezeRequest attendanceFreezeRequest);
    }
}
