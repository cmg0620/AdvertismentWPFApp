using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public int count;
        public UserWindow()
        {
            InitializeComponent();
            XDocument base_shows = XDocument.Load("C:\\Users\\cmg\\source\\repos\\WpfApp1\\WpfApp1\\shows.xml");
            ObservableCollection<Program> programs = new ObservableCollection<Program>();
            var shows = base_shows.Element("shows");
            
            foreach (XElement p in shows.Elements("show")) {
                programs.Add(new Program { Name = p.Element("name").Value.ToString(), Price = Int32.Parse(p.Element("price").Value.ToString()), Rate = Int32.Parse(p.Element("rate").Value.ToString()) });
                count++;
            }
            Step_2.Visibility = Visibility.Collapsed;
            Step_3.Visibility = Visibility.Collapsed;
            Subheader_1.Text = "Программы на выбор: " + count.ToString();
            prg.AutoGenerateColumns = false;
            prg.ItemsSource = programs;
        }
        public string c_pr_Name;
        public int c_pr_Rate;
        public int c_pr_Price;
        public string order_Name;
        public string order_Time;
        public string order_Drb;
        public void Select(object sender, RoutedEventArgs e)
        {
            var c_pr = (Program)prg.SelectedItem;

            if (c_pr != null)
            {
                
                prg.Visibility = Visibility.Collapsed;
                Button_Select.Visibility = Visibility.Collapsed;
                Header_Choose_Prg1.Text = "2. Информация о заказе";
                Subheader_1.Text = "Введите данные о рекламе";
                Step_2.Visibility = Visibility.Visible;
                c_pr_Name = c_pr.Name;
                c_pr_Price = c_pr.Price;
                c_pr_Rate = c_pr.Rate;
                
                
            }
            else
            {
                MessageBox.Show("Не выбрано ни одной программы");
            }
        }

        private void Select_2(object sender, RoutedEventArgs e)
        {

            if (Data_order.SelectedDate!= null && TextBox_order_name != null && ComboBox_order_drb != null)
            {
                Step_2.Visibility = Visibility.Collapsed;
                Step_3.Visibility = Visibility.Visible;
                order_Time = Data_order.SelectedDate.Value.Date.ToString().Replace("0:00:00", "");
                order_Name = TextBox_order_name.Text;
                order_Drb = ComboBox_order_drb.Text;
                Header_Choose_Prg1.Text = "3. Проверка данных";
                Subheader_1.Height = 100;
                Subheader_1.Text = "После подтверждения заказа Ваша заявка будет отправлена агентам";
                TextBlock_final_order_name.Text = order_Name;
                TextBlock_final_order_prg.Text = "Будет показан в передаче: " + c_pr_Name;
                TextBlock_final_order_drb.Text = "Продолжительность: " + order_Drb;
                TextBlock_final_order_time.Text = "Дата показа: " + order_Time;
                int order_min = Int32.Parse(order_Drb.Replace(" секунд", ""));
                TextBlock_final_order_price.Text = "Стоимость: " + (c_pr_Price * order_min / 60).ToString() + "Р";
            }
            else
            {
                MessageBox.Show("Не заполнено какое-либо поле");
            }

        }

        private void Button_discard(object sender, RoutedEventArgs e)
        {
            Step_3.Visibility = Visibility.Collapsed;
            prg.Visibility = Visibility.Visible;
            Subheader_1.Text = "Программы на выбор: " + count.ToString();
            Header_Choose_Prg1.Text = "1. Выбор передачи";
            Button_Select.Visibility = Visibility.Visible;
            Subheader_1.Height = 52;
        }

        private void Button_send(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отправлено");
        }

        public void CreateText(string name, string org, string bank, string phone)
        {
            TextBlock_name.Text = name;
            TextBlock_org.Text = org;
            TextBlock_bank.Text = bank;
            TextBlock_phone.Text = phone;

        }
        public void Id_Handler(string id)
        {
            Uid = id;
        }

        public void Button_change_info(object sender, RoutedEventArgs e)
        {
            Change_Info_Window change_Info_Window = new Change_Info_Window(Uid);
            change_Info_Window.Owner = this;
            change_Info_Window.Show();
        }
        private void Button_exit(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Hide();
        }

        public class Program
        {
            public string Name { get; set; }
            public int Rate { get; set; }
            public int Price { get; set; }

        }
    }
}