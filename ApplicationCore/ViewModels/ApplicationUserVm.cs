using System;

namespace ApplicationCore.ViewModels
{
    public class ApplicationUserVm
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public string ProfileImage { get; set; }
    }
}