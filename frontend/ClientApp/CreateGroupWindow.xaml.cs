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
    /// Logique d'interaction pour CreateGroupWindow.xaml
    /// </summary>
    public partial class CreateGroupWindow : Window
    {
        HttpClient httpc;

        User user;

        public CreateGroupWindow(HttpClient httpc, User user)
        {
            this.httpc = httpc;
            this.user = user;
            InitializeComponent();
        }

        private async void EnregistrerBtn_Click(object sender, RoutedEventArgs e)
        {
            string nom = groupTxtBox.Text;

            bool success = await CreateGroup(nom);

            if (success)
            {
                MessageBox.Show(String.Format("Le groupe {0} a été crée avec succès", nom), "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
                MessageBox.Show("Échec de l'ajout du groupe", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public async Task<bool> CreateGroup(string name)
        {
            if(!String.IsNullOrWhiteSpace(name))
            {
                var groupData = new Dictionary<string, string> { { "name", name } };
                HttpContent content = new FormUrlEncodedContent(groupData);

                try
                {
                    HttpResponseMessage response = httpc.PostAsync("api/groups/", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        List<Group> groups = await new GroupHelper(httpc).GetGroups();
                        Group group = groups.Find(g => g.Name == name);

                        var addUserData = new Dictionary<string, string> { { "isAdmin", "true" } };
                        content = new FormUrlEncodedContent(addUserData);

                        response = httpc.PostAsync(String.Format("api/groups/{0}/users/{1}", group.Id, user.Id), content).Result;

                        if(response.IsSuccessStatusCode)
                            return true;
                    }

                    throw new Exception();
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
