using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace siolReciever
{
    public sealed partial class AddUserInfoDialog : UserControl
    {
        private string m_TextMessage;
        private TaskCompletionSource<bool> m_TaskCompletionSource;
        public AddUserInfoDialog(string label)
        {
            Label = label;
            this.InitializeComponent();
        }

        public Task<bool> ShowAsync()
        {
            InitFields();
             
            m_Popup.IsOpen = true;
            m_TaskCompletionSource = new TaskCompletionSource<bool>();
            return m_TaskCompletionSource.Task;
        }

        public void InitFields()
        {          
            m_Popup.Width = Window.Current.Bounds.Width;
            m_Popup.Height = Window.Current.Bounds.Height;
            m_RectOne.Height = Window.Current.Bounds.Height;
            m_RectOne.Width = Window.Current.Bounds.Width;
            m_RectTwo.Width = Window.Current.Bounds.Width;
            m_firstname.Width = Window.Current.Bounds.Width / 2;
            m_lastname.Width = Window.Current.Bounds.Width / 2;
            m_firstname.PlaceholderText = "Firstname";
            m_lastname.PlaceholderText = "Lastname";
            m_LabelTxtBlock.Text = Label;
            m_LabelTxtBlock.Foreground = new SolidColorBrush(Colors.White);
        }

        public string Label
        {
            get { return m_TextMessage; }
            set { m_TextMessage = value; }
        }

        public TextBox Firstname
        {
            get { return m_firstname; }
        }

        public TextBox Lastname
        {
            get { return m_lastname; }
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            m_TaskCompletionSource.SetResult(true);
            m_Popup.IsOpen = false;
        }
    }
}
