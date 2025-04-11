using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace ToDo_MinimalAPI;

public static class ToDoRequests
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("/todos", ToDoRequests.GetAll)
            .Produces<List<ToDo>>()
            .RequireAuthorization();

        app.MapGet("/todos/{id}", ToDoRequests.GetById)
            .Produces<ToDo>()
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        app.MapPost("/todos/", ToDoRequests.Create)
            .Produces<ToDo>(StatusCodes.Status201Created)
            .Accepts<ToDo>("application/json")
            .RequireAuthorization();

        app.MapPut("/todos/{id}", ToDoRequests.Update)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<ToDo>("application/json")
            .RequireAuthorization();

        app.MapDelete("/todos/{id}", ToDoRequests.Delete)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        return app;
    }
    public static IResult GetAll(IToDoService service)
    {
        var toDos = service.GetAll();
        return Results.Ok(toDos);
    }

    public static IResult GetById(IToDoService service, Guid id)
    {
        var todo = service.GetById(id);
        if (todo == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(todo);
    }

    public static IResult Create(IToDoService service, ToDo toDo, IValidator<ToDo> validator)
    {
        var ValidationResult = validator.Validate(toDo);
        if (!ValidationResult.IsValid)
        {
            return Results.BadRequest(ValidationResult.Errors);
        }

        service.Create(toDo);
        return Results.Created($"/todos/{toDo.Id}", toDo);
    }

    public static IResult Update(IToDoService service, Guid id, ToDo toDo, IValidator<ToDo> validator)
    {
        var ValidationResult = validator.Validate(toDo);
        if (!ValidationResult.IsValid)
        {
            return Results.BadRequest(ValidationResult.Errors);
        }

        var todo = service.GetById(id);
        if (todo == null)
        {
            return Results.NotFound();
        }

        service.Update(toDo);
        return Results.NoContent();
    }

    public static IResult Delete(IToDoService service, Guid id)
    {
        var todo = service.GetById(id);
        if (todo == null)
        {
            return Results.NotFound();
        }
        service.Delete(id);
        return Results.NoContent();
    }
}