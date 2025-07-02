using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IGradeMasterService
    {
        Task<List<GradeMasterResponse>> GetAllGrades();
        Task<string> CreateGrade(GradeMasterRequest gradeMasterRequest);
        Task<string> UpdateGrade(UpdateGradeMaster gradeMaster);
    }
}
