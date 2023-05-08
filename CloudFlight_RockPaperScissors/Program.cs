# region Read and Ready

string? level = Console.ReadLine();
string input = File.ReadAllText(Directory.GetCurrentDirectory() + "/../../../input/level" + level + ".in");
input = input.Replace("\r\n", " ");
input = input.Replace("\n", " ");
input = input.Trim();

string[] data = input.Split(' ');
#endregion

int matches = int.Parse(data[0]);
int particip = int.Parse(data[1]);
StreamWriter sw = new(Directory.GetCurrentDirectory() + "/../../../output/level" + level + ".out", false);

// REQUIRED FOR LEVEL 5 (commented lines go inside the for)
int r, p, s, y, l;
string res;
int aR, aP, aS, aY, aL;
/*
aR = r = GetNumberFromString(data[2 + i * 5 + 0]);
aP = p = GetNumberFromString(data[2 + i * 5 + 1]);
aS = s = GetNumberFromString(data[2 + i * 5 + 2]);
aY = y = GetNumberFromString(data[2 + i * 5 + 3]);
aL = l = GetNumberFromString(data[2 + i * 5 + 4]);
*/

int SLICESIZE = 32;
int GROUPSIZE = 16;

bool found;
Dictionary<string, string>[] parts;
Dictionary<string, string>[] groups;
for (int i = 0; i < matches; i++)
{
    string tourney = data[i + 2];
    parts = new Dictionary<string, string>[particip / SLICESIZE];
    groups = new Dictionary<string, string>[particip / SLICESIZE / GROUPSIZE];
    found = false;
    Console.WriteLine("\n\n" + tourney + " ({0})", tourney.Count((x) => { return x == 'X'; }));

    for (int k = 0; k < parts.Length; k++)
    {
        parts[k] = new Dictionary<string, string>();
        SimplifySlices(tourney.Substring(SLICESIZE * k, SLICESIZE), 0, k);
    }

    ulong paths = 1;
    for (int k = 0; k < parts.Length; k++)
    {
        paths *= (ulong)parts[k].Keys.Count;
        Console.Write("[");
        foreach(var key in parts[k].Keys)
        {
            Console.Write(" {0}", key);
        }
        Console.Write(" ]   ");
    }
    Console.Write(paths);


    for (int k = 0; k < groups.Length; k++)
    {
        groups[k] = new Dictionary<string, string>();
        SimplifyGroups("", 0, k);
    }

    Console.WriteLine();
    paths = 1;
    for (int k = 0; k < groups.Length; k++)
    {
        paths *= (ulong)groups[k].Keys.Count;
        Console.Write("[");
        foreach (var key in groups[k].Keys)
        {
            Console.Write(" {0}", key);
        }
        Console.Write(" ]   ");
    }
    Console.Write(paths);

    StateTracking("", 0);
}

sw.Close();

void StateTracking(string sin, int level)
{
    if (found) return;

    if (level == groups.Length)
    {
        if (CompTourney(sin) == "S")
        {
            // compute full string
            string groupToParts = "";
            string partsToFull = "";

            for (int i = 0; i < groups.Length; i++) 
                groupToParts += groups[i][sin[i].ToString()];

            for (int i = 0; i < parts.Length; i++)
                partsToFull += parts[i][groupToParts[i].ToString()];

            Console.WriteLine("\n" + partsToFull);
            sw.WriteLine(partsToFull);
            found = true;
        }

        return;
    }

    foreach (var key in groups[level].Keys)
    {
        string aux = sin + key;
        StateTracking(aux, level + 1);
    }
}

void SimplifyGroups(string sin, int level, int group)
{
    if (level == GROUPSIZE)
    {
        string key = CompTourney(sin);
        if (groups[group].ContainsKey(key))
            groups[group][key] = sin;
        else groups[group].Add(key, sin);
    }
    else
    {
        foreach (var key in parts[group * GROUPSIZE + level].Keys)
        {
            string aux = sin + key;
            SimplifyGroups(aux, level + 1, group);
        }
    }
}

void SimplifySlices(string sin, int level, int part)
{
    if (level == sin.Length)
    {
        string key = CompTourney(sin);
        if (parts[part].ContainsKey(key))
            parts[part][key] = sin;
        else parts[part].Add(key, sin);
    }

    else
    {
        if (sin[level] == 'X')
        {
            string options = "SPLRY"; // only if pair is X

            if (sin[level + ((1 + level % 2) % 2)] == 'S') options = "RYS";
            if (sin[level + ((1 + level % 2) % 2)] == 'P') options = "LSP";
            if (sin[level + ((1 + level % 2) % 2)] == 'L') options = "SRL";
            if (sin[level + ((1 + level % 2) % 2)] == 'R') options = "PYR";
            if (sin[level + ((1 + level % 2) % 2)] == 'Y') options = "PLY";

            string aux; 
            
            foreach (char c in options)
            {
                aux = sin[..level] + c.ToString() + sin[(level + 1)..];
                SimplifySlices(aux, level + 1, part);
            }
        }
        else
        {
            SimplifySlices(sin, level + 1, part);
        }
    }
}

