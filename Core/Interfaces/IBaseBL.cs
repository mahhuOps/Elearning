using Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Core.Interfaces
{
    public interface IBaseBL<T>
    {
        Task<ServiceResponse> Delete(string id);

        Task<List<T>> GetByField(string fieldName, string fieldValue);

        Task<List<T>> GetAll();

        Task<T> GetByID(int id);

        Task<ServiceResponse> Save<Entity>(Entity entity);

        Task<ServiceResponse> SaveList(List<T> entities);
        Task<ServiceResponse> SaveList(List<T> entities,IDbConnection connection, IDbTransaction transaction);

        Task<ServiceResponse> DeleteMulti(List<int> ids);

        Task<List<T>> Search(string keyword);

        Task<ServiceResponse> UpdateField(ModelUpdateField model,int id);
    }
}
