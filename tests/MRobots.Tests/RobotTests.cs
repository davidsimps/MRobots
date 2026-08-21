namespace MRobots.Tests;

public sealed class RobotTests
{
    [Fact]
    public void Constructor_SetsInitialState()
    {
        var position = new Position(1, 2);

        var robot = new Robot(position, Orientation.East);

        Assert.Equal(position, robot.Position);
        Assert.Equal(Orientation.East, robot.Orientation);
    }
}
