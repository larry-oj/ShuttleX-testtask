using ChatShuttleX.Data;
using ChatShuttleX.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChatContext>(ops =>
{
    ops.UseNpgsql(builder.Configuration.GetSection("Database:ConnectionString").Value);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatroomRepository, ChatroomRepository>();
builder.Services.AddScoped<IChatUserRepository, ChatUserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}
app.UseHttpsRedirection();

app.Run();