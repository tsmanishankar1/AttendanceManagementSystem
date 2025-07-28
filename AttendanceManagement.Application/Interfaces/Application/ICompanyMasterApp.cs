using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ICompanyMasterApp
    {
        Task<List<CompanyMasterResponse>> GetAll();
        Task<string> Add(CompanyMasterRequest companyMasterRequest);
        Task<string> Update(CompanyMasterDto companyMaster);
    }
}
