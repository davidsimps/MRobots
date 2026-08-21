using MRobots;

var commandProcessor = new RobotCommandProcessor(
    [new LeftCommand(), new RightCommand(), new ForwardCommand()]);
var simulation = new RobotSimulation(commandProcessor);

simulation.Run(Console.In, Console.Out);
