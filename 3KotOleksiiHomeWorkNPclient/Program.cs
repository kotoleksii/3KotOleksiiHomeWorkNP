using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _3KotOleksiiHomeWorkNPclient
{
    class Program
    {
        static int port = 9999;
        static string ip = "127.0.0.1";

        static void Main(string[] args)
        {
            try
            {
                IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(remoteEndpoint);
               
                Console.Write("Write something to turn off neighboring computers: ");
                string message = Console.ReadLine();

                socket.Send(Encoding.UTF8.GetBytes(message));

                byte[] buffer = new byte[1024];
                int bytesCount = 0;
                string response = String.Empty;

                do
                {
                    bytesCount = socket.Receive(buffer);
                    response += Encoding.UTF8.GetString(buffer, 0, bytesCount);
                } while (socket.Available > 0);

                Console.WriteLine($"{response}");

                if (response != null)
                {
                    Process p = new Process();
                    var startInfo = new ProcessStartInfo();
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/c shutdown /s /t 0";
                    p.StartInfo = startInfo;
                    p.Start();
                }

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}