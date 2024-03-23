using System.Runtime.Serialization;

namespace consumer.DTOs;

public class ConcilliationMessageServiceDTO
{
  public required string Token { get; set; } = null!;
  public required DateTime Date { get; set; }
  public required string PSPfile { get; set; } = null!;
  public required string Postback { get; set; } = null!;
}

public class ConcilliationOutputDTO
{
  [DataMember(Name = "databaseToFile")]
  public ConcilliationFileContent[] DatabaseToFile { get; set; } = null!;

  [DataMember(Name = "fileToDatabase")]
  public ConcilliationFileContent[] FileToDatabase { get; set; } = null!;

  [DataMember(Name = "differentStatus")]
  public ConcilliationPaymentId[] DifferentStatus { get; set; } = null!;
}

public class ConcilliationFileContent
{
  [DataMember(Name = "id")]
  public long Id { get; set; }

  [DataMember(Name = "status")]
  public string Status { get; set; } = null!;
}

public class ConcilliationPaymentId
{
  [DataMember(Name = "id")]
  public long Id { get; set; }
}
