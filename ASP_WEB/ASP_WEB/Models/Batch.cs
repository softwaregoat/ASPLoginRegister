//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP_WEB.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Batch
    {
        public int BatchID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime DueDate { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public string BatchName { get; set; }
        public string Status { get; set; }
        public Nullable<int> Timer { get; set; }
        public int ProjectID { get; set; }
    }
}