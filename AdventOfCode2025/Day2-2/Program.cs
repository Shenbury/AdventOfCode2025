long result = 0;
List<List<long>> ListOfRanges = [];

List<string> Inputlist = [.. ParseInput()];
foreach (string s in Inputlist)
{
    List<string> StartAndEndOfRange = [.. s.Split('-')];
    long start = long.Parse(StartAndEndOfRange[0]);
    long end = long.Parse(StartAndEndOfRange[1]);
    List<long> Range = [];

    for (long i = start; i < end + 1; i++)
    {
        Range.Add(i);
    }
    ListOfRanges.Add(Range);
};

foreach (List<long> Range in ListOfRanges)
{
    foreach (long Id in Range)
    {
        int Increment = 1;
        string IdString = Id.ToString();
        while (Increment <= IdString.Length / 2)
        {
            if (Id.ToString().Length % Increment == 0)
            {
                string Block = Id.ToString()[..Increment];
                string RepeatedBlock = string.Concat(Enumerable.Repeat(Block, IdString.Length / Increment));
                if (RepeatedBlock == IdString)
                {
                    result += Id;
                    break;
                }
            }
            Increment++;
        }
    }
};

Console.WriteLine($"Sum of Invalid Ids: {result}");


static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(",");
}