using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.DTO.Core;

namespace Core.Contract
{
    public interface IUser
    {
        Task<bool> AddUser(UserAddDTQ userAddQuery);
        Task<UserDTO?> GetUser(UserGetDTQ userGetQuery);
    }
}
