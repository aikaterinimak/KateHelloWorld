using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KateHelloWorldHelperLib.Models.Requests
{
    public class postUserGreetingRequest
    {
        public string greeting { get; set; }

        public postUserGreetingRequest(string aGreeting)
        {
            greeting = aGreeting;
        }
    }
}
