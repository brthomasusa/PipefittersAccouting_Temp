using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;

using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using FluentValidation;
using Fluxor;

using PipefittersAccounting.UI;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Services.Sqlite;
using PipefittersAccounting.UI.Services.Finance;
using PipefittersAccounting.UI.Services.HumanResources;
using PipefittersAccounting.UI.Sqlite;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
  .AddBlazorise(options =>
 {
     options.Immediate = true;
 })
  .AddBootstrap5Providers()
  .AddFontAwesomeIcons()
  .AddFluentValidationHandler();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7035/api/")
});

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IFinanciersRepository, FinanciersRepository>();
builder.Services.AddScoped<ILoanAgreementRepository, LoanAgreementRepository>();
builder.Services.AddSingleton<DatabaseService<SqliteDbContext>>();
builder.Services.AddSqliteDbContextFeature();

var currentAssembly = typeof(Program).Assembly;
builder.Services.AddFluxor(options => options.ScanAssemblies(currentAssembly));

var host = builder.Build();
await host.InitializeLoanInstallmentFeature();

await host.RunAsync();