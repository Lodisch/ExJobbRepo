using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace siolReciever
{
    public class DialogComponents
    {
        public TextBox FirstnameTb { get; set; }
        public TextBox LastnameTb { get; set; }
        public TextBlock UserInfoMsg { get; set; }
        public Button SaveProfileInfoBtn { get; set; }

        public DialogComponents()
        {
            FirstnameTb = new TextBox
            {
                PlaceholderText = "Firstname"
            };
            LastnameTb = new TextBox
            {
                PlaceholderText = "Lastname"
            };

            SaveProfileInfoBtn = new Button
            {
                Content = "Save Profile Information"
            };

        }
    }
}
