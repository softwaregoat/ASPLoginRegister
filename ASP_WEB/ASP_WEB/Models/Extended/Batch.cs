using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP_WEB.Models
{
    [MetadataType(typeof(BatchMetadata))]
    public partial class Batch
    {

    }
    public class BatchMetadata
    {
        [Display(Name = "Start Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start Date required")]
        [DataType(DataType.Date)]
        public System.DateTime StartDate { get; set; }

        [Display(Name = "Due Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Due Date required")]
        [DataType(DataType.Date)]
        public System.DateTime DueDate { get; set; }

        [Display(Name = "Complete Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CompleteDate { get; set; }

        [Display(Name = "Batch Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Batch Name required")]
        public string BatchName { get; set; }

    }
}