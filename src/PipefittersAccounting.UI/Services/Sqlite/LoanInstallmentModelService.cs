using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Utilities;
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

            if (dbContext.LoanInstallments!.Count() > 0)
                return;

            var result = await _httpClient!.GetFromJsonAsync<Root<LoanInstallmentWriteModel>>("/sample-data/LoanInstallmentModel.json");

            if (result != null)
            {
                await dbContext.LoanInstallments!.AddRangeAsync(result.Items);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<OperationResult<List<LoanInstallmentWriteModel>>> GetLoanInstallmentModelsAsync()
        {
            try
            {
                await using var dbContext = await _factory!.CreateDbContextAsync();
                List<LoanInstallmentWriteModel>? installments = await dbContext.LoanInstallments!.ToListAsync();

                if (installments is not null)
                {
                    return OperationResult<List<LoanInstallmentWriteModel>>.CreateSuccessResult(installments);
                }
                else
                {
                    return OperationResult<List<LoanInstallmentWriteModel>>.CreateFailure("No loan installments were found.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<LoanInstallmentWriteModel>>.CreateFailure($"Error: {ex.Message}");
            }

        }

        public async Task<OperationResult<LoanInstallmentWriteModel>> GetLoanInstallmentModelAsync(int id)
        {
            try
            {
                await using var dbContext = await _factory!.CreateDbContextAsync();
                LoanInstallmentWriteModel? installment = await dbContext.LoanInstallments!.FindAsync(id);

                if (installment is not null)
                {
                    return OperationResult<LoanInstallmentWriteModel>.CreateSuccessResult(installment);
                }
                else
                {
                    return OperationResult<LoanInstallmentWriteModel>.CreateFailure($"Loan installment with installment number {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<LoanInstallmentWriteModel>.CreateFailure($"Error: {ex.Message}");
            }

        }

        public async Task<OperationResult<bool>> AddAsync(LoanInstallmentWriteModel installment)
        {
            try
            {
                await using var dbContext = await _factory!.CreateDbContextAsync();

                await dbContext.LoanInstallments!.AddAsync(installment);
                await dbContext.SaveChangesAsync();

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure($"Error: {ex.Message}.");
            }
        }

        public async Task<OperationResult<bool>> AddAsync(List<LoanInstallmentWriteModel> installments)
        {
            Console.WriteLine("AddAsync");
            try
            {
                await using var dbContext = await _factory!.CreateDbContextAsync();

                await dbContext.LoanInstallments!.AddRangeAsync(installments);
                await dbContext.SaveChangesAsync();

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure($"Error: {ex.Message}.");
            }
        }

        public async Task<OperationResult<bool>> DeleteAll()
        {
            Console.WriteLine("DeleteAll");

            try
            {
                await using var dbContext = await _factory!.CreateDbContextAsync();

                List<LoanInstallmentWriteModel>? installments = await dbContext.LoanInstallments!.ToListAsync();

                if (installments is not null && installments.Any())
                {
                    dbContext.RemoveRange(installments);
                    await dbContext.SaveChangesAsync();
                }

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure($"Error: {ex.Message}.");
            }
        }
    }
}
