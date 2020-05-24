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
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {
            List<Project> projects = new List<Project>();
            var userEmail = System.Web.HttpContext.Current.User.Identity.Name;
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var user1 = de.Users.Where(a => a.Email == userEmail).FirstOrDefault();
                projects = de.Projects.Where(a => a.CompanyID == user1.CompanyID).ToList();
            }
            return View(projects);
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            List<Batch> batches = new List<Batch>();
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                batches = de.Batches.Where(a => a.ProjectID == id).ToList();
            }
            return View(batches);
        }

        // GET: Project/EditBatch/5
        public ActionResult EditBatch(int id)
        {
            return RedirectToAction("Edit", "Batch", new { id = id });
        }

        // GET: Project/DetailsBatch/5
        public ActionResult DetailsBatch(int id)
        {
            return RedirectToAction("Details", "Batch", new { id = id });
        }

        // GET: Project/DeleteBatch/5
        public ActionResult DeleteBatch(int id)
        {
            return RedirectToAction("Delete", "Batch", new { id = id });
        }

        // GET: Project/CreateBatch
        public ActionResult CreateBatch(int id)
        {
            return RedirectToAction("Create", "Batch", new { id = id });
        }
        public ActionResult StartBatch(int id)
        {
            TempData["Batch-" + id] = DateTime.Now;
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var batch = de.Batches.Where(a => a.BatchID == id).FirstOrDefault();
                batch.Timer = 0;
                de.SaveChanges();
                var config = de.Configs.Where(a => a.ProjectID == batch.ProjectID).FirstOrDefault();
                StartBatch start = new StartBatch();
                start.config = config;
                start.Populate();
                start.GetProjectName();
                start.BatchName = batch.BatchName;
                start.BatchID = batch.BatchID;
                var path = Path.Combine(Server.MapPath("~/Content/Projects/" + batch.ProjectID + "/" + id));
                bool exists = System.IO.Directory.Exists(path);
                if (exists)
                {
                    start.Images = Directory.GetFiles(path);
                    for (int i = 0; i < start.Images.Length; i++)
                    {
                        string filepath = start.Images[i];
                        start.Images[i] = "/Content/Projects/" + batch.ProjectID + "/" + id + "/"+ Path.GetFileName(filepath);
                    }
                }
                    
                return View(start);
            }
        }
        [HttpPost]
        public ActionResult StartBatch()
        {
            int id = Convert.ToInt32(Request.Url.Segments.Last());
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var batch = de.Batches.Where(a => a.BatchID == id).FirstOrDefault();
                batch.Timer = 0;
                de.SaveChanges();
            }
            TempData["Batch-" + id] = DateTime.Now;
            return Json("SUCCESS to Start");
        }
        [HttpPost]
        public ActionResult StopBatch(int id)
        {
            int Timer = 0;
            if (TempData["Batch-" + id] != null)
            {
                var data = (DateTime)TempData["Batch-" + id];
                Timer = (int)(DateTime.Now - data).TotalSeconds;
            }
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var batch = de.Batches.Where(a => a.BatchID == id).FirstOrDefault();
                batch.Timer = Timer;
                de.SaveChanges();
            }
            return Json("SUCCESS to Stop at " + Timer + "Seconds");
        }
        [HttpPost]
        public ActionResult ResumeBatch(int id)
        {
            TempData["Batch-" + id] = DateTime.Now;
            return Json("SUCCESS to resume");
        }
        [HttpPost]
        public ActionResult VerifyBatch(int id)
        {
            int? timer = 0;
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var batch = de.Batches.Where(a => a.BatchID == id).FirstOrDefault();
                timer = batch.Timer;
            }
            return Json("SUCCESS to verify: Total Timer " + timer);
        }
        [HttpPost]
        public ActionResult CorrectBatch(int id)
        {
            int? timer = 0;
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var batch = de.Batches.Where(a => a.BatchID == id).FirstOrDefault();
                timer = batch.Timer;
            }
            return Json("SUCCESS to correct: Total Timer " + timer);
        }
        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(Project project)
        {
            try
            {
                // TODO: Add insert logic here
                var userEmail = System.Web.HttpContext.Current.User.Identity.Name;
                using (MyDatabaseEntities de = new MyDatabaseEntities())
                {
                    var user = de.Users.Where(a => a.Email == userEmail).FirstOrDefault();
                    project.CompanyID = user.CompanyID;
                    de.Projects.Add(project);
                    de.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            Project project = new Project();
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                project = de.Projects.Where(a => a.ProjectID == id).FirstOrDefault();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(Project project)
        {
            try
            {
                var userEmail = System.Web.HttpContext.Current.User.Identity.Name;
                using (MyDatabaseEntities de = new MyDatabaseEntities())
                {
                    var user = de.Users.Where(a => a.Email == userEmail).FirstOrDefault();
                    project.CompanyID = user.CompanyID;
                    de.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var project = de.Projects.Where(a => a.ProjectID == id).FirstOrDefault();
                de.Projects.Remove(project);
                de.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: Project/Config/5
        public ActionResult Config(int id)
        {
            Config config = new Config();
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                config = de.Configs.Where(a => a.ProjectID == id).FirstOrDefault();
            }
            if (config!=null)
            {
                return View(config);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditConfig(Config config)
        {
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var con = de.Configs.Where(a => a.ProjectID == config.ProjectID).FirstOrDefault();
                if (con!=null)
                {
                    con.MaxChar = config.MaxChar;
                    con.FieldNote = config.FieldNote;
                    con.CheckBox = config.CheckBox;
                    con.Alpha = config.Alpha;
                    con.Num = config.Num;
                    con.Dot = config.Dot;
                    con.Special = config.Special;
                    con.Custom = config.Custom;
                    con.Abbr = config.Abbr;
                    con.FieldNum = config.FieldNum;
                }
                else
                {
                    de.Configs.Add(config);
                }
                de.SaveChanges();
            }
            return RedirectToAction("Details", new { id = config.ProjectID });
        }

        public ActionResult UploadBatch(int id)
        {
            return View();
        }
        // POST: Project/UploadBatch/5
        [HttpPost]
        public ActionResult UploadBatch(IEnumerable<HttpPostedFileBase> Images, DateTime StartDate, DateTime DueDate, string Status)
        {
            var projectID = Request.Url.Segments.Last();
            string fName = "";
            try
            {
                foreach (HttpPostedFileBase file in Images)
                {
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = Path.Combine(Server.MapPath("~/Content/UploadBatch/" + projectID));
                        bool exists = System.IO.Directory.Exists(path);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(path);

                        string pathString = System.IO.Path.Combine(path.ToString());
                        var fileName1 = Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var uploadpath = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(uploadpath);
                        Batch batch = new Batch();
                        batch.BatchName = Path.GetFileNameWithoutExtension(fName);
                        batch.ProjectID =Convert.ToInt32(projectID);
                        batch.StartDate = StartDate;
                        batch.DueDate = DueDate;
                        batch.Status = Status;

                        using (MyDatabaseEntities de = new MyDatabaseEntities())
                        {
                            de.Batches.Add(batch);
                        }

                        //string[] lines = System.IO.File.ReadAllLines(uploadpath);
                        //foreach (string line in lines)
                        //{
                        //    // Use a tab to indent each line of the file.
                        //    Console.WriteLine("\t" + line);
                        //}

                    }
                }
            }
            catch (Exception)
            {
            }
            return RedirectToAction("Details", "Project", new { id = projectID });
        }
        public ActionResult ZONING(int id)
        {
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var con = de.Configs.Where(a => a.ProjectID == id).FirstOrDefault();
                Zoning zoning = new Zoning();
                zoning.config = con;
                zoning.Populate();
                var path = Path.Combine(Server.MapPath("~/Content/Zoning"));
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    var filename = Path.GetFileName(file).ToString();
                    var extension = Path.GetExtension(file);
                    if (filename.Replace(extension, "")==id.ToString())
                    {
                        zoning.ZoningFile = "/Content/Zoning/" + filename;
                    }
                }
                return View(zoning);
            }
        }
        [HttpPost]
        public ActionResult ZONING(FormCollection formCollection)
        {
            int id =Convert.ToInt32(formCollection["ProjectID"]);

            HttpPostedFileBase file = Request.Files["ImgUpload"];
            string fName = file.FileName;
            string extension;
            extension = Path.GetExtension(fName);
            var path = Path.Combine(Server.MapPath("~/Content/Zoning"));
            if (file != null && file.ContentLength > 0)
            {
                
                bool exists = System.IO.Directory.Exists(path);
                if (!exists)
                    System.IO.Directory.CreateDirectory(path);

                string pathString = System.IO.Path.Combine(path.ToString());
                var uploadpath = string.Format("{0}\\{1}", pathString, id + extension);
                file.SaveAs(uploadpath);
            }

            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var con = de.Configs.Where(a => a.ProjectID == id).FirstOrDefault();
                con.FullView = formCollection["FullView"];
                con.FieldView = formCollection["FieldView"];
                de.SaveChanges();

                return RedirectToAction("Details", "Project", new { id = id});
            }
        }
        public ActionResult UploadImage(int id)
        {
            return View();
        }
        // POST: Project/UploadImage/5
        [HttpPost]
        public ActionResult UploadImage()
        {
            var projectID = Request.Url.Segments.Last();
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = Path.Combine(Server.MapPath("~/Content/Image/" + projectID));
                        bool exists = System.IO.Directory.Exists(path);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(path);

                        string pathString = System.IO.Path.Combine(path.ToString());
                        var fileName1 = Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var uploadpath = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(uploadpath);
                    }
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new
                {
                    Message = fName
                });
            }
            else
            {
                return Json(new
                {
                    Message = "Error in saving file"
                });
            }
        }
    }
}
