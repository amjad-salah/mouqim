using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.EducationLevel;
using MouqimApi.Data;

namespace MouqimApi.Services.EducationLevel;

public class EducationLevelService(
    MouqimDbContext context,
    IValidator<AddEducationLevelDto> addValidator,
    IValidator<UpdateEducationLevelDto> updateValidator) : IEducationLevelService
{
    public async Task<EducationLevelResponseDto> GetAllEducationLevels()
    {
        var levels = await context.EducationLevels
            .AsNoTracking()
            .ProjectToType<EducationLevelsDto>()
            .ToListAsync();

        return new EducationLevelResponseDto
        {
            EducationLevels = levels
        };
    }

    public async Task<EducationLevelResponseDto> GetEducationLevelById(int id)
    {
        var level = await context.EducationLevels.AsNoTracking()
            .Include(l => l.Persons)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (level == null)
            return new EducationLevelResponseDto
            {
                Success = false,
                Message = "Education level not found"
            };

        return new EducationLevelResponseDto
        {
            EductionLevel = level.Adapt<EductionLevelDto>()
        };
    }

    public async Task<EducationLevelResponseDto> AddEducationLevel(AddEducationLevelDto dto)
    {
        var validationResult = await addValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return new EducationLevelResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingLevel = await context.EducationLevels.AsNoTracking()
            .FirstOrDefaultAsync(l => l.Name == dto.Name);

        if (existingLevel != null)
            return new EducationLevelResponseDto
            {
                Success = false,
                Message = "Education level name already exists"
            };

        var newLevel = dto.Adapt<Models.Entities.EducationLevel>();
        context.EducationLevels.Add(newLevel);
        await context.SaveChangesAsync();

        return new EducationLevelResponseDto
        {
            Success = true,
            EductionLevel = newLevel.Adapt<EductionLevelDto>(),
            Message = "Education level added successfully"
        };
    }

    public async Task<EducationLevelResponseDto> UpdateEducationLevel(UpdateEducationLevelDto dto)
    {
        var validationResult = await updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return new EducationLevelResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingLevel = await context.EducationLevels.FindAsync(dto.Id);

        if (existingLevel == null)
            return new EducationLevelResponseDto
            {
                Success = false,
                Message = "Education level not found"
            };

        if (existingLevel.Name != dto.Name)
        {
            var existingLevelName = await context.EducationLevels.AsNoTracking()
                .FirstOrDefaultAsync(l => l.Name == dto.Name);

            if (existingLevelName != null)
                return new EducationLevelResponseDto
                {
                    Success = false,
                    Message = "Education level name already exists"
                };
        }

        existingLevel.Name = dto.Name;
        await context.SaveChangesAsync();

        return new EducationLevelResponseDto
        {
            Success = true,
            Message = "Education level updated successfully"
        };
    }

    public async Task<EducationLevelResponseDto> DeleteEducationLevel(int id)
    {
        var existingLevel = await context.EducationLevels.FindAsync(id);

        if (existingLevel == null)
            return new EducationLevelResponseDto
            {
                Success = false,
                Message = "Education level not found"
            };

        var persons = await context.Persons.AsNoTracking()
            .Where(p => p.EducationLevelId == id).ToListAsync();

        if (persons.Count != 0)
            return new EducationLevelResponseDto
            {
                Success = false,
                Message = "Education level has members, please remove them first"
            };

        context.EducationLevels.Remove(existingLevel);
        await context.SaveChangesAsync();

        return new EducationLevelResponseDto
        {
            Success = true,
            Message = "Education level deleted successfully"
        };
    }
}