namespace BranchPredictionSimulator.Model.Memory
{
  public class Register
  {
    public int Id { get; }
    public int Value { get; set; }

    public Register(int id, int initialValue = 0)
    {
      Id = id;
      Value = initialValue;
    }

    public override string ToString()
    {
      return $"R{Id}({Value})";
    }
  }
}
