using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;

var ipEndPoint = new IPEndPoint(IPAddress.Any, 6379);
var listener = new TcpListener(ipEndPoint);
listener.Start();
Console.WriteLine("TCP Server is listening on port 6379");

while (await listener.AcceptTcpClientAsync() is { Client.RemoteEndPoint: IPEndPoint } client)
{
    _ = Task.Run(async () => await HandleClient(client));
}


Console.WriteLine("TCP Server is shutting down");
listener.Stop();

return;
static async Task HandleClient(TcpClient client)
{
    var clientAddress = ((IPEndPoint)client.Client.RemoteEndPoint!).Address;
    Console.WriteLine($"established connection with client {clientAddress}");
    
    await using var stream = client.GetStream();

    var buffer = ArrayPool<byte>.Shared.Rent(32);
    while (await stream.ReadAsync(buffer) is var byteRead and > 0) {
        var message = Encoding.UTF8.GetString(buffer, 0, byteRead);
        Console.WriteLine($"{DateTime.Now} Client Sent message at: \"{message}\"");
    }
    Console.WriteLine($"client {clientAddress} gracefully disconnected");
    client.Close();
    ArrayPool<byte>.Shared.Return(buffer);
}
