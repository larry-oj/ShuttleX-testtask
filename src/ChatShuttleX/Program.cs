using ChatShuttleX.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChatContext>(ops =>
{
    ops.UseNpgsql(builder.Configuration.GetSection("Database:ConnectionString").Value);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}
app.UseHttpsRedirection();

app.Run();