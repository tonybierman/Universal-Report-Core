namespace UniversalReportCore
{
    public class ReportColumnFactory : IReportColumnFactory
    {
        private readonly IEnumerable<IReportColumnProvider> _providers;

        public ReportColumnFactory(IEnumerable<IReportColumnProvider> providers)
        {
            _providers = providers;
        }

        public List<IReportColumnDefinition> GetReportColumns(string slug)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported report type: {slug}");
            }

            return provider.GetColumns();
        }
    }
}
