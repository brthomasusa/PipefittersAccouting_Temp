using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace PipefittersAccounting.UI.Sqlite
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<LoanInstallmentModel>? LoanInstallmentModel { get; set; }

        private readonly Lazy<Task<IJSObjectReference>>? _moduleTask;

        public SqliteDbContext(DbContextOptions<SqliteDbContext> options, IJSRuntime jsRuntime)
            : base(options)
        {
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./js/file.js").AsTask());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanInstallmentModelConfig).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}