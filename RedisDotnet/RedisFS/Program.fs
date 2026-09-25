// For more information see https://aka.ms/fsharp-console-apps
open System
open System.Net
open System.Net.Sockets
open System.Text

printfn "Hello from F#"
let work = task {
    let! localhost = Dns.GetHostEntryAsync(Dns.GetHostName())
    let ipEndpoint = IPEndPoint(localhost.AddressList[0], 8080)
    use client: Socket = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
    do! client.ConnectAsync(ipEndpoint)
    return client
}

let tcpListener = new TcpListener(IPAddress.Loopback, 5000)
    
    


let rec loop (client: Socket) = task {
    Console.WriteLine("Enter message. Enter \"done\" to exit.")
    let message = Console.ReadLine()
    let byte = Encoding.UTF8.GetBytes(message)
    let! _ = client.SendAsync(byte, SocketFlags.None)
    let buffer = Array.zeroCreate<byte> 1024
    let! received = client.ReceiveAsync(buffer, SocketFlags.None)
    let response = Encoding.UTF8.GetString(buffer, 0, received)
    Console.WriteLine($"received {response}")
    if response = "done" then printfn "exiting..."
    else loop client
}

let main = task {
    let! client = work
    loop client
}

main |> Async.AwaitTask |> Async.RunSynchronously