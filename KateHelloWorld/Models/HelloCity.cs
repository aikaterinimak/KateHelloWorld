//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KateHelloWorld.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HelloCity
    {
        public System.Guid UserId { get; set; }
        public System.Guid CityId { get; set; }
        public string Greeting { get; set; }
        public System.DateTime GreetingDateTime { get; set; }
    
        public virtual City City { get; set; }
        public virtual User User { get; set; }
    }
}
