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
      switch (op)
      {
        case Operator.Add:
          _function = () => regInLeft.Value + regInRight.Value;
          Str = $"#{line}:  {regOut} = {regInLeft} + {regInRight}";
          break;
        case Operator.Subtract:
          _function = () => regInLeft.Value - regInRight.Value;
          Str = $"#{line}:  {regOut} = {regInLeft} - {regInRight}";
          break;
        case Operator.Multiply:
          _function = () => regInLeft.Value * regInRight.Value;
          Str = $"#{line}:  {regOut} = {regInLeft} * {regInRight}";
          break;
        case Operator.Divide:
          _function = () => regInLeft.Value / regInRight.Value;
          Str = $"#{line}:  {regOut} = {regInLeft} / {regInRight}";
          break;
        default:
          _function = () => regOut.Value;
          break;
      }

      _regOut = regOut;
    }

    public override int Execute()
    {
      _regOut.Value = _function();
      return Line + 1;
    }
  }
}
