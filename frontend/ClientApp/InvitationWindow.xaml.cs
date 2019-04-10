using ClientApp.Helpers;
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
    /// Logique d'interaction pour InvitationWindow.xaml
    /// </summary>
    public partial class InvitationWindow : Window {

        HttpClient httpc;

        Group group;

        User user;

        public InvitationWindow(HttpClient httpc, Group group, User user) {

            this.httpc = httpc;

            this.group = group;

            this.user = user;

            InitializeComponent();

            DisplayUsers();
        }

        public async void DisplayUsers()
        {
            userLstBox.ItemsSource = await new UserHelper(httpc).GetUsers();
        }

        private async void InviteBtn_Click(object sender, RoutedEventArgs e) {

            User userToInvite = userLstBox.SelectedItem as User;

            bool success = await new InvitationHelper(httpc).InviteUser(userToInvite, group, adminChBox.IsChecked.Value, user);

            if(success)
                MessageBox.Show("L'utilisateur a été ajouté au groupe avec succès", "", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Échec de l'ajout de l'utilisateur au groupe", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
