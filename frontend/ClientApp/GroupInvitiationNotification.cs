using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebApi.Models;

namespace ClientApp
{
    class GroupInvitiationNotification : Notification
    {
        HttpClient httpc;

        int groupId;

        User user;

        int adminId;

        public GroupInvitiationNotification(HttpClient httpc, int groupId, User user, int adminId) : base("{0} vous invite à rejoindre le groupe {1}")
        {
            this.httpc = httpc;
            this.groupId = groupId;
            this.user = user;
            this.adminId = adminId;
        }

        override public void Trigger()
        {
            MessageBoxResult result = MessageBox.Show(String.Format(Message, adminId, groupId), "", MessageBoxButton.YesNo);

            InvitationStatus status;
            if(result == MessageBoxResult.Yes)
                status = InvitationStatus.Accepted;
            else
                status = InvitationStatus.Rejected;

            Respond(status);
        }

        public void Respond(InvitationStatus status)
        {
            var statusData = new Dictionary<string, string>
            {
                { "status", status.ToString() }
            };

            HttpContent content = new FormUrlEncodedContent(statusData);

            try
            {
                HttpResponseMessage response = httpc.PutAsync(String.Format("api/groups/{0}/invitations/{1}", groupId, user.Id), content).Result;

                if(response.IsSuccessStatusCode)
                    return;

                throw new Exception();
            }
            catch
            {
                return;
            }
        }
    }
}
