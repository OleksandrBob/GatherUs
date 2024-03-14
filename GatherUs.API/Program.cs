using System.Text;
using GatherUs.API.DTO.Event;
using GatherUs.API.Extensions;
using GatherUs.API.HostedServices;
using GatherUs.Core.Constants;
using GatherUs.Core.Mailing;
using GatherUs.Core.Mailing.SetUp;
using GatherUs.Core.RabbitMq;
using GatherUs.Core.RabbitMq.Interfaces;
using GatherUs.Core.Services;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Context;
using GatherUs.DAL.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(nameof(SmtpOptions)));
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
builder.Services.AddSingleton<ISmtpOptions>(sp => sp.GetRequiredService<IOptions<SmtpOptions>>().Value);
builder.Services.AddSingleton<IConnectionStrings>(sp => sp.GetRequiredService<IOptions<ConnectionStrings>>().Value);

builder.Services.AddDbContext<DataContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("MainDB")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<IGuestService, GuestService>();
builder.Services.AddSingleton<ITokenManager, TokenManager>();
builder.Services.AddSingleton<IMessageConsumer, RabbitMqMessageConsumer>();
builder.Services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
builder.Services.AddSingleton<IMailingService, MailingService>();
builder.Services.AddTransient<IOrganizerService, OrganizerService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddScoped<IEmailForRegistrationService, EmailForRegistrationService>();

builder.Services.AddHostedService<QueueMessageConsumerBackgroundServise>();

builder.Services.AddAutoMapper(typeof(MappingConfigurations));

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.UseDateOnlyTimeOnlyStringConverters();
    options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");
    
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "GatherUs API",
        Description = "API for GatherUs by Oleksandr Bob - 2024",
    });
    
    options.AddSecurityDefinition(AppConstants.BearerAuth, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = AppConstants.BearerAuth,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer",
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { 
            new OpenApiSecurityScheme 
            { 
                Reference = new OpenApiReference 
                { 
                    Id = AppConstants.BearerAuth,
                    Type = ReferenceType.SecurityScheme,
                } 
            },
            new string[] { } 
        } 
    });
});

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

 builder.Services.AddAuthentication(options =>
 {
     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
 })
     .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
              ValidateAudience = true,
              ValidateIssuer = true,
              ValidAudience = AppConstants.JwtAudience,
              ValidIssuer = AppConstants.JwtIssuer,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConstants.Salt)),
         };
     });

var app = builder.Build();

app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();