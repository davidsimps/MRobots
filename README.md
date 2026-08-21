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

The program reads from standard input until EOF. For manual input, press `Ctrl+Z` then Enter on Windows, or `Ctrl+D` on macOS/Linux.

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

The tests cover whitespace-separated input, inclusive grid boundaries, rotations, movement in every orientation, sequential commands, loss at every boundary, scent behaviour, continuing after an ignored move, stopping after a loss, and the supplied end-to-end example.

## Current architecture

The implementation deliberately remains one application project and one test project. This is proportionate for a two-to-three-hour exercise and keeps the domain easy to inspect:

- `Position` and `Orientation` model coordinates and direction.
- `Robot` holds position, orientation, and lost state.
- `MarsGrid` owns inclusive boundaries and scented positions.
- `LeftCommand`, `RightCommand`, and `ForwardCommand` implement `IRobotCommand`.
- `RobotCommandProcessor` keeps command lookup separate from individual command behaviour.
- `RobotSimulation` handles the input/output flow through `TextReader` and `TextWriter`.
- `Program.cs` only composes these parts with the console streams.

This separates the robot rules from the console entry point, keeps grid and scent behaviour in one responsibility, and makes the complete simulation straightforward to test without claiming the design is future-proof.

Grid coordinates are valid when `0 <= X <= MaxX` and `0 <= Y <= MaxY`.

## Command extensibility

Each command exposes the instruction character it handles and owns its behaviour. The processor builds a dictionary from the registered `IRobotCommand` implementations, so a command that fits the current model, such as `BackwardCommand`, would mainly require another implementation and registration.

Not every future requirement should be forced into this extension point. Supporting 45-degree orientations, for example, would reasonably require changes to the domain model. That complexity is better introduced when a real requirement justifies it.

## LOST robots and scents

`ForwardCommand` calculates the proposed position before moving. A valid position is applied normally. An invalid move from a scented coordinate is ignored and processing continues. Otherwise, the current coordinate is added to the grid's `HashSet<Position>`, the robot is marked `LOST`, and its remaining instructions are skipped. The same grid instance is used for every robot in a simulation, so scents persist.

Scents and simulation state are deliberately held in memory for this challenge. If state needed to survive restarts or be shared between application instances, suitable persistence could be introduced then; a relational database may be reasonable depending on the requirements. A larger codebase could place persistence behind an application-facing abstraction implemented by infrastructure, but adding repository or database abstractions now would solve a requirement that does not exist.

## Assumptions

- The lower-left grid coordinate is always `(0, 0)` and maximum coordinates are non-negative.
- Robot starting positions are inside the grid.
- Orientations and instructions use the uppercase values defined by the challenge.
- Input contains a grid line followed by pairs of robot-state and instruction lines.
- Robots are processed sequentially in input order.

## Technical choices

The solution uses a .NET 8 console application, an xUnit test project, a `record struct` for coordinate equality, and a `HashSet` for efficient scent lookups. These choices keep the implementation small while leaving useful boundaries around command behaviour, simulation input/output, and grid state.

## If this went further

If the application grew substantially, I would progressively separate concerns using Clean Architecture-style boundaries rather than introducing them in advance:

```text
MRobots.Domain
    Robot, Position, Orientation, MarsGrid

MRobots.Application
    RobotCommandProcessor, commands, simulation use cases

MRobots.Infrastructure
    Persistence, messaging, external integrations

MRobots.Api
    HTTP endpoints, request/response models
```

The important constraint would be that core robot rules remain independent of how the application is invoked and where its data is stored. These projects would only be introduced when application size and requirements justified them.

### Web application or API

The existing behaviour could be exposed without moving robot rules into HTTP endpoints:

```text
Web client
    ↓
ASP.NET Core API
    ↓
Application/service layer
    ↓
Existing robot domain logic
```

For example, `POST /api/simulations` could receive structured grid, robot, and instruction data and return the final robot states. Endpoints should remain thin so the same core behaviour remains reusable from the console, an API, a background worker, a message handler, and automated tests.

### Deployment

Depending on usage, a production version could run behind an ASP.NET Core API, worker, or message handler. It could be packaged as a container or deployed as an appropriate managed .NET service or job, with normal CI/CD, structured logging, and monitoring. Durable storage should only be added if persistent or shared state becomes a requirement.

## AI usage

AI tooling was used during requirements discussion, implementation assistance, test-case exploration and code review. I did not use AI as the source of truth for the requirements or as a substitute for executing and validating the program. Engineering decisions were checked against the supplied challenge specification, and behaviour was verified through automated tests and the supplied example.
