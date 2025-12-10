var redTileCoords = ParseInput();

long maxArea = 0;
Point2 coord1 = default;
Point2 coord2 = default;

for (int i = 0; i < redTileCoords.Length; i++)
{
    for (int j = i + 1; j < redTileCoords.Length; j++)
    {
        long width = Math.Abs(redTileCoords[j].X - redTileCoords[i].X) + 1;
        long height = Math.Abs(redTileCoords[j].Y - redTileCoords[i].Y) + 1;
        long area = width * height;
        
        if (area > maxArea)
        {
            maxArea = area;
            coord1 = redTileCoords[i];
            coord2 = redTileCoords[j];
        }
    }
}

Console.WriteLine($"Largest area: {maxArea}");
Console.WriteLine($"Largest area matches example of 50: {maxArea == 50}");
Console.WriteLine($"Coordinates: ({coord1.X},{coord1.Y}) and ({coord2.X},{coord2.Y})");

static Point2[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    var points = new Point2[lines.Length];
    for (int i = 0; i < lines.Length; i++)
    {
        var parts = lines[i].Split(',');
        points[i] = new Point2(int.Parse(parts[0]), int.Parse(parts[1]));
    }
    return points;
}

readonly struct Point2(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}