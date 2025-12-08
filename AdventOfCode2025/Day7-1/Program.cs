string[] gridLines = ParseInput();

int height = gridLines.Count();
int width = gridLines[0].Length;

int indexOfX = gridLines.First().IndexOf('S');

GridCoordinates startPosition = new(0, indexOfX);
Beam initialBeam = new(0, indexOfX);
List<GridCoordinates> totalSplitterLocations = [];

List<Beam> beams = [initialBeam];

for (int rowIndex = 1; rowIndex < gridLines.Length; rowIndex++)
{
    beams = [.. beams.Select(beam => new Beam(rowIndex, beam.Width))
                     .Where(beam => beam.Width >= 0 && beam.Width < width)];
    
    string line = gridLines[rowIndex];
    List<GridCoordinates> splitterLocations = [];

    for (int colIndex = 0; colIndex < line.Length; colIndex++)
    {
        if (line[colIndex] == '^')
        {
            bool beamHitsSplitter = beams.Any(beam => beam.Width == colIndex);
            if (beamHitsSplitter)
            {
                splitterLocations.Add(new GridCoordinates(rowIndex, colIndex));
            }
        }
    }
    
    if (splitterLocations.Count > 0)
    {
        beams = UpdateBeams(splitterLocations, beams);
        totalSplitterLocations.AddRange(splitterLocations);
    }

    Console.WriteLine($"After row {rowIndex}, beam count: {beams.Count}");
}

Console.WriteLine($"Final Count: {beams.Count}");
Console.WriteLine($"Splitter Count: {totalSplitterLocations.Count}");


static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
}


static List<Beam> UpdateBeams(List<GridCoordinates> splitterLocations, List<Beam> currentBeams)
{
    List<Beam> beams = [.. currentBeams];
    
    foreach (GridCoordinates location in splitterLocations)
    {
        beams.RemoveAll(beam => beam.Height == location.Height && beam.Width == location.Width);
        
        beams.Add(new Beam(location.Height, location.Width + 1));
        beams.Add(new Beam(location.Height, location.Width - 1));
    }
    
    return beams;
}

record GridCoordinates(int Height, int Width);

class Beam(int beamHeight, int beamWidth)
{
    public int Height { get; } = beamHeight;
    public int Width { get; } = beamWidth;
}
