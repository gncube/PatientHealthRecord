namespace PatientHealthRecord.Core.PatientAggregate;

public record PatientId(Guid Value)
{
  public static PatientId New() => new(Guid.NewGuid());
  public static PatientId From(string value) => new(Guid.Parse(value));
  public static PatientId From(Guid value) => new(value);
  public override string ToString() => Value.ToString();
}
