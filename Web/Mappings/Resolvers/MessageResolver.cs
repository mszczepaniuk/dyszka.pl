using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.BindingModels;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Services;
using AutoMapper;

namespace Web.Mappings.Resolvers
{
    public class MessageResolver : IValueResolver<MessageBm, Message, ApplicationUser>
    {
        private readonly IUserService userService;

        public MessageResolver(IUserService userService)
        {
            this.userService = userService;
        }

        public ApplicationUser Resolve(MessageBm source, Message destination, ApplicationUser destMember, ResolutionContext context)
        {
            var user = userService.GetAll().Where(u => u.UserName == source.ReceiverUserName).FirstOrDefault();
            if (user == null)
            {
                throw new ElementNotFoundException("User not found");
            }
            return user;
        }
    }
}
