using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace ClientApp.Helpers {
    class UserHelper
    {
        HttpClient httpc;

        public UserHelper(HttpClient httpc)
        {
            this.httpc = httpc;
        }

        private async Task<List<U>> GetUsersTemplate<U>(string route)
        {
            try
            {
                HttpResponseMessage response = await httpc.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {    
                    U[] users = await response.Content.ReadAsAsync<U[]>();
                    return new List<U>(users);
                }

                throw new Exception();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            return await GetUsersTemplate<User>("api/users");
        }

        public async Task<List<ConnectedUser>> GetConnectedUsers()
        {
            return await GetUsersTemplate<ConnectedUser>("api/connectedUsers");
        }

        public async Task<List<UserView>> GetUserViews()
        {
            List<User> users = await GetUsers();
            List<ConnectedUser> connectedUsers = await GetConnectedUsers();

            if(users != null && connectedUsers != null)
            {
                List<UserView> userViews = new List<UserView>();
                foreach(User user in users)
                {
                    bool isConnected = false;
                    if(connectedUsers.Exists(u => u.UserId == user.Id))
                        isConnected = true;
                    userViews.Add(UserToUserView(user, isConnected));
                }
                return userViews;
            }
            else
                return null;
        }

        private UserView UserToUserView(User user, bool isConnected)
        {
            return new UserView(user.Id, user.Username, isConnected);
        }
    }
}
