using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui.ViewModels
{
    public class ReportLinkViewModel
    {
        public string? ReportPath { get; set; }
        public string? Slug { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Subtitle { get; set; }
        public string? Taxonomy { get; set; }

        public ReportLinkViewModel(string reportPath, string slug, string title, string subtitle, string description,
            string taxonomy)
        {
            ReportPath = reportPath;
            Slug = slug;
            Title = title;
            Description = description;
            Subtitle = subtitle;
            Taxonomy = taxonomy;
        }

        public ReportLinkViewModel()
        {
        }
    }
}
