using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

public class PrefixAndSuffixService
{
    private readonly AttendanceManagementSystemContext _context;

    public PrefixAndSuffixService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<PrefixLeaveResponse>> GetAllPrefixLeaveType()
    {
        var getPrefix = await (from prefix in _context.PrefixLeaveTypes
                         select new PrefixLeaveResponse
                         {
                             PrefixLeaveTypeId = prefix.Id,
                             PrefixLeaveTypeName = prefix.Name                           
                         })
                         .ToListAsync();
        if(getPrefix.Count == 0)
        {
            throw new MessageNotFoundException("No prefix leave types found");
        }
        return getPrefix;
    }

    public async Task<string> AddPrefixLeaveType(PrefixLeaveRequest prefixLeaveType)
    {
        var message = "PrefixLeaveType added successfully";
        var prefixLeave = new PrefixLeaveType
        {
            Name = prefixLeaveType.PrefixLeaveTypeName,
            IsActive = true,
            CreatedBy = prefixLeaveType.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.PrefixLeaveTypes.Add(prefixLeave);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<IEnumerable<SuffixLeaveResponse>> GetAllSuffixLeaveType()
    {
        var getSuffix = await (from suffix in _context.SuffixLeaveTypes
                         select new SuffixLeaveResponse
                         {
                             SuffixLeaveTypeId = suffix.Id,
                             SuffixLeaveTypeName = suffix.Name
                         })
                         .ToListAsync();
        if(getSuffix.Count == 0)
        {
            throw new MessageNotFoundException("No suffix leave types found");
        }
        return getSuffix;
    }

    public async Task<string> Create(SuffixLeaveRequest suffixLeaveType)
    {
        var message = "Suffix leave added successfully";
        var suffixLeave = new SuffixLeaveType
        {
            Name = suffixLeaveType.SuffixLeaveTypeName,
            IsActive = true,
            CreatedBy = suffixLeaveType.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.SuffixLeaveTypes.Add(suffixLeave);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<IEnumerable<PrefixAndSuffixDto>> GetAllPrefixAndSuffixAsync()
    {
        var allPrefix = await (from prefix in _context.PrefixAndSuffixes
                               join leaveType in _context.LeaveTypes
                               on prefix.LeaveTypeId equals leaveType.Id
                               join prefixLeave in _context.PrefixLeaveTypes
                               on prefix.PrefixLeaveTypeId equals prefixLeave.Id
                               join suffixLeave in _context.SuffixLeaveTypes
                               on prefix.SuffixLeaveTypeId equals suffixLeave.Id
                               select new PrefixAndSuffixDto
                               {
                                   PrefixAndSuffixId = prefix.Id,
                                   LeaveTypeName = leaveType.Name,
                                   PrefixTypeId = prefixLeave.Id,
                                   SuffixTypeId = suffixLeave.Id,
                                   LeaveTypeId = leaveType.Id,
                                   PrefixName = prefixLeave.Name,
                                   SuffixName = suffixLeave.Name,
                                   IsActive = prefix.IsActive,
                                   CreatedBy = prefix.CreatedBy
                               })
                              .ToListAsync();
        if (allPrefix.Count == 0)
        {
            throw new MessageNotFoundException("No prefix and suffix found");
        }
        return allPrefix;
    }

    public async Task<string> Create(PrefixAndSuffixRequest prefixAndSuffixRequest)
    {
        var message = "Prefix and suffix added successfully";
        var prefixAndSuffix = new PrefixAndSuffix
        {
            PrefixLeaveTypeId = prefixAndSuffixRequest.PrefixTypeId,
            SuffixLeaveTypeId = prefixAndSuffixRequest.SuffixTypeId,
            LeaveTypeId = prefixAndSuffixRequest.LeaveTypeId,
            IsActive = prefixAndSuffixRequest.IsActive,
            CreatedBy = prefixAndSuffixRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        _context.PrefixAndSuffixes.Add(prefixAndSuffix);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> Update(UpdatePrefixAndSuffix updatedPrefixAndSuffix)
    {
        var message = "Prefix and suffix updated successfully";
        var existingRecord = _context.PrefixAndSuffixes.FirstOrDefault(p => p.Id == updatedPrefixAndSuffix.PrefixAndSuffixId);
        if (existingRecord == null)
        {
            throw new MessageNotFoundException("Prefix and suffix not found");
        }
        existingRecord.LeaveTypeId = updatedPrefixAndSuffix.LeaveTypeId;
        existingRecord.PrefixLeaveTypeId = updatedPrefixAndSuffix.PrefixTypeId;
        existingRecord.SuffixLeaveTypeId = updatedPrefixAndSuffix.SuffixTypeId;
        existingRecord.UpdatedBy = updatedPrefixAndSuffix.UpdatedBy;
        existingRecord.IsActive = updatedPrefixAndSuffix.IsActive;
        existingRecord.UpdatedUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return message;
    }
}