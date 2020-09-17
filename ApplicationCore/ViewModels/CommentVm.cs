using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ViewModels
{
    public class CommentVm
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsPositive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuthorUserName { get; set; }
        public string AuthorProfileImage { get; set; }
    }
}
