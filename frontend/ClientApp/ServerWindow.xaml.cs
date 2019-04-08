using System;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace ClientApp {
    /// <summary>
    /// Logique d'interaction pour ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window {
        public ServerWindow() {
            InitializeComponent();
        }

        private void ConnectionBtn_Click(object sender, RoutedEventArgs e) {

            string address = addressTxtBox.Text;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            using(HttpClient httpc = new HttpClient())
            {
                try
                {
                    httpc.BaseAddress = new Uri(address);
                }
                catch(Exception)
                {
                    MessageBox.Show("Le format de l'adresse est invalide", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    HttpResponseMessage response = httpc.GetAsync("api/users").Result;
                    if(response.IsSuccessStatusCode)
                    {
                        LoginWindow loginWindow = new LoginWindow(httpc);
                        this.Hide();
                        loginWindow.ShowDialog();
                        this.Show();
                    }
                    else
                        throw new Exception();
                }
                catch(Exception)
                {
                    MessageBox.Show("La connexion avec le serveur à échoué", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    /*
        private async Task<List<User>> DownloadPageAsync(HttpClient httpc)
        {
            HttpResponseMessage response = await httpc.GetAsync("https://localhost:5001/api/users");    
            if (response.IsSuccessStatusCode)
            {    
                User[] users = await response.Content.ReadAsAsync<User[]>();
                return new List<User>(users);
            }
            return null;
        }
        */
    }
}
