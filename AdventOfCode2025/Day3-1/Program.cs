var banks = ParseInput();

int sumOfVoltages = 0;

foreach (string bank in banks)
{
    int[] batteriesInBank = [.. bank.ToCharArray().Select(x => int.Parse(x.ToString()))];

    if (batteriesInBank.Length > 1)
    {
        int batteryCount = batteriesInBank.Length;
        int maximum = batteriesInBank.Max(x => x);
        int maxValueIndex = Array.FindIndex(batteriesInBank, 0, batteryCount, row => row == maximum) + 1;

        string subString = string.Join("", batteriesInBank)[maxValueIndex..];
        string subStringWithLastEntry = string.Join("", batteriesInBank).Substring(0, maxValueIndex - 1);

        string resultString = string.Empty;

        if (batteriesInBank.Length == 1)
        {
            resultString = maximum.ToString();
        }
        else if (maxValueIndex == batteriesInBank.Length)
        {
            var secondMax = subStringWithLastEntry.ToCharArray().Select(x => int.Parse(x.ToString())).Max(x => x);

            resultString = secondMax.ToString() + maximum.ToString();
        }
        else if (batteriesInBank.Length > 1)
        {
            var secondMax = subString.ToCharArray().Select(x => int.Parse(x.ToString())).Max(x => x);

            resultString = maximum.ToString() + secondMax.ToString();
        }

        int biggestVoltage = int.Parse(resultString);

        sumOfVoltages += biggestVoltage;
    }
}

Console.WriteLine(sumOfVoltages);

static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(Environment.NewLine);
}