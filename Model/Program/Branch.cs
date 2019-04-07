using System;
using BranchPredictionSimulator.Model.Memory;

namespace BranchPredictionSimulator.Model.Program
{
  public class Branch : Statement
  {
    private readonly int _destination;
    private readonly Func<bool> _function;

    public Branch(int line, int destination, Register regInLeft, Register regInRight, Comparison op) : base(line)
    {
      _destination = destination;
      switch (op)
      {
        case Comparison.Equal:
          _function = () => regInLeft.Value == regInRight.Value;
          Str = $"#{line}:  {regInLeft} == {regInRight} ?? {destination} <BRANCH>";
          break;
        case Comparison.NotEqual:
          _function = () => regInLeft.Value != regInRight.Value;
          Str = $"#{line}:  {regInLeft} != {regInRight} ?? {destination} <BRANCH>";
          break;
        case Comparison.GreaterThan:
          _function = () => regInLeft.Value > regInRight.Value;
          Str = $"#{line}:  {regInLeft} > {regInRight} ?? {destination} <BRANCH>";
          break;
        case Comparison.LessThan:
          _function = () => regInLeft.Value < regInRight.Value;
          Str = $"#{line}:  {regInLeft} < {regInRight} ?? {destination} <BRANCH>";
          break;
        case Comparison.GreaterThanOrEqual:
          _function = () => regInLeft.Value >= regInRight.Value;
          Str = $"#{line}:  {regInLeft} >= {regInRight} ?? {destination} <BRANCH>";
          break;
        case Comparison.LessThanOrEqual:
          _function = () => regInLeft.Value <= regInRight.Value;
          Str = $"#{line}:  {regInLeft} <= {regInRight} ?? {destination} <BRANCH>";
          break;
        default:
          _function = () => false;
          break;
      }
    }

    public override int Execute()
    {
      return _function() ? _destination : Line;
    }
  }
}
