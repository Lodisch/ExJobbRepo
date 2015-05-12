using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using siolRecieverService.DataObjects;
using siolRecieverService.Models;

namespace siolRecieverService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            config.SetIsHosted(true);

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            //config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            Database.SetInitializer(new siolRecieverInitializer());
        }
    }

    public class siolRecieverInitializer : ClearDatabaseSchemaIfModelChanges<siolReceiverContext>
    {
        protected override void Seed(siolReceiverContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            List<Announcement> announcements = new List<Announcement>
            {
                new Announcement
                {
                    Id = Guid.NewGuid().ToString(), Title = "FirstTitle", Message = "FirstMessage", IsRead = false,
                }
            };

            foreach (Announcement announcement in announcements)
            {
                context.Set<Announcement>().Add(announcement);
            }

            List<ReceiverGroup> recieverGroups = new List<ReceiverGroup>
            {
                new ReceiverGroup
                {
                    Id = Guid.NewGuid().ToString(), Groupname = "DefaultGroup"
                }
            };

            foreach (ReceiverGroup recieverGroup in recieverGroups)
            {
                context.Set<ReceiverGroup>().Add(recieverGroup);
            }

            var defaultgroup = recieverGroups.FirstOrDefault(q => q.Groupname == "DefaultGroup");
            List<Receivers> recievers = new List<Receivers>
            {
                new Receivers
                {
                    Id = Guid.NewGuid().ToString(), Firstname = "Kalle", Lastname = "Anka", ReceiverGroup = defaultgroup
                }
            };

            foreach (Receivers reciever in recievers)
            {
                context.Set<Receivers>().Add(reciever);
            }

            List<Sender> senders = new List<Sender>
            {
                new Sender
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            foreach (var sender in senders)
            {
                context.Set<Sender>().Add(sender);
            }

            base.Seed(context);
        }
    }
}

