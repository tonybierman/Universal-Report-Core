using UniversalReportCore.ViewModels;
using System.Reflection;

namespace UniversalReportCore
{
    public class ReportColumnDefinition : IReportColumnDefinition
    {
        public ReportColumnDefinition()
        {
            ValueSelector = (item, type, prop) => prop?.GetValue(item);
        }

        public string DisplayName { get; set; } = default!;
        public string PropertyName { get; set; } = default!;
        public string? ViewModelName { get; set; }
        public bool IsDisplayKey { get; set; }
        public bool IsSortable { get; set; }
        public string? DefaultSort { get; set; }
        public string? CssClass { get; set; }
        public bool IsSortDescending { get; set; }
        public string? RenderPartial { get; set; }
        public bool HideInPortrait { get; set; }
        public AggregationType Aggregation { get; set; }
        public Type? ViewModelType { get; set; }
        public Func<BaseEntityViewModel, Type?, PropertyInfo?, object>? ValueSelector { get; set; }
        public IFieldFormatter FieldFormatter { get; set; } = default!;
    }
}