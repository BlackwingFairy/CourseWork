using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Console_Client
{
    class Program
    {

        private const int PORT = 11000;
        private const int SIZE = 2048;
        //Строка и массив байт для получения данных от сервера 
        public static string dataReceive = null;
        public static byte[] bytesReceive = new byte[SIZE];
        //Для каждой асинхронной операции создаем объект класса ManualResetEvent.  
        public static ManualResetEvent ConnectDone = new ManualResetEvent(false);
        public static ManualResetEvent SendDone = new ManualResetEvent(false);
        public static ManualResetEvent ReceiveDone = new ManualResetEvent(false);


        static void Main(string[] args)
        {
            try
            {

                Thread thr = Thread.CurrentThread;
                int idThread = thr.ManagedThreadId;
                //Console.WriteLine("Метод Main клиента выполняется в потоке:" + idThread);
                IPHostEntry ipHost = Dns.Resolve("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint endpoint = new IPEndPoint(ipAddr, PORT);
                Socket sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                string dataSend = "";

                sClient.BeginConnect(endpoint, new AsyncCallback(ConnectCallback), sClient);
                ConnectDone.WaitOne();
                Console.WriteLine("\nВыберите действие: \n 1 - выборка по группе \n 2 - выборка по предмету \n 3 - выборка по фамилии\n");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Введите номер группы: ");
                        string number = Console.ReadLine();
                        dataSend = choice + number;
                        break;
                    case "2":
                        Console.WriteLine("Введите название предмета: ");
                        string subject = Console.ReadLine();
                        dataSend = choice + subject;
                        break;
                    case "3":
                        Console.WriteLine("Введите фамилию: ");
                        string surname = Console.ReadLine();
                        dataSend = choice + surname;
                        break;
                    default:
                        break;
                }
                

                
                byte[] bytesSend = Encoding.Unicode.GetBytes(dataSend + ".");

                sClient.BeginSend(bytesSend, 0, bytesSend.Length, 0, new AsyncCallback(SendCallback), sClient);

                SendDone.WaitOne();
                sClient.BeginReceive(bytesReceive, 0, bytesReceive.Length, 0, new AsyncCallback(ReceiveCallback), sClient);

                ReceiveDone.WaitOne();
                Console.WriteLine("\nПолучено от сервера: " + dataReceive);
                sClient.Shutdown(SocketShutdown.Both);
                sClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException");
            }
            finally
            {
                Console.ReadKey();
            }
        }

        public static void ConnectCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;   //Получаем текущий поток          
            int idThread = thr.ManagedThreadId;  //Получаем идентификатор потока    

            //Console.WriteLine("\nМетод ConnectCallback клиента выполняется в потоке:" + idThread);
            //Используем свойство  AsyncState интерфейса IAsyncResult для извлечения          
            //аргумента, который был передан в третьем параметре метода BeginConnect()          
            //Полученное значение явно приводим к типу Socket          
            Socket sClient = (Socket)ar.AsyncState;
            //Завершаем асинхронный запрос         
            sClient.EndConnect(ar);
            //Выводим удаленную конечную точку, с которой установлено соединение         
            Console.WriteLine("\nСокет соединился с точкой: " + sClient.RemoteEndPoint);

            //Сообщаем основному потоку, что завершили установление соединения.         
            //Для этого устанавливаем объект ConnectDone в сигнальное состояние с          
            // помощью метода Set класса ManualResetEvent         
            ConnectDone.Set();
        }


        public static void SendCallback(IAsyncResult ar)
        {        //Выводим идентификатор текущего потока        //   ...   
            Thread thr = Thread.CurrentThread;   //Получаем текущий поток          
            int idThread = thr.ManagedThreadId;  //Получаем идентификатор потока    

            //Console.WriteLine("\nМетод SendCallback клиента выполняется в потоке:" + idThread);

            Socket sClient = (Socket)ar.AsyncState;
            int lenBytesSend = sClient.EndSend(ar);
            Console.WriteLine("Отправлено серверу " + lenBytesSend + " байт.");
            SendDone.Set();
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            //Выводим идентификатор текущего потока         
            //   ... 
            Thread thr = Thread.CurrentThread;   //Получаем текущий поток          
            int idThread = thr.ManagedThreadId;  //Получаем идентификатор потока    

            //Console.WriteLine("\nМетод ReceiveCallback клиента выполняется в потоке:" + idThread);

            Socket sClient = (Socket)ar.AsyncState;
            int lenBytesReceive = sClient.EndReceive(ar);
            // Полученные данные сохраняются в строке         
            if (lenBytesReceive > 0)
            {
                dataReceive += Encoding.Unicode.GetString(bytesReceive, 0, lenBytesReceive);
                sClient.BeginReceive(bytesReceive, 0, bytesReceive.Length, 0, new AsyncCallback(ReceiveCallback), sClient);
            }
            else
            {
                ReceiveDone.Set();
            }


        }
    }
}
    