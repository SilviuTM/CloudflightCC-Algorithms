bool isInListofLists(List<List<int>> csharpFinalBoss, List<int> gigachadList)
{
    foreach (var list in csharpFinalBoss)
    {
        if (list.Count == gigachadList.Count)
        {
            bool ambasing = true;

            for (int i = 0; i < list.Count; i++)
                if (list[i] != gigachadList[i])
                    ambasing = false;

            if (ambasing == true)
                return true;
        }
    }

    return false;
}

Dictionary<int, int> debt = new();
Dictionary<int, List<int>> profit = new();
Dictionary<int, int> time = new();

string? input = Console.ReadLine();
string[] data = input != null ? input.Split(' ') : Array.Empty<string>();

int i;
for (i = 0; i < data.Length; i += 4)
{
    if (data[i] == "F")
    {
        debt.Add(int.Parse(data[i + 1]), int.Parse(data[i + 3]));
        time.Add(int.Parse(data[i + 1]), int.Parse(data[i + 2]));
    }
    else break;
}

while (i < data.Length)
{
    if (!profit.ContainsKey(int.Parse(data[i + 1])))
        profit.Add(int.Parse(data[i + 1]), new List<int>());

    profit[int.Parse(data[i + 1])].Add(int.Parse(data[i + 2]));
    i += 3;
}

int minLength = int.MaxValue;
List<List<int>> min = new();

void function(Dictionary<int, int> localDebt, int profitDay, int profitDayPart)
{
    while (!profit.ContainsKey(profitDay)) profitDay++;
    if (profitDayPart == profit[profitDay].Count) { profitDay++; profitDayPart = 0; }
    while (!profit.ContainsKey(profitDay) && profitDay <= profit.Keys.Max()) profitDay++;
    if (profitDay > profit.Keys.Max())
    {
        // end of story
        List<int> result = new();
        foreach (int key in localDebt.Keys)
            if (localDebt[key] > 0)
                result.Add(key);

        if (result.Count < minLength)
        {
            minLength = result.Count;
            min = new();
        }

        if (result.Count == minLength && !isInListofLists(min, result))
        {
            result.Sort();
            min.Add(result);
        }

        return;
    }

    // otherwise, let us commence forth
    bool premature = true;
    for (int i = 1; i <= profitDay && i <= localDebt.Keys.Max(); i++)
    {
        if (localDebt[i] >= profit[profitDay][profitDayPart] && i + time[i] > profitDay)
        {
            Dictionary<int, int> auxDebt = new(localDebt);
            auxDebt[i] -= profit[profitDay][profitDayPart];
            function(auxDebt, profitDay, profitDayPart + 1);
            premature = false;
        }
    }
    /*
    if (premature)
    {
        List<int> result = new();
        foreach (int key in localDebt.Keys)
            if (localDebt[key] > 0)
                result.Add(key);

        result.Sort();

        if (!isInListofLists(min, result))
            min.Add(result);
    }*/
}

function(new(debt), 0, 0);
foreach (List<int> l in min)
{
    foreach (int lDay in l)
        Console.Write(lDay + " ");

    Console.WriteLine();
}