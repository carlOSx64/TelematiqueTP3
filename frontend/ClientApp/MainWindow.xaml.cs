using ClientApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
        List<Notification> notifications;
        List<Group> groups;

        User currentUser;

        HttpClient httpc;
        UserHelper userHelper;
        UDPThread udp;
        Thread udpThread;

        public MainWindow(HttpClient httpc, User currentUser)
        {
            
            this.httpc = httpc;

            userHelper = new UserHelper(httpc);

            this.currentUser = currentUser;

            InitializeComponent();

            notifications = new List<Notification>();
            this.udp = new UDPThread(this);
            this.udpThread = new Thread(this.udp.run);
            this.udpThread.Start();

        }

        //S'exécute après l'ouverture de la fenêtre
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            RequestFolderLocation();
            var dueTime = TimeSpan.FromMinutes(2);
            var interval = TimeSpan.FromMinutes(2);

            RunPeriodicAsync(Update, dueTime, interval, CancellationToken.None);
            Update();
        }
        
        //Poour tester le timer
        private void TestTimer()
        {
            MessageBox.Show("Hello, world!");
        }

        private async void CreateDirectories()
        {
            string path;

            List<Group> groups = await new GroupHelper(httpc).GetUserGroups(currentUser);

            foreach(Group group in groups)
            {
                path = System.IO.Path.Combine(folderLocation, group.Name);

                System.IO.Directory.CreateDirectory(path);
            }
        }

        private async void CreateFileByGroup()
        {
            string path;

            List<Group> groups = await new GroupHelper(httpc).GetUserGroups(currentUser);

           

            foreach (Group group in groups)
            {
                
                List<File> files = new List<File>();
                

                files = await new FileHelper(httpc).GetGroupFiles(group);
                path = System.IO.Path.Combine(folderLocation, group.Name);
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                System.IO.FileInfo[] localFiles = di.GetFiles("*");
                List<string> localfileNames = new List<string>();
                List<string> remotefileNames = new List<string>();
                foreach (File f in files)
                {
                    remotefileNames.Add(f.Name);
                }
                foreach(System.IO.FileInfo fi in localFiles)
                {
                    localfileNames.Add(fi.Name);
                }
                //Verication si on a des nouveua file local
                foreach (System.IO.FileInfo file in localFiles)
                {
                    if (!remotefileNames.Contains(file.Name))
                    {
                        System.IO.FileStream fs = file.OpenRead();
                        byte[] fileContent;
                        using (System.IO.BinaryReader sr = new System.IO.BinaryReader(fs))
                        {
                            long numByte = file.Length;
                            fileContent = sr.ReadBytes((int)numByte);
                        }

                        await new FileHelper(httpc).CreateFile(file.Name, fileContent, group.Id);
                        fs.Close();
                    }
                }

                //Delete 
               /* List<File> filesToDelete = new List<File>();
                foreach (File file in files)
                {
                    if (!localfileNames.Contains(file.Name))
                    {
                        filesToDelete.Add(file);
                    }
                }

                foreach(File fileToDelete in filesToDelete)
                {
                    await new FileHelper(httpc).DeleteFile(fileToDelete.Id);
                }*/


            }

            groups = await new GroupHelper(httpc).GetUserGroups(currentUser);

            foreach (Group group in groups)
            {
                path = System.IO.Path.Combine(folderLocation, group.Name);
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                System.IO.FileInfo[] localFiles = di.GetFiles("*");
                List<string> localfileNames = new List<string>();
                foreach (System.IO.FileInfo fi in localFiles)
                {
                    fi.Delete();
                }

                path = System.IO.Path.Combine(folderLocation, group.Name);

                List<File> files = await new FileHelper(httpc).GetGroupFiles(group);
                Debug.WriteLine("Salut la gang");
                foreach (File file in files)
                {
                    string filePath = System.IO.Path.Combine(path, file.Name);
                    Debug.WriteLine("Salut la gang 22222");
                    Debug.WriteLine(filePath);

               

                    try
                    {
                        System.IO.FileStream fstream = System.IO.File.Create(filePath);
                        byte[] data = System.Convert.FromBase64String(file.Content);

                        fstream.Write(data, 0, data.Length);

                        fstream.Close();
                    }
                    catch { }
                    

                    
                    //TODO write file.data dans le fstream
                }
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

        public void Update()
        {
            TreatNotifications();
            UpdateUsers();
            UpdateGroups();
            CreateDirectories();
            CreateFileByGroup();
            this.Dispatcher.Invoke(() =>
            {
                updateTimeLabel.Content = "Dernière mise à jour: " + DateTime.Now.ToString();
            });
        }

        private async void UpdateUsers()
        {
            List<UserView> users = await new UserHelper(httpc).GetUserViews();
            this.Dispatcher.Invoke(() =>
            {
                usersListView.ItemsSource = users;
                //Tri pour que les utilisateurs connectés se retrouvent en haut
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(usersListView.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("IsConnected", ListSortDirection.Descending));
            });
        }

        private async void UpdateGroups()
        {
            groups = await new GroupHelper(httpc).GetUserGroups(currentUser);
            this.Dispatcher.Invoke(() =>
            {
                groupsItemControl.ItemsSource = groups;
            });
            
        }

        private async void TreatNotifications()
        {
            List<Notification> notifications = await new InvitationHelper(httpc).GetUserGroupInvitiationNotifications(currentUser);

            foreach(Notification notification in notifications)
            {
                notification.Trigger();
            }
            notifications.Clear();
        }

        //https://stackoverflow.com/questions/14296644/how-to-execute-a-method-periodically-from-wpf-client-application-using-threading
        // The `onTick` method will be called periodically unless cancelled.
        private static async Task RunPeriodicAsync(Action onTick,
                                                   TimeSpan dueTime,
                                                   TimeSpan interval,
                                                   CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Call our onTick function.
                onTick?.Invoke();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        private void SyncBtn_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void GroupBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as Button).Tag;
            Group group = groups.Find(g => g.Id == id);

            GroupWindow groupWindow = new GroupWindow(httpc, group, currentUser);
            groupWindow.ShowDialog();
        }

        private void NewGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateGroupWindow createGroupWindow = new CreateGroupWindow(this.httpc, currentUser);
            createGroupWindow.ShowDialog();
            UpdateGroups();
        }
    }
}
