using Microsoft.AspNetCore.Mvc;
using Models.DTOs.EducationLevel;
using MouqimApi.Services.EducationLevel;

namespace MouqimApi.Endpoints;

public static class EducationLevelsEndpoints
{
    public static RouteGroupBuilder MapEducationLevelsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/education-levels");

        //Get all education levels
        //GET /api/educations-levels
        group.MapGet("", async ([FromServices] IEducationLevelService service) =>
        {
            var response = await service.GetAllEducationLevels();

            return Results.Ok(response);
        });

        //Get education level by id
        //GET /api/educations-levels/:id
        group.MapGet("{id:int}", async ([FromServices] IEducationLevelService service, int id) =>
        {
            var response = await service.GetEducationLevelById(id);

            return !response.Success ? Results.NotFound(response) : Results.Ok(response);
        });

        //Add a new education level
        //POST /api/educations-levels
        group.MapPost("", async ([FromServices] IEducationLevelService service, AddEducationLevelDto dto) =>
        {
            var response = await service.AddEducationLevel(dto);

            return !response.Success ? Results.BadRequest(response) : Results.Created("", response);
        });

        //Update education level by id
        //PUT /api/educations-levels
        group.MapPut("", async ([FromServices] IEducationLevelService service, UpdateEducationLevelDto dto) =>
        {
            var response = await service.UpdateEducationLevel(dto);

            if (!response.Success && response.Message! == "Education level not found")
                return Results.NotFound(response);

            return !response.Success ? Results.BadRequest(response) : Results.Ok(response);
        });

        //Delete education level by id
        //DELETE /api/educations-levels/:id
        group.MapDelete("{id:int}", async ([FromServices] IEducationLevelService service, int id) =>
        {
            var response = await service.DeleteEducationLevel(id);

            if (!response.Success && response.Message! == "Education level not found")
                return Results.NotFound(response);

            return !response.Success ? Results.BadRequest(response) : Results.Ok(response);
        });

        return group;
    }
}