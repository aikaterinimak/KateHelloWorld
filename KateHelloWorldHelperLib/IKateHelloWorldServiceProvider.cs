using KateHelloWorldHelperLib.Models.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KateHelloWorldHelperLib
{
    public interface IKateHelloWorldServiceProvider
    {
        Task<IList<CityDto>> getCityListAsync(CancellationToken aCancelationToken = default(CancellationToken));
        Task<IList<string>> getUserGreetingsForCity(string aUserId, string aCityId, CancellationToken aCancellationToken = default(CancellationToken));
        Task<string> postUserGreetingForCity(string aUserId, string aCityId, string aGreeting, CancellationToken aCancellationToken = default(CancellationToken));
        Task<float> getUserRatingForCity(string aUserId, string aCityId, CancellationToken aCancellationToken = default(CancellationToken));
        Task<string> postUserRatingForCity(string aUserId, string aCityId, float aRating, CancellationToken aCancellationToken = default(CancellationToken));
    }
}
