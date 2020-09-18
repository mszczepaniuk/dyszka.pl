using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models;

namespace ApplicationCore.ViewModels
{
    public class MessageVm
    {
        public string AuthorUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReceiverUserName { get; set; }
        public string Text { get; set; }
    }
}
