﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using siolRecieverService.DataObjects;
using siolRecieverService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace siolRecieverService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class SenderController : TableController<Sender>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            siolReceiverContext context = new siolReceiverContext();
            DomainManager = new EntityDomainManager<Sender>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/Sender
        public IQueryable<Sender> GetAllSender()
        {
            return Query(); 
        }

        // GET tables/Sender/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Sender> GetSender(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Sender/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Sender> PatchSender(string id, Delta<Sender> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Sender
        public async Task<IHttpActionResult> PostSender(Sender item)
        {
            Sender current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Sender/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSender(string id)
        {
             return DeleteAsync(id);
        }

    }
}