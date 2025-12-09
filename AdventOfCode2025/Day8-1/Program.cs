List<ThreeDGridCoordinates> junctionBox3dCoords = ParseInput();
List<AllPairsWithDistance> allPairs = GetAllCoordinatePairsAndDistance(junctionBox3dCoords);
List<AllPairsWithDistance> oderedByDistance = [.. allPairs.OrderBy(x => x.distance)];
List<Circuit> allCircuits = [];

int currentLimit = 0;
int pairingLimit = 1000;

foreach (AllPairsWithDistance closePair in oderedByDistance)
{

    if (currentLimit == pairingLimit)
    {
        break;
    }

    var coord1 = closePair.allPairs[0];
    var coord2 = closePair.allPairs[1];
    
    var circuitWithCoord1 = allCircuits.FirstOrDefault(c => c.JunctionCoordinates.Contains(coord1));
    var circuitWithCoord2 = allCircuits.FirstOrDefault(c => c.JunctionCoordinates.Contains(coord2));

    if (circuitWithCoord1 != null && circuitWithCoord2 != null && circuitWithCoord1.Id != circuitWithCoord2.Id)
    {
        circuitWithCoord1.JunctionCoordinates.AddRange(circuitWithCoord2.JunctionCoordinates);
        circuitWithCoord1.Size = circuitWithCoord1.JunctionCoordinates.Count;
        allCircuits.Remove(circuitWithCoord2);
    }
    else if (circuitWithCoord1 != null && circuitWithCoord2 != null && circuitWithCoord1.Id == circuitWithCoord2.Id)
    {

    }
    else if (circuitWithCoord1 != null && circuitWithCoord2 == null)
    {
        circuitWithCoord1.JunctionCoordinates.Add(coord2);
        circuitWithCoord1.Size = circuitWithCoord1.JunctionCoordinates.Count;
    }
    else if (circuitWithCoord2 != null && circuitWithCoord1 == null)
    {
        circuitWithCoord2.JunctionCoordinates.Add(coord1);
        circuitWithCoord2.Size = circuitWithCoord2.JunctionCoordinates.Count;
    }
    else
    {
        allCircuits.Add(new Circuit
        {
            Size = 2,
            JunctionCoordinates = [.. closePair.allPairs]
        });
    }

    currentLimit++;
}

List<Circuit> circuitsBySize = [.. allCircuits.OrderByDescending(c => c.Size)];

foreach (Circuit circuit in circuitsBySize)
{
    Console.WriteLine($"Circuit Size: {circuit.Size} - Junctions: {circuit.JunctionCoordinates.Count}");
}

int result = circuitsBySize.Take(3).Aggregate(1, (acc, circuit) => acc * circuit.Size);
Console.WriteLine($"Result: {result}");

static List<AllPairsWithDistance> GetAllCoordinatePairsAndDistance(List<ThreeDGridCoordinates> coords)
{
    var pairs = new List<AllPairsWithDistance>();
    for (int i = 0; i < coords.Count; i++)
    {
        for (int j = i + 1; j < coords.Count; j++)
        {
            pairs.Add(new AllPairsWithDistance([coords[i], coords[j]], GetCoordinateDistanceSquared(coords[i], coords[j])));
        }
    }
    return pairs;
}

static List<ThreeDGridCoordinates> ParseInput()
{ 
    string input = File.ReadAllText("input.txt");
    var coords = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    List<ThreeDGridCoordinates> threeDCoords = [.. coords.Select(x => 
    {
        var xyz = x.Split(",");

        return new ThreeDGridCoordinates(
            int.Parse(xyz[0]),
            int.Parse(xyz[1]),
            int.Parse(xyz[2])
        );
    })];
    
    return threeDCoords;
}

static long GetCoordinateDistanceSquared(ThreeDGridCoordinates coord1, ThreeDGridCoordinates coord2)
{
    long deltaX = coord1.X - coord2.X;
    long deltaY = coord1.Y - coord2.Y;
    long deltaZ = coord1.Z - coord2.Z;

    return deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;
}

record AllPairsWithDistance(ThreeDGridCoordinates[] allPairs, long distance);
record ThreeDGridCoordinates(int X, int Y, int Z);
class Circuit
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Size { get; set; } = 0;
    public List<ThreeDGridCoordinates> JunctionCoordinates { get; set; } = [];
}