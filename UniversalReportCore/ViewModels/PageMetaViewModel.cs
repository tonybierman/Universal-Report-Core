namespace UniversalReportCore.ViewModels
{
    /// <summary>
    /// Represents metadata for a report page, including its title and subtitle.
    /// </summary>
    public class PageMetaViewModel
    {
        /// <summary>
        /// Gets or sets the title of the report page.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the subtitle of the report page.
        /// </summary>
        public string? Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the desciption of the report page.
        /// </summary>
        public string? Description { get; set; }

        public PageMetaViewModel(string title, string subtitle, string? description = null)
        {
            Title = title;
            Subtitle = subtitle;
            Description = description;
        }

        public PageMetaViewModel() { }
    }
}
