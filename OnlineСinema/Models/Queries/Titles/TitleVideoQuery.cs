﻿using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Internal.Titles;
using System.Security.Claims;

namespace OnlineСinema.Models.Queries.Titles
{
    public class TitleVideoQuery:IRequestModel<TitleVideoModel>
    {
        public Guid Id { get; set; }

        public ClaimsPrincipal Principal { get; set; }
    }
}
