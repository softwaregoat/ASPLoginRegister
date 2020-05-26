using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_WEB.Models
{
    public class Export
    {
        public IEnumerable<Batch> batches { get; set; }
        public IEnumerable<BatchData> batchDatas { get; set; }
    }
}