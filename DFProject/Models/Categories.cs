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
    
    public partial class Categories
    {
        public Categories()
        {
            this.Contents = new HashSet<Contents>();
        }
    
        public string CategoryName { get; set; }
        public int Id { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<bool> CategoryStatus { get; set; }
        public Nullable<int> CategoryOrder { get; set; }
        public Nullable<int> LangId { get; set; }
    
        public virtual ICollection<Contents> Contents { get; set; }
    }
}
