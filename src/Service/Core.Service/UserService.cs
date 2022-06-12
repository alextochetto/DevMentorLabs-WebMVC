using Core.Contract;
using Core.RepositoryContract;
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
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AddUser(UserAddDTQ userAddQuery)
        {
            userAddQuery.Password = SaltCryptography.CreatePassword(userAddQuery.Password);
            await _userRepository.Add(userAddQuery);

            return await Task.FromResult(true);
        }

        public async Task<UserDTO?> GetUser(UserGetDTQ userGetQuery)
        {
            UserDTO user = await this._userRepository.Get(userGetQuery);
            if (user is null)
                return null;

            bool isValid = SaltCryptography.VerifyPassword(userGetQuery.Password, user.Password);
            if (!isValid)
                return null;

            return user;
        }
    }
}