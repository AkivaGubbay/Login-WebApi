using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Contracts
{
    public class UserRegisterResponse
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public string Username { get; set;  }
    }
}
