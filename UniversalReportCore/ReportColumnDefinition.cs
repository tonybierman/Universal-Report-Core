namespace UniversalReportCore
{
    public class ReportColumnDefinition : IReportColumnDefinition
    {
        public string DisplayName { get; set; } = default!;
        public string PropertyName { get; set; } = default!;
        public string? ViewModelName { get; set; } = default!;
        public bool IsSortable { get; set; } = false;
        public string? DefaultSort { get; set; } = default!;
        public bool IsSortDescending { get; set; } = false; // New property for toggling sort direction
        public string? RenderPartial { get; set; } // Optional Razor partial for custom rendering
        public bool HideInPortrait { get; set; }
    }
}
