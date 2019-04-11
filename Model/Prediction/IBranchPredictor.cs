namespace BranchPredictionSimulator.Model.Prediction
{
  public interface IBranchPredictor
  {
    void HandleTaken();
    void HandleNotTaken();
    bool Predict();
  }
}
