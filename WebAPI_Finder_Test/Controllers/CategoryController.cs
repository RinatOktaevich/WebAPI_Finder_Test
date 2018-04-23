using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            if (name == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Name is null");
            }

            try
            {
                db.Categories.Add(new Category() { Name = name });
                await db.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (ex.HResult == -2146233087)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Category with name " + name + " already exist");
                }

            }


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
            db.Entry(cat).State = EntityState.Deleted;
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
            db.Entry(cat).State = EntityState.Modified;

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("ToList")]
        public IHttpActionResult GetCategories()
        {
            var cats = db.Categories.Select(x => new {x.Id,x.Name }).ToList();
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
        public async Task<HttpResponseMessage> BindCategory(string iduser, [ModelBinder] int[] categories)
        {
            var sd = Request;



            List<Category> cats = db.Categories.ToList();
            var user = db.Users.Find(iduser);
            Category temp = null;

            foreach (var item in categories)
            {
                temp = cats.Find(xr => xr.Id == item);
                user.Categories.Add(temp);
            }

            try
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                }

                Exception innEx = ex.InnerException;

                while (innEx.InnerException != null)
                {
                    innEx = innEx.InnerException;
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, innEx.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }



        [HttpPost]
        [Route("UnBindUser")]
        public async Task<HttpResponseMessage> UnBindCategory(string iduser, [ModelBinder] int[] categories)
        {
            List<Category> cats = db.Categories.ToList();
            var user = db.Users.Include(xr => xr.Categories).First(x => x.Id == iduser);
            Category temp = null;

            foreach (var item in categories)
            {
                temp = user.Categories.First(x => x.Id == item);

                user.Categories.Remove(temp);
            }

            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }



    }
}
