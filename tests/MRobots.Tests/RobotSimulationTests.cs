namespace MRobots.Tests;

public sealed class RobotSimulationTests
{
    [Fact]
    public void Run_SuppliedExample_WritesExpectedOutput()
    {
        const string input = """
            5 3
            1 1 E
            RFRFRFRF
            3 2 N
            FRRFLLFFRRFLL
            0 3 W
            LLFFFLFLFL
            """;
        var expected = string.Join(
            Environment.NewLine,
            "1 1 E",
            "3 3 N LOST",
            "2 3 S") + Environment.NewLine;
        var output = new StringWriter();
        var processor = new RobotCommandProcessor(
            [new LeftCommand(), new RightCommand(), new ForwardCommand()]);
        var simulation = new RobotSimulation(processor);

        simulation.Run(new StringReader(input), output);

        Assert.Equal(expected, output.ToString());
    }
}
