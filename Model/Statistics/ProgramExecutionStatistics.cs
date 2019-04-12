using System.Collections.Generic;
using System.Linq;

namespace BranchPredictionSimulator.Model.Statistics
{
  public class ProgramExecutionStatistics
  {
    public int NumStatements { get; set; }
    public int NumBranches { get; set; }
    public int NumRegisters { get; set; }
    public int MaxInitialRegisterValue { get; set; }
    public int StatementsExecuted { get; set; }
    public int BranchesExecuted { get; set; }
    public bool ExecutionHalted { get; set; }
    public List<RegisterReset> RegisterResets { get; set; } = new List<RegisterReset>();
    public List<BranchPrediction> Predictions { get; set; } = new List<BranchPrediction>();

    public double Accuracy
    {
      get { return Predictions.Count(p => p.Actual == p.Prediction) / (double) Predictions.Count; }
    }
  }
}
