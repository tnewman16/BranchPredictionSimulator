using System;
using System.Collections.Generic;

namespace BranchPredictionSimulator.Model.Prediction
{
  public class HistoryTable<T> where T : IBranchPredictor
  {
    private readonly List<T> _counters;
    private readonly int _numLines;
    private readonly int _chunkSize;

    private int _numPredictions;
    private int _numCorrect;

    public HistoryTable(Func<T> predictorCreator, int numLines = 1000, int chunkSize = 10)
    {
      _chunkSize = chunkSize;
      _numLines = numLines;
      _counters = new List<T>();
      for (var i = 0; i < (_numLines + _chunkSize - 1) / _chunkSize; i++)
        _counters.Add(predictorCreator());
    }

    public bool Predict(int lineNumber, bool actual)
    {
      _numPredictions++;
      var prediction = GetPredictor(lineNumber).Predict();
      if (prediction == actual)
        _numCorrect++;
      return prediction;
    }

    public void HandleTaken(int lineNumber)
    {
      GetPredictor(lineNumber).HandleTaken();
    }

    public void HandleNotTaken(int lineNumber)
    {
      GetPredictor(lineNumber).HandleNotTaken();
    }

    public double GetAccuracy()
    {
      return _numCorrect / (double) (_numPredictions == 0 ? 1 : _numPredictions);
    }

    private T GetPredictor(int lineNumber)
    {
      return _counters[lineNumber / _chunkSize];
    }
  }
}
