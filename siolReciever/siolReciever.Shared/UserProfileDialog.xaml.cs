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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace siolReciever
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserProfileDialog : UserControl
    {
        private string m_TextMessage;
        private TaskCompletionSource<bool> m_TaskCompletionSource;
        public UserProfileDialog(string label)
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
            m_Rect1.Height = Window.Current.Bounds.Height;
            m_Rect1.Width = Window.Current.Bounds.Width;
            m_Rect2.Width = Window.Current.Bounds.Width;
            m_TextBlock.Text = Label;
            m_firstnameTb.Width = Window.Current.Bounds.Width / 2;
            m_lastnameTb.Width = Window.Current.Bounds.Width / 2;
        }

        public string Label
        {
            get { return m_TextMessage; }
            set { m_TextMessage = value; }
        }

        public TextBox FirstnameTb
        {
            get { return m_firstnameTb; }
        }

        public TextBox LastnameTb
        {
            get { return m_lastnameTb; }
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            m_TaskCompletionSource.SetResult(true);
            m_Popup.IsOpen = false;
        }
       
    }
}
