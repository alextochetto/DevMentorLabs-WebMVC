using System;
using System.Threading.Tasks;
using Web.DTO.Core;

namespace Core.RepositoryContract
{
    public interface IUserRepository
    {
        Task<bool> Add(UserAddDTQ userAddQuery);
    }
}
