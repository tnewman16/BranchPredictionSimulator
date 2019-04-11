using System;
using System.Collections.Generic;
using System.Linq;
using BranchPredictionSimulator.Model.Memory;
using BranchPredictionSimulator.Model.Prediction;

namespace BranchPredictionSimulator.Model.Program
{
  public class Program<T> where T : IBranchPredictor
  {
    private readonly Random _random;

    private readonly int _numStatements;
    private readonly int _numOperations;
    private readonly int _numBranches;
    private readonly int _numRegisters;
    private readonly int _maxInitialRegisterValue;

    private DateTime _timeSinceRegisterReset;

    private readonly HistoryTable<T> _historyTable;
    private readonly List<Register> _registers = new List<Register>();
    private List<Statement> _statements;
    private int _current;

    public Program(HistoryTable<T> historyTable, int numRegisters = 4, int numStatements = 1000,
      double percentBranches = 0.15, int maxInitialRegisterValue = 50)
    {
      _random = new Random();
      _historyTable = historyTable;

      if (numRegisters < 4)
        numRegisters = 4;

      if (numStatements < 100)
        numStatements = 100;

      if (percentBranches > 1.0)
        percentBranches = 1.0;
      else if (percentBranches < 0.05)
        percentBranches = 0.05;

      _numRegisters = numRegisters;
      _numStatements = numStatements;
      _numBranches = (int) (_numStatements * percentBranches);
      _numOperations = _numStatements - _numBranches;
      _maxInitialRegisterValue = maxInitialRegisterValue;

      for (var i = 0; i < _numRegisters; i++)
        _registers.Add(new Register(i, _random.Next(_maxInitialRegisterValue)));
      Initialize();
    }

    private void Initialize()
    {
      var statements = new List<Statement>();

      var lineOrder = Enumerable.Range(0, _numStatements).OrderBy(x => _random.Next());
      using (var lines = lineOrder.GetEnumerator())
      {
        lines.MoveNext();
        var branchLines = new List<int>();
        for (var i = 0; i < _numBranches; i++)
        {
          branchLines.Add(lines.Current);
          lines.MoveNext();
        }

        var destinationOrder = Enumerable.Range(0, _numStatements)
          .Where(line => !branchLines.Contains(line))
          .OrderBy(x => _random.Next());

        using (var destinations = destinationOrder.GetEnumerator())
        {
          foreach (var line in branchLines)
          {
            var registers = GetThreeRandomRegisters();
            var branch = new Branch(line, destinations.Current, registers[0], registers[1], GetRandomComparison());
            statements.Add(branch);
            destinations.MoveNext();
          }
        }

        for (var i = 0; i < _numOperations; i++)
        {
          var registers = GetThreeRandomRegisters();
          var operation = new Operation(lines.Current, registers[0], registers[1], GetRandomOperator(), registers[2]);
          statements.Add(operation);
          lines.MoveNext();
        }
      }

      _statements = statements.OrderBy(statement => statement.Line).ToList();
    }

    public bool ExecuteNext()
    {
      var statement = _statements[_current];
      var result = statement.Execute();
      if (statement is Branch branch)
      {
        _historyTable.Predict(statement.Line, result);
        if (result)
        {
          _current = branch.Destination;
          _historyTable.HandleTaken(statement.Line);
        }
        else
        {
          _current = _current + 1;
          _historyTable.HandleNotTaken(statement.Line);
        }
      }
      else
        _current++;

      //  if all registers are 0, reset them with random values to avoid infinite loops
      if (_registers.TrueForAll(register => register.Value == 0) ||
          (DateTime.Now - _timeSinceRegisterReset).TotalSeconds > 2)
      {
        ResetRegisters();
//        Console.WriteLine($"RESET: {(DateTime.Now - _timeSinceRegisterReset).TotalSeconds}");
//        Console.WriteLine($"Registers: [{string.Join(", ", _registers)}]");
//        Console.WriteLine($"{_current} - {_statements[_current]}");
      }

//      Console.WriteLine($"Executing: {statement}");
//      Console.WriteLine($"Registers: [{string.Join(", ", _registers)}]");
      return _current < _statements.Count;
    }

    public override string ToString()
    {
      return $"--- BEGIN PROGRAM ---\n{string.Join("\n", _statements)}\n--- END PROGRAM ---\n";
    }

    private void ResetRegisters()
    {
      for (var i = 0; i < _numRegisters; i++)
        _registers[i].Value = _random.Next(_maxInitialRegisterValue);
      _timeSinceRegisterReset = DateTime.Now;
    }

    private List<Register> GetThreeRandomRegisters()
    {
      var registerOrder = Enumerable.Range(0, _registers.Count).OrderBy(x => _random.Next()).ToList();
      return new List<Register>
      {
        _registers[registerOrder[0]],
        _registers[registerOrder[1]],
        _registers[registerOrder[2]]
      };
    }

    private Comparison GetRandomComparison()
    {
      var values = Enum.GetValues(typeof(Comparison));
      return (Comparison) values.GetValue(_random.Next(values.Length));
    }

    private Operator GetRandomOperator()
    {
      var values = Enum.GetValues(typeof(Operator));
      return (Operator) values.GetValue(_random.Next(values.Length));
    }
  }
}
