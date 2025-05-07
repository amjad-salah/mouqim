using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Family;
using MouqimApi.Services.Family;

namespace MouqimApi.Endpoints;

public static class FamiliesEndpoints
{
    public static RouteGroupBuilder MapFamiliesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/families");

        //Get all families
        //GET /api/families
        group.MapGet("", [Authorize] async ([FromServices] IFamilyService service) =>
        {
            var result = await service.GetFamilies();
            return Results.Ok(result);
        });

        //Get family by id
        //GET /api/families/:id
        group.MapGet("{id:int}", [Authorize] async ([FromServices] IFamilyService service, int id) =>
        {
            var result = await service.GetFamilyById(id);

            return !result.Success ? Results.NotFound(result) : Results.Ok(result);
        });

        //Add a new family
        //POST /api/families
        group.MapPost("", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IFamilyService service, AddFamilyDto dto) =>
            {
                var result = await service.AddFamily(dto);

                return !result.Success ? Results.BadRequest(result) : Results.Created("", result);
            });

        //Update a family
        //PUT /api/families
        group.MapPut("", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IFamilyService service, UpdateFamilyDto dto) =>
            {
                var result = await service.UpdateFamily(dto);

                if (!result.Success && result.Message! == "Family not found") return Results.NotFound(result);

                return !result.Success ? Results.BadRequest(result) : Results.Ok(result);
            });

        //Delete family by id
        //DELETE /api/families/:id
        group.MapDelete("{id:int}", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IFamilyService service, int id) =>
            {
                var result = await service.DeleteFamily(id);

                if (!result.Success && result.Message! == "Family not found") return Results.NotFound(result);

                return !result.Success ? Results.BadRequest(result) : Results.Ok(result);
            });

        return group;
    }
}