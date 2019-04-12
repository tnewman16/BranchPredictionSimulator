using System;
using System.IO;
using BranchPredictionSimulator.Model.Prediction;
using BranchPredictionSimulator.Model.Program;
using Newtonsoft.Json;

namespace BranchPredictionSimulator
{
  internal static class ProgramMain
  {
    private static void Main(string[] args)
    {
      var numRegisters = int.Parse(args[0]);
      var numStatements = int.Parse(args[1]);
      var percentBranches = double.Parse(args[2]);
      var maxInitialRegisterValue = int.Parse(args[3]);
      var secondsUntilForceQuit = int.Parse(args[4]);
      var chunkSize = int.Parse(args[5]);

      var historyTable = new HistoryTable<SaturatingCounter>(() => new SaturatingCounter(), numStatements, chunkSize);
      var program = new Program<SaturatingCounter>(historyTable, numRegisters, numStatements, percentBranches,
        maxInitialRegisterValue, secondsUntilForceQuit);
      while (program.ExecuteNext())
      {
      }

      Console.WriteLine($"Statements Executed: {program.Statistics.StatementsExecuted}");
      Console.WriteLine($"Branches Executed: {program.Statistics.BranchesExecuted}");
      Console.WriteLine($"Accuracy: {program.Statistics.Accuracy}");
      Console.Write("Writing statistics to \"output/statistics.json\"...");
      var json = JsonConvert.SerializeObject(program.Statistics);
      File.WriteAllText("output/statistics.json", json);
      Console.WriteLine(" done!");
    }
  }
}
