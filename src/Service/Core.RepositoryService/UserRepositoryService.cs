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

        #region SQL
        private const string SQL_SELECT_USER_ACTIVE_BY_EMAIL = "SELECT codUser,dscFirstName,dscMiddleName,dscLastName,dscEmail,dscPassword,flgActive FROM I_USER WHERE dscEmail = @dscEmail AND flgActive = 1";
        private const string SQL_INSERT_USER = "INSERT INTO I_USER (dscFirstName,dscMiddleName,dscLastName,dscEmail,dscPassword,flgActive) VALUES (@dscFirstName,@dscMiddleName,@dscLastName,@dscEmail,@dscPassword,@flgActive)";
        #endregion

        #region Parameters
        private const string PARM_dscFirstName = "@dscFirstName";
        private const string PARM_dscMiddleName = "@dscMiddleName";
        private const string PARM_dscLastName = "@dscLastName";
        private const string PARM_dscEmail = "@dscEmail";
        private const string PARM_dscPassword = "@dscPassword";
        private const string PARM_flgActive = "@flgActive";

        private void AddParameterFirstName(List<IDbDataParameter> parameters, string firstName)
        {
            parameters.Add(_repository.CreateParameter(PARM_dscFirstName, DbType.String, firstName));
        }
        private void AddParameterMiddleName(List<IDbDataParameter> parameters, string middleName)
        {
            parameters.Add(_repository.CreateParameter(PARM_dscMiddleName, DbType.String, middleName));
        }
        private void AddParameterLastName(List<IDbDataParameter> parameters, string lastName)
        {
            parameters.Add(_repository.CreateParameter(PARM_dscLastName, DbType.String, lastName));
        }
        private void AddParameterEmail(List<IDbDataParameter> parameters, string email)
        {
            parameters.Add(_repository.CreateParameter(PARM_dscEmail, DbType.String, email));
        }
        private void AddParameterPassword(List<IDbDataParameter> parameters, string password)
        {
            parameters.Add(_repository.CreateParameter(PARM_dscPassword, DbType.String, password));
        }
        private void AddParameterActive(List<IDbDataParameter> parameters, int flgActive)
        {
            parameters.Add(_repository.CreateParameter(PARM_flgActive, DbType.Int32, flgActive));
        }
        #endregion

        public async Task<bool> Add(UserAddDTQ userAddQuery)
        {
            string[] nameSplit = userAddQuery.Name.Split(" ");
            string firstName = string.Empty;
            string middleName = string.Empty;
            string lastName = string.Empty;
            if (nameSplit.Length == 1)
            {
                firstName = nameSplit[0];
            }
            else if (nameSplit.Length == 2)
            {
                firstName = nameSplit[0];
                lastName = nameSplit[1];
            }
            else if (nameSplit.Length >= 3)
            {
                firstName = nameSplit[0];
                middleName = nameSplit[1];
                lastName = nameSplit[2];
            }

            List<IDbDataParameter> parameters = new List<IDbDataParameter>();
            AddParameterFirstName(parameters, firstName);
            AddParameterMiddleName(parameters, middleName);
            AddParameterLastName(parameters, lastName);
            AddParameterPassword(parameters, userAddQuery.Password);
            AddParameterEmail(parameters, userAddQuery.Email);
            AddParameterActive(parameters, 1);
            await _repository.ExecuteNonQuery(SQL_INSERT_USER, parameters);

            return true;
        }

        public async Task<UserDTO> Get(UserGetDTQ userGetQuery)
        {
            UserDTO user = null;
            List<IDbDataParameter> parameters = new List<IDbDataParameter>();
            AddParameterEmail(parameters, userGetQuery.Email);
            IDataReader reader = await _repository.GetReader(SQL_SELECT_USER_ACTIVE_BY_EMAIL, parameters);
            if (reader.Read())
            {
                user = new UserDTO();
                user.Code = reader.GetInt64(0);
                user.FirstName = reader.GetString(1);
                user.MiddleName = reader.GetString(2);
                user.LastName = reader.GetString(3);
                user.Email = reader.GetString(4);
                user.Password = reader.GetString(5);
                user.Active = reader.GetInt32(6);
            }
            return user;
        }
    }
}
