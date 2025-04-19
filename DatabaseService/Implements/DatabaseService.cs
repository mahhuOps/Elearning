using Dapper;
using DatabaseService.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace DatabaseService.Implements
{
    public class DatabaseService : IDatabaseService
    {
        protected string? connectString;


        public DatabaseService(IConfiguration configuration)
        {
            connectString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection GetDbConnection()
        {
            return new MySqlConnection(connectString);
        }

        #region Execute
        public int ExecuteUsingCommandText(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

                var result = connection.Execute(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text, transaction: transaction);

                var result = connection.Execute(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database");
        }

        public async Task<int> ExecuteUsingCommandTextAsync(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

                var result = await connection.ExecuteAsync(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text, transaction: transaction);

                var result = await connection.ExecuteAsync(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public int ExecuteUsingStoredProcedure(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure);

                var result = connection.Execute(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = connection.Execute(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public async Task<int> ExecuteUsingStoredProcedureAsync(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure);

                var result = await connection.ExecuteAsync(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = await connection.ExecuteAsync(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }
        #endregion

        #region Query
        public List<Entity> QueryUsingCommanText<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

                var result = connection.Query<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result.ToList();
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text, transaction: transaction);

                var result = connection.Query<Entity>(command);

                return result.ToList();
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public async Task<List<Entity>> QueryUsingCommanTextAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

                var result = await connection.QueryAsync<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result.ToList();
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text, transaction: transaction);

                var result = await connection.QueryAsync<Entity>(command);

                return result.ToList();
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public List<Entity> QueryUsingStoredProcedure<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = connection.Query<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result.ToList();
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = connection.Query<Entity>(command);

                return result.ToList();
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public async Task<List<Entity>> QueryUsingStoredProcedureAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure);

                var result = await connection.QueryAsync<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result.ToList();
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = await connection.QueryAsync<Entity>(command);

                return result.ToList();
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public async Task<List<object>> QueryUsingCommanTextAsync(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

                var result = await connection.QueryAsync(command);

                connection.Dispose();
                connection.Close();
                return result.ToList();
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text, transaction: transaction);

                var result = await connection.QueryAsync(command);

                return result.ToList();
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public async Task<List<object>> QueryUsingStoredProcedureAsync(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure);

                var result = await connection.QueryAsync(command);

                connection.Dispose();
                connection.Close();
                return result.ToList();
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = await connection.QueryAsync(command);

                return result.ToList();
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }
        #endregion

        #region Query single
        public Entity QuerySingleUsingCommanText<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

                var result = connection.QueryFirstOrDefault<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text, transaction: transaction);

                var result = connection.QueryFirstOrDefault<Entity>(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public async Task<Entity> QuerySingleUsingCommanTextAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

                var result = await connection.QueryFirstOrDefaultAsync<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text, transaction: transaction);

                var result = await connection.QueryFirstOrDefaultAsync<Entity>(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public Entity QuerySingleUsingStoredProcedure<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure);

                var result = connection.QueryFirstOrDefault<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = connection.QueryFirstOrDefault<Entity>(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public async Task<Entity> QuerySingleUsingStoredProcedureAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure);

                var result = await connection.QueryFirstOrDefaultAsync<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = await connection.QueryFirstOrDefaultAsync<Entity>(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }
        #endregion

        #region Execute scalar
        public Entity ExecuteScalarUsingCommandText<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

                var result = connection.ExecuteScalar<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text, transaction: transaction);

                var result = connection.ExecuteScalar<Entity>(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public async Task<Entity> ExecuteScalarUsingCommandTextAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text);

                var result = await connection.ExecuteScalarAsync<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.Text, transaction: transaction);

                var result = await connection.ExecuteScalarAsync<Entity>(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public Entity ExecuteScalarUsingStoreProcedure<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure);

                var result = connection.ExecuteScalar<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(commandText, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = connection.ExecuteScalar<Entity>(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }

        public async Task<Entity> ExecuteScalarUsingStoreProcedureAsync<Entity>(string storeProcedure, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            if (connection == null && transaction == null)
            {
                connection = GetDbConnection();
                connection.Open();
                var command = new CommandDefinition(storeProcedure, param, commandType: CommandType.StoredProcedure);

                var result = await connection.ExecuteScalarAsync<Entity>(command);

                connection.Dispose();
                connection.Close();
                return result;
            }
            else if (connection != null && transaction != null)
            {
                var command = new CommandDefinition(storeProcedure, param, commandType: CommandType.StoredProcedure, transaction: transaction);

                var result = await connection.ExecuteScalarAsync<Entity>(command);

                return result;
            }

            throw new Exception("Truyền cả connection và transaction nếu muốn sử dụng custom luồng tương tác với database !");
        }
        #endregion
    }
}
