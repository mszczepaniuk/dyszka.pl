using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.BindingModels
{
    public class MessageBm
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public string ReceiverUserName { get; set; }
    }
}
