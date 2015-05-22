using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace siolReciever
{
    public class ProfileFlyout: Flyout
    {
        public Grid MainGrid { get; set; }
        public StackPanel ContentPanel { get; set; }
        public StackPanel TextBlockPanel { get; set; }
        public StackPanel TextBoxPanel { get; set; }
        public StackPanel ButtonPanel { get; set; }
        public TextBox FirstnameTextBox { get; set; }
        public TextBox LastnameTextBox { get; set; }
        public TextBlock LabelBlock { get; set; }
        public Button OkButton { get; set; }
        public Rectangle MainRectangle { get; set; }
        public Rectangle ContentRectangle { get; set; }

        public ProfileFlyout()
        {            
            const string welcomeMsg = "Welcome! Looks like you're new. Submit your profile info here";

            MainGrid = new Grid 
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = new SolidColorBrush(Colors.DimGray),
                Opacity = 0.5,
                Height = Window.Current.Bounds.Height,
                Width = Window.Current.Bounds.Width,
            };
         

            MainRectangle = new Rectangle
            {
                Fill = new SolidColorBrush(Colors.LightGray),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Opacity = 0.25,
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height,
            };
            ContentRectangle = new Rectangle
            {
                Fill = new SolidColorBrush(Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = Window.Current.Bounds.Height/3,
                Width = Window.Current.Bounds.Width,
            };
            ContentPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            TextBlockPanel = new StackPanel
            {
              Orientation  = Orientation.Horizontal,
              HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBoxPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            ButtonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0,20,0,0)
            };
            FirstnameTextBox = new TextBox() { Height = 20, Width = Window.Current.Bounds.Width / 2, PlaceholderText = "Firstname" };
            LastnameTextBox = new TextBox() { Height = 20, Width = Window.Current.Bounds.Width / 2, PlaceholderText = "Lastname" };
            LabelBlock = new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 25,
                Text = welcomeMsg,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0,0,20,0)
            };
            OkButton = new Button { Content = "Ok", Margin = new Thickness(0,0,20,0)};
        }

        public void FlyoutConfiguration()
        {
            MainGrid.Children.Add(MainRectangle);
            MainGrid.Children.Add(ContentRectangle);
            ButtonPanel.Children.Add(OkButton);
            TextBoxPanel.Children.Add(FirstnameTextBox);
            TextBoxPanel.Children.Add(LastnameTextBox);
            TextBlockPanel.Children.Add(LabelBlock);
            ContentPanel.Children.Add(TextBlockPanel);
            ContentPanel.Children.Add(TextBoxPanel);
            ContentPanel.Children.Add(ButtonPanel);
            MainGrid.Children.Add(ContentPanel);    
            this.Content = MainGrid;
        }
    }

    
}
