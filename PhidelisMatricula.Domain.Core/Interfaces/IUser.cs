using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PhidelisMatricula.Domain.Core.Interfaces
{
    public interface IUser
    {
        public Guid Id { get; }
        string Name { get; }
        bool IsAdmin { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
