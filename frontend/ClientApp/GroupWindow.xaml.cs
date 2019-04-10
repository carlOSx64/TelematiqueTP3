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

namespace ClientApp
{
    /// <summary>
    /// Logique d'interaction pour GroupWindow.xaml
    /// </summary>
    public partial class GroupWindow : Window
    {
        HttpClient httpc;

        Group group;

        User user;

        public GroupWindow(HttpClient httpc, Group group, User user)
        {
            this.httpc = httpc;

            this.group = group;

            this.user = user;

            InitializeComponent();

            groupLbl.Content = group.Name;

            DisplayMembers();
            DisplayAdmins();
        }

        public void DisplayMembers()
        {
            List<User> users = new List<User>();
            foreach(UserGroup userGroup in group.UserGroups)
                users.Add(userGroup.User);
            userLstBox.ItemsSource = users;
        }

        public void DisplayAdmins()
        {
            List<User> users = new List<User>();
            foreach(UserGroup userGroup in group.UserGroups.Where(ug => ug.IsAdmin))
                users.Add(userGroup.User);
            userLstBox.ItemsSource = users;
        }

        private void InviteBtn_Click(object sender, RoutedEventArgs e) {
            InvitationWindow invitationWindow = new InvitationWindow(httpc, group, user);
            invitationWindow.ShowDialog();
        }
    }
}
