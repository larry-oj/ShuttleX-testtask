using ChatShuttleX.Data;
using ChatShuttleX.Data.Repositories;
using ChatShuttleX.Services;
using ChatShuttleX.Services.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChatContext>(ops =>
{
    ops.UseNpgsql(builder.Configuration.GetSection("Database:ConnectionString").Value);
});

builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatroomRepository, ChatroomRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatroomService, ChatroomService>();

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();