using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.JSInterop;
using PipefittersAccounting.UI.Sqlite;

namespace PipefittersAccounting.UI.Services.Sqlite
{
    public static class SqliteDbContextExtensions
    {
        public static void AddSqliteDbContextFeature(this IServiceCollection services)
        {
#if DEBUG
            services.AddDbContextFactory<SqliteDbContext>(
                    options =>
                    {
                        options.UseInMemoryDatabase("BlazorSqlite");
                        options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    });
#else
            services.AddDbContextFactory<SqliteDbContext>(
                    options => options.UseSqlite($"Filename={DatabaseService<SqliteDbContext>.FileName}"));
#endif

            services.AddScoped<LoanInstallmentModelService>();
        }

        public static async Task InitializeLoanInstallmentFeature(this WebAssemblyHost host)
        {
            // Initialize DatabaseContext and sync with IndexedDb Files
            var dbService = host.Services.GetRequiredService<DatabaseService<SqliteDbContext>>();
            await dbService.InitDatabaseAsync();

            // Sync LoanInstallmentModel
            var loanInstallmentService = host.Services.GetRequiredService<LoanInstallmentModelService>();
            await loanInstallmentService.InitializeAsync();
        }
    }
}