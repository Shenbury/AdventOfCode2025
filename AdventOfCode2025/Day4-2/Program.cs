string[] gridStrings = ParseInput();
char paper = '@';
bool canRemovePaper = true;
int totalSumOfPapersOver4 = 0;
while (canRemovePaper)
{
    int height = gridStrings.Length;
    int width = gridStrings[0].Length;
    int sumOfPapersOver4 = 0;
    List<GridCoordinates> gridCoordinatesToRemove = new();

    for (int h = 0; h < height; h++)
    {
        for (int w = 0; w < width; w++)
        {
            if (gridStrings[h][w] == paper)
            {
                int adjacentCount = 0;

                if (h > 0 && gridStrings[h - 1][w] == paper)
                {
                    adjacentCount++;
                }

                if (h < height - 1 && gridStrings[h + 1][w] == paper)
                {
                    adjacentCount++;
                }

                if (w > 0 && gridStrings[h][w - 1] == paper)
                {
                    adjacentCount++;
                }

                if (w < width - 1 && gridStrings[h][w + 1] == paper)
                {
                    adjacentCount++;
                }

                if (h > 0 && w > 0 && gridStrings[h - 1][w - 1] == paper)
                {
                    adjacentCount++;
                }

                if (h < height - 1 && w < width - 1 && gridStrings[h + 1][w + 1] == paper)
                {
                    adjacentCount++;
                }

                if (h < height - 1 && w > 0 && gridStrings[h + 1][w - 1] == paper)
                {
                    adjacentCount++;
                }

                if (h > 0 && w < width - 1 && gridStrings[h - 1][w + 1] == paper)
                {
                    adjacentCount++;
                }

                if (adjacentCount < 4)
                {
                    sumOfPapersOver4 += 1;
                    gridCoordinatesToRemove.Add(new GridCoordinates(h, w));
                    Console.WriteLine($"Paper at ({w}, {h}) has {adjacentCount} adjacent papers.");
                }
            }
        }

        foreach (var coord in gridCoordinatesToRemove)
        {
            gridStrings[coord.Height] = gridStrings[coord.Height].Remove(coord.Width, 1).Insert(coord.Width, ".");
        }
    }

    if (sumOfPapersOver4 == 0)
    {
        canRemovePaper = false;
    }
    else
    {
        totalSumOfPapersOver4 += sumOfPapersOver4;
    }

    Console.WriteLine("Sum of papers with less than 4 adjacent papers: " + sumOfPapersOver4);
}

Console.WriteLine("Total Sum of papers with less than 4 adjacent papers: " + totalSumOfPapersOver4);


static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
}

public record GridCoordinates(int Height, int Width);