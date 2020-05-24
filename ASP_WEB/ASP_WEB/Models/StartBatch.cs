using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_WEB.Models
{
    public class StartBatch : Zoning
    {
        public string[] Images { get; set; }
        public string ProjectName { get; set; }
        public string BatchName { get; set; }
        public int BatchID { get; set; }
        public void GetProjectName()
        {
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var project = de.Projects.Where(a => a.ProjectID == this.config.ProjectID).FirstOrDefault();
                this.ProjectName = project.ProjectName;
            }
        }
    }
}