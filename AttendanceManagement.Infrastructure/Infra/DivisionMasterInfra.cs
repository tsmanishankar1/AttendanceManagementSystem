using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class DivisionMasterInfra : IDivisionMasterInfra
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly IMemoryCache _cache;
        private const string DivisionCacheKey = "AllDivisions";
        public DivisionMasterInfra(AttendanceManagementSystemContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<DivisionResponse>> GetAllDivisions()
        {
            if (_cache.TryGetValue(DivisionCacheKey, out var cachedObj) && cachedObj is List<DivisionResponse> cachedDivisions)
            {
                return cachedDivisions;
            }

            var allDivision = await (from division in _context.DivisionMasters
                                     select new DivisionResponse
                                     {
                                         DivisionMasterId = division.Id,
                                         FullName = division.Name,
                                         ShortName = division.ShortName,
                                         IsActive = division.IsActive,
                                         CreatedBy = division.CreatedBy
                                     })
                                     .ToListAsync();

            if (allDivision.Count == 0)
            {
                throw new MessageNotFoundException("No divisions found");
            }

            _cache.Set(DivisionCacheKey, allDivision);

            return allDivision;
        }

        public async Task<string> AddDivision(DivisionRequest divisionRequest)
        {
            var message = "Division created successfully";

            var isDuplicate = await _context.DivisionMasters
                .AnyAsync(d => d.Name.ToLower() == divisionRequest.FullName.ToLower());
            if (isDuplicate) throw new ConflictException("Division name already exists");

            var division = new DivisionMaster
            {
                Name = divisionRequest.FullName,
                ShortName = divisionRequest.ShortName,
                IsActive = divisionRequest.IsActive,
                CreatedBy = divisionRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            await _context.DivisionMasters.AddAsync(division);
            await _context.SaveChangesAsync();

            _cache.Remove(DivisionCacheKey);

            return message;
        }

        public async Task<string> UpdateDivision(UpdateDivision division)
        {
            var message = "Division updated successfully";

            var existingDivision = await _context.DivisionMasters
                .FirstOrDefaultAsync(d => d.Id == division.DivisionMasterId);

            if (existingDivision == null)
            {
                throw new MessageNotFoundException("Division not found");
            }

            if (!string.IsNullOrWhiteSpace(division.FullName))
            {
                var isDuplicate = await _context.DivisionMasters
                    .AnyAsync(d => d.Id != division.DivisionMasterId &&
                                   d.Name.ToLower() == division.FullName.ToLower());
                if (isDuplicate) throw new ConflictException("Division name already exists");
            }

            existingDivision.Name = division.FullName ?? existingDivision.Name;
            existingDivision.ShortName = division.ShortName ?? existingDivision.ShortName;
            existingDivision.IsActive = division.IsActive;
            existingDivision.UpdatedBy = division.UpdatedBy;
            existingDivision.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _cache.Remove(DivisionCacheKey);

            return message;
        }
    }
}