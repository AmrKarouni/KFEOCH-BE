using KFEOCH.Contexts;
using KFEOCH.Models.Identity;
using KFEOCH.Services;
using KFEOCH.Services.Interfaces;
using KFEOCH.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using User.Management.Service.Models;
using User.Management.Service.Services;

var ConnectionString = "Connection";
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString(ConnectionString);
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = int.Parse(builder.Configuration["PasswordConfig:RequiredLength"]);
    opt.Password.RequireNonAlphanumeric = bool.Parse(builder.Configuration["PasswordConfig:RequireNonAlphanumeric"]);
    opt.Password.RequireDigit = bool.Parse(builder.Configuration["PasswordConfig:RequireDigit"]);
    opt.Password.RequireUppercase = bool.Parse(builder.Configuration["PasswordConfig:RequireUppercase"]);
    opt.Password.RequireLowercase = bool.Parse(builder.Configuration["PasswordConfig:RequireLowercase"]);
    opt.Password.RequiredUniqueChars = int.Parse(builder.Configuration["PasswordConfig:RequiredUniqueChars"]);
    opt.SignIn.RequireConfirmedEmail = true;
}
).
AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

//Add Configuration For Required Email 
builder.Services.Configure<IdentityOptions>(
    opts => opts.SignIn.RequireConfirmedEmail = true);

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDictionaryService, DictionaryService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IOfficeService, OfficeService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IOfficeOwnerService, OfficeOwnerService>();
builder.Services.AddScoped<IOfficeSpecialityService, OfficeSpecialityService>();
builder.Services.AddScoped<IOfficeActivityService, OfficeActivityService>();
builder.Services.AddScoped<IOwnerDocumentService, OwnerDocumentService>();
builder.Services.AddScoped<IOfficeContactService, OfficeContactService>();
builder.Services.AddScoped<IOfficeRequestService, OfficeRequestService>();
builder.Services.AddScoped<IOfficeDocumentService, OfficeDocumentService>();
builder.Services.AddScoped<IOfficeRegistrationService, OfficeRegistrationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IKnetPaymentService, KnetPaymentService>();
builder.Services.AddScoped<ISiteMessageService, SiteMessageService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddTransient<ApplicationDbContextSeed>();
builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    x.UseSqlServer(connectionString);
});



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                    };
                });


builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials();
}));


//Add Email Configurations
var emailconfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailconfig);


// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    await SeedData(app);

async Task SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory?.CreateScope())
    {
        var service = scope?.ServiceProvider.GetService<ApplicationDbContextSeed>();
        var userManager = scope?.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope?.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var dictionaryService = scope?.ServiceProvider.GetRequiredService<IDictionaryService>();
        await service.SeedEssentialsAsync(userManager, roleManager, dictionaryService);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "./App_Media/logos")),
    RequestPath = "/logos"

});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "./App_Media/owners")),
    RequestPath = "/owners"

});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "./App_Media/offices")),
    RequestPath = "/offices"

});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "./App_Media/pages")),
    RequestPath = "/pages"

});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
