using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.BindingModels
{
    public class CommentBm
    {
        [Required]
        [MaxLength(160)]
        public string Text { get; set; }
        public bool IsPositive { get; set; }
        [Required]
        public Guid OfferId { get; set; }
    }
}