string ComputeHalf(int bSize) // for level 5, call as string res = ComputeHalf(partic / 2)
{
    if (bSize == 0) return "S";

    string toReturn = "";
    if (p > 0)
    {
        while (toReturn.Length < bSize - 1)
        {
            if (r > 0) { toReturn += "R"; r--; }
            else if (y > 0) { toReturn += "Y"; y--; }
            else if (s > 1) { toReturn = "S" + toReturn; s--; }
            else if (l > 0) { toReturn = "L" + toReturn; l--; }
            else if (p > 1) { toReturn += "P"; p--; }
            else { toReturn = "S" + toReturn; s--; }
        }

        toReturn += "P"; p--;
        return toReturn + ComputeHalf(bSize / 2);
    }
    else
    {
        while (toReturn.Length < bSize)
        {
            if (r > 0)
            {
                int bracket = bSize / 2;
                while (r < bracket - 1 || toReturn.Length + bracket > bSize - 1) bracket /= 2;

                for (int _ = 0; _ < bracket - 1; _++)
                { toReturn += "R"; r--; }

                if (bracket > 1)
                {
                    toReturn += "Y"; y--;
                }
                else
                {
                    if (y > 0) { toReturn += "Y"; y--; }
                    else { toReturn += "L"; l--; }

                    toReturn += "L"; l--;
                }
            }

            else if (y > 0)
            {
                int bracket = bSize;
                while (y < bracket - 1 || toReturn.Length + bracket > bSize) bracket /= 2;

                for (int _ = 0; _ < bracket - 1; _++)
                { toReturn += "Y"; y--; }

                if (bracket > 1)
                {
                    toReturn += "L"; l--;
                }
                else
                {
                    if (y > 0) { toReturn += "Y"; y--; }
                    else if (l > 0) { toReturn += "L"; l--; }
                    else
                    { toReturn += "S"; s--; }

                    if (l > 0) { toReturn += "L"; l--; }
                    else
                    { toReturn += "S"; s--; }
                }
            }

            else
            {
                while (l > 0) { toReturn += "L"; l--; }
                while (s > 0) { toReturn += "S"; s--; }

                return toReturn;
            }

        }
        
        return toReturn + ComputeHalf(bSize / 2);
    }
}

string CompMatch(string sin) // for level 1, but updated to level 5
{
    string s = "";

    for (int j = 0; j < sin.Length; j += 2)
    {
        if (sin[j] == 'R') if (sin[j + 1] == 'P') s += "P"; else if (sin[j + 1] == 'Y') s += "Y"; else s += "R";
        if (sin[j] == 'S') if (sin[j + 1] == 'R') s += "R"; else if (sin[j + 1] == 'Y') s += "Y"; else s += "S";
        if (sin[j] == 'L') if (sin[j + 1] == 'R') s += "R"; else if (sin[j + 1] == 'S') s += "S"; else s += "L";
        if (sin[j] == 'P') if (sin[j + 1] == 'S') s += "S"; else if (sin[j + 1] == 'L') s += "L"; else s += "P";
        if (sin[j] == 'Y') if (sin[j + 1] == 'P') s += "P"; else if (sin[j + 1] == 'L') s += "L"; else s += "Y";
    }

    return s;
}

void ShowFighters(string sin)
{
    int r = 0, p = 0, s = 0, l = 0, y = 0;
    foreach (var c in sin)
    {
        if (c == 'R') r++;
        if (c == 'P') p++;
        if (c == 'S') s++;
        if (c == 'L') l++;
        if (c == 'Y') y++;
    }

    Console.WriteLine("{0}R {1}P {2}S {3}Y {4}L", r, p, s, y, l);
    if (aR != r || aP != p || aS != s || aL != l || aY != y)
        throw new Exception();
}

string CompTourney(string sin) // for level 2 and debugging
{
    string s = sin;
    while (s.Length > 1)
        s = CompMatch(s);

    return s;
} 

int GetNumberFromString(string s)
{
    int n = 0;

    foreach (char c in s)
    {
        if (c >= '0' && c <= '9')
            n = n * 10 + (c - '0');
    }

    return n;
}