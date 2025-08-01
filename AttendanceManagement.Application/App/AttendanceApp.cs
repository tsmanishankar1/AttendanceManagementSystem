using AttendanceManagement.Application.Dtos.Atrak;
using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;

public class AttendanceApp : IAttendanceApp
{
    private readonly IAttendanceInfra _attendanceInfra;

    public AttendanceApp(IAttendanceInfra attendanceInfra)
    {
        _attendanceInfra = attendanceInfra;
    }

    public async Task<string> AddGraceTimeAndBreakTime(AttendanceGraceTimeCalcRequest request)
        => await _attendanceInfra.AddGraceTimeAndBreakTime(request);

    public async Task<List<AttendanceRecordResponse>> AttendanceRecords()
        => await _attendanceInfra.AttendanceRecords();

    public async Task<string> FreezeAttendanceRecords(AttendanceFreezeRequest attendanceFreezeRequest)
        => await _attendanceInfra.FreezeAttendanceRecords(attendanceFreezeRequest);

    public async Task<List<StaffInfoDto>> GetAllStaffsByDepartmentAndDivision(GetStaffByDepartmentDivision staff)
        => await _attendanceInfra.GetAllStaffsByDepartmentAndDivision(staff);

    public async Task<List<AttendanceRecordDto>> GetAttendanceRecords(AttendanceStatusResponse attendanceStatus)
        => await _attendanceInfra.GetAttendanceRecords(attendanceStatus);

    public async Task<List<AttendanceRecordDto>> GetFreezedAttendanceRecords(AttendanceStatusResponse attendanceStatus)
    => await _attendanceInfra.GetFreezedAttendanceRecords(attendanceStatus);

    public async Task<SmaxTransactionResponse?> GetCheckInCheckOutAsync(int staffId)
        => await _attendanceInfra.GetCheckInCheckOutAsync(staffId);

    public async Task<List<AttendanceGraceTimeCalcResponse>> GetGraceTimeAndBreakTime()
        => await _attendanceInfra.GetGraceTimeAndBreakTime();

    public async Task<string> UpdateGraceTimeAndBreakTime(UpdateAttendanceGraceTimeCalc request)
        => await _attendanceInfra.UpdateGraceTimeAndBreakTime(request);
}