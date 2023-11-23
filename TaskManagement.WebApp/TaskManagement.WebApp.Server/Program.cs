using Microsoft.EntityFrameworkCore;
using TaskManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<TaskDbContext>((options) =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("TaskConnection"));
});

builder.Services.AddTransient<ITasksService, TasksService>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
