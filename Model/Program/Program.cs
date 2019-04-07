using System;
using System.Collections.Generic;
using System.Linq;
using BranchPredictionSimulator.Model.Memory;

namespace BranchPredictionSimulator.Model.Program
{
  public class Program
  {
    private readonly List<Register> _registers = new List<Register>();
    private List<Statement> _statements;
    private readonly int _numStatements;
    private readonly int _numOperations;
    private readonly int _numBranches;
    private readonly Random _random;

    public Program(int numRegisters = 4, int numStatements = 1000, double percentBranches = 0.15)
    {
      if (numRegisters < 4)
        numRegisters = 4;

      if (numStatements < 100)
        numStatements = 100;

      if (percentBranches > 1.0)
        percentBranches = 1.0;
      else if (percentBranches < 0.05)
        percentBranches = 0.05;

      for (var i = 0; i < numRegisters; i++)
        _registers.Add(new Register(i));

      _random = new Random();
      _numStatements = numStatements;
      _numBranches = (int) (_numStatements * percentBranches);
      _numOperations = _numStatements - _numBranches;

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

    public override string ToString()
    {
      return $"--- BEGIN PROGRAM ---\n{string.Join("\n", _statements)}\n--- END PROGRAM ---\n";
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
