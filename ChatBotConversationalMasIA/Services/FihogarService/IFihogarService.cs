using System.Threading.Tasks;
using Models;

namespace Services.FihogarService
{
    public interface IFihogarService{
        Task<AccessToken> AccessToken(string grantType, string token);
        Task<AccessTokenExtended> AuthorizeProvider(string provider, string username, string password, string grantType, string token);
        Task<AccountDetails> GetAccount();
    }

}