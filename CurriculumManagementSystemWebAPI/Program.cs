
using BusinessObject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Google;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using DataAccess.Models.DTO;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = "780549906802-4k5phhf2h582rbhfc55qqn9tmi3ir24k.apps.googleusercontent.com"; 
        options.ClientSecret = "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";
    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
       .AllowAnyHeader();
    });
});
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddControllers();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
});
//More Services
builder.Services.AddDbContext<CMSDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();
builder.Services.AddControllers();
//MAPPER
//var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new CurriculumManagementSystemWebAPI.Mappers.AutoMapper()); });
//var mapper = mapperConfig.CreateMapper();
//builder.Services.AddSingleton(mapper);

builder.Services.AddDbContext<CMSDbContext>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Configure the HTTP request pipeline.
var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
