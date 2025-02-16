﻿// -----------------------------------------------------------------------
// Copyright (c) MumsWhoCode. All rights reserved.
// -----------------------------------------------------------------------

using Xeptions;

namespace ArtGallery.Services.Api.Models.Artists.Exceptions
{
    public class ArtistDependencyException : Xeption
    {
        public ArtistDependencyException(Xeption innerException)
            : base(message: "Artist dependency error occured, contract support.", innerException)
        { }
    }
}
