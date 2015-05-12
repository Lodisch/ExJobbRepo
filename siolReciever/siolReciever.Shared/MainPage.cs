using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using siolReciever.DataModel;
using System.Linq;
using Windows.Security.Credentials;
using Windows.Storage.AccessCache;

// To add offline sync support, add the NuGet package Microsoft.WindowsAzure.MobileServices.SQLiteStore
// to your project. Then, uncomment the lines marked // offline sync
// For more information, see: http://aka.ms/addofflinesync
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync

namespace siolReciever
{
    sealed partial class MainPage : Page
    {
        private MobileServiceCollection<Announcement, Announcement> announcements;
        private MobileServiceCollection<Receiver, Receiver> receivers;
        private MobileServiceCollection<ReceiverGroup, ReceiverGroup> receiverGroups;
        private MobileServiceCollection<Sender, Sender> senders;
        private MobileServiceCollection<TodoItem, TodoItem> items;
        //private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();
        private IMobileServiceSyncTable<Announcement> announcementTable = App.MobileService.GetSyncTable<Announcement>();
        private IMobileServiceSyncTable<Receiver> receiverTable = App.MobileService.GetSyncTable<Receiver>();
        private IMobileServiceSyncTable<ReceiverGroup> receiverGroupsTable = App.MobileService.GetSyncTable<ReceiverGroup>();
        private IMobileServiceSyncTable<Sender> senderTable = App.MobileService.GetSyncTable<Sender>();

        private IMobileServiceSyncTable<TodoItem> todoTable = App.MobileService.GetSyncTable<TodoItem>(); // offline sync

        public MainPage()
        {
            this.InitializeComponent();
        }

        // Define a member variable for storing the signed-in user. 
        private MobileServiceUser user;

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            // Login the user and then load data from the mobile service.
            await AuthenticateAsync();

            // Hide the login button and load items from the mobile service.
            this.ButtonLogin.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            await RefreshTodoItems();
        }

        private async Task AuthenticateAsync()
        {
            string message;

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
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();


            }

            //Fix! User gets added to recievers after refresh button has been clicked.
            //Gets null reference exception before..

            var recieverResult =
                    await receiverTable.Where(u => u.UserId == App.MobileService.CurrentUser.UserId).ToListAsync();
            Receiver existingUser = recieverResult.FirstOrDefault();

            if (existingUser == null)
            {
                var dlg = new UserProfileDialog(string.Format(
                "Welcome {0}! It looks like you're new. Submit your profile info here.", user.UserId));
                bool result = await dlg.ShowAsync();
                if (result != false)
                {
                    string fnInput = dlg.FirstnameTb.Text;
                    string lnInput = dlg.LastnameTb.Text;

                    var currentUser = new Receiver
                    {
                        Id = Guid.NewGuid().ToString(),
                        Firstname = fnInput,
                        Lastname = lnInput,
                        UserId = user.UserId,                          
                    };

                    await InsertReciever(currentUser);
                    await RefreshTodoItems();
                }
            }
           
            await RefreshTodoItems();
        }

        private async Task InsertTodoItem(TodoItem todoItem)
        {
            // This code inserts a new TodoItem into the database. When the operation completes
            // and Mobile Services has assigned an Id, the item is added to the CollectionView
            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);

            //await SyncAsync(); // offline sync
        }

        private async Task InsertReciever(Receiver reciever)
        {
            await receiverTable.InsertAsync(reciever);
            receivers.Add(reciever);
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

            //await SyncAsync(); // offline sync
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
            //await RefreshTodoItems();
        }

        #region Offline sync

        private async Task InitLocalStoreAsync()
        {
            if (!App.MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("test8localstore.db");
                store.DefineTable<TodoItem>();
                store.DefineTable<Receiver>();
                store.DefineTable<ReceiverGroup>();
                store.DefineTable<Sender>();
                store.DefineTable<Announcement>();
                await App.MobileService.SyncContext.InitializeAsync(store);
            }

            await SyncAsync();
        }

        private async Task SyncAsync()
        {
            String errorString = null;

            try
            {

                await App.MobileService.SyncContext.PushAsync();
                await todoTable.PullAsync("todoItems", todoTable.CreateQuery()); // first param is query ID, used for incremental sync
            }
            catch (MobileServicePushFailedException ex)
            {
                errorString = "Push failed because of sync errors: " +
                  ex.PushResult.Errors.Count + " errors, message: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorString = "Pull failed: " + ex.Message +
                  "\n\nIf you are still in an offline scenario, " +
                  "you can try your Pull again when connected with your Mobile Serice.";
            }

            if (errorString != null)
            {
                MessageDialog d = new MessageDialog(errorString);
                await d.ShowAsync();
            }
        }

        #endregion
    }
}
