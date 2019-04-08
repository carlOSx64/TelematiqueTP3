using System;
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

namespace ClientApp {
    /// <summary>
    /// Logique d'interaction pour RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window {

        HttpClient httpc;

        public RegisterWindow(HttpClient httpc) {
            this.httpc = httpc;
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e) {

            string username = usernameTxtBox.Text;
            string password = passwordTxtBox.Text;

            bool success = Register(username, password);

            if(success)
            {
                MessageBox.Show("Le compte a été créé avec succès", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
                MessageBox.Show("La création du compte a échouée", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool Register(string username, string password)
        {
            var user = new Dictionary<string, string>
            {
                { "Username", username },
                { "Password", password }
            };

            HttpContent content = new FormUrlEncodedContent(user);

            HttpResponseMessage response = httpc.PostAsync("api/users/register", content).Result;

            if(response.IsSuccessStatusCode)
                return true;

            return false;
        }
    }
}
