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

    if (currentPosition == 0)
    {
        zeroCount++;
    }
}

Console.WriteLine($"Current Position: {currentPosition}, Zero Count: {zeroCount}");

int TurnLeft(int steps)
{
    currentPosition = (currentPosition - steps + safeDial.Length) % safeDial.Length;
    return currentPosition;
}

int TurnRight(int steps)
{
    currentPosition = (currentPosition + steps) % safeDial.Length;
    return currentPosition;
}

string[] ParseInput()
{
    string input = File.ReadAllText("input.txt");
    return input.Split(Environment.NewLine);
}