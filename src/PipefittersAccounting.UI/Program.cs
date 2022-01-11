using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using PipefittersAccounting.UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
  .AddBlazorise(options =>
 {
     options.ChangeTextOnKeyPress = true;
 })
  .AddBootstrap5Providers()
  .AddFontAwesomeIcons();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7035/api/")
});

await builder.Build().RunAsync();
