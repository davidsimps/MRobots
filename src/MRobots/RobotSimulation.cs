namespace MRobots;

public sealed class RobotSimulation
{
    private readonly RobotCommandProcessor _commandProcessor;

    public RobotSimulation(RobotCommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public void Run(TextReader input, TextWriter output)
    {
        var gridValues = SplitValues(ReadRequiredLine(input, "grid size"), 2);
        var grid = new MarsGrid(int.Parse(gridValues[0]), int.Parse(gridValues[1]));

        string? robotLine;
        while ((robotLine = input.ReadLine()) is not null)
        {
            var robotValues = SplitValues(robotLine, 3);
            var robot = new Robot(
                new Position(int.Parse(robotValues[0]), int.Parse(robotValues[1])),
                ParseOrientation(robotValues[2]));
            var instructions = ReadRequiredLine(input, "robot instructions");

            _commandProcessor.Process(robot, grid, instructions);

            output.WriteLine(FormatRobot(robot));
        }
    }

    private static string ReadRequiredLine(TextReader input, string description) =>
        input.ReadLine() ?? throw new FormatException($"Missing {description}.");

    private static string[] SplitValues(string line, int expectedCount)
    {
        var values = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        return values.Length == expectedCount
            ? values
            : throw new FormatException($"Expected {expectedCount} values in '{line}'.");
    }

    private static Orientation ParseOrientation(string value) => value switch
    {
        "N" => Orientation.North,
        "E" => Orientation.East,
        "S" => Orientation.South,
        "W" => Orientation.West,
        _ => throw new FormatException($"Unsupported orientation '{value}'.")
    };

    private static string FormatRobot(Robot robot)
    {
        var orientation = robot.Orientation switch
        {
            Orientation.North => "N",
            Orientation.East => "E",
            Orientation.South => "S",
            Orientation.West => "W",
            _ => throw new InvalidOperationException("Unsupported orientation.")
        };
        var lost = robot.IsLost ? " LOST" : string.Empty;

        return $"{robot.Position.X} {robot.Position.Y} {orientation}{lost}";
    }
}
