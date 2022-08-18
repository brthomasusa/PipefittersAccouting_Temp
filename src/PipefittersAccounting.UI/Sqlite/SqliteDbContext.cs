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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.LogTo(Console.WriteLine, LogLevel.Error)
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging(false);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
#if RELEASE
            await PersistDatabaseAsync(cancellationToken);       
#endif
            return result;
        }

        private async Task PersistDatabaseAsync(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Start saving database");

            var module = await _moduleTask!.Value;
            await module.InvokeVoidAsync("syncDatabase", false, cancellationToken);

            Console.WriteLine("Finish save database");
        }
    }
}