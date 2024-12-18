﻿using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MongoAuthenticatorAPI.Models
{
    [CollectionName("User")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string CurrentRole { get; internal set; }
    }
}

