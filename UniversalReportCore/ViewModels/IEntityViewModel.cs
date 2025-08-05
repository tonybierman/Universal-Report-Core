namespace UniversalReportCore.ViewModels
{
    public interface IEntityViewModel<T> : IBaseEntityViewModel where T : struct
    {
        T Id { get; }
    }
}
