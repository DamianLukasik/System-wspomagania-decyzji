//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CrystalSiege.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class News_Tags
    {
        public decimal Id { get; set; }
        public Nullable<decimal> NewsID { get; set; }
        public string TagsID { get; set; }
    
        public virtual News News { get; set; }
        public virtual Tags Tags { get; set; }
    }
}