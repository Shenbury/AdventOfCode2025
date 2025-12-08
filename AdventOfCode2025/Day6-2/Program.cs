string[] gridStrings = ParseInput();

long grandTotal = SolveWorksheet(gridStrings);
Console.WriteLine($"Grand Total: {grandTotal}");

static long SolveWorksheet(string[] lines)
{
    if (lines.Length == 0) return 0;
    
    int maxWidth = lines.Max(line => line.Length);
    
    for (int i = 0; i < lines.Length; i++)
    {
        if (lines[i].Length < maxWidth)
        {
            lines[i] = lines[i].PadRight(maxWidth);
        }
    }
    
    List<(int start, int end)> problemRanges = new List<(int, int)>();
    int? problemStart = null;
    
    for (int col = 0; col < maxWidth; col++)
    {
        bool isEmptyColumn = true;
        for (int row = 0; row < lines.Length; row++)
        {
            if (lines[row][col] != ' ')
            {
                isEmptyColumn = false;
                break;
            }
        }
        
        if (!isEmptyColumn)
        {
            if (problemStart == null)
            {
                problemStart = col;
            }
        }
        else
        {
            if (problemStart != null)
            {
                problemRanges.Add((problemStart.Value, col - 1));
                problemStart = null;
            }
        }
    }
    
    if (problemStart != null)
    {
        problemRanges.Add((problemStart.Value, maxWidth - 1));
    }
    
    long total = 0;
    
    foreach (var (start, end) in problemRanges)
    {
        List<long> numbers = new List<long>();
        char operation = ' ';
        
        for (int col = start; col <= end; col++)
        {
            char opChar = lines[lines.Length - 1][col];
            if (opChar == '+' || opChar == '*')
            {
                operation = opChar;
                break;
            }
        }
        
        for (int col = end; col >= start; col--)
        {
            string numberStr = "";
            for (int row = 0; row < lines.Length - 1; row++)
            {
                if (lines[row][col] != ' ')
                {
                    numberStr += lines[row][col];
                }
            }
            
            if (numberStr.Length > 0)
            {
                numbers.Add(long.Parse(numberStr));
            }
        }
        
        if (numbers.Count > 0 && (operation == '+' || operation == '*'))
        {
            long result = numbers[0];
            for (int i = 1; i < numbers.Count; i++)
            {
                if (operation == '+')
                    result += numbers[i];
                else
                    result *= numbers[i];
            }
            total += result;
        }
    }
    
    return total;
}

static string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    var lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    return lines.Where(l => l.Length > 0).ToArray();
}
