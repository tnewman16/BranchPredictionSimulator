using System;
using BranchPredictionSimulator.Model.Memory;

namespace BranchPredictionSimulator.Model.Program
{
  public class Operation : Statement
  {
    private readonly Register _regOut;
    private readonly Func<int> _function;

    public Operation(int line, Register regInLeft, Register regInRight, Operator op, Register regOut) : base(line)
    {
      Str = $"#{line}";
      switch (op)
      {
        case Operator.Add:
          _function = () =>
          {
            Str = $"#{line}:  {regOut} = {regInLeft} + {regInRight}";
            return regInLeft.Value + regInRight.Value;
          };
          break;
        case Operator.Subtract:
          _function = () =>
          {
            Str = $"#{line}:  {regOut} = {regInLeft} - {regInRight}";
            return regInLeft.Value - regInRight.Value;
          };
          break;
      }

      _regOut = regOut;
    }

    public override bool Execute()
    {
      _regOut.Value = _function();
      return false;
    }
  }
}
