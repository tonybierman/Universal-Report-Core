namespace UniversalReportHeavyDemo.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            //TODO: Validate args first ...
            var connectionString = args[0];

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        public DbSet<CityPopulation> CityPopulations { get; set; }
        public virtual DbSet<CityPopulationCohort> CityPopulationCohorts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-Many relationship (No explicit join entity)
            // Many-to-Many relationship (No explicit join entity)
            modelBuilder.Entity<CityPopulation>()
                .HasMany(cp => cp.CityPopulationCohorts)
                .WithMany(cpc => cpc.CityPopulations)
                .UsingEntity<Dictionary<string, object>>(
                    "CityPopulationCohortMapping", // Name the join table
                    j => j.HasOne<CityPopulationCohort>()
                          .WithMany()
                          .HasForeignKey("CohortId"),
                    j => j.HasOne<CityPopulation>()
                          .WithMany()
                          .HasForeignKey("CityPopulationId"));
        }
    }
}
