namespace MRobots;

public sealed class RobotCommandProcessor
{
    private readonly IReadOnlyDictionary<char, IRobotCommand> _commands;

    public RobotCommandProcessor(IEnumerable<IRobotCommand> commands)
    {
        _commands = commands.ToDictionary(command => command.Instruction);
    }

    public void Process(Robot robot, MarsGrid grid, string instructions)
    {
        foreach (var instruction in instructions)
        {
            if (robot.IsLost)
            {
                break;
            }

            if (!_commands.TryGetValue(instruction, out var command))
            {
                throw new ArgumentException(
                    $"Unsupported robot instruction '{instruction}'.",
                    nameof(instructions));
            }

            command.Execute(robot, grid);
        }
    }
}
