var banks = ParseInput();

long sumOfVoltages = 0;

foreach (string bank in banks)
{
    long[] batteriesInBank = [.. bank.ToCharArray().Select(x => int.Parse(x.ToString()))];

    if (batteriesInBank.Length > 0)
    {
        int numbersToTake = Math.Min(12, batteriesInBank.Length);
        List<long> selectedNumbers = [];
        long currentIndex = 0;

        for (int i = 0; i < numbersToTake; i++)
        {
            long remainingSlots = numbersToTake - i;
            long maxSearchIndex = batteriesInBank.Length - remainingSlots;

            long maxValue = long.MinValue;
            long maxIndex = currentIndex;
            
            for (long j = currentIndex; j <= maxSearchIndex; j++)
            {
                if (batteriesInBank[j] > maxValue)
                {
                    maxValue = batteriesInBank[j];
                    maxIndex = j;
                }
            }
            
            selectedNumbers.Add(maxValue);
            currentIndex = maxIndex + 1;
        }

        string resultString = string.Join("", selectedNumbers);
        long biggestVoltage = long.Parse(resultString);

        sumOfVoltages += biggestVoltage;
    }
}

Console.WriteLine(sumOfVoltages);

static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(Environment.NewLine);
}