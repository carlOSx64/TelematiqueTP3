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
            bool success = true;

            string username = usernameTxtBox.Text;
            string password = passwordTxtBox.Text;

            //Connexion...
            success = Login(username, password);

            if (success)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
            }
            else
                MessageBox.Show("La connexion au compte a échoué", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool Login(string username, string password)
        {
            var connectionData = new Dictionary<string, string>
            {
                { "Username", username },
                { "Password", password }
            };

            HttpContent content = new FormUrlEncodedContent(connectionData);

            HttpResponseMessage response = httpc.PostAsync("api/users/authenticate", content).Result;

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        private void CreateUserBtn_Click(object sender, RoutedEventArgs e) {
            RegisterWindow registerWindow = new RegisterWindow(httpc);
            registerWindow.ShowDialog();
        }
    }
}
