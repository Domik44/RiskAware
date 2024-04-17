namespace RiskAware.Server.ViewModels
{
    public class DtResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalRowCount { get; set; }
    }
}
