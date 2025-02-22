namespace UniversalReportDemo.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CityPopulation> CityPopulations { get; set; }
        public virtual DbSet<CityPopulationCohort> CityPopulationCohorts { get; set; }

        public DbSet<NationalGdp> NationalGdps { get; set; }

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
