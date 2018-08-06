using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LoginWebApi.Models
{
    public class GlobalParmeters
    {
        public static readonly int VALID = 1, NOT_VAID = 0, ENCOUNTERED_EXCEPTION = -1;
        public readonly static int RESPONSE_OK = 0, RESPONSE_USERERROR = 1, RESPONSE_SERVERERROR = 2;
    }
}
