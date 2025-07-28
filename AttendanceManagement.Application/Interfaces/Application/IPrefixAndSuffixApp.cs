using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IPrefixAndSuffixApp
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
