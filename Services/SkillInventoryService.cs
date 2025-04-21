using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceManagement.Services
{
    public class SkillInventoryService
    {
        private readonly AttendanceManagementSystemContext _context;

        public SkillInventoryService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<SkillInventoryResponseModel>> GetAllAsync()
        {
            var skills = await _context.SkillInventories
                .Select(skill => new SkillInventoryResponseModel
                {
                    SkillId = skill.Id,
                    StaffCreationId = skill.StaffCreationId,
                    Name = skill.Name,
                    LevelOfProficiency = skill.LevelOfProficiency,
                    Notes = skill.Notes,

                })
                .ToListAsync();
            if (skills.Count == 0) throw new MessageNotFoundException("No skill inventory found");
            return skills;
        }

        public async Task<string> CreateAsync(SkillInventoryRequestModel model)
        {
            var messaage = "Skill inventory created successfully.";
            var newSkill = new SkillInventory
            {
                Name = model.Name,
                LevelOfProficiency = model.LevelOfProficiency,
                Notes = model.Notes,
                IsActive = true,
                CreatedBy = model.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.SkillInventories.Add(newSkill);
            await _context.SaveChangesAsync();
            return messaage;
        }

        public async Task<string> UpdateAsync(SkillInventoryUpdateModel model)
        {
            var message = "Skill inventory updated successfully.";
            var skill = await _context.SkillInventories.FirstOrDefaultAsync(lg => lg.Id == model.SkillId && lg.IsActive);
            if (skill == null) throw new MessageNotFoundException("Skill inventory not found");
            skill.Id = model.SkillId;
            skill.Name = model.Name;
            skill.LevelOfProficiency = model.LevelOfProficiency;
            skill.Notes = model.Notes;
            skill.IsActive = model.IsActive;
            skill.UpdatedBy = model.UpdatedBy;
            skill.UpdatedUtc = DateTime.UtcNow;

            _context.SkillInventories.Update(skill);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
