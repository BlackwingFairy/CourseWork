using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_Client
{
    /// <summary>
    /// Логика взаимодействия для RedactionPage.xaml
    /// </summary>
    public partial class RedactionPage : Page
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

        public RedactionPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectDone.Reset();
                SendDone.Reset();
                ReceiveDone.Reset();
                dataReceive = null;


                Thread thr = Thread.CurrentThread;
                int idThread = thr.ManagedThreadId;

                IPHostEntry ipHost = Dns.Resolve("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint endpoint = new IPEndPoint(ipAddr, PORT);
                Socket sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                string dataSend = "";

                sClient.BeginConnect(endpoint, new AsyncCallback(ConnectCallback), sClient);
                ConnectDone.WaitOne();


                dataSend = "3" + textBox1.Text + " " + textBox2.Text + " " + textBox3.Text + " " + textBox4.Text;



                byte[] bytesSend = Encoding.Unicode.GetBytes(dataSend + ".");

                sClient.BeginSend(bytesSend, 0, bytesSend.Length, 0, new AsyncCallback(SendCallback), sClient);

                SendDone.WaitOne();
                //sClient.BeginReceive(bytesReceive, 0, bytesReceive.Length, 0, new AsyncCallback(ReceiveCallback), sClient);

                //ReceiveDone.WaitOne();

                sClient.Shutdown(SocketShutdown.Both);
                sClient.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("\nException");
            }
            finally
            {
                //Console.ReadKey();
            }            
        }



        public static void ConnectCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;   //Получаем текущий поток          
            int idThread = thr.ManagedThreadId;  //Получаем идентификатор потока    


            //Используем свойство  AsyncState интерфейса IAsyncResult для извлечения          
            //аргумента, который был передан в третьем параметре метода BeginConnect()          
            //Полученное значение явно приводим к типу Socket          
            Socket sClient = (Socket)ar.AsyncState;
            //Завершаем асинхронный запрос         
            sClient.EndConnect(ar);

            //Сообщаем основному потоку, что завершили установление соединения.         
            //Для этого устанавливаем объект ConnectDone в сигнальное состояние с          
            // помощью метода Set класса ManualResetEvent         
            ConnectDone.Set();
        }


        public static void SendCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;   //Получаем текущий поток          
            int idThread = thr.ManagedThreadId;  //Получаем идентификатор потока    


            Socket sClient = (Socket)ar.AsyncState;
            int lenBytesSend = sClient.EndSend(ar);

            SendDone.Set();
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            //Выводим идентификатор текущего потока         
            //   ... 
            Thread thr = Thread.CurrentThread;   //Получаем текущий поток          
            int idThread = thr.ManagedThreadId;  //Получаем идентификатор потока    



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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectDone.Reset();
                SendDone.Reset();
                ReceiveDone.Reset();
                dataReceive = null;


                Thread thr = Thread.CurrentThread;
                int idThread = thr.ManagedThreadId;

                IPHostEntry ipHost = Dns.Resolve("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint endpoint = new IPEndPoint(ipAddr, PORT);
                Socket sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                string dataSend = "";

                sClient.BeginConnect(endpoint, new AsyncCallback(ConnectCallback), sClient);
                ConnectDone.WaitOne();


                dataSend = "4" + textBox1.Text + " " + textBox2.Text + " " + textBox3.Text + " " + textBox4.Text;



                byte[] bytesSend = Encoding.Unicode.GetBytes(dataSend + ".");

                sClient.BeginSend(bytesSend, 0, bytesSend.Length, 0, new AsyncCallback(SendCallback), sClient);

                SendDone.WaitOne();
                //sClient.BeginReceive(bytesReceive, 0, bytesReceive.Length, 0, new AsyncCallback(ReceiveCallback), sClient);

                //ReceiveDone.WaitOne();

                sClient.Shutdown(SocketShutdown.Both);
                sClient.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("\nException");
            }
            finally
            {
                //Console.ReadKey();
            }
        }
    }
}
