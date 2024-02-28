﻿using ChitChat.AngularApp.Server.Hubs;
using ChitChat.AngularApp.Server.Middleware;
using ChitChat.Application.Implementations;
using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Infrastructure.Data;
using ChitChat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Configure logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("ChitChatConnectionString");
var dBPassword = builder.Configuration["DbPassword"];

var npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
{
    Password = dBPassword
};
var fullConnectionString = npgsqlConnectionStringBuilder.ToString();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ChitChatDbContext>(options => options.UseNpgsql(fullConnectionString));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");
app.MapHub<ChitChatHub>("/chitChatHub");

app.Run();
