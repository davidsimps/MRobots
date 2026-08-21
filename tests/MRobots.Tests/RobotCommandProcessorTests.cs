namespace MRobots.Tests;

public sealed class RobotCommandProcessorTests
{
    private readonly MarsGrid _grid = new(5, 5);
    private readonly RobotCommandProcessor _processor = new(
        [new LeftCommand(), new RightCommand(), new ForwardCommand()]);

    [Theory]
    [InlineData(Orientation.North, Orientation.West)]
    [InlineData(Orientation.West, Orientation.South)]
    [InlineData(Orientation.South, Orientation.East)]
    [InlineData(Orientation.East, Orientation.North)]
    public void Process_LeftInstruction_RotatesLeft(
        Orientation initial,
        Orientation expected)
    {
        var robot = new Robot(new Position(2, 2), initial);

        _processor.Process(robot, _grid, "L");

        Assert.Equal(expected, robot.Orientation);
        Assert.Equal(new Position(2, 2), robot.Position);
    }

    [Theory]
    [InlineData(Orientation.North, Orientation.East)]
    [InlineData(Orientation.East, Orientation.South)]
    [InlineData(Orientation.South, Orientation.West)]
    [InlineData(Orientation.West, Orientation.North)]
    public void Process_RightInstruction_RotatesRight(
        Orientation initial,
        Orientation expected)
    {
        var robot = new Robot(new Position(2, 2), initial);

        _processor.Process(robot, _grid, "R");

        Assert.Equal(expected, robot.Orientation);
        Assert.Equal(new Position(2, 2), robot.Position);
    }

    [Theory]
    [InlineData(Orientation.North, 2, 3)]
    [InlineData(Orientation.East, 3, 2)]
    [InlineData(Orientation.South, 2, 1)]
    [InlineData(Orientation.West, 1, 2)]
    public void Process_ForwardInstruction_MovesInFacingDirection(
        Orientation orientation,
        int expectedX,
        int expectedY)
    {
        var robot = new Robot(new Position(2, 2), orientation);

        _processor.Process(robot, _grid, "F");

        Assert.Equal(new Position(expectedX, expectedY), robot.Position);
        Assert.Equal(orientation, robot.Orientation);
    }

    [Fact]
    public void Process_MultipleInstructions_ExecutesSequentially()
    {
        var robot = new Robot(new Position(1, 1), Orientation.North);

        _processor.Process(robot, _grid, "RFFLFF");

        Assert.Equal(new Position(3, 3), robot.Position);
        Assert.Equal(Orientation.North, robot.Orientation);
    }
}
