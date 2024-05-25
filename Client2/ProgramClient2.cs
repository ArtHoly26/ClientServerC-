using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        TcpClient client = new TcpClient();
        client.Connect(IPAddress.Parse("127.0.0.1"), 8888);

        NetworkStream stream = client.GetStream();

        while (true)
        {
            byte[] data = Encoding.UTF8.GetBytes(Console.ReadLine());
            stream.Write(data, 0, data.Length);

            byte[] response = new byte[256];
            stream.Read(response, 0, response.Length);
            Console.WriteLine(Encoding.UTF8.GetString(response).Trim());
        }

        stream.Close();
        client.Close();
    }
}