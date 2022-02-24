using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using NLog.Web;

using Microsoft.AspNetCore.Mvc;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddApiVersioning(config =>
    {
        config.DefaultApiVersion = new ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;
        config.ReportApiVersions = true;
    });

    builder.Services.ConfigureCors();
    builder.Services.AddInfrastructureServices();
    builder.Services.AddAuthentication();
    builder.Services.ConfigureIdentity(builder.Configuration);
    builder.Services.ConfigureEfCoreDbContext(builder.Configuration);
    builder.Services.ConfigureDapper(builder.Configuration);
    builder.Services.ConfigureJWT(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseWebAssemblyDebugging();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.All
    });

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();
    app.UseCors("CorsPolicy");
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapRazorPages();
    app.MapFallbackToFile("index.html");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

public partial class Program { }

