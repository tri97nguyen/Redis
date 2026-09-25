open System.Net
open System.Net.Sockets
open System.Text

let tcpListener = new TcpListener(IPAddress.Loopback, 5000)

printfn "TCP server listening..."
tcpListener.Start()
printfn "TCP server started. Waiting for client connection..."
let client = tcpListener.AcceptTcpClient()
printfn "Client connected."
let ipEndpoint = client.Client.RemoteEndPoint :?> IPEndPoint
printfn $"Client IP: %s{ipEndpoint.Address.ToString()} %d{ipEndpoint.Port}"

let listener(stream: NetworkStream) = task {
    let mutable byteRead: int = 1
    while byteRead <> 0 do
        printfn "waiting for message from client"
        let buffer = Array.zeroCreate<byte> 1024
        let! br = stream.ReadAsync(buffer)
        byteRead <- br
        let message = Encoding.UTF8.GetString(buffer)
        printfn $"received message: %s{message}"
    
    printfn "client disconnected"
    stream.Close()
}

task.GetAwaiter().GetResult() // block
printfn "server app exit"
        