using System;
using System.Text;
using System.Net.Sockets;

namespace UdpClientConsole
{
    class Program
    {
        static string remoteAddress; 
        static int remotePort = 8001;
        static string secretKey = "super-sekret-key!";

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите удаленный адрес для подключения: ");
                remoteAddress = Console.ReadLine();

                // порт, к которому мы подключаемся
                //Console.Write("Введите порт для подключения: ");
                //remotePort = Int32.Parse(Console.ReadLine()); 

                Console.WriteLine("===========================================\n");                
                SendMessage(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SendMessage()
        {
            UdpClient sender = new UdpClient();
            try
            {
                while (true)
                {
                    Console.Write("Введите сообщение для отправки: ");
                    string message = Console.ReadLine(); // сообщение для отправки
                    byte[] data = Encoding.Unicode.GetBytes(secretKey + message); // добавляем секретный ключ
                    sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }
    }
}
