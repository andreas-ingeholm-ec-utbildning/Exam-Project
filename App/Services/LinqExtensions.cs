namespace App.Services;

public static class LinqExtensions
{
    public static IEnumerable<T> Search<T>(this IEnumerable<T> list, string q, Func<T, string> predicate)
    {
        return list.Where(i => predicate(i)?.Contains(q, StringComparison.InvariantCultureIgnoreCase) ?? false);
    }
}