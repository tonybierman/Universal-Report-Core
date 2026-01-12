using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace UniversalReportHeavyDemo.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            Debugger.Launch();
            var lobConnectionString = "Server=127.0.0.1:3037;Database=UniversalReportDB;User=demo;Password=password;";
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(lobConnectionString, ServerVersion.AutoDetect(lobConnectionString));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
