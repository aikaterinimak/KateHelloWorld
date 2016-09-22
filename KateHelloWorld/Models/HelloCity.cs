namespace KateHelloWorld.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HelloCity")]
    public partial class HelloCity
    {
        [Key]
        [Column(Order = 0)]
        public Guid UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid CityId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string Greeting { get; set; }

        public virtual City City { get; set; }

        public virtual User User { get; set; }
    }
}
