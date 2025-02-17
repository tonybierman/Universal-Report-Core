namespace UniversalReportCore
{
    public interface IReportColumnDefinition
    {
        string DisplayName { get; set; }
        bool IsSortable { get; set; }
        string? DefaultSort { get; set; }
        bool IsSortDescending { get; set; }
        string PropertyName { get; set; }
        string ViewModelName { get; set; }
        string? RenderPartial { get; set; }
        bool HideInPortrait { get; set; }
    }
}