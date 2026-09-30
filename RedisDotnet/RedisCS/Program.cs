// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

var tcpServer = new TcpListener(IPAddress.Loopback, 6379);
tcpServer.Start();
var a = await tcpServer.AcceptTcpClientAsync();