using System;
using System.Collections.Generic;

namespace BranchPredictionSimulator.Model.Statistics
{
  public class RegisterReset
  {
    public int StatementsExecutedSinceLastReset { get; set; }
    public double SecondsSinceLastReset { get; set; }
    public List<int> RegisterValuesOnReset { get; set; }
  }
}
