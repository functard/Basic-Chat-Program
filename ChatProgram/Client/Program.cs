using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Client
{
    class Program
    {
        static string name = "";
        static int port = 9000;
        static IPAddress ip;
        static Socket sck;
        static Thread rec;

        static void Receive()
        {
            while (true)
            {
                Thread.Sleep(500);

                byte[] buffer = new byte[255];
                int rec = sck.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rec);
                Console.WriteLine(Encoding.Default.GetString(buffer));
            }
        }

        static void Main(string[] args)
        {
            rec = new Thread(Receive);
            Console.WriteLine("Enter your name");
            name = Console.ReadLine();
            Console.WriteLine("Enter the ip of the server");
            ip = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Port");
            string inputPort = Console.ReadLine();

            try
            {
                port = Convert.ToInt32(inputPort);
            }
            catch
            {

                port = 9000;
            }

            sck = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            sck.Connect(new IPEndPoint(ip, port));
            rec.Start();

            byte[] conMsg = Encoding.Default.GetBytes("<" + name + ">" + "Connected");
            sck.Send(conMsg, 0, conMsg.Length, 0);

            while(sck.Connected)
            {
                byte[] message = Encoding.Default.GetBytes("<" + name + ">" + Console.ReadLine());
                sck.Send(message, 0, message.Length, 0);
            }
        }
    }
}
