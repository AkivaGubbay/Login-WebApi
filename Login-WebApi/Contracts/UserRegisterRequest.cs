using LoginWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWebApi.Contracts
{
    public class UserRegisterRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserPassword { get; set; }


        /*public void RequestValidation()
        {
            EmailValidation();

        }
        private void EmailValidation()
        {
            
        }
        */
    }
}
