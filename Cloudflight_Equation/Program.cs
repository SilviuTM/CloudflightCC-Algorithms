string? input = Console.ReadLine();

# region Self-Convert
List<int>[] convertFrom = new List<int>[10];
convertFrom[0] = new int[] { 6, 9 }.ToList<int>();
convertFrom[1] = new int[] { }.ToList<int>();
convertFrom[2] = new int[] { 3 }.ToList<int>();
convertFrom[3] = new int[] { 2, 5 }.ToList<int>();
convertFrom[4] = new int[] { }.ToList<int>();
convertFrom[5] = new int[] { 3 }.ToList<int>();
convertFrom[6] = new int[] { 0, 9 }.ToList<int>();
convertFrom[7] = new int[] { }.ToList<int>();
convertFrom[8] = new int[] { }.ToList<int>();
convertFrom[9] = new int[] { 0, 6 }.ToList<int>();
# endregion

# region AddStick
List<int>[] addTo = new List<int>[10];
addTo[0] = new int[] { 8 }.ToList<int>();
addTo[1] = new int[] { 7 }.ToList<int>();
addTo[2] = new int[0].ToList<int>();
addTo[3] = new int[] { 9 }.ToList<int>();
addTo[4] = new int[0].ToList<int>();
addTo[5] = new int[] { 6, 9 }.ToList<int>();
addTo[6] = new int[] { 8 }.ToList<int>();
addTo[7] = new int[0].ToList<int>();
addTo[8] = new int[0].ToList<int>();
addTo[9] = new int[] { 8 }.ToList<int>();
# endregion

# region RemoveStick
List<int>[] removeFrom = new List<int>[10];
removeFrom[0] = new int[0].ToList<int>();
removeFrom[1] = new int[0].ToList<int>();
removeFrom[2] = new int[0].ToList<int>();
removeFrom[3] = new int[0].ToList<int>();
removeFrom[4] = new int[0].ToList<int>();
removeFrom[5] = new int[0].ToList<int>();
removeFrom[6] = new int[] { 5 }.ToList<int>();
removeFrom[7] = new int[] { 1 }.ToList<int>();
removeFrom[8] = new int[] { 0, 6, 9 }.ToList<int>();
removeFrom[9] = new int[] { 3, 5 }.ToList<int>();
# endregion

backtrack(new(input), 0, false, false);

// check equal -> minus
for (int k = 0; k < input.Length; k++)
{
    if (input[k] == '-')
    {
        input = input[..input.IndexOf("=")] + "-" + input[(input.IndexOf("=") + 1)..]; // replace = with -
        input = input[..k] + "=" + input[(k + 1)..]; // replace - with =

        check(eqFromString(input));
    }
}

void backtrack(string equation, int k, bool mustAdd, bool mustRemove)
{
    // end recursivity
    if (k >= equation.Length)
    {
        if (!mustAdd && !mustRemove)
            check(eqFromString(equation));
        return;
    }

    bool isNumber = equation[k] >= '0' && equation[k] <= '9';

    for (int choice = 0; choice < 4; choice++)
    {
        // if 9, self, add, remove
        // if +, remove
        // if -, add

        // ignore any
        if (choice == 0)
            backtrack(new(equation), k + 1, mustAdd, mustRemove);

        // self
        else if (choice == 1 && isNumber && !mustAdd && !mustRemove)
        {
            for (int i = 0; i < convertFrom[equation[k] - '0'].Count; i++)
            {
                string newEq = equation[..k] + convertFrom[equation[k] - '0'][i] + equation[(k + 1)..];
                
                check(eqFromString(newEq));
            }
        }

        // add
        else if (choice == 2 && !mustRemove)
        {
            if (isNumber)
            {
                for (int i = 0; i < addTo[equation[k] - '0'].Count; i++)
                {
                    string newEq = equation[..k] + addTo[equation[k] - '0'][i] + equation[(k + 1)..];

                    if (mustAdd) check(eqFromString(newEq));
                    else backtrack(newEq, k + 1, mustAdd, true);
                }
            }
            else if (equation[k] == '-')
            {
                string newEq = equation[..k] + "+" + equation[(k + 1)..];

                if (mustAdd) check(eqFromString(newEq));
                else backtrack(newEq, k + 1, mustAdd, true);
            }
        }
        
        // remove
        else if (choice == 3 && !mustAdd)
        {
            if (isNumber)
            {
                for (int i = 0; i < removeFrom[equation[k] - '0'].Count; i++)
                {
                    string newEq = equation[..k] + removeFrom[equation[k] - '0'][i] + equation[(k + 1)..];

                    if (mustRemove) check(eqFromString(newEq));
                    else backtrack(newEq, k + 1, true, mustRemove);
                }
            }
            else if (equation[k] == '+')
            {
                string newEq = equation[..k] + "-" + equation[(k + 1)..];

                if (mustAdd) check(eqFromString(newEq));
                else backtrack(newEq, k + 1, true, mustRemove);
            }
        }
    }
}

void check(Equation e)
{
    int leftSide = 0, rightSide = 0;
    for (int i = 0; i < e.equalPos; i++)
        leftSide += e.numbers[i];

    for (int i = e.equalPos; i < e.numbers.Count; i++)
        rightSide += e.numbers[i];

    if (leftSide == rightSide)
        print(e);
}

void print(Equation e)
{
    for (int i = 0; i < e.equalPos; i++)
        Console.Write((i != 0 && e.numbers[i] >= 0 ? "+" : "") + e.numbers[i]);

    Console.Write("=");

    for (int i = e.equalPos; i < e.numbers.Count; i++)
        Console.Write((i != e.equalPos && e.numbers[i] >= 0 ? "+" : "") + e.numbers[i]);

    Console.Write("\n");
}

Equation eqFromString(string equation)
{
    Equation eq = new Equation
    {
        numbers = new List<int>()
    };

    int i = 0, aux = 0;
    int sign = 1;
    while (i < equation.Length)
    {
        if (equation[i] == '+')
        { sign = 1; i++; }

        else if (equation[i] == '-')
        { sign = -1; i++; }

        else if (equation[i] == '=')
        { sign = 1; i++; eq.equalPos = eq.numbers.Count; }

        while (i < equation.Length && equation[i] >= '0' && equation[i] <= '9')
        {
            aux = aux * 10 + (equation[i] - '0');
            i++;
        }

        eq.numbers.Add(sign * aux);
        aux = 0;
    }

    return eq;
}

struct Equation
{
    public List<int> numbers;
    public int equalPos;
};