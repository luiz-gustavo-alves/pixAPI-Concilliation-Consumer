using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using consumer.DTOs;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using consumer.Exceptions;
using consumer.Helpers;

string API_URL = "http://localhost:5000";
string PSP_URL = "http://localhost:5280";
HttpClient httpClient = new();

/* Concilliation Consumer */
ConnectionFactory factory = new()
{
  HostName = "172.17.0.1"
};

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare(
  queue: "concilliation",
  durable: true,
  exclusive: false,
  autoDelete: false,
  arguments: null
);

EventingBasicConsumer consumer = new(channel);
consumer.Received += async (model, ea) =>
{
  var body = ea.Body.ToArray();
  ConcilliationMessageServiceDTO? message = ConsumerHelper.GetConcilliationMessage(body);
  if (message is null)
  {
    channel.BasicReject(ea.DeliveryTag, false);
    return;
  }

  try
  {
    Console.WriteLine($"[*] Concilliation {message.Token} - Sending requests to API...");
    DateTime start = DateTime.Now;

    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", message.Token);
    HttpResponseMessage response = await httpClient
      .PostAsJsonAsync($"{API_URL}/concilliation/check", message);

    var content = await response.Content.ReadAsStringAsync();
    if (content is null)
      throw new InvalidResponseException("Invalid content response");

    ConcilliationOutputDTO? output = ConsumerHelper.GetConcilliationOutput(content);
    if (output is null)
      throw new InvalidResponseException("Invalid content response");

    await httpClient.PostAsJsonAsync($"{PSP_URL}/{message.Postback}", output);
    
    DateTime end = DateTime.Now;
    var timeDiff = (end - start).TotalSeconds;
    Console.WriteLine($"[*] Concilliation {message.Token} - Finished in {timeDiff} seconds!");
    channel.BasicAck(ea.DeliveryTag, false);
  }
  catch
  {
    Console.WriteLine($"[#] Concilliation {message.Token} - Failed to send requests to API...");
    channel.BasicReject(ea.DeliveryTag, false);
    channel.BasicPublish(exchange: "",
      routingKey: "concilliation",
      basicProperties: ea.BasicProperties,
      body: body
    );
  }
};

channel.BasicConsume(
  queue: "concilliation",
  autoAck: false,
  consumer: consumer
);

Console.WriteLine("[*] Waiting for new messages");
Console.ReadLine();