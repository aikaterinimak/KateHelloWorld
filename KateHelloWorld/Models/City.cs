namespace KateHelloWorld.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("City")]
    public partial class City
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public City()
        {
            HelloCities = new HashSet<HelloCity>();
            RateCities = new HashSet<RateCity>();
        }

        public Guid CityId { get; set; }

        [Required]
        [StringLength(50)]
        public string CityName { get; set; }

        [StringLength(50)]
        public string CityStateOrProvince { get; set; }

        [Required]
        [StringLength(50)]
        public string CityCountry { get; set; }

        [StringLength(100)]
        public string CityImgUri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HelloCity> HelloCities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RateCity> RateCities { get; set; }
    }
}
