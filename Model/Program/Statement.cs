namespace BranchPredictionSimulator.Model.Program
{
  public abstract class Statement
  {
    public readonly int Line;
    protected string Str { private get; set; }

    protected Statement(int line)
    {
      Line = line;
    }

    public abstract bool Execute();

    public override string ToString()
    {
      return Str;
    }
  }
}
