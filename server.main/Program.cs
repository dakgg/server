using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

StartServer();

static void StartServer()
{
    // Create a TCP/IP socket
    Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, 8888);

    try
    {
        // Bind the socket to the local endpoint and listen for incoming connections
        listener.Bind(ipLocal);
        listener.Listen(10);

        Console.WriteLine("Waiting for a connection...");

        // Accept incoming connections
        Socket handler = listener.Accept();

        string data = null;

        while (true)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            if (data.IndexOf("<EOF>") > -1)
            {
                break;
            }
        }

        Console.WriteLine("Text received : {0}", data);

        byte[] msg = Encoding.ASCII.GetBytes("Server received your message.");
        handler.Send(msg);

        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
    }
}