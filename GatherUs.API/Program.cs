using GatherUs.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

ApplicationConfigurator.ConfigureApplication(builder);

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