using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.IO;
using System.Linq;
using UniversalReportDemo.Data;
using UniversalReportDemo.Import;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UniversalReportDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CityPopulation> Data { get; set; }

        public async Task OnGet()
        {
            Data = await _dbContext.CityPopulations.ToListAsync();
        }
    }
}
