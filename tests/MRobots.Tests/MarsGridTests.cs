namespace MRobots.Tests;

public sealed class MarsGridTests
{
    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(5, 3, true)]
    [InlineData(6, 3, false)]
    [InlineData(5, 4, false)]
    [InlineData(-1, 0, false)]
    [InlineData(0, -1, false)]
    public void Contains_ReturnsWhetherPositionIsWithinInclusiveBoundaries(
        int x,
        int y,
        bool expected)
    {
        var grid = new MarsGrid(5, 3);

        var result = grid.Contains(new Position(x, y));

        Assert.Equal(expected, result);
    }
}
