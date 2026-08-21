namespace MRobots;

public sealed class MarsGrid
{
    private readonly HashSet<Position> _scents = [];

    public MarsGrid(int maxX, int maxY)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxX);
        ArgumentOutOfRangeException.ThrowIfNegative(maxY);

        MaxX = maxX;
        MaxY = maxY;
    }

    public int MaxX { get; }

    public int MaxY { get; }

    public bool Contains(Position position) =>
        position.X >= 0 && position.X <= MaxX &&
        position.Y >= 0 && position.Y <= MaxY;

    public bool HasScent(Position position) => _scents.Contains(position);

    public void RecordScent(Position position) => _scents.Add(position);
}
