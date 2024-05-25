using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static List<TcpClient> clients = new List<TcpClient>();
    static Random random = new Random();

    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
        server.Start();
        Console.WriteLine("Сервер запущен. Ожидание подключений...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            clients.Add(client);
            Console.WriteLine("Клиент подключен. Всего клиентов: " + clients.Count);

            if (clients.Count >= 2)
            {
                int numberToGuess = random.Next(1, 10);
                Console.WriteLine("Сервер сгенерировал число: " + numberToGuess);

                BroadcastMessage("Игра началась! Угадайте число от 1 до 9.");

                foreach (TcpClient c in clients)
                {
                    NetworkStream stream = c.GetStream();
                    byte[] data = new byte[256];
                    stream.Read(data, 0, data.Length);
                    int guess = Convert.ToInt32(Encoding.UTF8.GetString(data).Trim());
                    Console.WriteLine("Получено число от клиента: " + guess);

                    int difference = Math.Abs(numberToGuess - guess);
                    string result = (difference == 0) ? "Победитель!" : "Проигрыш...";
                    byte[] resultData = Encoding.UTF8.GetBytes("Результат: " + result);
                    stream.Write(resultData, 0, resultData.Length);
                }
            }
        }
    }

    static void BroadcastMessage(string message)
    {
        foreach (TcpClient c in clients)
        {
            NetworkStream stream = c.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
}
