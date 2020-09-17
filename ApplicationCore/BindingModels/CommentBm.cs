using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.BindingModels
{
    public class CommentBm
    {
        public string Text { get; set; }
        public bool IsPositive { get; set; }
        public Guid OfferId { get; set; }
    }
}
