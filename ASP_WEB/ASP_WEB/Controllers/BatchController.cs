using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_WEB.Controllers
{
    public class BatchController : Controller
    {
        // GET: Batch
        public ActionResult Index(int id)
        {
            return RedirectToAction("Details","Project", new { id = id });
        }

        // GET: Batch/Details/5
        public ActionResult Details(int id)
        {
            Batch batch = new Batch();
            try
            {
                using (MyDatabaseEntities de = new MyDatabaseEntities())
                {
                    batch = de.Batches.Where(a => a.BatchID == id).FirstOrDefault();
                }
                return View(batch);
            }
            catch
            {
                return View(batch);
            }
        }

        // GET: Batch/Create
        public ActionResult Create(int id)
        {
            return View();
        }

        // POST: Batch/Create
        [HttpPost]
        public ActionResult Create(IEnumerable<HttpPostedFileBase> Images, Batch batch)
        {
            try
            {
                using (MyDatabaseEntities de = new MyDatabaseEntities())
                {
                    batch = de.Batches.Add(batch);
                    de.SaveChanges();
                }

                int id = batch.BatchID;
                var path = Path.Combine(Server.MapPath("~/Content/Projects/" + batch.ProjectID + "/" + id));

                foreach (HttpPostedFileBase file in Images)
                {
                    //HttpPostedFileBase file = Request.Files[fileName];
                    string fName = file.FileName;
                    string extension;
                    extension = Path.GetExtension(fName);

                    if (file != null && file.ContentLength > 0)
                    {
                        bool exists = System.IO.Directory.Exists(path);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(path);

                        string pathString = System.IO.Path.Combine(path.ToString());
                        var uploadpath = string.Format("{0}\\{1}", pathString, fName);
                        file.SaveAs(uploadpath);
                    }
                }

                return RedirectToAction("Details", new { id = batch.BatchID });
            }
            catch
            {
                return View(batch);
            }
        }
        // GET: Batch/Edit/5
        public ActionResult Edit(int id)
        {
            Batch batch = new Batch();
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                batch = de.Batches.Where(a => a.BatchID == id).FirstOrDefault();
            }
            return View(batch);
        }

        // POST: Batch/Edit/5
        [HttpPost]
        public ActionResult Edit(Batch batch)
        {
            var id = batch.BatchID;
            try
            {
                using (MyDatabaseEntities de = new MyDatabaseEntities())
                {
                    var b = de.Batches.Where(a => a.BatchID == id).FirstOrDefault();
                    b.StartDate = batch.StartDate;
                    b.DueDate = batch.DueDate;
                    b.CompleteDate = batch.CompleteDate;
                    b.BatchName = batch.BatchName;
                    b.Status = batch.Status;
                    de.SaveChanges();
                }
                return RedirectToAction("Details", "Project", new { id = batch.ProjectID});
            }
            catch
            {
                return View();
            }
        }

        // GET: Batch/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                using (MyDatabaseEntities de = new MyDatabaseEntities())
                {
                    var batch = de.Batches.Where(a => a.BatchID == id).FirstOrDefault();
                    de.Batches.Remove(batch);
                    de.SaveChanges();
                    return RedirectToAction("Details", "Project", new { id = batch.ProjectID });
                }
            }
            catch
            {
                return View();
            }
        }

        // POST: Batch/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
