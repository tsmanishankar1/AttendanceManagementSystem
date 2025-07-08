using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class CompanyMasterApp : ICompanyMasterApp
    {
        private readonly ICompanyMasterInfra _companyMasterInfra;
        public CompanyMasterApp(ICompanyMasterInfra companyMasterInfra)
        {
            _companyMasterInfra = companyMasterInfra;
        }

        public async Task<string> Add(CompanyMasterRequest companyMasterRequest)
            => await _companyMasterInfra.Add(companyMasterRequest);

        public async Task<List<CompanyMasterResponse>> GetAll()
            => await _companyMasterInfra.GetAll();

        public async Task<string> Update(CompanyMasterDto companyMaster)
            => await _companyMasterInfra.Update(companyMaster);
    }
}