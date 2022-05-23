using System.Threading.Tasks;
using Web.DTO.Identity;

namespace Web.Contract.Identity
{
    public interface IIdentityAuthentication
    {
        Task<bool> SignIn(IdentityAuthenticateDTQ identityAuthenticateQuery);
    }
}