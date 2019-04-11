using System;
using BranchPredictionSimulator.Model.Memory;

namespace BranchPredictionSimulator.Model.Program
{
  public class Branch : Statement
  {
    public int Destination { get; }
    private readonly Func<bool> _function;

    public Branch(int line, int destination, Register regInLeft, Register regInRight, Comparison op) : base(line)
    {
      Str = $"#{line}";
      Destination = destination;
      switch (op)
      {
        case Comparison.Equal:
          _function = () =>
          {
            Str = $"#{line}:  {regInLeft} == {regInRight} ?? {destination} <BRANCH>";
            return regInLeft.Value == regInRight.Value;
          };
          break;
        case Comparison.NotEqual:
          _function = () =>
          {
            Str = $"#{line}:  {regInLeft} != {regInRight} ?? {destination} <BRANCH>";
            return regInLeft.Value != regInRight.Value;
          };
          break;
        case Comparison.GreaterThan:
          _function = () =>
          {
            Str = $"#{line}:  {regInLeft} > {regInRight} ?? {destination} <BRANCH>";
            return regInLeft.Value > regInRight.Value;
          };
          break;
        case Comparison.LessThan:
          _function = () =>
          {
            Str = $"#{line}:  {regInLeft} < {regInRight} ?? {destination} <BRANCH>";
            return regInLeft.Value < regInRight.Value;
          };
          break;
        case Comparison.GreaterThanOrEqual:
          _function = () =>
          {
            Str = $"#{line}:  {regInLeft} >= {regInRight} ?? {destination} <BRANCH>";
            return regInLeft.Value >= regInRight.Value;
          };
          break;
        case Comparison.LessThanOrEqual:
          _function = () =>
          {
            Str = $"#{line}:  {regInLeft} <= {regInRight} ?? {destination} <BRANCH>";
            return regInLeft.Value <= regInRight.Value;
          };
          break;
        default:
          _function = () => false;
          break;
      }
    }

    public override bool Execute()
    {
      return _function();
    }
  }
}
