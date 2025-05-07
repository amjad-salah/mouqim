using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.User;
using MouqimApi.Services.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MouqimApi.Endpoints;

public static class UsersEndpoints
{
    public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users");

        //Login user
        //POST /api/users/login
        group.MapPost("login",
            async (HttpContext http, [FromServices] IUserService service, LoginDto login) =>
            {
                var result = await service.Login(login);

                if (!result.Success) return Results.Unauthorized();

                await http.SignInAsync("default", new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new(ClaimTypes.NameIdentifier, result.FullName!),
                            new(ClaimTypes.Role, result.Role!)
                        },
                        "default"
                    )
                ));

                return Results.Ok(result);
            });

        //Get all users
        //GET /api/users
        group.MapGet("", [Authorize(Roles = "Admin")] async ([FromServices] IUserService service) =>
        {
            var result = await service.GetUsers();

            return Results.Ok(result);
        });

        //Add user
        //POST /api/users
        group.MapPost("", [Authorize(Roles = "Admin")] async ([FromServices] IUserService service, AddUserDto user) =>
        {
            var result = await service.AddUsee(user);

            return !result.Success ? Results.BadRequest(result) : Results.Created("", result);
        });

        //Update user
        //PUT /api/users
        group.MapPut("", [Authorize(Roles = "Admin")] async ([FromServices] IUserService service, UpdateUserDto user) =>
        {
            var result = await service.UpdateUser(user);

            if (!result.Success && result.Message! == "User not found") return Results.NotFound(result);

            return !result.Success ? Results.BadRequest(result) : Results.Ok(result);
        });

        //Reset password
        //PUT /api/users/reset-password
        group.MapPut("reset-password", [Authorize(Roles = "Admin")]
            async ([FromServices] IUserService service, ResetPasswordDto user) =>
            {
                var result = await service.ResetPassword(user);

                if (!result.Success && result.Message! == "User not found") return Results.NotFound(result);

                return !result.Success ? Results.BadRequest(result) : Results.Ok(result);
            });

        //Delete user
        //DELETE /api/users/:id
        group.MapDelete("{id:int}", [Authorize(Roles = "Admin")] async ([FromServices] IUserService service, int id) =>
        {
            var result = await service.DeleteUser(id);

            if (!result.Success && result.Message! == "User not found") return Results.NotFound(result);

            return Results.Ok(result);
        });

        return group;
    }
}