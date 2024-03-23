using System.Text;
using Newtonsoft.Json;
using consumer.DTOs;

namespace consumer.Helpers;

public class ConsumerHelper
{
  public static ConcilliationMessageServiceDTO? GetConcilliationMessage(byte[] body)
  {
    try
    {
      ConcilliationMessageServiceDTO? message = JsonConvert.DeserializeObject<ConcilliationMessageServiceDTO>
        (Encoding.UTF8.GetString(body));

      return message;
    }
    catch
    {
      return null;
    }
  }

  public static ConcilliationOutputDTO? GetConcilliationOutput(string body)
  {
    try
    {
      ConcilliationOutputDTO? output = JsonConvert
        .DeserializeObject<ConcilliationOutputDTO>(body);

      return output;
    }
    catch
    {
      return null;
    }
  }
}