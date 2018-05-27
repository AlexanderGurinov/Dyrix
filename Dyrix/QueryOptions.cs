namespace Dyrix
{
    public sealed class QueryOptions
    {
        public int? Top { get; set; }
        public string Filter { get; set; }
        public string OrderBy { get; set; }
        public bool? Count { get; set; }
        public int? MaxPageSize { get; set; }
        public Annotations? Annotations { get; set; }
    }
}