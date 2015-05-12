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
    public class TodoItemController : TableController<TodoItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            siolReceiverContext context = new siolReceiverContext();
            DomainManager = new EntityDomainManager<TodoItem>(context, Request, Services, enableSoftDelete: true);
        }

       
        // GET tables/TodoItem
        public IQueryable<TodoItem> GetAllTodoItems()
        {
            var currentUser = User as ServiceUser;

            return Query().Where(todo => todo.UserId == currentUser.Id);
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TodoItem> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {                                      // Get the logged in user
            var currentUser = User as ServiceUser;

            // Set the user ID on the item
            item.UserId = currentUser.Id;

            TodoItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}