using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        SQLiteConnection connection = new SQLiteConnection("Data Source=" + @"appdata.db");

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
                //Console.WriteLine("Идентификатор основного потока сервера: " + idThread);
                //byte[] bytes = new byte[SIZE];
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, PORT);
                Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sListener.Bind(localEndPoint);
                sListener.Listen(LEN);

                while (true)
                {
                    socketEvent.Reset();
                    Console.WriteLine("Сервер ожидает подключения клиента");
                    AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
                    sListener.BeginAccept(aCallback, sListener);
                    socketEvent.WaitOne();
                }
                
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
            socketEvent.Set();
            Thread thr = Thread.CurrentThread;
            int idThread = thr.ManagedThreadId;
            //Console.WriteLine("Идентификатор потока выполнения метода AcceptCallback: " + idThread);
            Socket listener = (Socket)ar.AsyncState;
            Socket client_soc = listener.EndAccept(ar);
            client_soc.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), client_soc);
        }


        public static void ReceiveCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            int idThread = thr.ManagedThreadId;
            //Console.WriteLine("Идентификатор потока выполнения метода ReceiveCallback: " + idThread);
            string content = String.Empty;
            Socket client_soc = (Socket)ar.AsyncState;
            int lenByteReceive = client_soc.EndReceive(ar);
            

            if (lenByteReceive > 0)
            {
                content += Encoding.Unicode.GetString(buffer, 0, lenByteReceive);

                if (content.IndexOf(".") > -1)
                {
                    Console.WriteLine("Получено от клиента {0} байт", lenByteReceive);
                    Console.WriteLine("Сообщение: {0}", content);

                    if (content.Remove(content.Length - 1, 1) != "")
                    {
                        string choice = content.Remove(1, content.Length - 1);
                        string data = content.Substring(1, content.Length - 2);

                        switch (choice)
                        {
                            case "1":
                                try
                                {
                                    string[] items = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    string newmsg = DataWorker.Load_Group(items[0]);
                                    byte[] byteSend1 = Encoding.Unicode.GetBytes(newmsg);
                                    client_soc.BeginSend(byteSend1, 0, byteSend1.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                catch
                                {

                                    byte[] byteSend1 = Encoding.Unicode.GetBytes("error");
                                    client_soc.BeginSend(byteSend1, 0, byteSend1.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                break;
                            case "2":
                                try
                                {
                                    string[] items = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    string newmsg = DataWorker.Load_Subject(items[0]);
                                    byte[] byteSend2 = Encoding.Unicode.GetBytes(newmsg);
                                    // Отправляем сообщение клиенту                      
                                    client_soc.BeginSend(byteSend2, 0, byteSend2.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                catch
                                {

                                    byte[] byteSend2 = Encoding.Unicode.GetBytes("error");
                                    client_soc.BeginSend(byteSend2, 0, byteSend2.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                break;
                            case "0":
                                try
                                {
                                    string[] items = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    string newmsg = DataWorker.Load_Surname(items[0]);
                                    byte[] byteSend0 = Encoding.Unicode.GetBytes(newmsg);
                                    // Отправляем сообщение клиенту                      
                                    client_soc.BeginSend(byteSend0, 0, byteSend0.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                catch
                                {

                                    byte[] byteSend0 = Encoding.Unicode.GetBytes("error");
                                    client_soc.BeginSend(byteSend0, 0, byteSend0.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                break;
                            case "3":
                                try
                                {
                                    string[] items = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    DataWorker.Add_Record(items[0], items[1], items[2], items[3]);
                                    byte[] byteSend3 = Encoding.Unicode.GetBytes("3");
                                    client_soc.BeginSend(byteSend3, 0, byteSend3.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                catch
                                {
                                    byte[] byteSend3 = Encoding.Unicode.GetBytes("error");
                                    client_soc.BeginSend(byteSend3, 0, byteSend3.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                break;
                            case "4":
                                try
                                {
                                    string[] items = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    DataWorker.Delete_Name(items[0]);
                                    byte[] byteSend4 = Encoding.Unicode.GetBytes("4");
                                    client_soc.BeginSend(byteSend4, 0, byteSend4.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                catch
                                {
                                    byte[] byteSend4 = Encoding.Unicode.GetBytes("error");
                                    client_soc.BeginSend(byteSend4, 0, byteSend4.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                break;
                            case "5":
                                try
                                {
                                    string[] items = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    DataWorker.Delete_Group(items[0]);
                                    byte[] byteSend5 = Encoding.Unicode.GetBytes("5");
                                    client_soc.BeginSend(byteSend5, 0, byteSend5.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                catch
                                {
                                    byte[] byteSend5 = Encoding.Unicode.GetBytes("error");
                                    client_soc.BeginSend(byteSend5, 0, byteSend5.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                break;
                                
                            case "6":
                                try
                                {
                                    string[] items = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    DataWorker.Delete_Subject(items[0]);
                                    byte[] byteSend6 = Encoding.Unicode.GetBytes("6");
                                    client_soc.BeginSend(byteSend6, 0, byteSend6.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                catch
                                {
                                    byte[] byteSend6 = Encoding.Unicode.GetBytes("error");
                                    client_soc.BeginSend(byteSend6, 0, byteSend6.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                }
                                break;
                            default:
                                byte[] byteSend = Encoding.Unicode.GetBytes("error");
                                client_soc.BeginSend(byteSend, 0, byteSend.Length, 0, new AsyncCallback(SendCallback), client_soc);
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Сообщение пустое");
                    }
                    


                    
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
            //Console.WriteLine("Идентификатор потока выполнения SendCallback: " + idThread);
            Socket client_soc = (Socket)ar.AsyncState;
            int lenByteSend = client_soc.EndSend(ar);
            Console.WriteLine("Послано клиенту {0} байт", lenByteSend);
            client_soc.Shutdown(SocketShutdown.Both); client_soc.Close();
            //Устанавливаем событие для основного потока
            socketEvent.Set();
        }
    }

}
    

