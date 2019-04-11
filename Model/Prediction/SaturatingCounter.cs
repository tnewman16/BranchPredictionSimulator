using System;

namespace BranchPredictionSimulator.Model.Prediction
{
  public class SaturatingCounter : IBranchPredictor
  {
    private readonly int _max;
    private int Position { get; set; }

    public SaturatingCounter(int numBits = 2)
    {
      _max = (int) Math.Pow(2, numBits);
      Position = _max / 2 + 1;
    }

    public void HandleTaken()
    {
      Position = Position >= _max ? Position : Position + 1;
    }

    public void HandleNotTaken()
    {
      Position = Position <= 1 ? Position : Position - 1;
    }

    public bool Predict()
    {
      return Position > _max / 2;
    }

    public override string ToString()
    {
      return Position.ToString();
    }
  }
}
