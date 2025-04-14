namespace UniversalReportCore
{
    public class ReportColumnDefinition : IReportColumnDefinition
    {
        public ReportColumnDefinition()
        {
            
        }

        public string DisplayName { get; set; } = default!;
        public string PropertyName { get; set; } = default!;
        public string? ViewModelName { get; set; } = default!;
        public bool IsDisplayKey { get; set; } // TODO: Enforce one and one column must be display key
        public bool IsSortable { get; set; } = false;
        public string? DefaultSort { get; set; } = default!;
        public string? CssClass { get; set; } = default!;
        public bool IsSortDescending { get; set; } = false; // New property for toggling sort direction
        public string? RenderPartial { get; set; } // Optional Razor partial for custom rendering
        public bool HideInPortrait { get; set; }
        public AggregationType Aggregation { get; set; }
        public Type? ViewModelType { get; set; }
        public IFieldFormatProvider? FieldFormatProvider { get; set; }
    }
}
