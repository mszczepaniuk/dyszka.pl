﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.BindingModels;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using AutoMapper;
using Web.Services.Interfaces;

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
            var user = userService.GetByUserName(source.ReceiverUserName);
            if (user == null)
            {
                throw new ElementNotFoundException("User not found");
            }
            return user;
        }
    }
}
