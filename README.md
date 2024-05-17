# Pix API Concilliation Consumer

Consumer aplication to estabilish communication with PSPs and process difference between PSP payment log file to API data records.
- Process concilliation from API and send Concilliation Output to PSP.

## How to Install and Run the Project
Clone this repository: `git clone https://github.com/luiz-gustavo-alves/pixAPI-Concilliation-Consumer.git`
<br>
Access root folder and run consumer environment:
```bash
dotnet run
```

## Message DTO
```c#
public class ConcilliationMessageServiceDTO
{
  public required string Token { get; set; } = null!;
  public required DateTime Date { get; set; }
  public required string PSPfile { get; set; } = null!;
  public required string Postback { get; set; } = null!;
}

public class ConcilliationOutputDTO
{
  public ConcilliationFileContent[] DatabaseToFile { get; set; } = null!;
  public ConcilliationFileContent[] FileToDatabase { get; set; } = null!;
  public ConcilliationPaymentId[] DifferentStatus { get; set; } = null!;
}
```

## Error Handler

API or/and PSP unavaible:
  - Reject and publish message to queue.

## Links

| Description | URL |
| --- | --- |
| Pix API | https://github.com/luiz-gustavo-alves/pixAPI
| PSP Mock | https://github.com/luiz-gustavo-alves/pixAPI-PSP-Mock
| Payment Consumer | https://github.com/luiz-gustavo-alves/pixAPI-Payments-Consumer

