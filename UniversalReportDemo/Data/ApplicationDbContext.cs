namespace UniversalReportDemo.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CityPopulation> CityPopulations { get; set; }
        public DbSet<NationalGdp> NationalGdps { get; set; }
    }
}
