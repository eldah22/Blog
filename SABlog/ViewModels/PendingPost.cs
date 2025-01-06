using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SABlog.ViewModels
{
    public class PendingPost
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public int FileNumber { get; set; }
        public string AuthorUsername { get; set; }
        public string DatePosted { get; set; }
        public string PostStatus { get; set; }


        public PendingPost(int id, string t, int fn, string au, string dp, string sts)
        {
            PostId = id;
            Title = t;
            FileNumber = fn;
            AuthorUsername = au;
            DatePosted = dp;
            PostStatus = sts;

        }

        public PendingPost()
        {

        }

    }
}