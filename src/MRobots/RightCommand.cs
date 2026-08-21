namespace MRobots;

public sealed class RightCommand : IRobotCommand
{
    public char Instruction => 'R';

    public void Execute(Robot robot, MarsGrid grid)
    {
        var orientation = robot.Orientation switch
        {
            Orientation.North => Orientation.East,
            Orientation.East => Orientation.South,
            Orientation.South => Orientation.West,
            Orientation.West => Orientation.North,
            _ => throw new InvalidOperationException("Unsupported orientation.")
        };

        robot.TurnTo(orientation);
    }
}
