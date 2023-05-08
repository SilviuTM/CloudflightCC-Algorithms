string? input = Console.ReadLine();
string[] data = input != null ?  input.Split(' ') : Array.Empty<string>();

int rows = int.Parse(data[0]);
int cols = int.Parse(data[1]);

int[,] land =  new int[rows + 1, cols + 1];
for (int i = 1; i <= rows; i++)
    for (int j = 1; j <= cols; j++)
        land[i, j] = (i - 1) * cols + j;

int curRow = int.Parse(data[2]);
int curCol = int.Parse(data[3]);

char direction = data[4][0];
char mode = data[5][0];

int width = int.Parse(data[6]);

int directionRow = curRow == 1 ? 1 : -1;
int directionCol = curCol == 1 ? 1 : -1;

// serpentine harvest
if (mode == 'S')
{
    if (direction == 'O' || direction == 'W') // harvest rows
    {
        while (curRow > -width + 1 && curRow <= rows + width - 1)
        {
            while (curCol > 0 && curCol <= cols)
            {
                harvest(curRow, curCol);
                curCol += directionCol;
            }

            if (curCol == 0) curCol = 1;
            else if (curCol == cols + 1) curCol = cols;
            directionCol *= -1;

            if (direction == 'O' && directionRow == -1 && curRow - width + 1 <= 1)
                break;
            if (direction == 'W' && directionRow == 1 && curRow + width - 1 >= rows)
                break;
            if (direction == 'W' && directionRow == -1 && curRow - width + 1 <= 1)
                break;
            if (direction == 'O' && directionRow == 1 && curRow + width - 1 >= rows)
                break;

            if (direction == 'O' && directionRow == 1) // we went east, turn down, so we do a big turn
                curRow += directionRow * (2 * width - 1);
            else if (direction == 'O' && directionRow == -1) // we went east, turn up, so we do a small turn
                curRow += directionRow;
            else if (direction == 'W' && directionRow == 1) // we went west, turn down, so we do a small turn
                curRow += directionRow;
            else if (direction == 'W' && directionRow == -1) // we went west, turn up, so we do a big turn
                curRow += directionRow * (2 * width - 1);

            if (direction == 'O') direction = 'W';
            else if (direction == 'W') direction = 'O';
        }
    }
    else // harvest columns
    {
        while (curCol > -width + 1 && curCol <= cols + width - 1)
        {
            while (curRow > 0 && curRow <= rows)
            {
                harvest(curRow, curCol);
                curRow += directionRow;
            }

            if (curRow == 0) curRow = 1;
            else if (curRow == rows + 1) curRow = rows;
            directionRow *= -1;

            if (direction == 'N' && directionCol == -1 && curCol - width + 1 <= 1)
                break;
            if (direction == 'S' && directionCol == -1 && curCol - width + 1 <= 1)
                break;
            if (direction == 'N' && directionCol == 1 && curCol + width - 1 >= cols)
                break;
            if (direction == 'S' && directionCol == 1 && curCol + width - 1 >= cols)
                break;

            if (direction == 'N' && directionCol == 1) // we went north, turn right, big turn
                curCol += directionCol * (2 * width - 1);
            if (direction == 'N' && directionCol == -1) // we went north, turn left, small turn
                curCol += directionCol;
            if (direction == 'S' && directionCol == 1) // we went south, turn right, big turn
                curCol += directionCol;
            if (direction == 'S' && directionCol == -1) // we went south, turn left, small turn
                curCol += directionCol * (2 * width - 1);

            if (direction == 'N') direction = 'S';
            else if (direction == 'S') direction = 'N';
        }
    }
}

