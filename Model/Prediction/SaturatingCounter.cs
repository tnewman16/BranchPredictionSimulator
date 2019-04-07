using System;

namespace BranchPredictionSimulator.Model.Prediction
{
  public class SaturatingCounter
  {
    private readonly int _max;
    public int Position { get; private set; }

    public SaturatingCounter(int numBits)
    {
      _max = (int) Math.Pow(2, numBits);
      Position = _max / 2 + 1;
    }

    public void Increment()
    {
      Position = Position >= _max ? Position : Position + 1;
    }

    public void Decrement()
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
