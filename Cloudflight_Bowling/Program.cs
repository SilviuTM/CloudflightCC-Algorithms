string? input = Console.ReadLine();
string[] data = input != null ?  input.Split(',') : Array.Empty<string>();

int rounds = int.Parse(data[0]);

int i = 1; int current = 0;
int score = 0;
while (current < rounds)
{
    int x1 = int.Parse(data[i]);
    int x2 = int.Parse(data[i + 1]);

    score += x1 + x2;

    if (x1 == 10 || x1 + x2 == 10)
        score += int.Parse(data[i + 2]);

    Console.Write(score + ",");
    if (x1 == 10)
        i += 1;
    else i += 2;

    current++;
}