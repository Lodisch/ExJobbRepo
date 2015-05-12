using System.Linq;
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
    public class ReceiverGroupController : TableController<ReceiverGroup>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            siolReceiverContext context = new siolReceiverContext();
            DomainManager = new EntityDomainManager<ReceiverGroup>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/RecieverGroup
        public IQueryable<ReceiverGroup> GetAllRecieverGroup()
        {
            return Query(); 
        }

        // GET tables/RecieverGroup/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ReceiverGroup> GetRecieverGroup(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/RecieverGroup/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ReceiverGroup> PatchRecieverGroup(string id, Delta<ReceiverGroup> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/RecieverGroup
        public async Task<IHttpActionResult> PostRecieverGroup(ReceiverGroup item)
        {
            ReceiverGroup current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/RecieverGroup/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteRecieverGroup(string id)
        {
             return DeleteAsync(id);
        }

    }
}