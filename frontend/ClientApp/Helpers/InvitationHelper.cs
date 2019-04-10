using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace ClientApp.Helpers {
    class InvitationHelper {
        HttpClient httpc;

        public InvitationHelper(HttpClient httpc)
        {
            this.httpc = httpc;
        }

        public async Task<List<InvitationDto>> GetUserGroupInvitations(User user)
        {
            try
            {
                HttpResponseMessage response = await httpc.GetAsync(String.Format("api/users/{0}/invitations", user.Id));
                if (response.IsSuccessStatusCode)
                {    
                    InvitationDto[] invitations = await response.Content.ReadAsAsync<InvitationDto[]>();
                    return new List<InvitationDto>(invitations);
                }

                throw new Exception();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Notification>> GetUserGroupInvitiationNotifications(User user)
        {
            List<InvitationDto> invitations = await GetUserGroupInvitations(user);

            if(invitations == null)
                return null;

            List<Notification> notifications = new List<Notification>();

            foreach(InvitationDto invitation in invitations)
            {
                if(invitation.Status == InvitationStatus.Pending)
                    notifications.Add(InvitationToNotification(invitation, user));
            }

            return notifications;
        }

        private GroupInvitiationNotification InvitationToNotification(InvitationDto invitation, User user)
        {
            return new GroupInvitiationNotification(httpc, invitation.GroupId, user, invitation.InvitedById);
        }

        public async Task<bool> InviteUser(User userToInvite, Group group, bool admin, User user)
        {
            try
            {
                var inviteData = new Dictionary<string, string>
                {
                    { "apiKey", user.ApiKey },
                    { "isAdmin", admin.ToString() }
                };
                HttpContent content = new FormUrlEncodedContent(inviteData);

                string request = String.Format("api/groups/{0}/invitations/{1}", group.Id, userToInvite.Id);

                HttpResponseMessage response = await httpc.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                    return true;

                throw new Exception();
            }
            catch
            {
                return false;
            }
        }
    }
}
