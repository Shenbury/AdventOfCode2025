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


static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split("\r\n\r\n");
}