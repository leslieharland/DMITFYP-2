namespace WebApp.ViewModels
{
    public enum RangeOptions { Current, All }
    public enum Output { Excel, Csv }

    public class ExportParameters
    {
        public RangeOptions Range { get; set; }
        public Output OutputType { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool PagingEnabled { get; set; }
    }
}