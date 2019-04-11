using System;
using BranchPredictionSimulator.Model.Prediction;
using BranchPredictionSimulator.Model.Program;

namespace BranchPredictionSimulator
{
  internal static class ProgramMain
  {
    private static void Main(string[] args)
    {
      var historyTable = new HistoryTable<SaturatingCounter>(() => new SaturatingCounter());
      var program = new Program<SaturatingCounter>(historyTable);
      while (program.ExecuteNext())
      {
      }

      Console.WriteLine($"Accuracy: {historyTable.GetAccuracy()}");
    }
  }
}
