bool isPrime(long nr)
{
    if (nr < 2) return false;

    for (int i = 2; i <= Math.Sqrt(nr); i++)
        if (nr % i == 0)
            return false;

    return true;
}

int solution(string nr)
{
    if (!isPrime(long.Parse(nr))) return 0;
    else if (nr.Length == 1)
        return 1;
    else
    {
        int ways = 0;

        for (int i = 0; i < nr.Length; i++)
            ways += solution(nr[..i] + nr[(i + 1)..]);

        return ways;
    }
}

Console.WriteLine(solution(Console.ReadLine()));