namespace MRobots;

public interface IRobotCommand
{
    char Instruction { get; }

    void Execute(Robot robot, MarsGrid grid);
}
