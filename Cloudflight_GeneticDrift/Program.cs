string? input = Console.ReadLine();
string[] data = input != null ?  input.Split(' ') : Array.Empty<string>();

List<int> perm = new List<int>();
for (int i = 1; i < data.Length; i++)
    perm.Add(int.Parse(data[i]));

Console.WriteLine(computeResult());

int computeResult()
{
    List<(int x, int xi, int y, int yi)> pairs = computePairs(perm);

    int maxscore = -1;
    foreach (var x in pairs)
    {
        int score = computeScore(x.xi, x.yi, new(perm));
        if (score > maxscore) maxscore = score;
    }

    if (maxscore == 0) return 1;

    foreach (var x in pairs)
    {
        int score = computeScore(x.xi, x.yi, new(perm));
        if (score == maxscore)
        {
            perm = invertPermutation(x.xi, x.yi, perm);
            return computeResult() + 1;
        }
    }

    return 0;
}

int computeScore(int xi, int yi, List<int> permutation)
{
    return computePairs(invertPermutation(xi, yi, new(permutation))).Count;
}

List<int> invertPermutation(int xi, int yi, List<int> permutation)
{
    if (permutation[xi] + permutation[yi] == 1)
        yi--;
    else xi++;

    for (int i = 0; i <= (yi - xi) / 2; i++)
    {
        int aux = permutation[xi + i];
        permutation[xi + i] = permutation[yi - i];
        permutation[yi - i] = aux;

        if (xi + i != yi - i)
            permutation[xi + i] *= -1;
        permutation[yi - i] *= -1;
    }

    return permutation;
}

List<(int x, int xi, int y, int yi)> computePairs(List<int> permutation)
{
    List<(int x, int xi, int y, int yi)> pairs = new();

    for (int i = 0; i < permutation.Count; i++)
        for (int j = i + 1; j < permutation.Count; j++)
            if (Math.Abs(Math.Abs(permutation[i]) - Math.Abs(permutation[j])) == 1 &&
               ((permutation[i] >= 0 && permutation[j] < 0) || (permutation[i] < 0 && permutation[j] >= 0))
               )
                pairs.Add((permutation[i], i, permutation[j], j));

    return pairs;
}