using Web.RepositoryContract;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Web.RepositoryService
{
    public class RepositorySqlServer : IRepository
    {
        private readonly IConfiguration _configuration;
        private IDbTransaction _transaction = null;
        private string _connectionString = null;

        public RepositorySqlServer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<long> ExecuteNonQuery(string sql, List<IDbDataParameter> parameters = null)
        {
            SqlConnection connection = CreateConnectionInternal();
            try
            {
                using (SqlCommand command = CreateCommandInternal(sql, connection))
                {
                    InsertParameters(command, parameters);
                    long affected = await command.ExecuteNonQueryAsync();
                    return (affected);
                }
            }
            finally
            {
                this.CloseConnection(connection);
            }
        }

        private SqlConnection CreateConnectionInternal()
        {
            if (string.IsNullOrEmpty(this._connectionString))
                this._connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(this._connectionString);
            connection.Open();
            return (connection);
        }

        public string GetConnectionString()
        {
            return (_configuration.GetSection("connectionString")).Value;
        }

        private SqlCommand CreateCommandInternal(string sql, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(sql, connection);
            // if (this.IsConnectionScoped(connection))
            //     command.Transaction = this._transaction as SqlTransaction;
            command.CommandTimeout = 0;
            return (command);
        }

        private void InsertParameters(SqlCommand command, List<IDbDataParameter> parameters = null, bool allowInjectParameters = false, bool allowInjectParametersDeclare = true)
        {
            if (parameters == null)
                return;
            StringBuilder sqlDeclare = new StringBuilder();
            bool canInjectParameters = (allowInjectParameters) && (this.CanInjectParameters(parameters, allowInjectParametersDeclare));
            for (int i = parameters.Count - 1; i >= 0; i--)
            {
                IDbDataParameter parameter = parameters[i];
                if ((canInjectParameters) && (this.CanInjectParameterType(parameter.DbType)))
                {
                    if (allowInjectParametersDeclare)
                    {
                        sqlDeclare.AppendLine($"DECLARE {parameter.ParameterName} AS {GetParameterType(parameter.DbType)} = {parameter.Value};");
                    }
                    else
                    {
                        command.CommandText = command.CommandText.Replace(parameter.ParameterName, parameter.Value.ToString());
                    }
                }
                else
                {
                    SqlParameter parameterSQL = command.CreateParameter();
                    parameterSQL.ParameterName = parameter.ParameterName;
                    parameterSQL.Value = parameter.Value;
                    parameterSQL.DbType = parameter.DbType;
                    command.Parameters.Add(parameterSQL);
                }
            }
            if (sqlDeclare.Length > 0)
                command.CommandText = sqlDeclare.ToString() + System.Environment.NewLine + command.CommandText;
        }

        private bool CanInjectParameters(List<IDbDataParameter> parameters, bool allowInjectParametersDeclare)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                IDbDataParameter parameter = parameters[i];
                if ((allowInjectParametersDeclare) && (!this.CanInjectParameterType(parameter.DbType)))
                    return (false);
                for (int j = 0; j < parameters.Count; j++)
                {
                    if (i == j)
                        continue;
                    IDbDataParameter parameterCheck = parameters[j];
                    if (parameter.ParameterName.Contains(parameterCheck.ParameterName))
                        return (false);
                }
            }
            return (true);
        }

        private bool CanInjectParameterType(DbType type)
        {
            if (type == DbType.Int32)
                return (true);
            if (type == DbType.Int64)
                return (true);
            return (false);
        }

        private string GetParameterType(DbType type)
        {
            if (type == DbType.Int32)
                return ("INT");
            if (type == DbType.Int64)
                return ("BIGINT");
            return (string.Empty);
        }

        private void CloseConnection(SqlConnection connection)
        {
            if ((this._transaction != null) && (this._transaction.Connection == connection))
                return;
            if (connection.State != ConnectionState.Open)
                return;
            connection.Close();
        }
    }
}