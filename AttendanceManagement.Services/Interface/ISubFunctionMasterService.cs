using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ISubFunctionMasterService
    {
        Task<List<SubFunctionResponse>> GetAllSubFunctionsAsync();
        Task<string> CreateSubFunctionAsync(SubFunctionRequest subFunctionMaster);
        Task<string> UpdateSubFunctionAsync(UpdateSubFunction subFunctionMaster);
    }
}
