string extractNumberAfterEqual(string s)
{
    int i = 0;
    while (i < s.Length && s[i] != '=')
        i++;

    return s[(i + 1)..];
}

string? input = Console.ReadLine();
string[] data = input != null ? input.Split(' ') : Array.Empty<string>();

int length = int.Parse(extractNumberAfterEqual(data[1]));
int iterations = int.Parse(extractNumberAfterEqual(data[2]));

if (data[0] == "tri") // level 1
{
    Console.WriteLine(3f * length * MathF.Pow(4, iterations) / MathF.Pow(3, iterations));
}
if (data[0] == "sq") // level 2
{
    Console.WriteLine(4f * length * MathF.Pow(5, iterations) / MathF.Pow(3, iterations));
}