using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebPocHu.dal;
using WebPocHu.Dal; // These are add in the program.cs file
using WebPocHub.Models;
using WebPocHub.WebApi.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// This is add for DbContext 
// This is modify by me
builder.Services.AddDbContext<WebPocHubDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MySqlConstr")));
builder.Services.AddTransient<ICommonRepository<Employee>, CommonRepository<Employee>>();
builder.Services.AddTransient<ICommonRepository<Event>, CommonRepository<Event>>();

builder.Services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<ITokenManager, TokenManager>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddPolicy("PublicPolicy",
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                      });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["WebPocHubJWT:Secret"])),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Authentication Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JsonWebToken",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
