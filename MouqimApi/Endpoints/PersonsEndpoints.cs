using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Person;
using MouqimApi.Services.Person;

namespace MouqimApi.Endpoints;

public static class PersonsEndpoints
{
    public static RouteGroupBuilder MapPersonsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/persons");

        //Get all persons
        //GET /api/persons
        group.MapGet("", [Authorize(Roles = "Admin,User")] async ([FromServices] IPersonService service) =>
        {
            var response = await service.GetAllPersons();
            return Results.Ok(response);
        });

        //Get person by id
        //GET /api/persons/:id
        group.MapGet("{id:int}", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IPersonService service, int id) =>
            {
                var response = await service.GetPersonById(id);

                return !response.Success ? Results.NotFound(response) : Results.Ok(response);
            });

        //Add a new person
        //POST /api/persons
        group.MapPost("", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IPersonService service, AddPersonDto dto) =>
            {
                var response = await service.AddPerson(dto);

                return !response.Success ? Results.BadRequest(response) : Results.Created("", response);
            });

        //Update person by id
        //PUT /api/persons
        group.MapPut("", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IPersonService service, UpdatePersonDto dto) =>
            {
                var response = await service.UpdatePerson(dto);

                if (!response.Success && response.Message! == "Person not found")
                    return Results.NotFound(response);

                return !response.Success ? Results.BadRequest(response) : Results.Ok(response);
            });

        //Delete a person by id
        //DELETE /api/persons/:id
        group.MapDelete("{id:int}", [Authorize(Roles = "Admin,User")]
            async ([FromServices] IPersonService service, int id) =>
            {
                var response = await service.DeletePerson(id);

                return !response.Success ? Results.NotFound(response) : Results.Ok(response);
            });

        return group;
    }
}