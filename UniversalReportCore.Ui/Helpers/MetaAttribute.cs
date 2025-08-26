namespace UniversalReportCore.Ui.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MetaAttribute : Attribute
    {
        public string Category { get; }
        public string? Name { get; }

        public MetaAttribute(string category, string? name = null)
        {
            Category = category;
            Name = name;
        }
    }
}
