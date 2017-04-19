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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
        static TextBlock globalTextBlock = null;


        public MainWindow()
        {
            InitializeComponent();
        }

        public delegate void myDelegate();

  

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                textBlock.Text = "";
                globalTextBlock = textBlock;
                Thread thr = Thread.CurrentThread;
                int idThread = thr.ManagedThreadId;
                textBlock.Text = textBlock.Text+"Метод Main клиента выполняется в потоке:" + idThread;
                IPHostEntry ipHost = Dns.Resolve("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint endpoint = new IPEndPoint(ipAddr, PORT);
                Socket sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                sClient.BeginConnect(endpoint, new AsyncCallback(ConnectCallback), sClient);
                ConnectDone.WaitOne();

                string dataSend = "This is test:";
                for (int i = 1; i < 200; i++)
                    dataSend += "i = " + i.ToString() + ";";
                globalTextBlock.Text = globalTextBlock.Text + "\nБудем отправлять серверу сообщение:";
                globalTextBlock.Text = globalTextBlock.Text + "\n" + dataSend;
                byte[] bytesSend = Encoding.ASCII.GetBytes(dataSend + ".");

                sClient.BeginSend(bytesSend, 0, bytesSend.Length, 0, new AsyncCallback(SendCallback), sClient);

                for(int i = 0; i < 5; i++)
                {
                    globalTextBlock.Text= globalTextBlock.Text+"\n" +i;
                    Thread.Sleep(100);
                }

                SendDone.WaitOne();
                sClient.BeginReceive(bytesReceive, 0, bytesReceive.Length, 0, new AsyncCallback(ReceiveCallback), sClient);

                ReceiveDone.WaitOne();
                globalTextBlock.Text = globalTextBlock.Text + "\nПолучено от сервера: " +dataReceive;
                sClient.Shutdown(SocketShutdown.Both);
                sClient.Close();
            }
            catch (Exception ex)
            {
                //.Text = "Exception";
            }
        }

        public static void ConnectCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;   //Получаем текущий поток          
            int idThread = thr.ManagedThreadId;  //Получаем идентификатор потока    

            globalTextBlock.Text = globalTextBlock.Text + "\nМетод ConnectCallback клиента выполняется в потоке:" + idThread;
            //Используем свойство  AsyncState интерфейса IAsyncResult для извлечения          
            //аргумента, который был передан в третьем параметре метода BeginConnect()          
            //Полученное значение явно приводим к типу Socket          
            Socket sClient = (Socket)ar.AsyncState;
            //Завершаем асинхронный запрос         
            sClient.EndConnect(ar);
            //Выводим удаленную конечную точку, с которой установлено соединение         
            globalTextBlock.Text = globalTextBlock.Text + "\nСокет соединился с точкой: " + sClient.RemoteEndPoint;

            //Сообщаем основному потоку, что завершили установление соединения.         
            //Для этого устанавливаем объект ConnectDone в сигнальное состояние с          
            // помощью метода Set класса ManualResetEvent         
            ConnectDone.Set();
        }


        public static void SendCallback(IAsyncResult ar)
        {        //Выводим идентификатор текущего потока        //   ...   
            Thread thr = Thread.CurrentThread;   //Получаем текущий поток          
            int idThread = thr.ManagedThreadId;  //Получаем идентификатор потока    

            globalTextBlock.Text = globalTextBlock.Text + "\nМетод SendCallback клиента выполняется в потоке:" + idThread;

            Socket sClient = (Socket)ar.AsyncState;
            int lenBytesSend = sClient.EndSend(ar);
            globalTextBlock.Text = globalTextBlock.Text + "Отправлено серверу " + lenBytesSend + " байт.";
            SendDone.Set();
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            //Выводим идентификатор текущего потока         
            //   ... 
            Thread thr = Thread.CurrentThread;   //Получаем текущий поток          
            int idThread = thr.ManagedThreadId;  //Получаем идентификатор потока    

            globalTextBlock.Text = globalTextBlock.Text + "\nМетод ReceiveCallback клиента выполняется в потоке:" + idThread;

            Socket sClient = (Socket)ar.AsyncState;
            int lenBytesReceive = sClient.EndReceive(ar);
            // Полученные данные сохраняются в строке         
            if (lenBytesReceive > 0)
            {
                dataReceive += Encoding.ASCII.GetString(bytesReceive, 0, lenBytesReceive);
                sClient.BeginReceive(bytesReceive, 0, bytesReceive.Length, 0, new AsyncCallback(ReceiveCallback), sClient);
            }
            else
            {
                ReceiveDone.Set();
            }


        }
    }
}



