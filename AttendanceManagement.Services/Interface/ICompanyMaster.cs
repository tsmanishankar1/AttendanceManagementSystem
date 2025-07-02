using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ICompanyMaster
    {
        Task<List<CompanyMasterResponse>> GetAll();
        Task<string> Add(CompanyMasterRequest companyMasterRequest);
        Task<string> Update(CompanyMasterDto companyMaster);
    }
}
