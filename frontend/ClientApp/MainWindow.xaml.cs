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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String folderLocation;
        List<UserView> users;
        List<Group> groups;
        List<Notification> notifications;
        HttpClient httpc;

        public MainWindow(HttpClient httpc)
        {
            this.httpc = httpc;
            users = GetUsers();
            groups = GetUserGroups();
            
            //group = GetUserGroupsRequest();
            InitializeComponent();

            notifications = new List<Notification>();
            InitializeUserListView();
            InitializeGroupItemControl();
            
        }

        //S'exécute après l'ouverture de la fenêtre
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            RequestFolderLocation();
            createrDir();
            Update();
        }

        private List<UserView> GetUsers()
        {
            //Code placeholder
            List<UserView> placeholder = new List<UserView>();

            placeholder.Add(new UserView("JDISMaster", true));
            placeholder.Add(new UserView("Bessamlol", false));
            placeholder.Add(new UserView("Carl++", true));
            placeholder.Add(new UserView("Natrelcul", false));
            placeholder.Add(new UserView("Info tout nu", false));

            return placeholder;
        }

        private List<Group> GetUserGroups()
        {
            //Code placeholder
            List<Group> placeholder = new List<Group>();

            List<UserView> users = new List<UserView>();
            foreach(UserView uv in this.users)
                users.Add(uv);

            List<UserView> admin = new List<UserView>();
            admin.Add(this.users.First());

            placeholder.Add(new Group("32", "IFT585", users, admin));
            placeholder.Add(new Group("33", "JDIS", users, admin));

            return placeholder;
        }


       /* public async Task<List<Group>> GetUserGroupsRequest()
        {

            List<Group> groups = null ;

            HttpResponseMessage response = await httpc.GetAsync("api/groups");

            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
                groups = await response.Content.ReadAsAsync<List<Group>>();

            return groups;
        }*/

        public void createrDir()
        {
            string path;
            foreach(Group g in groups)
            {
                path = System.IO.Path.Combine(folderLocation, g.Name);

                System.IO.Directory.CreateDirectory(path);
            }
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

        private void InitializeUserListView()
        {
            //Liaison de usersListView avec users
            usersListView.ItemsSource = users;

            //Tri pour que les utilisateurs connectés se retrouvent en haut
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(usersListView.ItemsSource);
			view.SortDescriptions.Add(new SortDescription("IsConnected", ListSortDirection.Descending));
        }

        private void InitializeGroupItemControl()
        {
            //Liaison de groupsItemControl avec groups
            groupsItemControl.ItemsSource = groups;
        }

        private void UpdateUsers()
        {

        }

        private void UpdateGroups()
        {

        }

        private void PullNotifications()
        {
            //Code placeholder
            notifications.Add(new Notification("Ceci est une notification"));
            notifications.Add(new GroupInvitiationNotification(groups.First(), groups.First().Admins.First()));
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
            string id = (string)(sender as Button).Tag;
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
