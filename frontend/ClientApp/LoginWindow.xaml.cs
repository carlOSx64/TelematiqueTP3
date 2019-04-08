﻿using System;
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
using System.Windows.Shapes;

namespace ClientApp {
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        public LoginWindow() {
            InitializeComponent();
        }

        private void ConnectionBtn_Click(object sender, RoutedEventArgs e) {
            bool success = true;

            string username = usernameTxtBox.Text;
            string password = passwordTxtBox.Text;

            //Connexion...

            if(success)
            {
                MainWindow mainWindow = new MainWindow();
                this.Close();
            }
            else
                MessageBox.Show("La connexion au compte a échoué", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CreateUserBtn_Click(object sender, RoutedEventArgs e) {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
    }
}