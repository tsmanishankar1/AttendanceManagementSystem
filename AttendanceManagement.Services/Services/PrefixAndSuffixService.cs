using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;

public class PrefixAndSuffixService : IPrefixAndSuffixService
{
    private readonly AttendanceManagementSystemContext _context;

    public PrefixAndSuffixService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }
    public async Task<List<PrefixLeaveResponse>> GetAllPrefixLeaveType()
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
        await _context.PrefixLeaveTypes.AddAsync(prefixLeave);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<SuffixLeaveResponse>> GetAllSuffixLeaveType()
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
        await _context.SuffixLeaveTypes.AddAsync(suffixLeave);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<PrefixAndSuffixDto>> GetAllPrefixAndSuffixAsync()
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

    private async Task PrefixAndSuffixMethod(int prefixTypeId, int suffixTypeId, int leaveTypeId)
    {
        var prefix = await _context.PrefixLeaveTypes.AnyAsync(h => h.Id == prefixTypeId && h.IsActive);
        if (!prefix) throw new MessageNotFoundException("Prefix type not found");
        var suffix = await _context.SuffixLeaveTypes.AnyAsync(h => h.Id == suffixTypeId && h.IsActive);
        if (!suffix) throw new MessageNotFoundException("Suffix type not found");
        var leaveType = await _context.LeaveTypes.AnyAsync(h => h.Id == leaveTypeId && h.IsActive);
        if (!leaveType) throw new MessageNotFoundException("Leave type not found");
    }

    public async Task<string> Create(PrefixAndSuffixRequest prefixAndSuffixRequest)
    {
        var message = "Prefix and suffix added successfully";
        await PrefixAndSuffixMethod(prefixAndSuffixRequest.PrefixTypeId, prefixAndSuffixRequest.SuffixTypeId, prefixAndSuffixRequest.LeaveTypeId);
        var prefixAndSuffix = new PrefixAndSuffix
        {
            PrefixLeaveTypeId = prefixAndSuffixRequest.PrefixTypeId,
            SuffixLeaveTypeId = prefixAndSuffixRequest.SuffixTypeId,
            LeaveTypeId = prefixAndSuffixRequest.LeaveTypeId,
            IsActive = prefixAndSuffixRequest.IsActive,
            CreatedBy = prefixAndSuffixRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow
        };
        await _context.PrefixAndSuffixes.AddAsync(prefixAndSuffix);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<string> Update(UpdatePrefixAndSuffix updatedPrefixAndSuffix)
    {
        var message = "Prefix and suffix updated successfully";
        await PrefixAndSuffixMethod(updatedPrefixAndSuffix.PrefixTypeId, updatedPrefixAndSuffix.SuffixTypeId, updatedPrefixAndSuffix.LeaveTypeId);
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