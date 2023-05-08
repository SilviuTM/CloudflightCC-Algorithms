StreamReader sr = new(Directory.GetCurrentDirectory() + @"\..\..\..\" + Console.ReadLine());

string? input = sr.ReadToEnd();
string[] data = input != null ? input.TrimEnd().Split(' ') : Array.Empty<string>();

int rows = int.Parse(data[0]);
int cols = int.Parse(data[1]);
int nPts = int.Parse(data[2]);

int[,] board = new int[rows, cols];

int i;
for (i = 3; i < nPts * 2 + 3; i += 2)
{
    int p = int.Parse(data[i]);
    int c = int.Parse(data[i + 1]);

    var aux = computePosition(p);
    board[aux.row, aux.column] = c * 1000; // point
}

int nPaths = int.Parse(data[i++]);
for (int path = 0; path < nPaths; path++)
{
    int pathColor = int.Parse(data[i++]);
    (int row, int col) startPos = computePosition(int.Parse(data[i++]));
    int posRow = startPos.row;
    int posCol = startPos.col;
    int pathLength = int.Parse(data[i++]);

    int step, errorCode = 0;
    for (step = 0; step < pathLength && errorCode == 0; step++)
    {
        // process move
        if (data[i] == "N")
            posRow--;
        if (data[i] == "S")
            posRow++;
        if (data[i] == "W")
            posCol--;
        if (data[i] == "E")
            posCol++;

        // error checking
        if (errorCode == 0)
        if (posRow < 0 || posRow >= rows || posCol < 0 || posCol >= cols) // OoB
            errorCode = 2;

        else if (step == pathLength - 1 && (board[posRow, posCol] != pathColor * 1000 ||
                (board[posRow, posCol] == pathColor * 1000 &&
                        startPos.row == posRow && startPos.col == posCol))) // does not end
            errorCode = 1;

        else if (board[posRow, posCol] == pathColor) // crosses itself
            errorCode = 3;

        else if (board[posRow, posCol] != 0 &&
            board[posRow, posCol] != pathColor &&
            board[posRow, posCol] != pathColor * 1000) // crosses another color
            errorCode = 4;

        // save state
        if (!(posRow < 0 || posRow >= rows || posCol < 0 || posCol >= cols))
            board[posRow, posCol] = pathColor;
        
        i++;
    }

    if (errorCode == 0) // completely valid, show 1 and length
        Console.WriteLine("1 " + pathLength);
    else if (errorCode == 1) // does not end in corresponding place, show -1 and length
        Console.WriteLine("-1 " + pathLength);
    else if (errorCode == 2) // goes OoB, show -1 and step of error
        Console.WriteLine("-1 " + step);
    else if (errorCode == 3) // crosses itself, show -1 and step
        Console.WriteLine("-1 " + step);
    else if (errorCode == 4) // crosses another color, show -1 and step of error
        Console.WriteLine("-1 " + step);
    else
        Console.WriteLine("this is not possible. bug. report.");
}

(int row, int column) computePosition(int position)
{
    return ((int)MathF.Ceiling(position * 1f / cols) - 1, (position - 1) % cols);
}

int computeDistance((int row, int column) pos1, (int row, int column) pos2)
{
    return Math.Abs(pos1.row - pos2.row) + Math.Abs(pos1.column - pos2.column);
}

int findAllSquaresOfColor(int color)
{
    int nr = 0;
    for (int i = 0; i < cols; i++)
        for (int j = 0; j < rows; j++)
            if (board[j, i] == color)
                nr++;

    return nr;
}