using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Occupation;
using MouqimApi.Services.Occupation;

namespace MouqimApi.Endpoints;

public static class OccupationsEndpoints
{
    public static RouteGroupBuilder MapOccupationsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/occupations");

        //Get all occupations
        //GET /api/occupations
        group.MapGet("", [Authorize(Roles = "Admin,User")] async ([FromServices] IOccupationsService service) =>
        {
            var result = await service.GetAllOccupations();

            return Results.Ok(result);
        });

        //Get occupation by id
        //GET /api/occupations/:id
        group.MapGet("{id:int}", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IOccupationsService service, int id) =>
            {
                var result = await service.GetOccupationById(id);

                return !result.Success ? Results.NotFound(result) : Results.Ok(result);
            });

        //Add a new occupation
        //POST /api/occupations
        group.MapPost("", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IOccupationsService service, AddOccupationDto dto) =>
            {
                var result = await service.AddOccupation(dto);

                return !result.Success ? Results.BadRequest(result) : Results.Created("", result);
            });

        //Update occupation by id
        //PUT /api/occupations
        group.MapPut("", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IOccupationsService service, UpdateOccupationDto dto) =>
            {
                var result = await service.UpdateOccupation(dto);

                if (!result.Success && result.Message! == "Occupation not found") return Results.NotFound(result);

                return !result.Success ? Results.BadRequest(result) : Results.Ok(result);
            });

        //Delete occupation by id
        //DELETE /api/occupations/:id
        group.MapDelete("{id:int}", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IOccupationsService service, int id) =>
            {
                var result = await service.DeleteOccupation(id);

                if (!result.Success && result.Message! == "Occupation not found") return Results.NotFound(result);

                return !result.Success ? Results.BadRequest(result) : Results.Ok(result);
            });

        return group;
    }
}