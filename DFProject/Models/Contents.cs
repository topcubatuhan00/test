//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DFProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public partial class Contents
    {
        [AllowHtml]
        public string ContentBody { get; set; }
        public Nullable<int> ContentViewCount { get; set; }
        public int CategoryId { get; set; }
        public string ContentTitle { get; set; }
        public int Id { get; set; }
        public string ContentSummary { get; set; }
        public Nullable<int> ContentOrder { get; set; }
        public Nullable<int> LangId { get; set; }
    
        public virtual Categories Categories { get; set; }
    }
}
