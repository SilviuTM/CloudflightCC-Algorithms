# region Read and Ready
using System.ComponentModel.Design;

string? level = Console.ReadLine();
string input = File.ReadAllText(Directory.GetCurrentDirectory() + "/../../../input/level" + level + ".in");
input = input.Replace("\r\n", " ");
input = input.Replace("\n", " ");
input = input.Trim();

string[] data = input.Split(' ');
# endregion

int matches = int.Parse(data[0]);
int competitors = int.Parse(data[1]);

StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/../../../output/level" + level + ".out", false);
int index = 2; string match = string.Empty;
while (index < data.Length)
{
    int r, p, s, l, y;
    match = data[index++];
    r = getNumberofFighterLevel3(match);
    match = data[index++];
    p = getNumberofFighterLevel3(match);
    match = data[index++];
    s = getNumberofFighterLevel3(match);
    match = data[index++];
    y = getNumberofFighterLevel3(match);
    match = data[index++];
    l = getNumberofFighterLevel3(match);
    
    string myMatch = "";

    int brSize = competitors / 2;
    int Rsize = brSize;

    string bracket = "";

    while (r > 0)
    {
        bracket = "";
        for (int i = 0; i < brSize - 1; i++)
        {
            bracket += "R"; r--;
        }

        if (y > 0)
        {
            bracket += "Y"; y--;
        }
        if (p > 0)
        {
            bracket += "P"; p--;
        }
        else
        {
            bracket += "S"; s--;
        }

        myMatch += bracket;
    }

    bracket = "";

    
        while (r > 0)
        {
            bracket += "R"; r--;
        }
   
    while (bracket.Length < brSize - 1) 
    {
        if (p > 0)
        {
            bracket += "P"; p--;
        }
        else if (l > 0)
        {
            bracket += "L"; l--;
        }
        else
        {
            bracket += "S"; s--;
        }
    }

    myMatch += bracket; bracket = "";

    int Ysize = brSize;
    while (y > 0)
    {
        while (y < Ysize - 1)
        {
            Ysize /= 2;
        }

        bracket = "";
        for (int i = 0; i < Ysize - 1; i++)
        {
            bracket += "Y"; y--;
        }

        if (p > 0)
        {
            bracket += "P"; p--;
        }
        else if (l > 0)
        {
            bracket += "L"; l--;
        }
        else
        {
            bracket += "S"; s--;
        }

        myMatch += bracket;
    }






    while (p > 0) 
    {
        myMatch += "P"; p--;
    }

    while (l > 0)
    {
        myMatch += "L"; l--;
    }

    while (s > 0)
    {
        myMatch += "S"; s--;
    }

    Console.WriteLine(myMatch);
    sw.WriteLine(myMatch);
}

string computeRound(string match)
{
    string newMatch = string.Empty;

    int i = 0;
    while (i < match.Length - 1)
    {
        if (match[i] == 'P')
            if (match[i + 1] == 'S')
                newMatch += "S";
            else newMatch += "P";

        if (match[i] == 'R')
            if (match[i + 1] == 'P')
                newMatch += "P";
            else newMatch += "R";

        if (match[i] == 'S')
            if (match[i + 1] == 'R')
                newMatch += "R";
            else newMatch += "S";

        i += 2;
    }
    if (i < match.Length) newMatch += match[i];

    return newMatch;
}
/*
string BackwardsRound(string match, int s, int p, int r)
{
    string newMatch = string.Empty;

    foreach (var c in match)
    {
        if (c == 'S')
            if (s - getCountChar(newMatch, 'S') > 1)
                newMatch += "SS";
            else newMatch += "PS";
    }
}*/

int getNumberofFighterLevel3(string data)
{
    int n = 0;
    for (int i = 0; i < data.Length - 1; i++)
    {
        n = n * 10 + (data[i] - '0');
    }
    return n;
}

int getCountChar(string data, char c)
{
    int n = 0;
    foreach (var item in data)
    {
        if (item == c)
            n++;
    }
    return n;
}

sw.Close();