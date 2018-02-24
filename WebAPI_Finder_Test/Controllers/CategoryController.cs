using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using WebAPI_Finder_Test.Models;

namespace WebAPI_Finder_Test.Controllers
{
    [Authorize]
    [RoutePrefix("api/Category")]
    public class CategoryController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("Add")]
        public async Task<HttpResponseMessage> AddCategory(string name)
        {
            if(name==null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Name is null");
            }


            db.Categories.Add(new Category() { Name = name });
            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }



        [HttpPost]
        [Route("Delete")]
        public async Task<HttpResponseMessage> DeleteCategory(int idcategory)
        {
            var cat = db.Categories.Find(idcategory);

            if (cat == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Category Id doesn`t exist");
            }

            db.Categories.Remove(cat);
            db.Entry(cat).State = System.Data.Entity.EntityState.Deleted;
            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }



        [HttpPost]
        [Route("Rename")]
        public async Task<HttpResponseMessage> RenameCategory(int idcategory, string name)
        {
            if (name == null)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "Name is null");
            }

            var cat = db.Categories.Find(idcategory);

            if (cat == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Category Id doesn`t exist");
            }

            cat.Name = name;
            db.Entry(cat).State = System.Data.Entity.EntityState.Modified;

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }



        [HttpGet]
        [Route("ToList")]
        public IHttpActionResult GetCategories()
        {
            var cats = db.Categories.ToList();
            return Ok(cats);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="iduser">Пользователь которого мы хотим присвоить какой либо категории</param>
        /// <param name="categories">Коллекция категорий которые мы присвоим пользоавтелю</param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindUser")]
        public async Task<HttpResponseMessage> RenameCategory(string iduser, [ModelBinder] int[] categories)
        {
            List<Category> cats = db.Categories.ToList();
            var user = db.Users.Find(iduser);
            Category temp = null;

            foreach (var item in categories)
            {
                temp = cats.Find(xr => xr.Id == item);
                user.Categories.Add(temp);
            }

            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }



    }
}
