using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using siolRecieverService.DataObjects;
using siolRecieverService.Models;

namespace siolRecieverService.Controllers
{
   [AuthorizeLevel(AuthorizationLevel.User)]
    public class ReceiverController : TableController<Receiver>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            siolRecieverContext context = new siolRecieverContext();
            DomainManager = new EntityDomainManager<Receiver>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/Receiver
        public IQueryable<Receiver> GetAllReceiver()
        {
            return Query(); 
        }

        // GET tables/Receiver/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Receiver> GetReceiver(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Receiver/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Receiver> PatchReceiver(string id, Delta<Receiver> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Receiver
        public async Task<IHttpActionResult> PostReceiver(Receiver item)
        {
            Receiver current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Receiver/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteReceiver(string id)
        {
             return DeleteAsync(id);
        }

    }
}