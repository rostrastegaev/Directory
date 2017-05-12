﻿using System.Security.Claims;

namespace Auth
{
    public class AuthRequest : IAuthRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Confirmation { get; set; }
    }
}
