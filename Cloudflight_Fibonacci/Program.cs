int n = int.Parse(Console.ReadLine());

int x1 = 0, x2 = 1;
while (n > 0)
{
    x2 = x1 + x2;
    x1 = x2 - x1;
    n--;
}

Console.WriteLine(x1);