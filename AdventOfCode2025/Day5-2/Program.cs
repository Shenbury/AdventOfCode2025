string[] input = ParseInput();
string[] freshFoodRanges = input.First().Split(Environment.NewLine);
string[] foodIds = [.. input.Last().Split(Environment.NewLine).Where(x => !string.IsNullOrEmpty(x))];
List<string> spoiledFoodIds = [];

Enumerable.Range(0, foodIds.Length)
    .Select(i => long.Parse(foodIds[i]))
    .Where(foodId => !freshFoodRanges.Any(range =>
    {
        var parts = range.Split('-');
        long min = long.Parse(parts[0]);
        long max = long.Parse(parts[1]);
        return foodId >= min && foodId <= max;
    }))
    .ToList()
    .ForEach(invalidFoodId => {
        Console.WriteLine($"Spoiled Food ID: {invalidFoodId}");
        spoiledFoodIds.Add(invalidFoodId.ToString());
    });

Console.WriteLine($"Fresh Food IDs: {foodIds.Except(spoiledFoodIds).Count()}");

HashSet<long> spoiledFoodIdSet = [.. spoiledFoodIds.Select(long.Parse)];

List<(long min, long max)> ranges = [.. freshFoodRanges
    .Select(range =>
    {
        var parts = range.Split('-');
        return (min: long.Parse(parts[0]), max: long.Parse(parts[1]));
    })
    .OrderBy(r => r.min)];

List<(long min, long max)> mergedRanges = [];
foreach (var range in ranges)
{
    if (mergedRanges.Count == 0 || mergedRanges[^1].max < range.min - 1)
    {
        mergedRanges.Add(range);
    }
    else
    {
        var last = mergedRanges[^1];
        mergedRanges[^1] = (last.min, Math.Max(last.max, range.max));
    }
}

long totalRangeCount = mergedRanges.Sum(r => r.max - r.min + 1);

long spoiledInRangesCount = spoiledFoodIdSet.Count(spoiledId =>
    mergedRanges.Any(r => spoiledId >= r.min && spoiledId <= r.max));

long freshPossibleCount = totalRangeCount - spoiledInRangesCount;

Console.WriteLine("Count of Possibly Fresh Ids: " + freshPossibleCount);

static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split("\r\n\r\n");
}