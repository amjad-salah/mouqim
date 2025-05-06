using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Occupation;
using MouqimApi.Data;

namespace MouqimApi.Services.Occupation;

public class OccupationsService(
    MouqimDbContext context,
    IValidator<AddOccupationDto> addValidator,
    IValidator<UpdateOccupationDto> updateValidator) : IOccupationsService
{
    public async Task<OccupationResponseDto> GetAllOccupations()
    {
        var occupations = await context.Occupations.AsNoTracking()
            .ProjectToType<OccupationsDto>()
            .ToListAsync();

        return new OccupationResponseDto
        {
            Success = true,
            Occupations = occupations
        };
    }

    public async Task<OccupationResponseDto> GetOccupationById(int id)
    {
        var occupation = await context.Occupations.AsNoTracking()
            .Include(o => o.Persons)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (occupation == null)
            return new OccupationResponseDto
            {
                Success = false,
                Message = "Occupation not found"
            };

        return new OccupationResponseDto
        {
            Success = true,
            Occupation = occupation.Adapt<OccupationDto>()
        };
    }

    public async Task<OccupationResponseDto> AddOccupation(AddOccupationDto dto)
    {
        var validationResult = await addValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return new OccupationResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingOccupation = await context.Occupations.FirstOrDefaultAsync(o => o.Name == dto.Name);

        if (existingOccupation != null)
            return new OccupationResponseDto
            {
                Success = false,
                Message = "Occupation name already exists"
            };

        var newOccupation = dto.Adapt<Models.Entities.Occupation>();
        context.Occupations.Add(newOccupation);
        await context.SaveChangesAsync();

        return new OccupationResponseDto
        {
            Success = true,
            Occupation = newOccupation.Adapt<OccupationDto>(),
            Message = "Occupation added successfully"
        };
    }

    public async Task<OccupationResponseDto> UpdateOccupation(UpdateOccupationDto dto)
    {
        var validationResult = await updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return new OccupationResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingOccupation = await context.Occupations.FindAsync(dto.Id);

        if (existingOccupation == null)
            return new OccupationResponseDto
            {
                Success = false,
                Message = "Occupation not found"
            };

        if (existingOccupation.Name != dto.Name)
        {
            var existingOccupationName = await context.Occupations.AsNoTracking()
                .FirstOrDefaultAsync(o => o.Name == dto.Name);

            if (existingOccupationName != null)
                return new OccupationResponseDto
                {
                    Success = false,
                    Message = "Occupation name already exists"
                };
        }

        existingOccupation.Name = dto.Name;
        await context.SaveChangesAsync();

        return new OccupationResponseDto
        {
            Success = true,
            Message = "Occupation updated successfully"
        };
    }

    public async Task<OccupationResponseDto> DeleteOccupation(int id)
    {
        var existingOccupation = await context.Occupations.FindAsync(id);

        if (existingOccupation == null)
            return new OccupationResponseDto
            {
                Success = false,
                Message = "Occupation not found"
            };

        var persons = await context.Persons.AsNoTracking()
            .Where(p => p.OccupationId == id).ToListAsync();

        if (persons.Count != 0)
            return new OccupationResponseDto
            {
                Success = false,
                Message = "Occupation has members, please remove them first"
            };

        context.Occupations.Remove(existingOccupation);
        await context.SaveChangesAsync();

        return new OccupationResponseDto
        {
            Success = true,
            Message = "Occupation deleted successfully"
        };
    }
}