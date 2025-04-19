using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Common.Extension;
using Common.CustomAttributes;
using DatabaseService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using Common.Models;
using DatabaseService.Enums;
using Microsoft.AspNetCore.Http;
using Common.Extensions;
using System.Collections;

namespace Core.Implements
{
    public class BaseBL<T> : IBaseBL<T>
    {
        protected IDatabaseService _dataBaseService;
        private static string? _tableName;
        private static List<string>? _columnSearch;
        private static bool? _isMater;
        private static PropertyInfo? _primaryKeyPropertyInfor;
        private static string? _fieldPrimaryKey;
        private static List<string>? _tableDetail;

        public BaseBL(IDatabaseService databaseService)
        {
            _dataBaseService = databaseService;

            SetupBL();
        }

        #region SetupBL
        public void SetupBL()
        {
            var configTable = typeof(T).GetCustomAttributes(typeof(ConfigTable), true).FirstOrDefault();
            var primaryKey = typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PrimaryKey))).FirstOrDefault();
            if (primaryKey != null)
            {
                _primaryKeyPropertyInfor = primaryKey;
                _fieldPrimaryKey = primaryKey.Name;
            }
            else
            {
                throw new CustomException("Chưa setup khóa chính cho model");
            }
            if (configTable != null)
            {
                _tableName = (configTable as ConfigTable)?.TableName;
                _columnSearch = (configTable as ConfigTable)?.ColumnSearch;
                _isMater = (configTable as ConfigTable)?.IsMaster;
                _tableDetail = (configTable as ConfigTable)?.DetailTables;

            }
            else
            {
                throw new CustomException("Chưa thêm attribute config table");
            }
        }
        #endregion

        #region Delete
        public async Task<ServiceResponse> Delete(string id)
        {
            var result = new ServiceResponse();
            var validates = ValidateBeforeDelete(id);

            if (validates != null && validates.Count > 0)
            {
                var isUsedValidate = validates.Find(validate => validate.ErrorCode == ErrorCodeEnum.IsUsed) != null ? true : false;
                result.ValidateInfo = validates;
                result.Success = false;
                result.ErrorCode = isUsedValidate ? ErrorCodeEnum.IsUsed : ErrorCodeEnum.NoError;
                return result;
            }

            BeforeDelete(Int32.Parse(id));
            IDbConnection connection = _dataBaseService.GetDbConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            result.Data = DoDelete(id, connection, transaction) > 0;

            AfterDelete(id, connection, transaction);

            transaction.Commit();
            transaction.Dispose();
            connection.Dispose();
            connection.Close();
            AfterCommitDelete(id);

            return result;
        }
        #endregion

        #region Get ALL
        public virtual async Task<List<T>> GetAll()
        {
            var command = GetCommandGetALL();
            var result = await _dataBaseService.QueryUsingCommanTextAsync<T>(command);

            return result.ToList();
        }
        #endregion

        #region Get By ID
        public async Task<T?> GetByID(int id)
        {
            var command = GetCommandGetByID();
            var param = new Dictionary<string, object>()
            {
                {$"@{_fieldPrimaryKey}", id}
            };
            var result = await _dataBaseService.QuerySingleUsingCommanTextAsync<T>(command, param);

            if (result == null) return result;
            await SetDetails(id, result);

            return result;
        }

        private async Task SetDetails(int id, object result, Type? type = null)
        {
            var detailAttributes = type == null ? result.GetType().GetCustomAttributes(typeof(Detail)) : type.GetCustomAttributes(typeof(Detail));
            for(int i=0; i < detailAttributes.Count; i++)
            {
                var detailAttribute = detailAttributes[i];
                var commandGetDetail = (detailAttribute as Detail)?.CommandGetDetail;
                var property = (detailAttribute as Detail)?.PropertyInMaster;
                var typeDetails = (detailAttribute as Detail)?.Type;

                var paramGetDetail = new Dictionary<string, object>()
                {
                    {"@MasterID",id }
                };

                if (string.IsNullOrWhiteSpace(commandGetDetail)) continue;
                var details = await _dataBaseService.QueryUsingCommanTextAsync(commandGetDetail, paramGetDetail);

                if (details != null && details.Count > 0 && property != null && typeDetails != null)
                {
                    var detailConverts = ConvertUtility.DeserializeObject(ConvertUtility.Serialize(details), typeDetails);
                    var typeDetail = typeDetails.GetGenericArguments()[0];
                    result?.SetValue(property, detailConverts);
                    for (int j = 0; j < details.Count; j++) await SetDetails(details[j].GetPrimaryKey(typeDetail), details[j], typeDetail);
                }
            }
        }
        #endregion

        #region
        public string GetCommandGetByID()
        {
            return $"SELECT * FROM {_tableName} WHERE {_fieldPrimaryKey} = @{_fieldPrimaryKey};";
        }
        #endregion

        #region Get proc
        public virtual string GetCommandGetALL()
        {
            return $"SELECT * FROM {_tableName} ORDER BY ModifiedDate Desc;";
        }

        public string GetProcGetByID(Type? type = null)
        {
            if (type != null)
            {
                var configTable = type.GetCustomAttributes(typeof(ConfigTable), true).FirstOrDefault();
                return $"Proc_{(configTable as ConfigTable)?.TableName}_GetByID";
            }

            return $"Proc_{_tableName}_GetByID";
        }

        public string GetProcPaging()
        {
            return $"Proc_Paging";
        }

        public string GetProcInsert(Type? type = null)
        {
            if(type != null)
            {
                var configTable = type.GetCustomAttributes(typeof(ConfigTable), true).FirstOrDefault();
                return $"Proc_{(configTable as ConfigTable)?.TableName}_Insert";
            }

            return $"Proc_{_tableName}_Insert";
        }

        public string GetProcUpdate(Type? type = null)
        {
            if (type != null)
            {
                var configTable = type.GetCustomAttributes(typeof(ConfigTable), true).FirstOrDefault();
                return $"Proc_{(configTable as ConfigTable)?.TableName}_Update";
            }

            return $"Proc_{_tableName}_Update";
        }
        #endregion

        #region Flow delete
        public virtual List<ValidateResult> ValidateBeforeDelete(string id)
        {
            var result = new List<ValidateResult>();

            if (_tableDetail != null && _tableDetail.Count > 0)
            {
                foreach (var tableRelated in _tableDetail)
                {
                    var checkRelatedTable = $"SELECT COUNT(1) FROM {tableRelated} WHERE {_fieldPrimaryKey} = @ID";
                    var param = new Dictionary<string, object>()
                    {
                        {"@ID",id }
                    };

                    var isUsed = _dataBaseService.ExecuteScalarUsingCommandText<int>(checkRelatedTable, param) > 0;
                    if (isUsed)
                    {
                        result.Add(new ValidateResult()
                        {
                            ErrorCode = ErrorCodeEnum.IsUsed,
                            FieldError = _fieldPrimaryKey
                        });
                    }
                }
            }

            return result;
        }

        public virtual int DoDelete(string id, IDbConnection connection, IDbTransaction transaction, Type? type = null)
        {
            var fieldPrimaryKey = _fieldPrimaryKey;
            var tableName = _tableName;

            if (type != null)
            {
                var configTable = type?.GetCustomAttributes(typeof(ConfigTable), true).FirstOrDefault();
                var primaryKey = type?.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PrimaryKey))).FirstOrDefault();
                fieldPrimaryKey = primaryKey?.Name;
                tableName = (configTable as ConfigTable)?.TableName;
            }

            var commandDelete = $"DELETE FROM {tableName} WHERE {fieldPrimaryKey} = @ID;";

            return _dataBaseService.ExecuteUsingCommandText(commandDelete, new Dictionary<string, object>
            {
                {"@ID", id }
            }, transaction, connection);
        }

        public virtual void AfterDelete(string id, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public virtual void AfterCommitDelete(string id)
        {

        }
        #endregion

        #region Save
        public async Task<ServiceResponse> Save<Entity>(Entity entity)
        {
            var result = new ServiceResponse();
            result.Success = false;
            var validates = ValidateBeforeSave(entity);

            if (validates != null && validates.Count > 0)
            {
                result.ValidateInfo = validates;
                result.Success = false;

                return result;
            }

            BeforeSave(entity);

            var connection = _dataBaseService.GetDbConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();

            result.Data = await DoSave(entity, connection, transaction);

            AfterSave(entity, connection, transaction);

            transaction.Commit();
            transaction.Dispose();
            connection.Dispose();
            connection.Close();
            result.Success = true;

            AfterCommitSave(entity);

            return result;
        }
        #endregion

        #region Save List
        public async Task<ServiceResponse> SaveList(List<T> entities)
        {
            var result = new ServiceResponse();
            var validates = ValidateSaveList(entities);
            if (validates != null && validates.Count > 0)
            {
                result.Success = false;
                result.ValidateInfo = validates;

                return result;
            }

            var connection = _dataBaseService.GetDbConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                await DoSave(entity, connection, transaction);
                AfterSave(entity, connection, transaction);
            }

            transaction.Commit();
            transaction.Dispose();
            connection.Dispose();
            connection.Close();

            for (int i = 0; i < entities.Count; i++) AfterCommitSave(entities[i]);

            return result;
        }

        public async Task<ServiceResponse> SaveList(List<T> entities, IDbConnection connection, IDbTransaction transaction)
        {
            var result = new ServiceResponse();
            var validates = ValidateSaveList(entities);
            if (validates != null && validates.Count > 0)
            {
                result.Success = false;
                result.ValidateInfo = validates;

                return result;
            }

            foreach (var T in entities)
            {
                BeforeSave(T);
            }

            var TInserts = entities.Where(T => T.GetValue<ModelStateEnum>("State").Equals(ModelStateEnum.Insert))?.ToList();
            var TDeletes = entities.Where(T => T.GetValue<ModelStateEnum>("State").Equals(ModelStateEnum.Delete))?.ToList();
            var TUpdates = entities.Where(T => T.GetValue<ModelStateEnum>("State").Equals(ModelStateEnum.Update))?.ToList();

            var quantityDelete = 0;
            var quantityInsert = 0;
            var quantityUpdate = 0;

            if (TDeletes != null && TDeletes.Count > 0)
                quantityDelete = DoDeleteMulti(TDeletes.Select(a => a.GetPrimaryKey<T>()).ToList(), connection, transaction);

            if (TInserts != null && TInserts.Count > 0)
                quantityInsert = DoInsertMulti(TInserts, connection, transaction);

            if (TUpdates != null && TUpdates.Count > 0)
                quantityUpdate = DoUpdateMulti(TUpdates, connection, transaction);

            AfterDeleteMulti(entities, connection, transaction);
            AfterInsertMulti(entities, connection, transaction);
            AfterUpdateMulti(entities, connection, transaction);

            AfterDeleteMultiCommit(entities);
            AfterInsertMultiCommit(entities);
            AfterUpdateMultiCommit(entities);

            result.Data = new
            {
                QuantityDelete = quantityDelete,
                QuantityInsert = quantityInsert,
                QuantityUpdate = quantityUpdate
            };

            return result;
        }

        public virtual List<ValidateResult> ValidateSaveList(List<T> entities)
        {
            var result = new List<ValidateResult>();
            foreach (var T in entities)
            {
                var validates = ValidateBeforeSave(T);
                result.Concat(validates);
            }

            return result;
        }

        public async Task<ServiceResponse> DeleteMulti(List<int> ids)
        {
            var result = new ServiceResponse();
            var validates = new List<ValidateResult>();

            foreach (var id in ids)
            {
                var validate = ValidateBeforeDelete(id.ToString());
                validates = validates.Concat(validate).ToList();
            }

            if (validates != null && validates.Count > 0)
            {
                result.Success = false;
                result.ValidateInfo = validates;
                return result;
            }

            BeforeDeleteMulti(ids);
            var connection = _dataBaseService.GetDbConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();

            result.Data = DoDeleteMulti(ids, connection, transaction);

            transaction.Commit();
            transaction.Dispose();
            connection.Dispose();
            connection.Close();

            AfterDeleteMultiCommit(ids);

            return result;
        }

        public virtual void BeforeDelete(int id)
        {

        }

        public virtual void BeforeDeleteMulti(List<int> ids)
        {

        }

        public int DoDeleteMulti(List<int> ids, IDbConnection connection, IDbTransaction transaction)
        {
            var commandDeleteMulti = $"DELETE FROM {_tableName} WHERE {_fieldPrimaryKey} IN " + "({0});";

            return _dataBaseService.ExecuteUsingCommandText(string.Format(commandDeleteMulti, string.Join(",", ids)), transaction: transaction, connection: connection);
        }

        public int DoInsertMulti(List<T> entities, IDbConnection connection, IDbTransaction transaction)
        {
            var propInserts = typeof(T).GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(NotMap)) && !Attribute.IsDefined(prop, typeof(PrimaryKey)));
            var commandInsert = $"INSERT INTO {_tableName} (" + "{0}) VALUES ";
            commandInsert = string.Format(commandInsert, string.Join(",", propInserts.Select(prop => prop.Name)));

            var index = 0;
            var param = new Dictionary<string, object>();
            foreach (var T in entities)
            {
                var valueInsert = "(";
                foreach (var prop in propInserts)
                {
                    valueInsert += $"@{prop.Name}_{index},";
                    param.Add($"@{prop.Name}_{index}", prop.GetValue(T));
                }
                valueInsert = valueInsert.Substring(0, valueInsert.Length - 1) + "),";
                commandInsert += valueInsert;
                index++;
            }
            commandInsert = commandInsert.Substring(0, commandInsert.Length - 1) + ";";

            return _dataBaseService.ExecuteUsingCommandText(commandInsert, param, transaction, connection);
        }

        public int DoInsertMulti<T>(List<T> entities, IDbConnection connection, IDbTransaction transaction)
        {
            var propInserts = typeof(T).GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(NotMap)));
            var tableName = ((typeof(T).GetCustomAttributes(typeof(ConfigTable), true).FirstOrDefault()) as ConfigTable).TableName;
            var commandInsert = $"INSERT INTO {tableName} (" + "{0}) VALUES ";
            commandInsert = string.Format(commandInsert, string.Join(",", propInserts.Select(prop => prop.Name)));

            var index = 0;
            var param = new Dictionary<string, object>();
            foreach (var entity in entities)
            {
                var valueInsert = "(";
                foreach (var prop in propInserts)
                {
                    valueInsert += $"@{prop.Name}_{index},";
                    param.Add($"@{prop.Name}_{index}", prop.GetValue(entity));
                }
                valueInsert = valueInsert.Substring(0, valueInsert.Length - 1) + "),";
                commandInsert += valueInsert;
                index++;
            }
            commandInsert = commandInsert.Substring(0, commandInsert.Length - 1) + ";";

            return _dataBaseService.ExecuteUsingCommandText(commandInsert, param, transaction, connection);
        }
        public int DoUpdateMulti(List<T> entities, IDbConnection connection, IDbTransaction transaction)
        {
            var propUpdates = typeof(T).GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(NotMap)) && !Attribute.IsDefined(prop, typeof(PrimaryKey))).ToList();

            var commandUpdates = string.Empty;
            var index = 0;
            var param = new Dictionary<string, object>();
            foreach (var T in entities)
            {
                var commandUpdate = $"UPDATE {_tableName} SET ";
                foreach (var prop in propUpdates)
                {
                    commandUpdate += $"{prop.Name} = @{prop.Name}_{index},";
                    param.Add($"@{prop.Name}_{index}", prop.GetValue(T));
                }

                commandUpdate = commandUpdate.Substring(0, commandUpdate.Length - 1) + $" WHERE {_primaryKeyPropertyInfor.Name} = @ID_{index};";
                param.Add($"@ID_{index}", T.GetPrimaryKey<T>());
                commandUpdates += commandUpdate;
                index++;
            }

            return _dataBaseService.ExecuteUsingCommandText(commandUpdates, param, transaction, connection);
        }

        public virtual void AfterDeleteMulti(List<T> entities, IDbConnection connection, IDbTransaction transaction)
        {

        }
        public virtual void AfterInsertMulti(List<T> entities, IDbConnection connection, IDbTransaction transaction)
        {

        }
        public virtual void AfterUpdateMulti(List<T> entities, IDbConnection connection, IDbTransaction transaction)
        {

        }

        public virtual void AfterDeleteMultiCommit(List<int> ids)
        {

        }

        public virtual void AfterDeleteMultiCommit(List<T> entities)
        {

        }
        public virtual void AfterInsertMultiCommit(List<T> entities)
        {

        }
        public virtual void AfterUpdateMultiCommit(List<T> entities)
        {

        }
        #endregion

        #region Validate Func
        public virtual List<ValidateResult> ValidateEmail<Entity>(Entity entity)
        {
            var result = new List<ValidateResult>();
            var emailProps = entity?.GetType().GetProperties(typeof(Email));

            if (emailProps != null && emailProps.Count > 0)
            {
                foreach (var emailProp in emailProps)
                {
                    var email = emailProp.GetValue(entity)?.ToString();
                    if (!string.IsNullOrWhiteSpace(email) && !email.ValidateEmail())
                    {
                        result.Add(new ValidateResult()
                        {
                            ErrorCode = ErrorCodeEnum.EmailInValid,
                            FieldError = emailProp.Name
                        });
                    }
                }
            }
            return result;
        }

        public virtual List<ValidateResult> ValidatePhone<Entity>(Entity entity)
        {
            var result = new List<ValidateResult>();
            var phoneProps = entity?.GetType().GetProperties(typeof(Phone));

            if (phoneProps != null && phoneProps.Count > 0)
            {
                foreach (var phoneProp in phoneProps)
                {
                    var phone = phoneProp.GetValue(entity)?.ToString();
                    if (!string.IsNullOrWhiteSpace(phone) && !phone.ValidatePhone())
                    {
                        result.Add(new ValidateResult()
                        {
                            ErrorCode = ErrorCodeEnum.PhoneInValid,
                            FieldError = phoneProp.Name
                        });
                    }
                }
            }
            return result;
        }

        public virtual List<ValidateResult> ValidateUnique<Entity>(Entity entity)
        {
            var result = new List<ValidateResult>();
            var uniqueProps = entity?.GetType().GetProperties(typeof(Unique));

            if (uniqueProps != null && uniqueProps.Count > 0)
            {
                foreach (var uniqueProp in uniqueProps)
                {
                    var value = uniqueProp.GetValue(entity)?.ToString();
                    if (!string.IsNullOrWhiteSpace(value) && CheckExitByField(uniqueProp.Name, value))
                    {
                        result.Add(new ValidateResult()
                        {
                            ErrorCode = ErrorCodeEnum.UniqueInValid,
                            FieldError = uniqueProp.Name
                        });
                    }
                }
            }
            return result;
        }

        public virtual List<ValidateResult> ValidateLength<Entity>(Entity entity)
        {
            var result = new List<ValidateResult>();
            var lengthProps = entity?.GetType().GetProperties(typeof(Length));

            if (lengthProps != null && lengthProps.Count > 0)
            {
                foreach (var uniqueProp in lengthProps)
                {
                    var value = uniqueProp.GetValue(entity)?.ToString();

                    var maxLength = (uniqueProp.GetCustomAttribute(typeof(Length)) as Length)?.MaxLength;
                    if (!string.IsNullOrWhiteSpace(value) && value.Length > maxLength)
                    {
                        result.Add(new ValidateResult()
                        {
                            ErrorCode = ErrorCodeEnum.MaxLengthInValid,
                            FieldError = uniqueProp.Name
                        });
                    }
                }
            }
            return result;
        }

        public virtual List<ValidateResult> ValidateRequired<Entity>(Entity entity)
        {
            var result = new List<ValidateResult>();
            var requriedProps = entity?.GetType().GetProperties(typeof(Required));

            if (requriedProps != null && requriedProps.Count > 0)
            {
                foreach (var requriedProp in requriedProps)
                {
                    var value = requriedProp.GetValue(entity)?.ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        result.Add(new ValidateResult()
                        {
                            ErrorCode = ErrorCodeEnum.RequiredInValid,
                            FieldError = requriedProp.Name
                        });
                    }
                }
            }

            return result;
        }
        #endregion

        public bool CheckExitByField(string fieldName, string fieldValue)
        {
            var commandCheckExist = $"SELECT COUNT(1) FROM {_tableName} WHERE {fieldName} = @Value";
            var param = new Dictionary<string, object>()
            {
                {"@Value",fieldValue }
            };

            return _dataBaseService.ExecuteScalarUsingCommandText<int>(commandCheckExist, param) > 0;
        }

        #region Do Insert
        public virtual async Task<int> DoInsert<Entity>(Entity entity, IDbConnection connection, IDbTransaction transaction)
        {
            var procInsert = GetProcInsert(entity?.GetType());
            var param = BuildParam(entity);

            return await _dataBaseService.ExecuteScalarUsingStoreProcedureAsync<int>(procInsert, param, transaction, connection);
        }
        #endregion

        #region Do Update
        public virtual async Task<int> DoUpdate<Entity>(Entity entity, IDbConnection connection, IDbTransaction transaction)
        {
            var procUpdate = GetProcUpdate(entity?.GetType());
            var param = BuildParam(entity);

            return await _dataBaseService.ExecuteUsingStoredProcedureAsync(procUpdate, param, transaction, connection);
        }
        #endregion

        #region Build Param from T
        public virtual Dictionary<string, object> BuildParam<Entity>(Entity entity)
        {
            var param = new Dictionary<string, object>();

            var props = entity?.GetType().GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(NotMap))).ToList();

            if (props == null) return param;
            foreach (var prop in props)
            {
                var fieldName = prop.Name;
                var fieldValue = prop.GetValue(entity);

                param.Add($"v_{fieldName}", fieldValue ?? 0);
            }

            return param;
        }
        #endregion

        #region Save Flow
        public virtual List<ValidateResult> ValidateBeforeSave<Entity>(Entity entity)
        {
            var result = new List<ValidateResult>();

            switch (entity?.GetValue<ModelStateEnum>("State"))
            {
                case ModelStateEnum.None:
                    break;
                case ModelStateEnum.Insert:
                case ModelStateEnum.Update:
                    var validateEmail = ValidateEmail(entity);
                    var validatePhone = ValidatePhone(entity);
                    var validateRequired = ValidateRequired(entity);
                    var validateUnique = ValidateUnique(entity);
                    var validateLength = ValidateLength(entity);
                    result.Concat(validateEmail).Concat(validatePhone).Concat(validateRequired).Concat(validateUnique).Concat(validateLength);
                    break;
                case ModelStateEnum.Delete:
                    var validateDelete = ValidateBeforeDelete(entity.GetPrimaryKey<Entity>().ToString());
                    result.Concat(validateDelete);
                    break;
                case ModelStateEnum.Duplicate:
                    break;
            }

            return result;
        }

        public virtual void BeforeSave<Entity>(Entity entity)
        {
            switch (entity.GetValue<ModelStateEnum>("State"))
            {
                case ModelStateEnum.None:
                    break;
                case ModelStateEnum.Insert:
                    entity.SetValue("CreatedDate", DateTime.Now);
                    entity.SetValue("CreatedBy", BaseInfo.UserName);
                    entity.SetValue("ModifiedDate", DateTime.Now);
                    entity.SetValue("ModifiedBy", BaseInfo.UserName);
                    break;
                case ModelStateEnum.Update:
                    entity.SetValue("ModifiedDate", DateTime.Now);
                    entity.SetValue("ModifiedBy", BaseInfo.UserName);
                    break;
                case ModelStateEnum.Delete:
                    break;
                case ModelStateEnum.Duplicate:
                    break;
            }
        }

        public async Task<bool> DoSave<Entity>(Entity entity, IDbConnection connection, IDbTransaction transaction)
        {
            switch (entity?.GetValue<ModelStateEnum>("State"))
            {
                case ModelStateEnum.None:
                    break;
                case ModelStateEnum.Insert:
                    var id = await DoInsert<Entity>(entity, connection, transaction);
                    entity.SetPrimaryKey<Entity>(id);
                    break;
                case ModelStateEnum.Update:
                    await DoUpdate<Entity>(entity, connection, transaction);
                    break;
                case ModelStateEnum.Delete:
                    DoDelete(entity.GetPrimaryKey<Entity>().ToString(), connection, transaction, entity.GetType());
                    break;
                case ModelStateEnum.Duplicate:
                    break;
            }
            
            var modelDetailConfigs = entity?.GetValue<List<ModelDetailConfig>>("ModelDetailConfigs");
            if(modelDetailConfigs != null)
            {
                for (int i = 0; i < modelDetailConfigs.Count; i++)
                {
                    var modelDetailConfig = modelDetailConfigs[i];

                    IList listDetail = entity?.GetValue<IList>(modelDetailConfig.PropertyOnMasterModel);

                    if (listDetail != null)
                    {
                        foreach (var detail in listDetail)
                        {
                            var state = detail.GetValue<ModelStateEnum>("State");
                            detail.SetValue(modelDetailConfig.ForeignKeyName, entity.GetPrimaryKey<Entity>());
                            BeforeSave(detail);

                            await DoSave(detail, connection, transaction);
                        }
                    }
                }
            }

            return true;
        }

        public virtual void AfterSave<Entity>(Entity entity, IDbConnection connection, IDbTransaction transaction)
        {

        }

        public void AfterCommitSave<Entity>(Entity entity)
        {

        }
        #endregion

        public async Task<List<T>> GetByField(string fieldName, string fieldValue)
        {
            var command = $"SELECT * FROM {_tableName} WHERE {fieldName} = @{fieldName}";
            var param = new Dictionary<string, object>()
            {
                {$"@{fieldName}",fieldValue }
            };
            return await _dataBaseService.QueryUsingCommanTextAsync<T>(command, param);
        }

        public async Task<List<T>> Search(string keyword)
        {
            string commandSearch = CreateSearchCommand();

            return await _dataBaseService.QueryUsingCommanTextAsync<T>(commandSearch, param: new Dictionary<string, object>
            {
                {"@Keyword", keyword }
            });
        }

        public virtual string CreateSearchCommand()
        {
            var commandSearch = $"SELECT * FROM {_tableName} ";

            if (_columnSearch?.Count > 0)
            {
                commandSearch += "WHERE";

                foreach (var column in _columnSearch)
                {
                    commandSearch += $" {column} LIKE CONCAT('%',@Keyword,'%') AND";
                }

                commandSearch = commandSearch.Substring(0, commandSearch.Length - 3) + ";";
            }

            return commandSearch;
        }

        bool CheckFieldName(string fieldName)
        {
            return typeof(T).GetAllFieldName().Contains(fieldName);
        }

        public async Task<ServiceResponse> UpdateField(ModelUpdateField model, int id)
        {
            var result = new ServiceResponse();

            if (CheckFieldName(model.FieldName))
            {
                var command = $"UPDATE {_tableName} SET {model.FieldName} = @{model.FieldName} WHERE {_fieldPrimaryKey} = @{_fieldPrimaryKey};";

                var param = new Dictionary<string, object>() {
                    {$"@{model.FieldName}", model.FieldValue},
                    {$"@{_fieldPrimaryKey}", id }
                };

                result.Data = (await _dataBaseService.ExecuteUsingCommandTextAsync(command, param: param)) > 0;
            }
            else throw new Exception("Thông tin không tồn tại!");

            return result;
        }
    }
}
