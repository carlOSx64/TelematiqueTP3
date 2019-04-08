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

namespace ClientApp {
    /// <summary>
    /// Logique d'interaction pour ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window {
        public ServerWindow() {
            InitializeComponent();
        }

        private void ConnectionBtn_Click(object sender, RoutedEventArgs e) {
            bool success = true;

            string ip = IPTxtBox.Text;

            // Connexion...


            if(success)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
                MessageBox.Show("La connexion avec le serveur à échoué", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
