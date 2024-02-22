using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Infrastructure.Data;
using ChitChat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ChitChat.Web.Hubs;
using Serilog;
using ChitChat.Web.Middleware;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Application.Implementations;

var builder = WebApplication.CreateBuilder(args);

//Configure logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// Configure Database Connection
var connectionString = builder.Configuration.GetConnectionString("ChitChatConnectionString");
var dBPassword = builder.Configuration["DbPassword"];

var npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
{
    Password = dBPassword
};
var fullConnectionString = npgsqlConnectionStringBuilder.ToString();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ChitChatDbContext>(options => options.UseNpgsql(fullConnectionString));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChitChatHub>("/chitChatHub");

app.Run();
