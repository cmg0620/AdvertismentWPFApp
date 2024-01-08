using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
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
using static WpfApp1.UserWindow;
using System.Xml.Linq;
using static WpfApp1.Agent_Window;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Agent_Window.xaml
    /// </summary>
    public partial class Agent_Window : Window
    {
        string UID;
        public int count;
        string client_id;
        string order_name;
        string Agent_name;

        XDocument base_orders = XDocument.Load("..\\..\\orders.xml");
        XDocument base_clients = XDocument.Load("..\\..\\clients.xml");
        XDocument base_shows = XDocument.Load("..\\..\\shows.xml");
       
        Dictionary<string, List<Orders>> orders_dict = new Dictionary<string, List<Orders>>();

        public Agent_Window(string uID, string agent_name)
        {
            InitializeComponent();
            UID = uID;

            ObservableCollection<Orders> orders_collention = new ObservableCollection<Orders>();
            var orders = base_orders.Element("orders");

            foreach (XElement p in orders.Elements("order"))
            {
                if (p.Element("agent_id") == null)
                {
                    orders_collention.Add(new Orders { Order_name = p.Element("name").Value.ToString(), Order_date = p.Element("date").Value.ToString(), Order_price = p.Element("price").Value.ToString() });
                    count++;
                }
                else
                {
                    if (p.Element("agent_id").Value.ToString() == UID)
                    {
                        var c_l_ord = new Orders { Order_client = p.Element("client_id").Value.ToString(), Order_date = p.Element("date").Value.ToString(), Order_name = p.Element("name").Value.ToString(), Order_price = p.Element("price").Value.ToString() };
                        if (orders_dict.ContainsKey(p.Element("date").Value.ToString().Substring(3, 7)))
                        {
                            orders_dict[p.Element("date").Value.ToString().Substring(3, 7)].Add(c_l_ord);
                        }
                        else
                        {
                            var new_list_orders = new List<Orders>() { c_l_ord };
                            orders_dict.Add(p.Element("date").Value.ToString().Substring(3, 7), new_list_orders);
                        }

                    }
                }

            }
            Work.Visibility = Visibility.Collapsed;
            Header_Choose_Client.Text = "Клиентов доступно: " + count.ToString();
            prg.AutoGenerateColumns = false;
            prg.ItemsSource = orders_collention;




            create_report();
            Agent_name = agent_name;
        }

        public void create_report()
        {

                    int sum_price = 0;
                    int max_price = 0;
                    string top_ord = "-1";
            string top_ord_client = "-1";
            foreach (KeyValuePair<string, List<Orders>> el in orders_dict)
                    {
                        for (int i = 0; i < el.Value.Count; i++)
                {
                    sum_price += Int32.Parse(el.Value[i].Order_price);
                    if (Int32.Parse(el.Value[i].Order_price) > max_price)
                    {
                        max_price = Int32.Parse(el.Value[i].Order_price);
                        top_ord = el.Value[i].Order_name;
                        top_ord_client = el.Value[i].Order_client;
                    }
                    el.Value[i].Max_price = max_price;
                    el.Value[i].Top_order = top_ord;
                    el.Value[i].Top_order_client = top_ord_client;
                    Console.WriteLine(el.Value[i].Order_name, el.Value[i].Max_price);
                }

                    }

                
            
            var keys = orders_dict.Keys.ToList();
            month_picker.ItemsSource = keys;
            month_picker.SelectedIndex = 0;
        }

        private void month_picker_selection_changed(object sender, SelectionChangedEventArgs e)
        {
            
            var keys = orders_dict[month_picker.SelectedItem.ToString()].Count();
            var get_list = orders_dict[month_picker.SelectedItem.ToString()];
            salary.Text = get_list[(keys-1)].Max_price.ToString() + "Р";
            deals.Text = orders_dict[month_picker.SelectedItem.ToString()].Count.ToString();
            top.Text = orders_dict[month_picker.SelectedItem.ToString()][keys - 1].Top_order;
            var clients = base_clients.Element("clients");
            var top_client_name = "-1";
            foreach (XElement p in clients.Elements("client"))
            {
                if (p.Attribute("id").Value.ToString() == orders_dict[month_picker.SelectedItem.ToString()][keys - 1].Top_order_client)
                {
                    top_client_name = p.Element("name").Value.ToString();
                }
            }
            top_client.Text = top_client_name;
        }
        public void CreateText(string name, string org, string bank, string phone)
        {
            TextBlock_name.Text = name;
            TextBlock_org.Text = org;
            TextBlock_bank.Text = bank;
            TextBlock_phone.Text = phone;

        }
        private void Change_info(object sender, RoutedEventArgs e)
        {
            Change_Info_Window change_Info_Window = new Change_Info_Window(UID);
            change_Info_Window.Owner = this;
            change_Info_Window.Show();
            change_Info_Window.LogAgent();
        }

        private void Button_in_work(object sender, RoutedEventArgs e)
        {
            Header_Choose_Client.Text = "Информация о заказе и клиенте";
            prg.Visibility = Visibility.Collapsed;
            In_work.Visibility = Visibility.Collapsed;
            Work.Visibility = Visibility.Visible;
            var c_ord = (Orders)prg.SelectedItem;
            order_name = c_ord.Order_name;
            var order_price = c_ord.Order_price;
            var order_date = c_ord.Order_date;
            var orders = base_orders.Element("orders");
            var clients = base_clients.Element("clients");
            var show_id ="0";

            TextBlock_order_name.Text = order_name;
            TextBlock_order_price.Text = "Стоимость:" + Environment.NewLine +  order_price + "Р";
            TextBlock_show_date.Text = "Дата показа:" + Environment.NewLine + order_date;

            foreach (XElement p in orders.Elements("order"))
            {
                if (p.Element("name").Value.ToString() == order_name) 
                {
                    client_id = p.Element("client_id").Value.ToString();
                    show_id = p.Element("show_id").Value.ToString();
                    p.SetElementValue("status", "Назначен агент");
                    p.SetElementValue("agent_id", UID.ToString());
                    base_orders.Save("..\\..\\orders.xml");
                    break;
                }
            }

            foreach (XElement p in clients.Elements("client"))
            {
                if (p.Attribute("id").Value.ToString() == client_id)
                {
                    TextBlock_client_name.Text = p.Element("name").Value.ToString();
                    TextBlock_client_bank.Text = "Банковский счет:"+ Environment.NewLine + p.Element("bank").Value.ToString();
                    TextBlock_client_phone.Text = "Телефон:" + Environment.NewLine + p.Element("phone").Value.ToString();
                }
            }
            var shows = base_shows.Element("shows");
            foreach (XElement p in shows.Elements("show"))
            {
                if (p.Element("id").Value.ToString() == show_id)
                {
                    TextBlock_show_name.Text = "Будет показано в" + Environment.NewLine + p.Element("name").Value.ToString();
                }
            }
            create_report();
        }

        private void Button_back(object sender, RoutedEventArgs e)
        {
            prg.Visibility = Visibility.Visible;
            In_work.Visibility = Visibility.Visible;
            Work.Visibility = Visibility.Collapsed;
        }

        private void Button_comp_deal(object sender, RoutedEventArgs e)
        {
            var orders = base_orders.Element("orders");
            foreach (XElement p in orders.Elements("order"))
            {
                if (p.Element("name").Value.ToString() == order_name)
                {
                    p.SetElementValue("status", "Завершен");
                    base_orders.Save("..\\..\\orders.xml");
                }
            }
            prg.Visibility = Visibility.Visible;
            In_work.Visibility = Visibility.Visible;
            Work.Visibility = Visibility.Collapsed;
            create_report();
        }

        private void Button_exit(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.user_name.Text = Agent_name+"!";
            mw.Show();
            this.Hide();
        }

        public class Clients
        {
            public string Name { get; set; }
            public string Org { get; set; }
            public string Bank { get; set; }
            public string Phone { get; set; }

        }
        public class Orders
        {
            public int Max_price { get; set; }
            public string Top_order {  get; set; }
            public string Top_order_client { get; set; }
            public string Order_client {  get; set; }
            public string Order_name { get; set; }
            public string Order_date { get; set; }
            public string Order_price { get; set; }

        }
    }
}
