namespace MRobots;

public sealed class LeftCommand : IRobotCommand
{
    public char Instruction => 'L';

    public void Execute(Robot robot, MarsGrid grid)
    {
        var orientation = robot.Orientation switch
        {
            Orientation.North => Orientation.West,
            Orientation.West => Orientation.South,
            Orientation.South => Orientation.East,
            Orientation.East => Orientation.North,
            _ => throw new InvalidOperationException("Unsupported orientation.")
        };

        robot.TurnTo(orientation);
    }
}
