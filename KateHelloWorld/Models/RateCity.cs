namespace KateHelloWorld.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RateCity")]
    public partial class RateCity
    {
        [Key]
        [Column(Order = 0)]
        public Guid UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid CityId { get; set; }

        public double Rating { get; set; }

        public virtual City City { get; set; }

        public virtual User User { get; set; }
    }
}