// circular harvest
else if (mode == 'Z')
{
    if (direction == 'O' || direction == 'W') // harvest rows
    {
        bool upPos = curRow <= width;
        
        int upEnd = 1, downEnd = rows;
        while (upEnd <= downEnd)
        {
            while (curCol > 0 && curCol <= cols)
            {
                harvest(curRow, curCol);
                curCol += directionCol;
            }
            if (curCol == 0) curCol = 1;
            else if (curCol == cols + 1) curCol = cols;
            directionCol *= -1;

            if (direction == 'O')
            {
                if (upPos) { upEnd += width; curRow = downEnd; }
                else { downEnd -= width; curRow = upEnd + width - 1; }
            }
            else if (direction == 'W')
            {
                if (upPos) { upEnd += width; curRow = downEnd - width + 1; }
                else { downEnd -= width; curRow = upEnd; }
            }

            if (upPos && direction == 'W' && (downEnd - upEnd + 1 < width) && (downEnd - upEnd >= 0)) curRow = upEnd;
            else if (!upPos && direction == 'O' && (downEnd - upEnd + 1 < width) && (downEnd - upEnd >= 0)) curRow = downEnd;

            if (direction == 'O') direction = 'W';
            else if (direction == 'W') direction = 'O';

            upPos = !upPos;
        }
    }
    else // harvest columns
    {
        bool leftPos = curCol < width;

        int leftEnd = 1, rightEnd = cols;
        while (leftEnd <= rightEnd)
        {
            while (curRow > 0 && curRow <= rows)
            {
                harvest(curRow, curCol);
                curRow += directionRow;
            }
            if (curRow == 0) curRow = 1;
            else if (curRow == rows + 1) curRow = rows;
            directionRow *= -1;

            if (direction == 'N')
            {
                if (leftPos) { leftEnd += width; curCol = rightEnd; }
                else { rightEnd -= width; curCol = leftEnd + width - 1; }
            }
            else if (direction == 'S')
            {
                if (leftPos) { leftEnd += width; curCol = rightEnd - width + 1; }
                else { rightEnd -= width; curCol = leftEnd; }
            }

            if (leftPos && direction == 'N' && (rightEnd - leftEnd + 1 < width) && (rightEnd - leftEnd >= 0)) curCol = leftEnd;
            else if (!leftPos && direction == 'S' && (rightEnd - leftEnd + 1 < width) && (rightEnd - leftEnd >= 0)) curCol = rightEnd;

            if (direction == 'N') direction = 'S';
            else if (direction == 'S') direction = 'N';

            leftPos = !leftPos;
        }
    }
}

void harvest(int thisRow, int thisCol)
{
    if (direction == 'O') // that means left is above it all, and we go row++
        for (int i = 0; i < width; i++)
        {
            if (thisRow + i <= rows && thisRow + i > 0 && land[thisRow + i, thisCol] != 0)
            {
                Console.Write(land[thisRow + i, thisCol] + " ");
                land[thisRow + i, thisCol] = 0;
            }
            else Console.Write("0 "); // for level 6
        }

    else if (direction == 'W') // that means left is below it all, and we go row--
        for (int i = 0; i < width; i++)
        {
            if (thisRow - i > 0 && thisRow - i <= rows && land[thisRow - i, thisCol] != 0)
            {
                Console.Write(land[thisRow - i, thisCol] + " ");
                land[thisRow - i, thisCol] = 0;
            }
            else Console.Write("0 "); // for level 6
        }

    else if (direction == 'N') // that means left is to the left based on user perspective, we go col++
        for (int i = 0; i < width; i++)
        {
            if (thisCol + i <= cols && thisCol + i > 0 && land[thisRow, thisCol + i] != 0)
            {
                Console.Write(land[thisRow, thisCol + i] + " ");
                land[thisRow, thisCol + i] = 0;
            }
            else Console.Write("0 "); // for level 6
        }

    else if (direction == 'S') // that means left is to the right based on user perspective, we go col--
        for (int i = 0; i < width; i++)
        {
            if (thisCol - i > 0 && thisCol - i <= cols && land[thisRow, thisCol - i] != 0)
            {
                Console.Write(land[thisRow, thisCol - i] + " ");
                land[thisRow, thisCol - i] = 0;
            }
            else Console.Write("0 "); // for level 6
        }
}