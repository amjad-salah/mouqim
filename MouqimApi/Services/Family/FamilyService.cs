using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Family;
using MouqimApi.Data;

namespace MouqimApi.Services.Family;

public class FamilyService(
    MouqimDbContext context,
    IValidator<AddFamilyDto> addValidator,
    IValidator<UpdateFamilyDto> updateValidator) : IFamilyService
{
    public async Task<FamilyResponseDto> GetFamilies()
    {
        var families = await context.Families.AsNoTracking()
            .OrderByDescending(f => f.CreatedAt)
            .ProjectToType<FamiliesDto>()
            .ToListAsync();

        return new FamilyResponseDto
        {
            Success = true,
            Families = families
        };
    }

    public async Task<FamilyResponseDto> GetFamilyById(int id)
    {
        var family = await context.Families.AsNoTracking()
            .Include(f => f.Persons)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (family == null)
            return new FamilyResponseDto
            {
                Success = false,
                Message = "Family not found"
            };

        return new FamilyResponseDto
        {
            Success = true,
            Family = family.Adapt<FamilyDto>()
        };
    }

    public async Task<FamilyResponseDto> AddFamily(AddFamilyDto dto)
    {
        var validationResult = await addValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return new FamilyResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingFamily = await context.Families.FirstOrDefaultAsync(f => f.FamilyName == dto.FamilyName);

        if (existingFamily != null)
            return new FamilyResponseDto
            {
                Success = false,
                Message = "Family name already exists"
            };

        var newFamily = dto.Adapt<Models.Entities.Family>();
        context.Families.Add(newFamily);
        await context.SaveChangesAsync();

        return new FamilyResponseDto
        {
            Success = true,
            Family = newFamily.Adapt<FamilyDto>(),
            Message = "Family added successfully"
        };
    }

    public async Task<FamilyResponseDto> UpdateFamily(UpdateFamilyDto dto)
    {
        var validationResult = await updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return new FamilyResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingFamily = await context.Families.FindAsync(dto.Id);

        if (existingFamily == null)
            return new FamilyResponseDto
            {
                Success = false,
                Message = "Family not found"
            };

        existingFamily.FamilyName = dto.FamilyName;
        existingFamily.Neighbourhood = dto.Neighbourhood;
        existingFamily.HousingType = dto.HousingType;
        existingFamily.IncomeStatus = dto.IncomeStatus;
        existingFamily.State = dto.State;
        existingFamily.RegisterDate = dto.RegisterDate;
        existingFamily.DeactivatedDate = dto.DeactivatedDate;
        existingFamily.DeactivationReason = dto.DeactivationReason;

        await context.SaveChangesAsync();

        return new FamilyResponseDto
        {
            Success = true,
            Message = "Family updated successfully"
        };
    }

    public async Task<FamilyResponseDto> DeleteFamily(int id)
    {
        var existingFamily = await context.Families.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

        if (existingFamily == null)
            return new FamilyResponseDto
            {
                Success = false,
                Message = "Family not found"
            };

        var persons = await context.Persons.AsNoTracking()
            .Where(p => p.FamilyId == id).ToListAsync();

        if (persons.Count != 0)
            return new FamilyResponseDto
            {
                Success = false,
                Message = "Family has members, please remove them first"
            };

        context.Families.Remove(existingFamily);
        await context.SaveChangesAsync();

        return new FamilyResponseDto
        {
            Success = true,
            Message = "Family deleted successfully"
        };
    }
}