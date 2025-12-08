string[] gridLines = ParseInput();

int height = gridLines.Count();
int width = gridLines[0].Length;

int indexOfS = gridLines.First().IndexOf('S');

long timelineCount = SimulateQuantumBeam(gridLines, indexOfS, width);

Console.WriteLine($"Timeline Count: {timelineCount}");


static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
}

static long SimulateQuantumBeam(string[] gridLines, int startColumn, int width)
{
    Dictionary<int, long> columnCounts = new() { [startColumn] = 1 };

    for (int rowIndex = 1; rowIndex < gridLines.Length; rowIndex++)
    {
        string currentRow = gridLines[rowIndex];
        
        var newColumnCounts = columnCounts
            .AsParallel()
            .SelectMany(kvp =>
            {
                int col = kvp.Key;
                long count = kvp.Value;
                
                if (col < 0 || col >= width)
                {
                    return Enumerable.Empty<(int Column, long Count)>();
                }
                
                if (currentRow[col] == '^')
                {
                    return new[]
                    {
                        (Column: col - 1, Count: count),
                        (Column: col + 1, Count: count)
                    };
                }
                else
                {
                    return new[] { (Column: col, Count: count) };
                }
            })
            .GroupBy(x => x.Column)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Count));
        
        columnCounts = newColumnCounts;
        
        Console.WriteLine($"Row {rowIndex}: {columnCounts.Values.Sum()} timelines across {columnCounts.Count} unique positions");
    }

    return columnCounts.Values.Sum();
}

record GridCoordinates(int Height, int Width);
