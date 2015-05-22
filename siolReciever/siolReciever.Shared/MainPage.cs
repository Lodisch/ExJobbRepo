using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.Xaml.Media;

// To add offline sync support, add the NuGet package Microsoft.WindowsAzure.MobileServices.SQLiteStore
// to your project. Then, uncomment the lines marked // offline sync
// For more information, see: http://aka.ms/addofflinesync
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
using Microsoft.WindowsAzure.MobileServices.Sync;
using siolReciever.DataModel;

// offline sync

namespace siolReciever
{
    sealed partial class MainPage : Page
    {
        private MobileServiceCollection<TodoItem, TodoItem> items;
        private MobileServiceCollection<Receiver, Receiver> receivers;
        private MobileServiceCollection<Announcement, Announcement> announcements;
        private MobileServiceCollection<ReceiverGroup, ReceiverGroup> receiverGroups;
        //private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();
        private IMobileServiceSyncTable<TodoItem> todoTable = App.MobileService.GetSyncTable<TodoItem>(); // offline sync
        private IMobileServiceSyncTable<Receiver> receiverTable = App.MobileService.GetSyncTable<Receiver>(); // offline sync
        private IMobileServiceSyncTable<Announcement> announcementTable = App.MobileService.GetSyncTable<Announcement>(); // offline sync
        private IMobileServiceSyncTable<ReceiverGroup> groupTable = App.MobileService.GetSyncTable<ReceiverGroup>(); // offline sync

        //public static ProfileFlyout MyFlyout;
        public MobileServiceUser user;
        public MainPage()
        {
            //CreateProfileFlyout();

            this.InitializeComponent();
        }

        // Define a member variable for storing the signed-in user.

        public void CreateProfileFlyout()
        {
            //var profile = new ProfileFlyout();
            //profile.FlyoutConfiguration();
            //MyFlyout = profile;
        }

        private async System.Threading.Tasks.Task AuthenticateAsync()
        {
            string message;
            // This sample uses the Facebook provider.
            var provider = "MicrosoftAccount";

            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            while (credential == null)
            {
                try
                {
                    // Try to get an existing credential from the vault.
                    credential = vault.FindAllByResource(provider).FirstOrDefault();
                }
                catch (Exception)
                {
                    // When there is no matching resource an error occurs, which we ignore.
                }

                if (credential != null)
                {
                    // Create a user from the stored credentials.
                    user = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    user.MobileServiceAuthenticationToken = credential.Password;

                    // Set the user from the stored credentials.
                    App.MobileService.CurrentUser = user;

                    try
                    {
                        // Try to return an item now to determine if the cached credential has expired.
                        await App.MobileService.GetTable<TodoItem>().Take(1).ToListAsync();
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            // Remove the credential with the expired token.
                            vault.Remove(credential);
                            credential = null;
                            continue;
                        }
                    }
                }
                else
                {
                    try
                    {
                        // Login with the identity provider.
                        user = await App.MobileService
                            .LoginAsync(provider);

                        // Create and store the user credentials.
                        credential = new PasswordCredential(provider,
                            user.UserId, user.MobileServiceAuthenticationToken);
                        vault.Add(credential);
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        message = "You must log in. Login Required";
                    }
                }
                message = string.Format("You are now logged in - {0}", user.UserId);
                var dialog = new MessageDialog(message);


                await dialog.ShowAsync();

            }

        }

        private async Task UserProfileDialog(Receiver existingUser)
        {
            if (existingUser == null)
            {
                var dialog = new AddUserInfoDialog(
                    String.Format("Welcome {0}! It looks like you're new. Register you're profile information here!",
                        App.MobileService.CurrentUser.UserId));

                var groupResult = await groupTable.Where(g => g.Groupname == "DefaultGroup").ToListAsync();
                ReceiverGroup defaultGroup = groupResult.FirstOrDefault();

                bool result = await dialog.ShowAsync();
                if (result != false)
                {
                    string firstname = dialog.Firstname.Text;
                    string lastname = dialog.Lastname.Text;

                    existingUser = new Receiver
                    {
                        Id = Guid.NewGuid().ToString(),
                        Firstname = firstname,
                        Lastname = lastname,
                        UserId = user.UserId,
                        ReceiverGroup = defaultGroup
                    };

                    await receiverTable.InsertAsync(existingUser);
                }

                await RefreshTodoItems();
            }
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            // Login the user and then load data from the mobile service.
            await AuthenticateAsync();

            // Hide the login button and load items from the mobile service.
            this.ButtonLogin.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            await RefreshTodoItems();
        }

