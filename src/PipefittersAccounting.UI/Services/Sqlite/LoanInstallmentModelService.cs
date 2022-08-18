using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using PipefittersAccounting.UI.Sqlite;

namespace PipefittersAccounting.UI.Services.Sqlite
{
    public class LoanInstallmentModelService
    {
        private readonly IDbContextFactory<SqliteDbContext>? _factory;
        private readonly HttpClient? _httpClient;
        private bool _hasSynced = false;

        public LoanInstallmentModelService
        (
            IDbContextFactory<SqliteDbContext> factory,
            HttpClient httpClient
        )
        {
            _factory = factory;
            _httpClient = httpClient;
        }

        public async Task InitializeAsync()
        {
            if (_hasSynced)
                return;

            await using var dbContext = await _factory!.CreateDbContextAsync();

            if (dbContext.LoanInstallmentModel!.Count() > 0)
                return;

            var result = await _httpClient!.GetFromJsonAsync<Root<LoanInstallmentModel>>("/sample-data/LoanInstallmentModel.json");

            if (result != null)
            {
                await dbContext.LoanInstallmentModel!.AddRangeAsync(result.Items);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<LoanInstallmentModel>> GetLoanInstallmentModelsAsync()
        {
            await using var dbContext = await _factory!.CreateDbContextAsync();

            return await dbContext.LoanInstallmentModel!.ToListAsync();
        }
    }
}