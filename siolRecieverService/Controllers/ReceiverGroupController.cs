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
    public class ReceiverGroupController : TableController<ReceiverGroup>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            siolRecieverContext context = new siolRecieverContext();
            DomainManager = new EntityDomainManager<ReceiverGroup>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/ReceiverGroup
        public IQueryable<ReceiverGroup> GetAllReceiverGroup()
        {
            return Query(); 
        }

        // GET tables/ReceiverGroup/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ReceiverGroup> GetReceiverGroup(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ReceiverGroup/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ReceiverGroup> PatchReceiverGroup(string id, Delta<ReceiverGroup> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/ReceiverGroup
        public async Task<IHttpActionResult> PostReceiverGroup(ReceiverGroup item)
        {
            ReceiverGroup current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ReceiverGroup/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteReceiverGroup(string id)
        {
             return DeleteAsync(id);
        }

    }
}