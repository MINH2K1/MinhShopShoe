﻿using ShopShoe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Application.ViewModel.Query
{
    public class AppUserViewModel
    {
        public AppUserViewModel()
        {
            Roles = new List<string>();
        }
        public Guid? Id { set; get; }
        public string? FullName { set; get; }
        public string? BirthDay { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string? UserName { set; get; }
        public string? Address { get; set; }
        public string? PhoneNumber { set; get; }
        public string? Avatar { get; set; }
        public Status Status { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? Gender { get; set; }

        public DateTime? DateCreated { get; set; }

        public List<string>? Roles { get; set; }
    }
}
