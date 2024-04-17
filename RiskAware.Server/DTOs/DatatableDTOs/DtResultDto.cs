namespace RiskAware.Server.DTOs.DatatableDTOs
{
    public class DtResultDto<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalRowCount { get; set; }
    }
}
