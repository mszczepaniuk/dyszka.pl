using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class ApplicationUser : BaseEntity
    {
        public string Description { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public string ProfileImage { get; set; }
        public IList<Offer> Offers { get; set; }
        public IList<Message> ReceivedMessages { get; set; }
        public IList<Message> SendMessages { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}
