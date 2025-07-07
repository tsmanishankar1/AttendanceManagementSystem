using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface ICompanyMasterInfra
    {
        Task<List<CompanyMasterResponse>> GetAll();
        Task<string> Add(CompanyMasterRequest companyMasterRequest);
        Task<string> Update(CompanyMasterDto companyMaster);
    }
}
