using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Change_Info_Window.xaml
    /// </summary>


    public partial class Change_Info_Window : Window
    {
        string Uid = string.Empty;

        public Change_Info_Window(string LUid)
        {
            InitializeComponent();
            Uid = LUid;
        }


        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        public void Button_Apply(object sender, RoutedEventArgs e)
        {
            List<string> list_changes = new List<string>() { TextBox_name.Text, TextBox_org.Text, TextBox_bank.Text, TextBox_phone.Text };
            List<string> list_fields = new List<string>() { "name", "org", "bank", "phone" };
            XDocument base_clients = XDocument.Load("..\\..\\clients.xml");
            var clients = base_clients.Element("clients");
            Console.WriteLine("3Wind");
            Console.WriteLine(Uid);


            foreach (XElement client in clients.Elements("client"))
            {
                Console.WriteLine(client.Attribute("id").Value.ToString());

                if (client.Attribute("id").Value.ToString() == Uid.ToString())
                {
                    
                    for (int i = 0; i < list_changes.Count; i++)
                    {
                        

                        if (list_changes[i].Length != 0)
                        {
                            client.Element(list_fields[i]).Value = list_changes[i];
                            client.Element("phone").Value = "changed";
                            base_clients.Save("..\\..\\clients.xml");
                            Console.WriteLine("changed");
                        }
                    }
                }
            }
            foreach (XElement client in clients.Elements("client"))
            {
                if (client.Attribute("id").Value == Uid)
                {
                    UserWindow uw = this.Owner as UserWindow;
                    uw.CreateText(client.Element("name").Value, client.Element("org").Value, client.Element("bank").Value, client.Element("phone").Value);
                }
            }
            this.Close();
        }
    }
}
