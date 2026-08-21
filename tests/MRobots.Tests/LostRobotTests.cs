namespace MRobots.Tests;

public sealed class LostRobotTests
{
    private readonly RobotCommandProcessor _processor = new(
        [new LeftCommand(), new RightCommand(), new ForwardCommand()]);

    [Theory]
    [InlineData(2, 3, Orientation.North)]
    [InlineData(2, 0, Orientation.South)]
    [InlineData(5, 2, Orientation.East)]
    [InlineData(0, 2, Orientation.West)]
    public void Process_ForwardBeyondBoundary_MarksRobotLostAtLastValidPosition(
        int x,
        int y,
        Orientation orientation)
    {
        var grid = new MarsGrid(5, 3);
        var startingPosition = new Position(x, y);
        var robot = new Robot(startingPosition, orientation);

        _processor.Process(robot, grid, "F");

        Assert.True(robot.IsLost);
        Assert.Equal(startingPosition, robot.Position);
    }

    [Fact]
    public void Process_RobotBecomesLost_RecordsScentAtLastValidPosition()
    {
        var grid = new MarsGrid(5, 3);
        var position = new Position(5, 2);
        var robot = new Robot(position, Orientation.East);

        _processor.Process(robot, grid, "F");

        Assert.True(grid.HasScent(position));
    }

    [Fact]
    public void Process_RobotBecomesLost_StopsProcessingRemainingInstructions()
    {
        var grid = new MarsGrid(5, 3);
        var robot = new Robot(new Position(5, 3), Orientation.East);

        _processor.Process(robot, grid, "FLF");

        Assert.True(robot.IsLost);
        Assert.Equal(new Position(5, 3), robot.Position);
        Assert.Equal(Orientation.East, robot.Orientation);
    }

    [Fact]
    public void Process_ForwardBeyondBoundaryFromScentedPosition_ProtectsLaterRobot()
    {
        var grid = new MarsGrid(5, 3);
        var position = new Position(5, 2);
        var firstRobot = new Robot(position, Orientation.East);
        var laterRobot = new Robot(position, Orientation.East);

        _processor.Process(firstRobot, grid, "F");
        _processor.Process(laterRobot, grid, "F");

        Assert.True(firstRobot.IsLost);
        Assert.False(laterRobot.IsLost);
        Assert.Equal(position, laterRobot.Position);
    }

    [Fact]
    public void Process_IgnoredScentedForwardInstruction_ContinuesRemainingInstructions()
    {
        var grid = new MarsGrid(5, 3);
        var position = new Position(5, 2);
        var firstRobot = new Robot(position, Orientation.East);
        var laterRobot = new Robot(position, Orientation.East);

        _processor.Process(firstRobot, grid, "F");
        _processor.Process(laterRobot, grid, "FLF");

        Assert.False(laterRobot.IsLost);
        Assert.Equal(new Position(5, 3), laterRobot.Position);
        Assert.Equal(Orientation.North, laterRobot.Orientation);
    }
}
