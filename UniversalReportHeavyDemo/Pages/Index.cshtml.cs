using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.IO;
using System.Linq;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.Import;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UniversalReportHeavyDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel()
        {
        }

        public async Task OnGet()
        {
        }
    }
}
