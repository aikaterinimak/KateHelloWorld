using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KateHelloWorldHelperLib.Models.Requests
{
    public class postUserRatingRequest
    {
        public float rating { get; set; }

        public postUserRatingRequest(float aRating)
        {
            rating = aRating;
        }
    }
}
