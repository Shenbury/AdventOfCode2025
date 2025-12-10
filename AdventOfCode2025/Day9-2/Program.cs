var seats = ParseInput();

Console.WriteLine("Part 2: " + Part2(seats));

static int Part2(Point2[] seats)
{
    return Solve(seats, Traversed);
}

static int Solve(Point2[] seats, Func<Rectangle, Point2[], bool> filtered)
{
    int maxArea = 0;
    
    for (int i = 0; i < seats.Length - 1; i++)
    {
        for (int j = i + 1; j < seats.Length; j++)
        {
            var a = seats[i];
            var b = seats[j];
            var (minX, minY, maxX, maxY) = MinMax(a, b);
            int area = (1 + maxX - minX) * (1 + maxY - minY);
            var r = new Rectangle(minX, minY, maxX, maxY, area);
            
            if (!filtered(r, seats) && r.Area > maxArea)
            {
                maxArea = r.Area;
            }
        }
    }
    
    return maxArea;
}

static (int minX, int minY, int maxX, int maxY) MinMax(Point2 a, Point2 b)
{
    int minX = Math.Min(a.X, b.X);
    int minY = Math.Min(a.Y, b.Y);
    int maxX = Math.Max(a.X, b.X);
    int maxY = Math.Max(a.Y, b.Y);
    return (minX, minY, maxX, maxY);
}

static bool Traversed(Rectangle r, Point2[] seats)
{
    for (int i = 0; i < seats.Length; i++)
    {
        var (minX, minY, maxX, maxY) = MinMax(seats[i], seats[(i + 1) % seats.Length]);
        
        if (maxY < r.MinY + 1 || minY > r.MaxY - 1 || maxX < r.MinX + 1 || minX > r.MaxX - 1)
        {
            continue;
        }
        return true;
    }
    return false;
}

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

readonly struct Point2(int x, int y) : IEquatable<Point2>
{
    public int X { get; } = x;
    public int Y { get; } = y;
    
    public bool Equals(Point2 other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Point2 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
}

readonly struct Rectangle(int minX, int minY, int maxX, int maxY, int area)
{
    public int MinX { get; } = minX;
    public int MinY { get; } = minY;
    public int MaxX { get; } = maxX;
    public int MaxY { get; } = maxY;
    public int Area { get; } = area;
}