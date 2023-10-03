using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ExtCore.FileStorage;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using MyTemplate.Application;
using MyTemplate.Domain.Common.Entities;
using MyTemplate.Domain.Entities.Security;
using MyTemplate.Infrastructure;
using MyTemplate.Infrastructure.Persistence;
using MyTemplate.Web;
using MyTemplate.Web.Extensions;
using MyTemplate.Web.Filters;
using MyTemplate.Application.Security.Enums;
using MyTemplate.Web.Security.Token.Configuration;
using MyTemplate.Web.Security.Token.Providers;
using Newtonsoft.Json.Converters;
using Serilog;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

var builder = WebApplication.CreateBuilder(args);
#region logging

Log.Logger = new LoggerConfiguration()
  .WriteTo
  .Console()
  .CreateBootstrapLogger();

builder
  .Host
  .UseSerilog((context, loggingConfig) => loggingConfig
    .ReadFrom
    .Configuration(context.Configuration));

#endregion

#region Identity Configuration

builder.Services.AddIdentity<User, Role>(options =>
    {
        //identity configuration goes here
    })
    .AddSignInManager()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<MyTemplateDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<JwtProvider>(nameof(LoginProviders.MyTemplate));

#endregion 

#region JWT
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<IJwtConfig, JwtConfig>(services => services.GetRequiredService<IOptions<JwtConfig>>().Value);

builder
.Services
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JWT_OR_COOKIES";
    options.DefaultChallengeScheme = "JWT_OR_COOKIES";
})
.AddCookie(options =>
{
    //cookie authentication options goes here
    //login, logout paths, and cookie expiration options
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secrets"])),
        RoleClaimType = nameof(ClaimsTypes.Roles)
    };
})
.AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
{
    options.ForwardDefaultSelector = context =>
    {
        string authorization = context.Request.Headers[HeaderNames.Authorization];
        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
            return JwtBearerDefaults.AuthenticationScheme;
                
        return CookieAuthenticationDefaults.AuthenticationScheme;
    };
});

#endregion

#region authorization
builder.Services.AddAuthorization(options => options.AddPolicies());
#endregion 

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

#region ef core config
var sqlServerConnection = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext(sqlServerConnection);
#endregion

#region fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationModule>();
// Add services to the container.
#endregion

builder.Services.AddControllersWithViews().AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()));
builder.Services.AddRazorPages();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AuthorizeOperationFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTemplate", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new InfrastructureModule());
    containerBuilder.RegisterModule(new ApplicationModule());
    containerBuilder.RegisterModule(new WebModule());
});

builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Logging.ClearProviders().AddConsole();

builder.Services.Configure<FileStorageOptions>(options =>
{
    var path = Path.Combine(builder.Environment.ContentRootPath, "UploadedFiles");
    options.RootPath = path;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(
  options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
);

app.UseCookiePolicy();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyTemplate V1");
  c.DocumentTitle = "MyTemplate";
});

app.MapDefaultControllerRoute();
app.MapRazorPages();

//migrate db context
var scope = app.Services.CreateAsyncScope();
await using (scope)
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyTemplateDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();
