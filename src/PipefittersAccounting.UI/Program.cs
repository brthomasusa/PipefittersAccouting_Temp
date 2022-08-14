using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using FluentValidation;

using PipefittersAccounting.UI;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Services.Finance;
using PipefittersAccounting.UI.Services.HumanResources;

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

await builder.Build().RunAsync();