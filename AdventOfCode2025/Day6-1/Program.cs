using System.Linq;

string[] gridStrings = ParseInput();
int iterator = 0;

long sumOfOperations = 0;

int maxColumns = gridStrings.Max(x => x.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Count());

while (iterator < maxColumns)
{
    string[] iterationStrings = [.. gridStrings.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray()[iterator])];
    string mathOperator = iterationStrings.Last();

    sumOfOperations += PerformOperation([.. iterationStrings.Where(x => mathOperator != x).Select(long.Parse)], mathOperator);

    iterator++;
}

Console.WriteLine(sumOfOperations);

static long PerformOperation(long[] numbers, string op)
{
    return op switch
    {
        "+" => numbers.Aggregate((a, b) => a + b),
        "-" => numbers.Aggregate((a, b) => a - b),
        "*" => numbers.Aggregate((a, b) => a * b),
        "/" => numbers.Aggregate((a, b) => a / b),
        _ => throw new InvalidOperationException($"Unsupported operation: {op}"),
    };
}

static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(Environment.NewLine);
}