using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Exceptions
{
    public class BadRequestRestException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; }
        
        public BadRequestRestException(string message, IEnumerable<IdentityError> errors = null)
            : base(message)
        {
            Errors = errors;
        }
    }
}