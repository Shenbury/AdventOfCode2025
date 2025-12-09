using System.Diagnostics;

string input = File.ReadAllText("input.txt");
List<Point3> nodes = ParsePoint3List(input);

Console.WriteLine("Part 1:");
Stopwatch sw = Stopwatch.StartNew();
string part1Result = SolvePart1(nodes);
sw.Stop();
Console.WriteLine($"Result: {part1Result} in {sw.Elapsed.TotalMilliseconds}ms");

Console.WriteLine("\nPart 2:");
sw.Restart();
string part2Result = SolvePart2(nodes);
sw.Stop();
Console.WriteLine($"Result: {part2Result} in {sw.Elapsed.TotalMilliseconds}ms");


static string SolvePart1(List<Point3> nodes)
{
    GraphEdgeComparer comp = new GraphEdgeComparer(nodes);
    Dictionary<int, NodeGroupInfo> nodeToInfo = new();
    SortedSet<(int, int)> openEdges = new(comp);

    int numNodesToConnect = nodes.Count == 20 ? 10 : 1000;

    BuildOpenEdges(nodes, openEdges);

    long nextUnusedID = 1;
    for (int k = 0; k < numNodesToConnect; ++k)
    {
        (int lowi, int lowj) = openEdges.Min;

        nodeToInfo.TryGetValue(lowi, out NodeGroupInfo? infoI);
        nodeToInfo.TryGetValue(lowj, out NodeGroupInfo? infoJ);

        if (infoI is not null && infoJ is not null)
        {
            if (infoI.ID == infoJ.ID)
            {
            }
            else
            {
                foreach (int idx in nodeToInfo.Keys)
                {
                    if (nodeToInfo.TryGetValue(idx, out NodeGroupInfo? infoOther) && infoOther.ID == infoJ.ID)
                    {
                        nodeToInfo[idx] = infoI;
                    }
                }
                infoI.NumInGroup += infoJ.NumInGroup;
            }
        }
        else if (infoI is not null && infoJ is null)
        {
            nodeToInfo[lowj] = infoI;
            infoI.NumInGroup += 1;
        }
        else if (infoI is null && infoJ is not null)
        {
            nodeToInfo[lowi] = infoJ;
            infoJ.NumInGroup += 1;
        }
        else
        {
            NodeGroupInfo newGroup = new NodeGroupInfo();
            newGroup.ID = nextUnusedID++;
            newGroup.NumInGroup = 2;
            nodeToInfo[lowi] = newGroup;
            nodeToInfo[lowj] = newGroup;
        }

        openEdges.Remove((lowi, lowj));
    }

    SortedSet<NodeGroupInfo> uniqueGroups = new(Comparer<NodeGroupInfo>.Create((x, y) => x.NumInGroup.CompareTo(y.NumInGroup)));
    foreach (NodeGroupInfo nodeGroupInfo in nodeToInfo.Values)
    {
        uniqueGroups.Add(nodeGroupInfo);
    }

    int countedIds = 0;
    long answer = 1;
    while (countedIds < 3 && uniqueGroups.Count > 0)
    {
        NodeGroupInfo? info = uniqueGroups.Max;
        if (info is null) throw new Exception("Can't find max?");

        Console.WriteLine($"Counting group {info.ID} of size {info.NumInGroup}");
        answer *= info.NumInGroup;
        countedIds++;

        uniqueGroups.Remove(info);
    }

    return answer.ToString();
}

