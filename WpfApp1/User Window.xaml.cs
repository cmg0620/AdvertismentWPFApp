﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            XDocument base_shows = XDocument.Load("C:\\Users\\cmg\\source\\repos\\WpfApp1\\WpfApp1\\shows.xml");
            ObservableCollection<Program> programs = new ObservableCollection<Program>();
            var shows = base_shows.Element("shows");
            foreach (XElement p in shows.Elements("show")) {
                programs.Add(new Program { Name = p.Element("name").Value.ToString(), Price = p.Element("price").Value.ToString(), Rate = p.Element("rate").Value.ToString() });
                Console.WriteLine('1');
                Console.WriteLine(p.Element("name").Value.ToString(),  p.Element("price").Value.ToString(), p.Element("rate").Value.ToString());
            }
                
            prg.AutoGenerateColumns = false;
            prg.ItemsSource = programs;

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
            public string Rate { get; set; }
            public string Price { get; set; }

        }
    }
}