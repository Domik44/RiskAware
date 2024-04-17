namespace RiskAware.Server.DTOs.DatatableDTOs
{
    public class ColumnFilter
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string PropertyName => char.ToUpper(Id[0]) + Id.Substring(1);
    }

    public class Sorting
    {
        public string Id { get; set; }
        public bool Desc { get; set; }
        public string Dir => Desc ? "desc" : "asc";
    }

    public class DtParamsDto
    {
        public int Start { get; set; }
        public int Size { get; set; }
        public IEnumerable<ColumnFilter> Filters { get; set; }
        public IEnumerable<Sorting> Sorting { get; set; }
    }
}
