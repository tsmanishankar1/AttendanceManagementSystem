using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class PrefixAndSuffixApp : IPrefixAndSuffixApp
    {
        private readonly IPrefixAndSuffixInfra _prefixAndSuffixInfra;

        public PrefixAndSuffixApp(IPrefixAndSuffixInfra prefixAndSuffixInfra)
        {
            _prefixAndSuffixInfra = prefixAndSuffixInfra;
        }

        public async Task<string> AddPrefixLeaveType(PrefixLeaveRequest prefixLeaveType)
            => await _prefixAndSuffixInfra.AddPrefixLeaveType(prefixLeaveType);

        public async Task<string> Create(SuffixLeaveRequest suffixLeaveType)
            => await _prefixAndSuffixInfra.Create(suffixLeaveType);

        public async Task<string> Create(PrefixAndSuffixRequest prefixAndSuffixRequest)
            => await _prefixAndSuffixInfra.Create(prefixAndSuffixRequest);

        public async Task<List<PrefixAndSuffixDto>> GetAllPrefixAndSuffixAsync()
            => await _prefixAndSuffixInfra.GetAllPrefixAndSuffixAsync();

        public async Task<List<PrefixLeaveResponse>> GetAllPrefixLeaveType()
            => await _prefixAndSuffixInfra.GetAllPrefixLeaveType();

        public async Task<List<SuffixLeaveResponse>> GetAllSuffixLeaveType()
            => await _prefixAndSuffixInfra.GetAllSuffixLeaveType();

        public async Task<string> Update(UpdatePrefixAndSuffix updatedPrefixAndSuffix)
            => await _prefixAndSuffixInfra.Update(updatedPrefixAndSuffix);
    }
}
