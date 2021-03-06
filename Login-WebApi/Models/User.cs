﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWebApi.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserPassword { get; set; }

        public override string ToString()
        {
            return "UserName: " + UserName +
                "\nFirstName: " + FirstName +
                "\nLastName: " + LastName +
                "\nEmail: " + Email +
                "\nUserPassword: " + UserPassword;
        }
    }
}
