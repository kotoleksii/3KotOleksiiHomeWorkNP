using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _3KotOleksiiHomeWorkNP
{
    class Program
    {
        static int port = 9999;
        static string localIp = "127.0.0.1";

        static void Main(string[] args)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(localIp), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(iPEndPoint);
                socket.Listen(15);

                Console.WriteLine($"({localIp}:{port}) computer started");

                while (true)
                {
                    Socket remoteSocket = socket.Accept();

                    byte[] buffer = new byte[1024];
                    int bytesCount = 0;
                    string message = String.Empty;

                    do
                    {
                        bytesCount = remoteSocket.Receive(buffer);
                        message += Encoding.UTF8.GetString(buffer, 0, bytesCount);
                    } while (remoteSocket.Available > 0);

                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {message}");

                    string response = $"({localIp}:{port}) computer is turned off";
                    remoteSocket.Send(Encoding.UTF8.GetBytes(response));

                    Console.WriteLine($"\nbye, bye ...");

                    remoteSocket.Shutdown(SocketShutdown.Both);
                    remoteSocket.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}