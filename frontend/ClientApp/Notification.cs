using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApp
{
    class Notification
    {
        public Notification(string message)
        {
            Message = message;
        }

        virtual public void Trigger()
        {
            MessageBox.Show(Message);
        }

        protected string Message { get; }
    }
}