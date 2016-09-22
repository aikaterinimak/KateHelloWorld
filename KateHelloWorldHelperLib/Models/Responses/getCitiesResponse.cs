using KateHelloWorldHelperLib.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KateHelloWorldHelperLib.Models.Responses
{
    public class getCitiesResponse
    {
        public List<CityDto> cities { get; set; }
    }
}
