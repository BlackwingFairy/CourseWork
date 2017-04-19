using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        private const int PORT = 11000;
        private const int SIZE = 2048;
        private const int LEN = 10;
        public static byte[] buffer = new byte[SIZE];
        public static ManualResetEvent socketEvent = new ManualResetEvent(false);


        public static void Main(string[] args)
        {
            try {
                Thread thr = Thread.CurrentThread;
                int idThread = thr.ManagedThreadId;
                Console.WriteLine("Идентификатор основного потока сервера: " + idThread);
                //byte[] bytes = new byte[SIZE];
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, PORT);
                Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sListener.Bind(localEndPoint);
                sListener.Listen(LEN);
                Console.WriteLine("Сервер ожидает подключения клиента");
                AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
                sListener.BeginAccept(aCallback, sListener);
                socketEvent.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.ReadKey();
            }
        } //Main 


        public static void AcceptCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            int idThread = thr.ManagedThreadId;
            Console.WriteLine("Идентификатор потока выполнения метода AcceptCallback: " + idThread);
            Socket listener = (Socket)ar.AsyncState;
            Socket client_soc = listener.EndAccept(ar);
            client_soc.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), client_soc);
        }


        public static void ReceiveCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            int idThread = thr.ManagedThreadId;
            Console.WriteLine("Идентификатор потока выполнения метода ReceiveCallback: " + idThread);
            string content = String.Empty;
            Socket client_soc = (Socket)ar.AsyncState;
            int lenByteReceive = client_soc.EndReceive(ar);

            if (lenByteReceive > 0)
            {
                content += Encoding.ASCII.GetString(buffer, 0, lenByteReceive);

                if (content.IndexOf(".") > -1)
                {
                    Console.WriteLine("Получено от клиента {0} байт", lenByteReceive);
                    Console.WriteLine("Сообщение: {0}", content);

                    //место для нашего кода


                    byte[] byteSend = Encoding.ASCII.GetBytes(content);
                    // Отправляем то же сообщение клиенту                      
                    client_soc.BeginSend(byteSend, 0, byteSend.Length, 0, new AsyncCallback(SendCallback), client_soc);
                }
                else
                {
                    //Иначе получаем оставшиеся данные
                    client_soc.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), client_soc);
                }
            }
        }
        
          
        public static void SendCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            int idThread = thr.ManagedThreadId;
            Console.WriteLine("Идентификатор потока выполнения SendCallback: " + idThread);
            Socket client_soc = (Socket)ar.AsyncState;
            int lenByteSend = client_soc.EndSend(ar);
            Console.WriteLine("Послано клиенту {0} байт", lenByteSend);
            client_soc.Shutdown(SocketShutdown.Both); client_soc.Close();
            //Устанавливаем событие для основного потока
            socketEvent.Set();
        }
    }

}
    

