using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ASP_WEB.Models
{
    public class Zoning
    {
        public Config config { get; set; }
        public string[] GetData(string data)
        {
            return data.Split('|');
        }
        public void Populate()
        {
            this.FieldNum =Convert.ToInt32(config.FieldNum);
            this.MaxChar =  GetData(config.MaxChar);
            this.FieldNote =  GetData(config.FieldNote);
            this.CheckBox =  GetData(config.CheckBox);
            this.Alpha =  GetData(config.Alpha);
            this.Num =  GetData(config.Num);
            this.Dot =  GetData(config.Dot);
            this.Special =  GetData(config.Special);
            this.Custom =  GetData(config.Custom);
            this.Abbr =  GetData(config.Abbr);
            this.FullView = config.FullView!=null?GetData(config.FullView):null;
            this.FieldView = config.FieldView!=null?GetData(config.FieldView):null;
        }
        public string[] FieldView { get; set; }
        public string[] FullView { get; set; }
        public string[] FieldNote { get; set; }
        public string[] MaxChar { get; set; }
        public int FieldNum { get; set; }
        public string[] CheckBox { get; set; }
        public string[] Alpha { get; set; }
        public string[] Num { get; set; }
        public string[] Dot { get; set; }
        public string[] Special { get; set; }
        public string[] Custom { get; set; }
        public string[] Abbr { get; set; }
        public string ZoningFile { get; set; }

    }
}