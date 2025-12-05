int[] safeDial = [.. Enumerable.Range(0, 100)];
int currentPosition = 50;

string[] instructions = ParseInput();

int zeroCount = 0;

foreach (string toDo in instructions)
{
    char direction = toDo[0];
    int steps = int.Parse(toDo[1..]);
    if (direction == 'L')
    {
        TurnLeft(steps);
    }
    else if (direction == 'R')
    {
        TurnRight(steps);
    }
}

Console.WriteLine($"Current Position: {currentPosition}, Zero Count: {zeroCount}");

int TurnLeft(int steps)
{
    for (int i = 0; i < steps; i++)
    {
        currentPosition = (currentPosition - 1 + safeDial.Length) % safeDial.Length;
        if (currentPosition == 0)
        {
            zeroCount++;
        }
    }

    return currentPosition;
}

int TurnRight(int steps)
{
    for (int i = 0; i < steps; i++)
    {
        currentPosition = (currentPosition + 1) % safeDial.Length;
        if (currentPosition == 0)
        {
            zeroCount++;
        }
    }

    return currentPosition;
}

string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(Environment.NewLine);
}