namespace Common
{
    public interface IFetch
    {
        string Value { get; }
        int PageSize { get; }
        int PageNumber { get; }
    }
}
