namespace MRobots;

public sealed class Robot
{
    public Robot(Position position, Orientation orientation)
    {
        Position = position;
        Orientation = orientation;
    }

    public Position Position { get; private set; }

    public Orientation Orientation { get; private set; }

    public void MoveTo(Position position) => Position = position;

    public void TurnTo(Orientation orientation) => Orientation = orientation;
}
