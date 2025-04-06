namespace ToDo_MinimalAPI;

public class ToDoRequests
{
    public static IResult GetAll(IToDoService service)
    {
        var toDos = service.GetAll();
        return Results.Ok(toDos);
    }

    public static IResult GetById(IToDoService service, Guid id)
    {
        var todo = service.GetById(id);
        if(todo == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(todo);
    }

    public static IResult Create(IToDoService service, ToDo toDo)
    {
        service.Create(toDo);
        return Results.Created($"/todos/{toDo.Id}", toDo);
    }

    public static IResult Update(IToDoService service, Guid id, ToDo toDo)
    {
        var todo = service.GetById(id);
        if(todo == null)
        {
            return Results.NotFound();
        }
        service.Update(toDo);
        return Results.NoContent();
    }

    public static IResult Delete(IToDoService service, Guid id)
    {
        var todo = service.GetById(id);
        if(todo == null)
        {
            return Results.NotFound();
        }
        service.Delete(id);
        return Results.NoContent();
    }
}