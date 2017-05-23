using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        private string dataReceive;
        private string comment;

        public DataPage(string dataReceive, string comment)
        {
            InitializeComponent();
            this.dataReceive = dataReceive;
            this.comment = comment;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList records = new ArrayList();

            if (dataReceive != "")
            {
                string[] data = dataReceive.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
               
                for (int i = 0; i < data.Length; i += 4)
                {
                    Record rec = new Record(data[i], data[i + 1], data[i + 2], data[i + 3]);
                    records.Add(rec);
                }
            }
            else
            {
                //
            }

            dataGrid.ItemsSource = records;
            label.Content = comment;
            
        }
    }
}
