using Core.Interfaces;
using Elearning.Models;
namespace Elearning.BL.Interfaces
{
    public interface IUserBL: IBaseBL<User>
    {
        /// <summary>
        /// Lấy user theo firebaseid
        /// </summary>
        /// <param name="firebaseid"></param>
        /// <returns></returns>
        Task<User> GetUser(string firebaseid);
    }
}
