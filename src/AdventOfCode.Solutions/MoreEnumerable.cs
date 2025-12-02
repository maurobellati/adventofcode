namespace AdventOfCode;

public static class MoreEnumerable
{
    public static IEnumerable<long> Range(long start, long end)
    {
        for (var i = start; i <= end; i++)
        {
            yield return i;
        }
    }
}
