using GatherUs.Core.Mailing;
using GatherUs.Core.Mailing.SetUp;
using GatherUs.Core.Services;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Context;
using GatherUs.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
builder.Services.AddSingleton<IConnectionStrings>(sp => sp.GetRequiredService<IOptions<ConnectionStrings>>().Value);
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(nameof(SmtpOptions)));
builder.Services.AddSingleton<ISmtpOptions>(sp => sp.GetRequiredService<IOptions<SmtpOptions>>().Value);


builder.Services.AddDbContext<DataContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("MainDB")));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IGuestService, GuestService>();
builder.Services.AddSingleton<IMailingService, MailingService>();
builder.Services.AddTransient<IOrganizerService, OrganizerService>();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();