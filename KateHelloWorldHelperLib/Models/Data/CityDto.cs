using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KateHelloWorldHelperLib.Models.Data
{
    public class CityDto
    {
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public string CityStateOrProvince { get; set; }
        public string CityCountry { get; set; }
        public string CityImgUri { get; set; }
        public float CityRating { get; set; }
    }
}
