using System.Reflection;
using UniversalReportCore.ViewModels;

namespace UniversalReportCore
{
    public interface IReportColumnDefinition
    {
        string DisplayName { get; set; }
        public string? Description { get; set; }
        bool IsSortable { get; set; }
        bool IsDisplayKey { get; set; }
        bool IsSearchable { get; set; }
        string? DefaultSort { get; set; }
        string? CssClass { get; set; }
        bool IsSortDescending { get; set; }
        string PropertyName { get; set; }
        string ViewModelName { get; set; }
        string? RenderPartial { get; set; }
        bool HideInPortrait { get; set; }
        AggregationType Aggregation { get; set; }
        Type? ViewModelType { get; }
        Func<BaseEntityViewModel, Type?, PropertyInfo?, object>? ValueSelector { get; set; }
        public IFieldFormatter FieldFormatter { get; set; }
    }
}