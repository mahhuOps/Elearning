using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseService.Interfaces
{
    public interface IDatabaseService
    {
        public IDbConnection GetDbConnection();

        #region Query
        public List<Entity> QueryUsingCommanText<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<Entity>> QueryUsingCommanTextAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public List<Entity> QueryUsingStoredProcedure<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<Entity>> QueryUsingStoredProcedureAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<object>> QueryUsingCommanTextAsync(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<object>> QueryUsingStoredProcedureAsync(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);
        #endregion

        #region Execute
        public int ExecuteUsingCommandText(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<int> ExecuteUsingCommandTextAsync(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public int ExecuteUsingStoredProcedure(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<int> ExecuteUsingStoredProcedureAsync(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);
        #endregion

        #region Query single
        public Entity QuerySingleUsingCommanText<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<Entity> QuerySingleUsingCommanTextAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public Entity QuerySingleUsingStoredProcedure<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<Entity> QuerySingleUsingStoredProcedureAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);
        #endregion

        #region Execute scalar
        public Entity ExecuteScalarUsingCommandText<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<Entity> ExecuteScalarUsingCommandTextAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);
        public Entity ExecuteScalarUsingStoreProcedure<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<Entity> ExecuteScalarUsingStoreProcedureAsync<Entity>(string commandText, Dictionary<string, object> param = null, IDbTransaction transaction = null, IDbConnection connection = null);
        #endregion
    }
}
