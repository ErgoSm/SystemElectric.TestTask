using Microsoft.AspNetCore.SignalR.Client;
using SystemElectric.TestTask.Domain.Entities;

try
{
    var connection = new HubConnectionBuilder()
    .WithUrl(new Uri("http://127.0.0.1:5000/mainhub"))
    .WithAutomaticReconnect()
    .Build();

    connection.On<Entry>("EntryAdded", (entry) => Console.WriteLine($"{entry.Timestamp}: {entry.Name}"));

    await connection.StartAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.ReadLine();
