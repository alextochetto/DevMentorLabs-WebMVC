using Core.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Web.DTO.Core;
using Web.RepositoryContract;

namespace Core.RepositoryService
{
    public class UserRepositoryService : IUserRepository
    {
        private readonly IRepository _repository;

        public UserRepositoryService(IRepository repository)
        {
            _repository = repository;
        }

        private const string PARM_flgActive = "@flgActive";

        private void AddParameterCodUser(List<IDbDataParameter> parameters, long codUser)
        {
            parameters.Add(_repository.CreateParameter(PARM_flgActive, DbType.Int64, codUser));
        }

        public async Task<bool> Add(UserAddDTQ userAddQuery)
        {
            //_repository.ExecuteNonQuery()
            //List<IDbDataParameter> parameters = new List<IDbDataParameter>();
            //AddParameterDscEmail(parameters, providerUserSaveQuery.Email);
            //AddParameterDscPassword(parameters, providerUserSaveQuery.Password);
            //providerUserSaveQuery.Code = await _repository.ExecuteNonQueryIdentity(SQL_INSERT_USER, parameters);

            return true;
        }
    }
}
