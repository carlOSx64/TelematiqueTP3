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

namespace ClientApp
{
    /// <summary>
    /// Logique d'interaction pour CreateGroupWindow.xaml
    /// </summary>
    public partial class CreateGroupWindow : Window
    {
        HttpClient httpc;
        public CreateGroupWindow(HttpClient httpc)
        {
            InitializeComponent();
            this.httpc = httpc;
        }

        public async void EnregistrerBtn_Click(object sender, RoutedEventArgs e)
        {
            bool success = true;

            string nom = addressTxtBox.Text;
            //Connexion...
            success = createGroup(nom);

            if (success)
            {
                MainWindow mainWindow = new MainWindow(httpc);//Pas sur comment revenir a mainwindow ?
                mainWindow.ShowDialog();
            }
            else
                MessageBox.Show("Echec ajout de groupe", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool createGroup(string name)
        {
            var connectionData = new Dictionary<string, string>
            {
                { "name", name },
            };

            HttpContent content = new FormUrlEncodedContent(connectionData);

            HttpResponseMessage response = httpc.PostAsync("api/groups/", content).Result;

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

    }
}
