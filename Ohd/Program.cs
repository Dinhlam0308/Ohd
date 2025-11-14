using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ohd.Data;
using Ohd.Utils;
using Ohd.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ohd.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using Ohd.Validators.Auth;

var builder = WebApplication.CreateBuilder(args);
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);

// config: read connection string + jwt key from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("OhdConnection");
builder.Services.AddDbContext<OhdDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
var MyAllowSpecificOrigins = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173", "http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // dev: false, production: true
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfig["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtConfig["Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

// 3) Thêm Authorization (có thể khai báo policy sau)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddControllers();
// register helpers / services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<FacilityService>();
builder.Services.AddScoped<RequestStatusService>();
builder.Services.AddScoped<SeverityService>();
builder.Services.AddScoped<RequestPriorityService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<UserTeamService>();
builder.Services.AddScoped<SystemSettingService>();
builder.Services.AddScoped<SlaPolicyService>();
builder.Services.AddScoped<MaintenanceWindowService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<RequestCommentService>();
builder.Services.AddScoped<AttachmentService>();
builder.Services.AddScoped<RequestTagService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// configure JWT bearer for validating tokens on other endpoints (optional)
var jwtKey = builder.Configuration["Jwt:Key"];
if (!string.IsNullOrEmpty(jwtKey))
{
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

// nếu có CORS thì để trước auth
app.UseCors("AllowFrontend");

// RẤT QUAN TRỌNG:
app.UseAuthentication();    // 👈 bắt buộc trước UseAuthorization
app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();