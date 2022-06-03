using Core.Contract;
using Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.DTO.Core;

namespace Core.Service
{
    public class UserService : IUser
    {
        public async Task<bool> AddUser(UserAddDTQ userAddQuery)
        {
            userAddQuery.Password = SaltCryptography.CreatePassword(userAddQuery.Password);

            return await Task.FromResult(true);
        }
    }
}