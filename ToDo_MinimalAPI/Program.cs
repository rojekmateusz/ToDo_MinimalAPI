using ToDo_MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IToDoService, ToDoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/todos", ToDoRequests.GetAll);
app.MapGet("/todos/{id}", ToDoRequests.GetById);
app.MapPost("/todos/", ToDoRequests.Create);
app.MapPut("/todos/{id}", ToDoRequests.Update);
app.MapDelete("/todos/{id}", ToDoRequests.Delete);

app.Run();
