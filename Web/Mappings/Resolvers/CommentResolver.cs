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
    public class CommentResolver : IValueResolver<CommentBm, Comment, Offer>
    {
        private readonly IOfferService offerService;

        public CommentResolver(IOfferService offerService)
        {
            this.offerService = offerService;
        }

        public Offer Resolve(CommentBm source, Comment destination, Offer destMember, ResolutionContext context)
        {
            var offer = offerService.GetById(source.OfferId);
            if (offer == null)
            {
                throw new ElementNotFoundException("Comment not found");
            }
            return offerService.GetById(source.OfferId);
        }
    }
}
