using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using siolRecieverService.DataObjects;
using siolRecieverService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using siolRecieverService.DataObjects.DataTransferObjects;

namespace siolRecieverService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class ReceiverController : TableController<Receivers>
    {
        public siolReceiverContext context = new siolReceiverContext();
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DomainManager = new EntityDomainManager<Receivers>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/Reciever
        public IQueryable<Receivers> GetAllReceiver()
        {
            return Query();
        }

        // GET tables/Reciever/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Receivers> GetReceiver(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Reciever/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Receivers> PatchReceiver(string id, Delta<Receivers> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Reciever
        public async Task<IHttpActionResult> PostReceiver(Receivers item)
        {           
            Receivers current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Reciever/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteReceiver(string id)
        {
             return DeleteAsync(id);
        }

    }
}