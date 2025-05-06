using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs.Person;
using MouqimApi.Data;

namespace MouqimApi.Services.Person;

public class PersonService(
    MouqimDbContext context,
    IValidator<AddPersonDto> addValidator,
    IValidator<UpdatePersonDto> updateValidator) : IPersonService
{
    public async Task<PersonResponseDto> GetAllPersons()
    {
        var persons = await context.Persons.AsNoTracking()
            .Include(p => p.Family)
            .ProjectToType<PersonsDto>()
            .ToListAsync();

        return new PersonResponseDto
        {
            Success = true,
            Persons = persons
        };
    }

    public async Task<PersonResponseDto> GetPersonById(int id)
    {
        var person = await context.Persons.AsNoTracking()
            .Include(p => p.Family)
            .Include(p => p.Occupation)
            .Include(p => p.EducationLevel)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
            return new PersonResponseDto
            {
                Success = false,
                Message = "Person not found"
            };

        return new PersonResponseDto
        {
            Success = true,
            Person = person.Adapt<PersonDto>()
        };
    }

    public async Task<PersonResponseDto> AddPerson(AddPersonDto dto)
    {
        var validationResult = await addValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return new PersonResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingPerson = await context.Persons.AsNoTracking()
            .Where(p => p.FullName == dto.FullName && p.FamilyId == dto.FamilyId)
            .FirstOrDefaultAsync();

        if (existingPerson != null)
            return new PersonResponseDto
            {
                Success = false,
                Message = "Person already exists"
            };

        var existingFamily = await context.Families.AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == dto.FamilyId);

        var existingOccupation = await context.Occupations.AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == dto.OccupationId);

        var existingLevel = await context.EducationLevels.AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == dto.EducationLevelId);

        if (existingFamily == null || existingOccupation == null || existingLevel == null)
            return new PersonResponseDto
            {
                Success = false,
                Message = "Family, occupation or education level not found"
            };

        if (dto.RelationType == RelationType.Head)
        {
            var existingHead = await context.Persons.AsNoTracking()
                .Where(p => p.RelationType == RelationType.Head && p.FamilyId == dto.FamilyId)
                .FirstOrDefaultAsync();

            if (existingHead != null)
                return new PersonResponseDto
                {
                    Success = false,
                    Message = "Family head already has a head"
                };
        }

        var newPerson = dto.Adapt<Models.Entities.Person>();
        context.Persons.Add(newPerson);
        await context.SaveChangesAsync();

        return new PersonResponseDto
        {
            Success = true,
            Person = newPerson.Adapt<PersonDto>(),
            Message = "Person added successfully"
        };
    }

    public async Task<PersonResponseDto> UpdatePerson(UpdatePersonDto dto)
    {
        var validationResult = await updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return new PersonResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingPerson = await context.Persons.FindAsync(dto.Id);

        if (existingPerson == null)
            return new PersonResponseDto
            {
                Success = false,
                Message = "Person not found"
            };

        if (existingPerson.FullName != dto.FullName && existingPerson.FamilyId != dto.FamilyId)
        {
            var existingPersonFullName = await context.Persons.AsNoTracking()
                .Where(p => p.FullName == dto.FullName && p.FamilyId == dto.FamilyId)
                .FirstOrDefaultAsync();

            if (existingPersonFullName != null)
                return new PersonResponseDto
                {
                    Success = false,
                    Message = "Person already exists"
                };
        }

        var existingFamily = await context.Families.AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == dto.FamilyId);

        var existingOccupation = await context.Occupations.AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == dto.OccupationId);

        var existingLevel = await context.EducationLevels.AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == dto.EducationLevelId);

        if (existingFamily == null || existingOccupation == null || existingLevel == null)
            return new PersonResponseDto
            {
                Success = false,
                Message = "Family, occupation or education level not found"
            };

        if (dto.RelationType == RelationType.Head)
        {
            var existingHead = await context.Persons.AsNoTracking()
                .Where(p => p.RelationType == RelationType.Head && p.FamilyId == dto.FamilyId)
                .FirstOrDefaultAsync();

            if (existingHead != null)
                return new PersonResponseDto
                {
                    Success = false,
                    Message = "Family head already has a head"
                };
        }

        existingPerson.FullName = dto.FullName;
        existingPerson.NationalNo = dto.NationalNo;
        existingPerson.PhoneNo = dto.PhoneNo;
        existingPerson.Gender = dto.Gender;
        existingPerson.RelationType = dto.RelationType;
        existingPerson.Status = dto.Status;
        existingPerson.BirthDate = dto.BirthDate;
        existingPerson.EducationLevelId = dto.EducationLevelId;
        existingPerson.OccupationId = dto.OccupationId;
        existingPerson.FamilyId = dto.FamilyId;

        await context.SaveChangesAsync();

        return new PersonResponseDto
        {
            Success = true,
            Message = "Person updated successfully"
        };
    }

    public async Task<PersonResponseDto> DeletePerson(int id)
    {
        var existingPerson = await context.Persons.FindAsync(id);

        if (existingPerson == null)
            return new PersonResponseDto
            {
                Success = false,
                Message = "Person not found"
            };

        context.Persons.Remove(existingPerson);
        await context.SaveChangesAsync();

        return new PersonResponseDto
        {
            Success = true,
            Message = "Person deleted successfully"
        };
    }
}