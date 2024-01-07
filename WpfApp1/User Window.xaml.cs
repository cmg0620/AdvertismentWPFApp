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
using static WpfApp1.UserWindow;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public int count;
        public string UID;
        public UserWindow(string uID)
        {
            InitializeComponent();
            XDocument base_shows = XDocument.Load("..\\..\\shows.xml");
            ObservableCollection<Program> programs = new ObservableCollection<Program>();
            var shows = base_shows.Element("shows");

            foreach (XElement p in shows.Elements("show"))
            {
                programs.Add(new Program { Name = p.Element("name").Value.ToString(), Price = Int32.Parse(p.Element("price").Value.ToString()), Rate = Int32.Parse(p.Element("rate").Value.ToString()) });
                count++;
            }
            Step_2.Visibility = Visibility.Collapsed;
            Step_3.Visibility = Visibility.Collapsed;
            Subheader_1.Text = "Программы на выбор: " + count.ToString();
            prg.AutoGenerateColumns = false;
            prg.ItemsSource = programs;
            UID = uID;
            Load_Last_Orders();
            
        }
        public string c_pr_Name;
        public int c_pr_Rate;
        public int c_pr_Price;
        public string order_Name;
        public string order_Time;
        public string order_Drb;

        XDocument base_orders = XDocument.Load("..\\..\\orders.xml");
        public void Load_Last_Orders()
        {
            
            var orders = base_orders.Element("orders");
            List<Orders> orders_list = new List<Orders>();

            List<TextBlock> last_orders_lst_1 = new List<TextBlock>();
            List<TextBlock> last_orders_lst_2 = new List<TextBlock>();
            List<TextBlock> last_orders_lst_3 = new List<TextBlock>();
            List<TextBlock> last_orders_lst_4 = new List<TextBlock>();
            Dictionary<int, List<TextBlock>> last_prp_dict = new Dictionary<int, List<TextBlock>>();

            var items_1 = last_order_1.Items;

            foreach (var item in items_1)
            {
                var ListBoxItem = item as ListBoxItem;
                var itemSP = ListBoxItem.Content as TextBlock;
                last_orders_lst_1.Add(itemSP);
            }

            var items_2 = last_order_2.Items;
            foreach (var item in items_2)
            {
                var ListBoxItem = item as ListBoxItem;
                var itemSP = ListBoxItem.Content as TextBlock;
                last_orders_lst_2.Add(itemSP);
            }

            var items_3 = last_order_3.Items;
            foreach (var item in items_3)
            {
                var ListBoxItem = item as ListBoxItem;
                var itemSP = ListBoxItem.Content as TextBlock;
                last_orders_lst_3.Add(itemSP);
            }

            var items_4 = last_order_4.Items;
            foreach (var item in items_4)
            {
                var ListBoxItem = item as ListBoxItem;
                var itemSP = ListBoxItem.Content as TextBlock;
                last_orders_lst_4.Add(itemSP);
            }

            last_prp_dict.Add(0, last_orders_lst_1);
            last_prp_dict[1] = last_orders_lst_2;
            last_prp_dict[2] = last_orders_lst_3;
            last_prp_dict[3] = last_orders_lst_4;

            Console.WriteLine("Все работает тут");
            Console.WriteLine(UID);
            foreach (XElement p in orders.Elements("order"))
            {
                Console.WriteLine(p.Element("client_id").Value.ToString());
                if (p.Element("client_id").Value.ToString() == UID.ToString())
                {
                    Console.WriteLine("UID совпал");
                    Console.WriteLine("It ok");
                    if (p.Element("status") != null && p.Element("status").Value.ToString() == "Завершен")
                    {
                        orders_list.Add(new Orders { Client_id = p.Element("client_id").Value.ToString(), Order_id = p.Element("order_id").Value.ToString(), Order_name = p.Element("name").Value.ToString(), Show_date = p.Element("date").Value.ToString(), Show_id = p.Element("show_id").Value.ToString(), Is_agent = p.Element("agent_id").Value.ToString(), Order_price = p.Element("price").Value.ToString(), Order_status = p.Element("status").Value.ToString() });

                    }
                    else
                    {
                        if (p.Element("agent_id") != null)
                        {
                            orders_list.Add(new Orders { Client_id = p.Element("client_id").Value.ToString(), Order_id = p.Element("order_id").Value.ToString(), Order_name = p.Element("name").Value.ToString(), Show_date = p.Element("date").Value.ToString(), Show_id = p.Element("show_id").Value.ToString(), Order_status = "Назначен агент", Order_price = p.Element("price").Value.ToString(), Is_agent = p.Element("agent_id").Value.ToString() });
                        }
                        else
                        {
                            orders_list.Add(new Orders
                            {
                                Client_id = p.Element("client_id").Value.ToString(),
                                Order_id = p.Element("order_id").Value.ToString(),
                                Order_name = p.Element("name").Value.ToString(),
                                Show_date = p.Element("date").Value.ToString(),
                                Show_id = p.Element("show_id").Value.ToString(),
                                Order_status = "В обработке",
                                Order_price = p.Element("price").Value.ToString(),
                                Is_agent = "Ожидает агента"
                            });
                        }
                    }
                }
                if (orders_list.Count > 0)
                {

                    for (int i = 0; i < orders_list.Count; i++)
                    {
                        last_prp_dict[i%4][0].Text = orders_list[i].Order_name;
                        last_prp_dict[i%4][1].Text = orders_list[i].Order_status;
                        last_prp_dict[i%4][2].Text = orders_list[i].Show_date;
                        last_prp_dict[i%4][3].Text = orders_list[i].Order_price + "Р";
                    }
                }
            }


        }

        Orders current_order = new Orders();
        XDocument base_shows = XDocument.Load("..\\..\\shows.xml");
        public void Select(object sender, RoutedEventArgs e)
        {
            var c_pr = (Program)prg.SelectedItem;

            if (c_pr != null)
            {
                
                current_order.Client_id = UID;
                prg.Visibility = Visibility.Collapsed;
                Button_Select.Visibility = Visibility.Collapsed;
                Header_Choose_Prg1.Text = "2. Информация о заказе";
                Subheader_1.Text = "Введите данные о рекламе";
                Step_2.Visibility = Visibility.Visible;
                c_pr_Name = c_pr.Name;
                c_pr_Price = c_pr.Price;
                c_pr_Rate = c_pr.Rate;
                
                var shows = base_shows.Element("shows");
                foreach (XElement p in shows.Elements("show"))
                {
                    if (p.Element("name").Value.ToString() == c_pr_Name)
                    {
                        current_order.Show_id = p.Element("id").Value.ToString();
                    }
                }
                var get_last_oder_id = base_orders.Element("orders").Elements("order").Last().Element("order_id").Value.ToString();
                current_order.Order_id = (Int32.Parse(get_last_oder_id) + 1).ToString();
                Console.WriteLine(current_order.Order_id);
            }
            else
            {
                MessageBox.Show("Не выбрано ни одной программы");
            }
        }

        

        private void Select_2(object sender, RoutedEventArgs e)
        {

            if (Data_order.SelectedDate != null && TextBox_order_name != null && ComboBox_order_drb != null)
            {
                Step_2.Visibility = Visibility.Collapsed;
                Step_3.Visibility = Visibility.Visible;
                current_order.Show_date = Data_order.SelectedDate.Value.Date.ToString().Replace("0:00:00", "");
                current_order.Order_name = TextBox_order_name.Text;
                order_Drb = ComboBox_order_drb.Text;
                Header_Choose_Prg1.Text = "3. Проверка данных";
                Subheader_1.Height = 100;
                Subheader_1.Text = "После подтверждения заказа Ваша заявка будет отправлена агентам";
                TextBlock_final_order_name.Text = current_order.Order_name;
                TextBlock_final_order_prg.Text = "Будет показан в передаче: " + c_pr_Name;
                TextBlock_final_order_drb.Text = "Продолжительность: " + order_Drb;
                TextBlock_final_order_time.Text = "Дата показа: " + current_order.Show_date;
                int order_min = Int32.Parse(order_Drb.Replace(" секунд", ""));
                current_order.Order_price = (c_pr_Price * order_min / 60).ToString();
                TextBlock_final_order_price.Text = "Стоимость: " + current_order.Order_price + "Р";
            }
            else
            {
                MessageBox.Show("Не заполнено какое-либо поле");
            }

        }

        public void Discard()
        {
            Step_3.Visibility = Visibility.Collapsed;
            prg.Visibility = Visibility.Visible;
            Subheader_1.Text = "Программы на выбор: " + count.ToString();
            Header_Choose_Prg1.Text = "1. Выбор передачи";
            Button_Select.Visibility = Visibility.Visible;
            Subheader_1.Height = 52;
        }
        public void Button_discard(object sender, RoutedEventArgs e)
        {
            Discard();
        }

        private void Button_send(object sender, RoutedEventArgs e)
        {
            XElement root = new XElement("order");
            root.Add(new XElement("client_id", current_order.Client_id));
            root.Add(new XElement("order_id", current_order.Order_id));
            root.Add(new XElement("show_id", current_order.Show_id));
            root.Add(new XElement("date", current_order.Show_date));
            root.Add(new XElement("name", current_order.Order_name));
            root.Add(new XElement("price", current_order.Order_price));
            MessageBox.Show("Отправлено");
            base_orders.Element("orders").Add(root);
            base_orders.Save("..\\..\\orders.xml");
            Discard();
        }

        public void CreateText(string name, string org, string bank, string phone)
        {
            TextBlock_name.Text = name;
            TextBlock_org.Text = org;
            TextBlock_bank.Text = bank;
            TextBlock_phone.Text = phone;

        }


        public void Button_change_info(object sender, RoutedEventArgs e)
        {
            Change_Info_Window change_Info_Window = new Change_Info_Window(UID);
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
        public class Orders
        {
            public string Client_id { get; set; } 
            public string Order_id { get; set; } 
            public string Show_id { get; set; } 
            public string Order_name { get; set; } 
            public string Show_date { get; set; } 
            public string Order_status { get; set; }
            public string Order_price { get; set; } 
            public string Is_agent { get; set; }
        }
    }
}