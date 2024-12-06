namespace AdventOfCode;

public static class MathExtensions
{
    public static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }

        return a;
    }

    public static long Lcm(long a, long b)
    {
        var gcd = Gcd(a, b);
        return gcd == 0 ? 0 : a * b / gcd;
    }

    /// <summary>
    ///     Computes a mod b.
    ///     This behaves differently than C# % operator that is a remainder, NOT a modulo.
    ///     % operator returns a negative number when input is negative.
    /// </summary>
    /// <returns>a mod b also for negative values of a. The return value is always between 0 and b</returns>
    public static int Mod(this int a, int b) => ((a % b) + b) % b;
}