static string SolvePart2(List<Point3> nodes)
{
    GraphEdgeComparer comp = new GraphEdgeComparer(nodes);
    Dictionary<int, NodeGroupInfo> nodeToInfo = new();
    HashSet<NodeGroupInfo> uniqueGroups = new();
    SortedSet<(int, int)> openEdges = new(comp);

    BuildOpenEdges(nodes, openEdges);

    long nextUnusedID = 1;
    while (openEdges.Count > 0)
    {
        (int lowi, int lowj) = openEdges.Min;

        nodeToInfo.TryGetValue(lowi, out NodeGroupInfo? infoI);
        nodeToInfo.TryGetValue(lowj, out NodeGroupInfo? infoJ);

        if (infoI is not null && infoJ is not null)
        {
            if (infoI.ID == infoJ.ID)
            {
            }
            else
            {
                foreach (int idx in nodeToInfo.Keys)
                {
                    if (nodeToInfo.TryGetValue(idx, out NodeGroupInfo? infoOther) && infoOther.ID == infoJ.ID)
                    {
                        nodeToInfo[idx] = infoI;
                    }
                }
                uniqueGroups.Remove(infoJ);
                infoI.NumInGroup += infoJ.NumInGroup;
            }
        }
        else if (infoI is not null && infoJ is null)
        {
            nodeToInfo[lowj] = infoI;
            infoI.NumInGroup += 1;
        }
        else if (infoI is null && infoJ is not null)
        {
            nodeToInfo[lowi] = infoJ;
            infoJ.NumInGroup += 1;
        }
        else
        {
            NodeGroupInfo newGroup = new NodeGroupInfo();
            newGroup.ID = nextUnusedID++;
            newGroup.NumInGroup = 2;
            nodeToInfo[lowi] = newGroup;
            nodeToInfo[lowj] = newGroup;
            uniqueGroups.Add(newGroup);
        }

        if (uniqueGroups.Count == 1 && nodeToInfo.Count == nodes.Count)
        {
            Point3 pti = nodes[lowi];
            Point3 ptj = nodes[lowj];

            return (pti.mX * ptj.mX).ToString();
        }

        openEdges.Remove((lowi, lowj));
    }

    throw new Exception("Ran out of edges but didn't connect everything?");
}

static void BuildOpenEdges(List<Point3> nodes, SortedSet<(int, int)> allPossibleEdges)
{
    for (int i = 0; i < nodes.Count; ++i)
    {
        for (int j = i + 1; j < nodes.Count; ++j)
        {
            allPossibleEdges.Add((i, j));
        }
    }
}

static List<Point3> ParsePoint3List(string input)
{
    List<Point3> res = new();
    List<string> lines = GetNonEmptyLines(input);
    foreach (string line in lines)
    {
        res.Add(ParseLineAsPoint3(line));
    }
    return res;
}

static Point3 ParseLineAsPoint3(string line)
{
    string[] numStrs = line.Split(",");
    if (numStrs.Length != 3) throw new Exception($"Invalid vector {line}");
    return new Point3(long.Parse(numStrs[0]), long.Parse(numStrs[1]), long.Parse(numStrs[2]));
}

static List<string> GetNonEmptyLines(string input)
{
    string[] lines = input.Split('\n');
    List<string> result = new();
    foreach (string line in lines)
    {
        string newLine = line.Trim();
        if (newLine.Length != 0)
        {
            result.Add(line.Trim());
        }
    }
    return result;
}

class GraphEdgeComparer : IComparer<(int, int)>
{
    List<Point3> mPts;

    public GraphEdgeComparer(List<Point3> nodes)
    {
        mPts = nodes;
    }

    public int Compare((int, int) x, (int, int) y)
    {
        long xDistSq = mPts[x.Item1].DistSqI(mPts[x.Item2]);
        long yDistSq = mPts[y.Item1].DistSqI(mPts[y.Item2]);
        return xDistSq.CompareTo(yDistSq);
    }
}

class NodeGroupInfo
{
    public long ID { get; set; } = 0;
    public int NumInGroup { get; set; } = 0;
}

struct Point3(long x, long y, long z) : IEquatable<Point3>
{
    public long mX = x;
    public long mY = y;
    public long mZ = z;

    public static Point3 operator -(Point3 left, Point3 right) => 
        new Point3(left.mX - right.mX, left.mY - right.mY, left.mZ - right.mZ);

    public long DistSqI(Point3 other)
    {
        Point3 dt = this - other;
        return dt.mX * dt.mX + dt.mY * dt.mY + dt.mZ * dt.mZ;
    }

    public override string ToString()
    {
        return $"({mX}, {mY}, {mZ})";
    }

    public bool Equals(Point3 other)
    {
        return other.mX == mX && other.mY == mY && other.mZ == mZ;
    }
}