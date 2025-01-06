using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SABlog.ViewModels
{
    public class PostInfoViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int PostStatusId { get; set; }
        public string PostStatus { get; set; } // From PostStatu.Status
        public int UserId { get; set; }
        public string UserName { get; set; } // From User.UserName
        public DateTime CreatedAt { get; set; }
        public DateTime? PublicatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public UserViewModel User { get; set; }
        // Related entities
        public List<CategoryViewModel> PostCategories { get; set; }
        public List<PostFileViewModel> PostFiles { get; set; }

        // Additional data for User and PostStatus, if necessary
        public string UserFullName { get; set; } // User's full name (FirstName + LastName)
    }
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }  // Email of the user (optional)
        public bool EmailVerified { get; set; }  // Email verified flag (optional)
        public DateTime CreatedAt { get; set; }
    }
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } // Assuming Category model has a Name property
    }

    public class PostFileViewModel
    {
        public int FileId { get; set; }
        public string Content { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Deleted { get; set; }
    }

}