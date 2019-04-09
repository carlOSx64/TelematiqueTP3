using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace ClientApp.Helpers {
    class GroupHelper
    {
        HttpClient httpc;

        public GroupHelper(HttpClient httpc)
        {
            this.httpc = httpc;
        }

        public async Task<List<Group>> GetUserGroups(User user)
        {
            try
            {
                HttpResponseMessage response = await httpc.GetAsync(String.Format("api/users/{0}/groups", user.Id));
                if (response.IsSuccessStatusCode)
                {    
                    Group[] groups = await response.Content.ReadAsAsync<Group[]>();
                    return new List<Group>(groups);
                }

                throw new Exception();
            }
            catch
            {
                return null;
            }
        }
    }
}
