namespace UniversalReportCoreTests.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class AcmeDbContext : DbContext
    {
        public AcmeDbContext() { }

        public DbSet<Widget> Widgets { get; set; }

        public AcmeDbContext(DbContextOptions<AcmeDbContext> options) : base(options) { }
    }
}
