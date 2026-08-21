# MRobots

A .NET implementation of the Martian Robots programming challenge.

## Problem Understanding

Mars is represented as a rectangular grid.

- The lower-left coordinate is always `(0, 0)`.
- The first input line defines the maximum X and Y coordinates.
- Each robot has a starting X coordinate, Y coordinate and orientation.
- Robots are processed sequentially.
- Supported instructions are:
  - `L` turns the robot 90 degrees left.
  - `R` turns the robot 90 degrees right.
  - `F` moves the robot forward one grid position.
- A robot that moves beyond the grid boundary is lost.
- A lost robot leaves a scent at its last valid position.
- If a later robot attempts to leave the grid from a scented position, that forward instruction is ignored.
- Once a robot is lost, no further instructions are processed for that robot.

## Assumptions

- Grid boundaries are inclusive. For a grid defined as `5 3`, valid coordinates are `0 <= X <= 5` and `0 <= Y <= 3`.
- Robot starting positions will be within the grid.
- Orientations will be one of `N`, `E`, `S` or `W`.
- Instructions will contain supported command characters.
- A scent belongs to the last valid coordinate occupied by a lost robot.
- Scents remain available while subsequent robots are processed.

## Proposed Design

The solution will remain small and focused on the domain problem.

The main responsibilities are expected to be:

- `Robot` holds the robot's current position, orientation and lost state.
- `MarsGrid` owns the grid boundaries and scented positions.
- `IRobotCommand` defines the contract for robot commands.
- `LeftCommand`, `RightCommand` and `ForwardCommand` implement the current commands.
- `RobotCommandProcessor` processes an instruction sequence in order.

Command behaviour will be separated behind `IRobotCommand` so that additional command types can be introduced without expanding a central switch statement.

## Movement Rules

Forward movement depends on the robot's current orientation:

| Orientation | Movement |
| --- | --- |
| North | `Y + 1` |
| South | `Y - 1` |
| East | `X + 1` |
| West | `X - 1` |

Left and right instructions change orientation without changing position.

## Lost Robot Behaviour

Before applying a forward movement:

1. Calculate the proposed position.
2. Check whether it is within the grid.
3. If it is valid, move the robot.
4. If it is outside the grid and the current position has a scent, ignore the instruction.
5. Otherwise, mark the robot as lost and record a scent at its current position.

## Testing Approach

Unit tests will cover the core behaviour, including:

- Grid boundary validation.
- Left and right rotations.
- Forward movement in each orientation.
- Multiple instructions being processed in sequence.
- Robots becoming lost at each grid boundary.
- Recording a scent at the last valid position.
- A later robot being protected by an existing scent.
- Remaining instructions not executing after a robot is lost.
- The supplied sample input producing the expected output.

## Planned Implementation

1. Create the .NET application and unit test projects.
2. Implement the grid and robot domain models.
3. Implement extensible command handling.
4. Implement lost robot and scent behaviour.
5. Add input parsing and output formatting.
6. Verify the supplied example and edge cases.
7. Complete the README with running instructions, final technical decisions and possible production evolution.

## AI Usage

AI tooling has been used during requirements analysis and design discussion to help explore the brief and challenge assumptions.

At this planning stage, implementation code has not yet been produced.

This section will be updated as development progresses to describe where AI was used and where implementation or engineering decisions were made and reviewed independently.
