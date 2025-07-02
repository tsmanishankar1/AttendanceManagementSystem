using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IPrefixAndSuffixService
    {
        Task<List<PrefixLeaveResponse>> GetAllPrefixLeaveType();
        Task<string> AddPrefixLeaveType(PrefixLeaveRequest prefixLeaveType);
        Task<List<SuffixLeaveResponse>> GetAllSuffixLeaveType();
        Task<string> Create(SuffixLeaveRequest suffixLeaveType);
        Task<List<PrefixAndSuffixDto>> GetAllPrefixAndSuffixAsync();
        Task<string> Create(PrefixAndSuffixRequest prefixAndSuffixRequest);
        Task<string> Update(UpdatePrefixAndSuffix updatedPrefixAndSuffix);
    }
}
