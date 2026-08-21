namespace MRobots;

public sealed class ForwardCommand : IRobotCommand
{
    public char Instruction => 'F';

    public void Execute(Robot robot, MarsGrid grid)
    {
        var current = robot.Position;
        var proposed = robot.Orientation switch
        {
            Orientation.North => current with { Y = current.Y + 1 },
            Orientation.East => current with { X = current.X + 1 },
            Orientation.South => current with { Y = current.Y - 1 },
            Orientation.West => current with { X = current.X - 1 },
            _ => throw new InvalidOperationException("Unsupported orientation.")
        };

        if (grid.Contains(proposed))
        {
            robot.MoveTo(proposed);
            return;
        }

        if (grid.HasScent(current))
        {
            return;
        }

        grid.RecordScent(current);
        robot.MarkLost();
    }
}
