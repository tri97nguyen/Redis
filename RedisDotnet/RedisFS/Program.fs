// For more information see https://aka.ms/fsharp-console-apps
open System
open System.Net
open System.Net.Sockets
open System.Text

let ``connect to server``() = task {
    let ipEndPoint = IPEndPoint(IPAddress.Loopback, 6379)
    use client = new TcpClient()
    do! client.ConnectAsync(ipEndPoint)
    Console.WriteLine("")
    
    use stream = client.GetStream()
    
    let mutable quit = false
    while not quit do
        Console.WriteLine("enter message to send to server. Enter \"q\" to exit")
        match Console.ReadLine() with
        | "q" -> quit <- true
        | message ->
            let buffer = Encoding.UTF8.GetBytes(message)
            do! stream.WriteAsync(buffer)
            Console.WriteLine($"send message {message} to server")
     
    Console.WriteLine($"client disconnecting")
}

``connect to server``() |> Async.AwaitTask |> Async.RunSynchronously