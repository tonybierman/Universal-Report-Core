namespace UniversalReportDemo.ViewModels
{
    public interface IEntityViewModel<T> where T : struct
    {
        T Id { get; }
    }
}
