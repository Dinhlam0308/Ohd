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
using Ohd.Repositories.Interfaces;
using Ohd.Repositories.Implementations;
using Ohd.Mappings;
using Ohd.Background;
using OfficeOpenXml;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);


ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
// =============================
// 1. CONFIG: DB + JWT
// =============================
var connectionString = builder.Configuration.GetConnectionString("OhdConnection");
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);
var cloudSettings = builder.Configuration.GetSection("Cloudinary");
var cloudinary = new Cloudinary(new Account(
    cloudSettings["CloudName"],
    cloudSettings["ApiKey"],
    cloudSettings["ApiSecret"]
));
// DB
builder.Services.AddDbContext<OhdDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// =============================
// 2. CORS
// =============================
var MyAllowSpecificOrigins = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// =============================
// 3. AUTH + JWT
// =============================
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
 
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

builder.Services.AddSingleton(cloudinary);
// Authorization (ví dụ policy AdminOnly)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// =============================
// 4. CONTROLLERS + SWAGGER
// =============================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =============================
// 5. SERVICES (DI)
// =============================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<AdminUserService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddHostedService<ReminderWorker>();
builder.Services.AddScoped<IRequestEndUserService, RequestEndUserService>();




builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<RequestCommentService>();
builder.Services.AddScoped<AttachmentService>();
builder.Services.AddScoped<RequestTagService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<MailSender>();
builder.Services.AddHostedService<EmailOutboxWorker>();
builder.Services.AddScoped<AdminConfigService>();
builder.Services.AddScoped<AdminUserService>();
// =============================
// 6. REPOSITORIES
// =============================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<ILookupRepository, LookupRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddScoped<AdminDashboardService>();
builder.Services.AddScoped<JwtHelper>();
// =============================
// 7. FLUENT VALIDATION
// =============================
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
builder.Services.AddAutoMapper(typeof(RequestMappingProfile).Assembly);

var app = builder.Build();

// =============================
// 8. MIDDLEWARE PIPELINE
// =============================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

// CORS phải trước Auth
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
