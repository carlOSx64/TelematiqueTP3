﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using WebApi.Models;
using System.Web;

namespace ClientApp {
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {

        HttpClient httpc;

        public LoginWindow(HttpClient httpc) {
            this.httpc = httpc;
            InitializeComponent();
        }

        private async void ConnectionBtn_Click(object sender, RoutedEventArgs e) {
            string username = usernameTxtBox.Text;
            string password = passwordTxtBox.Password;

            User user = await Login(username, password);

            if (user != null)
            {
                MainWindow mainWindow = new MainWindow(httpc, user);
                mainWindow.ShowDialog();

                if(!Logout(user))
                    MessageBox.Show("Un problème est survenu lors de la déconnexion", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("La connexion au compte a échoué", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async Task<User> Login(string username, string password)
        {
            var connectionData = new Dictionary<string, string>
            {
                { "Username", username },
                { "Password", password }
            };

            HttpContent content = new FormUrlEncodedContent(connectionData);

            HttpResponseMessage response = httpc.PostAsync("api/users/authenticate", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<User>();
            }
            return null;
        }

        private bool Logout(User user)
        {
            try
            {
                HttpResponseMessage response = httpc.GetAsync(String.Format("api/users/logout?apiKey={0}", user.ApiKey)).Result;
                if(response.IsSuccessStatusCode)
                    return true;

                throw new Exception();
            }
            catch(Exception)
            {
                return false;
            }
        }

        private void CreateUserBtn_Click(object sender, RoutedEventArgs e) {
            RegisterWindow registerWindow = new RegisterWindow(httpc);
            registerWindow.ShowDialog();
        }
    }
}
