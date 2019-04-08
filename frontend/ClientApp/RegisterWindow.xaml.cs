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
    /// Logique d'interaction pour RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window {
        public RegisterWindow() {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e) {
            bool success = true;

            string username = usernameTxtBox.Text;
            string password = passwordTxtBox.Text;

            //Création du compte...

            if(success)
            {
                MessageBox.Show("Le compte a été créé avec succès", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
                MessageBox.Show("La création du compte à échoué", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