        private async void ManageProfile_Click(object sender, RoutedEventArgs e)
        {
            if (!ProfilePopup.IsOpen) { ProfilePopup.IsOpen = true; }

            var results = await receiverTable.Where(u => u.UserId == App.MobileService.CurrentUser.UserId).ToListAsync();
            Receiver existingUser = results.FirstOrDefault();

            if (existingUser != null)
            {
                FirstnameTb.Text = existingUser.Firstname;
                LastnameTb.Text = existingUser.Lastname;             
            }
        }

        private async void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            await ManageUserProfile();
            if (ProfilePopup.IsOpen) { ProfilePopup.IsOpen = false; }
        }

        private void CloseProfile_Click(object sender, RoutedEventArgs e)
        {
            if (ProfilePopup.IsOpen) { ProfilePopup.IsOpen = false; }
        }

        private async Task ManageUserProfile()
        {
            var results = await receiverTable.Where(u => u.UserId == App.MobileService.CurrentUser.UserId).ToListAsync();
            Receiver existingUser = results.FirstOrDefault();

            if (existingUser == null)
            {
                await SaveProfileInformation();
            }
            else
            {
                await UpdateUserInformation(existingUser);
            }
                      
        }

        private async Task UpdateUserInformation(Receiver existingUser)
        {
            var currentFirstname = existingUser.Firstname;
            var currentLastname = existingUser.Lastname;

            FirstnameTb.Text = existingUser.Firstname;
            LastnameTb.Text = existingUser.Lastname;

            if (currentFirstname != FirstnameTb.Text || currentLastname != LastnameTb.Text)
            {
                var currentUser = new Receiver
                {
                    Id = existingUser.Id,
                    Firstname = FirstnameTb.Text,
                    Lastname = LastnameTb.Text,
                    UserId = existingUser.UserId,
                    ReceiverGroup = existingUser.ReceiverGroup
                };
                await receiverTable.UpdateAsync(currentUser);
            }
        }

        private async Task SaveProfileInformation()
        {
            var groupResult = await groupTable.Where(g => g.Groupname == "DefaultGroup").ToListAsync();
            ReceiverGroup defaultGroup = groupResult.FirstOrDefault();

            string firstname = FirstnameTb.Text;
            string lastname = LastnameTb.Text;
           
            var currentUser = new Receiver
            {
                Id = Guid.NewGuid().ToString(),
                Firstname = firstname,
                Lastname = lastname,
                UserId = user.UserId,
                ReceiverGroup = defaultGroup
            };

            await receiverTable.InsertAsync(currentUser);
        }

        private async Task InsertTodoItem(TodoItem todoItem)
        {
            // This code inserts a new TodoItem into the database. When the operation completes
            // and Mobile Services has assigned an Id, the item is added to the CollectionView
            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);

            await SyncAsync(); // offline sync
        }

        private async Task RefreshTodoItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems
                items = await todoTable
                    .Where(todoItem => todoItem.Complete == false)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                ListItems.ItemsSource = items;
                this.ButtonSave.IsEnabled = true;
            }
        }

        private async Task UpdateCheckedTodoItem(TodoItem item)
        {
            // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
            // responds, the item is removed from the list 
            await todoTable.UpdateAsync(item);
            items.Remove(item);
            ListItems.Focus(Windows.UI.Xaml.FocusState.Unfocused);

            await SyncAsync(); // offline sync
        }

        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            ButtonRefresh.IsEnabled = false;

            await SyncAsync(); // offline sync
            await RefreshTodoItems();

            ButtonRefresh.IsEnabled = true;
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var todoItem = new TodoItem { Text = TextInput.Text };
            await InsertTodoItem(todoItem);
        }

        private async void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            await UpdateCheckedTodoItem(item);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await InitLocalStoreAsync(); // offline sync
            await RefreshTodoItems();
        }

        #region Offline sync

        private async Task InitLocalStoreAsync()
        {
            if (!App.MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("localstore.db");
                store.DefineTable<TodoItem>();
                store.DefineTable<Receiver>();
                store.DefineTable<ReceiverGroup>();
                store.DefineTable<Announcement>();
                await App.MobileService.SyncContext.InitializeAsync(store);
            }

            //await SyncAsync();
        }

        private async Task SyncAsync()
        {
            await App.MobileService.SyncContext.PushAsync();
            await todoTable.PullAsync("todoItems", todoTable.CreateQuery());
        }

        #endregion

    }
}
