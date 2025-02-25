using UniversalReportCore.ViewModels;

namespace UniversalReportCore.Ui.ViewModels
{
    public class EntityFieldViewModelBase<T> where T : IEntityViewModel<int>
    {
        public T Parent { get; private set; }
        public string? Slug { get; set; }

        public EntityFieldViewModelBase(T parent)
        {
            Parent = parent;
        }
    }
}
