using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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
using System.Xml;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        XDocument base_clients = XDocument.Load("C:\\Users\\cmg\\source\\repos\\WpfApp1\\WpfApp1\\clients.xml");
        XDocument base_orders = XDocument.Load("C:\\Users\\cmg\\source\\repos\\WpfApp1\\WpfApp1\\orders.xml");
        XDocument base_shows = XDocument.Load("C:\\Users\\cmg\\source\\repos\\WpfApp1\\WpfApp1\\shows.xml");
        XDocument base_agents = XDocument.Load("C:\\Users\\cmg\\source\\repos\\WpfApp1\\WpfApp1\\agents.xml");

        bool is_agent = false;
        public void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            is_agent = true;

        }

        private void TextBox_login_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_login.Text = string.Empty;
        }
        private void TextBox_password_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_password.Text = string.Empty;
        }
        public void Button_enter_click(object sender, EventArgs e)
        {
            string login = TextBox_login.Text;
            string password = TextBox_password.Text;
            var clients = base_clients.Element("clients");
            var agents = base_agents.Element("agents");

            if (!is_agent){

                foreach (XElement client in clients.Elements("client"))
                {
                    if (client.Element("name").Value == login)
                    {
                        Console.WriteLine(client.Element("name").ToString());
                    }
                    else {
                        TextBox_login.ToolTip = "Ошибка";
                    }
                    if (client.Element("password").Value == password)
                    {
                        Console.WriteLine(client.Element("name").ToString());
                    }
                    else
                    {
                        TextBox_password.ToolTip = "Ошибка";
                    }
                    if ((client.Element("password").Value == password) && (client.Element("name").Value == login)) {
                        
                        string name = client.Element("name").Value;
                        string org = client.Element("org").Value;
                        string bank = client.Element("bank").Value;
                        string phone = client.Element("phone").Value;
                        string user_id = client.Attribute("id").Value;
                        Console.WriteLine(user_id);

                        UserWindow uw = new UserWindow(user_id);
                        uw.Show();
                        uw.CreateText(name, org, bank, phone);
                        this.Hide();
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Agent");
                foreach (XElement agent in agents.Elements("agent"))
                {
                    if (agent.Element("name").Value == login)
                    {
                        Console.WriteLine(agent.Element("name").ToString());
                        TextBox_login.Background = Brushes.White;

                    }
                    else
                    {
                        TextBox_login.ToolTip = "Ошибка";
                        TextBox_login.Background = Brushes.Yellow;
                    }
                    if (agent.Element("password").Value == password)
                    {
                        Console.WriteLine(agent.Element("name").ToString());
                        TextBox_password.Background = Brushes.White;
                    }
                    else
                    {
                        TextBox_password.ToolTip = "Ошибка";
                        TextBox_password.Background = Brushes.Yellow;
                    }
                    if ((agent.Element("password").Value == password) && (agent.Element("name").Value == login))
                    {

                        break;
                    }

                }

            }
        }


    }

}