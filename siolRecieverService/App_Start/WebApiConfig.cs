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

           

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            Database.SetInitializer(new siolRecieverInitializer());
        }
    }

    public class siolRecieverInitializer : ClearDatabaseSchemaIfModelChanges<siolRecieverContext>
    {
        protected override void Seed(siolRecieverContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            List<ReceiverGroup> groups = new List<ReceiverGroup>
            {
                new ReceiverGroup { Id = Guid.NewGuid().ToString(), Groupname = "DefaultGroup"}
            };

            var defaultGroup = groups.FirstOrDefault(q => q.Groupname == "DefaultGroup");
            List<Receiver> receivers = new List<Receiver>
            {
                new Receiver { Id = Guid.NewGuid().ToString(), Firstname = "Kalle", Lastname = "Anka", ReceiverGroup = defaultGroup}
            };

            List<Announcement> announcements = new List<Announcement>
            {
                new Announcement { Id = Guid.NewGuid().ToString(), Title = "First Title", Message = "First Message", IsRead = false }
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem); 
            }

            foreach (ReceiverGroup group in groups)
            {
                context.Set<ReceiverGroup>().Add(group);
            }

            foreach (Receiver receiver in receivers)
            {
                context.Set<Receiver>().Add(receiver);
            }
            
            foreach (Announcement announcement in announcements)
            {
                context.Set<Announcement>().Add(announcement);
            }

            base.Seed(context);

            
        }
    }
}

