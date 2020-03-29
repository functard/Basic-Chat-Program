using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatProgram
{
    class Program
    {
        static Socket sck;
        static Socket acc;
        static int port = 9000;
        static IPAddress ip;
        static Thread rec;
        static string name;

        static string GetIp()
        {
            string hostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(hostName);
            IPAddress[] addresses = ipEntry.AddressList;
            return addresses[addresses.Length - 1].ToString();
        }

        static void Receive()
        {
            while(true)
            {
                Thread.Sleep(500);

                byte[] buffer = new byte[255];
                int rec = acc.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rec);
                Console.WriteLine(Encoding.Default.GetString(buffer));
            }
        }

        static void Main(string[] args)
        {
            rec = new Thread(Receive);
            Console.WriteLine("Your local ip is : " + GetIp());
            Console.WriteLine("Enter your name");
            name = Console.ReadLine();

            Console.WriteLine("Enter your host port");
            string inputPort = Console.ReadLine();

            try
            {
                port = Convert.ToInt32(inputPort);
            }
            catch
            {

                port = 9000;
            }

            ip = IPAddress.Parse(GetIp());
            sck = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            //IPAddress addr = IPAddress.Parse("127.0.0.1");
            sck.Bind(new IPEndPoint(ip, port));
            sck.Listen(0);
            acc = sck.Accept();
            rec.Start();

            while(true)
            {
                byte[] message = Encoding.Default.GetBytes("<" + name + ">" + Console.ReadLine());
                acc.Send(message, 0, message.Length, 0);
            }
        }
    }
}
