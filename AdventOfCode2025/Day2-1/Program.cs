string[] instructions = ParseInput();

long sumOfInvalidIds = 0;

foreach (var instruction in instructions)
{
    string[] parts = instruction.Split("-");
    long start = long.Parse(parts[0]);
    long end = long.Parse(parts[1]);

    long[] rangeNumbersToCheck = [.. Enumerable.Range(start, end)];

    foreach (var number in rangeNumbersToCheck)
    {
        long invalidOr0ToSum = FindRecurringCharacterArrayInANumber(number);
        sumOfInvalidIds += invalidOr0ToSum;
    }
}

Console.WriteLine($"Sum of Invalid Ids: {sumOfInvalidIds}");
Console.WriteLine($"Sum of Invalid Ids is Correct To Example: {sumOfInvalidIds == 4174379265}");


long FindRecurringCharacterArrayInANumber(long number) {
    long numberLength = number.ToString().ToCharArray().Length;

    if (numberLength <= 1)
    {
        return 0;
    }

    long numberLengthHalved = numberLength / 2;
    long firstHalf = long.Parse(number.ToString()[..(int)numberLengthHalved]);
    long secondHalf = long.Parse(number.ToString()[(int)numberLengthHalved..]);

    if((firstHalf.ToString().Length + secondHalf.ToString().Length) != numberLength)
    {
        return 0;
    }

    if (firstHalf == secondHalf)
    {
        return number;
    }
    else
    {
        return 0;
    }
}

string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(",");
}

public static class Enumerable 
{
    public static long[] Range(long startNumber, long endNumber)
    {
        long[] result = new long[endNumber - startNumber + 1];
        for (long i = startNumber; i <= endNumber; i++)
        {
            result[i - startNumber] = i;
        }
        return result;
    }
}