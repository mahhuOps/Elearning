using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.Interfaces;
using Elearning.Models;

namespace Elearning.Implements
{
    public class UserBL : BaseBL<User>, IUserBL
    {
        public UserBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
