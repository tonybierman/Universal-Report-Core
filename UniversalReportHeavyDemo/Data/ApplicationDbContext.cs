namespace UniversalReportHeavyDemo.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using System;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public static ApplicationDbContext CreateDbContext(string[] args)
        {
            if (args == null || args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                throw new ArgumentException("A valid connection string must be provided.", nameof(args));
            }

            try
            {
                var connectionString = args[0];
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                return new ApplicationDbContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create database context with provided connection string.", ex);
            }
        }

        public DbSet<CityPopulation> CityPopulations { get; set; }
        public virtual DbSet<CityPopulationCohort> CityPopulationCohorts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure CityPopulations index
            modelBuilder.Entity<CityPopulation>()
                .HasIndex(cp => new { cp.City, cp.Sex, cp.Year, cp.Id })
                .HasDatabaseName("idx_city_sex_year_id")
                .IsDescending(false, false, true, false); // Only Year is DESC as per SQL

            // Configure CityPopulationCohorts index
            modelBuilder.Entity<CityPopulationCohort>()
                .HasIndex(cpc => cpc.Id)
                .HasDatabaseName("idx_city_population_cohort");

            // Configure many-to-many relationship with index
            modelBuilder.Entity<CityPopulation>()
                .HasMany(cp => cp.CityPopulationCohorts)
                .WithMany(cpc => cpc.CityPopulations)
                .UsingEntity<Dictionary<string, object>>(
                    "CityPopulationCohortMapping",
                    j => j.HasOne<CityPopulationCohort>()
                          .WithMany()
                          .HasForeignKey("CohortId"),
                    j => j.HasOne<CityPopulation>()
                          .WithMany()
                          .HasForeignKey("CityPopulationId"),
                    j =>
                    {
                        // Configure composite index for mapping table
                        j.HasIndex(new[] { "CityPopulationId", "CohortId" })
                         .HasDatabaseName("idx_city_population_cohort_mapping");
                    });

            // Assuming CityPopulation has these properties - adjust lengths as needed
            modelBuilder.Entity<CityPopulation>()
                .Property(cp => cp.City)
                .HasMaxLength(100);

            modelBuilder.Entity<CityPopulation>()
                .Property(cp => cp.Sex)
                .HasMaxLength(10);
        }
    }
}