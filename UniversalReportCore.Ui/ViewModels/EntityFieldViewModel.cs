using UniversalReportCore.ViewModels;

namespace UniversalReportCore.Ui.ViewModels
{
    public class EntityFieldViewModel
    {
        public IEntityViewModel<int> Parent { get; private set; }
        public IReportColumnDefinition Column { get; private set; }
        public string? Slug { get; set; }

        public EntityFieldViewModel(IEntityViewModel<int> parent, IReportColumnDefinition column)
        {
            Parent = parent;
            Column = column;
        }
    }
}
