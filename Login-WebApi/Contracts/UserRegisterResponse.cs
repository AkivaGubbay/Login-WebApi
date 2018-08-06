using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWebApi.Contracts
{
    public class UserRegisterResponse
    {
        //public string msg { get; set; }
        public UserRegisterResponse(string userName, string firstName, string lastName, string email)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }

        //Does it need to have the location of the created object?
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
