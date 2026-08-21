# MRobots

A small .NET 8 console implementation of the Martian Robots programming challenge. It reads a grid and a sequence of robots from standard input, processes the robots in order, and writes each final state to standard output.

## Requirements

- .NET 8 SDK

## Run

From the repository root:

```shell
dotnet restore
dotnet run --project src/MRobots/MRobots.csproj
```

Enter input in the challenge format:

```text
5 3
1 1 E
RFRFRFRF
3 2 N
FRRFLLFFRRFLL
0 3 W
LLFFFLFLFL
```

The example produces:

```text
1 1 E
3 3 N LOST
2 3 S
```

## Tests

Run the xUnit test suite from the repository root:

```shell
dotnet test
```

The tests cover inclusive grid boundaries, rotations, movement in every orientation, sequential commands, loss at every boundary, scent behaviour, command processing after an ignored move, stopping after a loss, and the supplied end-to-end example.

## Approach

- `Position` is a value type containing X and Y coordinates.
- `Orientation` represents north, east, south, and west.
- `Robot` holds the current position, orientation, and lost state.
- `MarsGrid` owns the inclusive boundaries and persistent scented positions.
- `RobotCommandProcessor` resolves instructions and executes them sequentially.
- `RobotSimulation` parses input and formats output through `TextReader` and `TextWriter`, keeping the console entry point small and the complete flow testable.

Grid coordinates are valid when `0 <= X <= MaxX` and `0 <= Y <= MaxY`.

## Command extensibility

Each command implements `IRobotCommand`, exposes the instruction character it handles, and owns its behaviour. The processor builds a dictionary from the supplied commands, so a future command mainly requires a new implementation and registration rather than another branch in a central behaviour switch.

## LOST robots and scents

`ForwardCommand` calculates the proposed position before moving. A valid position is applied normally. An invalid move from a scented coordinate is ignored and processing continues. Otherwise, the current coordinate is added to the grid's `HashSet<Position>`, the robot is marked `LOST`, and its remaining instructions are skipped. The same grid instance is used for every robot in a simulation, so scents persist.

## Assumptions

- The lower-left grid coordinate is always `(0, 0)` and maximum coordinates are non-negative.
- Robot starting positions are inside the grid.
- Orientations and instructions use the uppercase values defined by the challenge.
- Input contains a grid line followed by pairs of robot-state and instruction lines.
- Robots are processed sequentially in input order.

## Technical choices

The solution uses a .NET 8 console application, an xUnit test project, a `record struct` for coordinate equality, and a `HashSet` for efficient scent lookups. The domain remains in the application project because separating a small challenge into additional projects would add structure without improving clarity.

If developed further, the simulation could sit behind an API or message handler, with stronger input validation, structured error reporting, logging, CI, and operational monitoring added at the boundaries. Persistence or distributed processing would only be introduced if new requirements made in-memory sequential processing insufficient.

## AI usage

AI tooling was used during requirements discussion, implementation assistance, test-case exploration and code review. Engineering decisions were checked against the supplied challenge specification, and behaviour was validated through automated tests including the supplied example. AI was not treated as the source of truth for the requirements.
