namespace BranchPredictionSimulator.Model.Program
{
  public abstract class Statement
  {
    public readonly int Line;
    protected string Str;

    public Statement(int line)
    {
      Line = line;
    }

    public abstract int Execute();

    public override string ToString()
    {
      return Str;
    }
  }
}
