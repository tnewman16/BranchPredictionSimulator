# BranchPredictionSimulator
A branch prediction simulator created for CSIS 612 at the College of Charleston during the 2019 Spring semester.

## Running
To run the project, you must have the .NET Core CLI installed on your machine.
To install this, go to https://dotnet.microsoft.com/download and download the appropriate SDK.

Once you have the .NET Core CLI installed on your machine, you can run the project by simply navigating to the project's
root directory and running a command similar to the following:

```sh
dotnet run 8 1000 0.1 50 30 10
```

The arguments represent the following (in order):

1. Number of registers present (>= 4)
2. Number of overall statements the program should contain
3. Percentage of those statements that should be branches
4. Maximum value used fo filling a register with a random value (for both initializing a register and register resets)
5. Number of seconds the program will wait until force halting execution
6. Size of chunks in the branch history table (i.e. number of statements per chunk)

## Implementation Details
This branch prediction simulator creates and runs a "program" that consists of various statements, which can either be
operations or branches. The operations perform additions and subtractions against "registers", which each contain a
value and can be set at any time. The branches compare registers to each other and determine whether to move to a
different line in the program or continue to the next one. The program uses a branch history table in order to make
predictions on whether the branches within the program will be "taken" or "not taken" at any given point in time.

#### Registers
* Have an ID associated with them
* Have a value that can be retrieved or replaced at any given time

#### Operations
* Operators can be `+` or `-`
* Operate on two registers
* Record the result into a different register

#### Branches
* Have a destination that represents the program line to branch to if the branch is "taken"
* Operators can be `==`, `!=`, `>`, `<`, `>=`, or `<=`
* Compare between two registers
* Return the result of the comparison

#### 2-Bit Saturating Counter
* Four possible states
* Left-most states (0, 1) return a prediction of "not taken"
* Right-most states (2, 3) return a prediction of "taken"
* Position can be incremented or decremented at any time

#### Branch History Table
* Contains a list of 2-bit saturating counters
* Counters correspond to a specific section, or chunk, of the program
* Counters are used to predict that section's branches

#### Statistics
* Represents various information about the program that has been run
* Serialized to a JSON file at `output/statistics.json` when the program finishes running
* This file looks like the following:
  ```json
  {
      "NumStatements": 1000,
      "NumBranches": 100,
      "NumRegisters": 8,
      "MaxInitialRegisterValue": 50,
      "StatementsExecuted": 1959610,
      "BranchesExecuted": 122518,
      "ExecutionHalted": false,
      "RegisterResets": [
          {
              "StatementsExecutedSinceLastReset": 980862,
              "SecondsSinceLastReset": 2.005543,
              "RegisterValuesOnReset": [
                  1080403472,
                  -1727880304,
                  313647392,
                  1391896016,
                  -311492544,
                  -1822667808,
                  744419184,
                  2136315200
              ]
          }
      ],
      "Predictions": [
          {
              "Line": 0,
              "Actual": false,
              "Prediction": true
          }
      ],
      "Accuracy": 0.99939600711732157
  }
  ```

## Important Information
Due to the fact that the program's contents are randomly generated, it may sometimes produce code that contains infinite
loops.

The program has measures built in to reset registers whenever they contain all `0`s _OR_ when the program has
been executing for longer than 2 seconds. These measures may allow certain programs to escape an infinite loop, but that
is not always the case.

The final measure that is built in to prevent this from happening is halting the program's
execution after a certain period of time has passed (argument #5). Doing this will allow the statistics to be retrieved
(since they are likely still usable) and prevent the program from running into eternity.