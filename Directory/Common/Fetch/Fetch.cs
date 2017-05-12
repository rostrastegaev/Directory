namespace Common
{
    public class Fetch : IFetch
    {
        public string Value { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
