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
    
    public partial class News
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public News()
        {
            this.News_Tags = new HashSet<News_Tags>();
        }
    
        public decimal Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string author { get; set; }
        public string image { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News_Tags> News_Tags { get; set; }
    }
}