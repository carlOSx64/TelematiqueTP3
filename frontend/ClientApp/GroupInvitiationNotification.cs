using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApp
{
    class GroupInvitiationNotification : Notification
    {
        public GroupInvitiationNotification(Group group, User admin) : base("{0} vous invite à rejoindre le groupe {1}.")
        {
            Group = group;
            Admin = admin;
        }

        private Group Group { get; }

        private User Admin { get; }

        override public void Trigger()
        {
            MessageBoxResult result = MessageBox.Show(String.Format(Message, Admin.Username, Group.Name), "", MessageBoxButton.YesNo);

            // Traiter result... passer des références dans le constructeur au besoin
        }
    }
}
