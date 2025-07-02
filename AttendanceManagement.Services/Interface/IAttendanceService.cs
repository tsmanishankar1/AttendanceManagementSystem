using AttendanceManagement.AtrakInputModels;
using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IAttendanceService
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
