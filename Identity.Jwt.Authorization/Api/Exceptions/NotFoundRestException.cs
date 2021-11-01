using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Exceptions
{
    public class NotFoundRestException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; }

        public NotFoundRestException(string message, IEnumerable<IdentityError> errors = null)
            : base(message)
        {
            Errors = errors;
        }
    }
}