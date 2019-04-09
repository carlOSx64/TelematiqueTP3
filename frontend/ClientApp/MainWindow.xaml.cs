using ClientApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebApi.Models;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String folderLocation;
        List<UserView> userViews;
        List<Group> groups ;
        List<Notification> notifications;

        User currentUser;

        HttpClient httpc;
        UserHelper userHelper;

        public MainWindow(HttpClient httpc, User currentUser)
        {
            this.httpc = httpc;

            userHelper = new UserHelper(httpc);

            this.currentUser = currentUser;

            InitializeComponent();

            notifications = new List<Notification>();
        }

        //S'exécute après l'ouverture de la fenêtre
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            RequestFolderLocation();
            Update();
        }

        private void RequestFolderLocation()
        {
            MessageBox.Show("Veuillez choisir un emplacement sur votre disque");
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                bool folderLocationSelected = false;
                do
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    folderLocation = dialog.SelectedPath;

                    folderLocationSelected = folderLocation != "";
                    if(!folderLocationSelected)
                        MessageBox.Show("Le programme à besoin d'un répertoire pour continuer", "", MessageBoxButton.OK, MessageBoxImage.Error);
                } while(!folderLocationSelected);
            }
        }

        private void Update()
        {
            UpdateUsers();
            UpdateGroups();
            PullNotifications();
            TreatNotifications();
        }

        private async void UpdateUsers()
        {
            usersListView.ItemsSource = await new UserHelper(httpc).GetUserViews();

            //Tri pour que les utilisateurs connectés se retrouvent en haut
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(usersListView.ItemsSource);
			view.SortDescriptions.Add(new SortDescription("IsConnected", ListSortDirection.Descending));
        }

        private async void UpdateGroups()
        {

        }

        private void PullNotifications()
        {
            //Code placeholder
            notifications.Add(new Notification("Ceci est une notification"));
            //notifications.Add(new GroupInvitiationNotification(groups.First(), groups.First().Admins.First()));
        }

        private void TreatNotifications()
        {
            foreach(Notification notification in notifications)
            {
                notification.Trigger();
            }
            notifications.Clear();
        }

        private void SyncBtn_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void GroupBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as Button).Tag;
            Group group = groups.Find(g => g.Id == id);

            GroupWindow groupWindow = new GroupWindow(group);
            groupWindow.ShowDialog();
        }

        private void NewGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateGroupWindow createGroupWindow = new CreateGroupWindow();
            createGroupWindow.ShowDialog();
        }
    }
}
