//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SABlog.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PostFile
    {
        public int FileId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    
        public virtual Post Post { get; set; }
    }
}