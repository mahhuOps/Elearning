using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;

namespace Elearning.BL.Implements
{
    public class UserBL : BaseBL<User>, IUserBL
    {
        public UserBL(IDatabaseService databaseService) : base(databaseService)
        {
        }

        public async Task<User> GetUser(string firebaseid)
        {
            var sql = "SELECT * FROM users u WHERE u.FirebaseID = @FirebaseID;";

            var user = await _dataBaseService.QuerySingleUsingCommanTextAsync<User>(sql, new Dictionary<string, object>
            {
                {"@FirebaseID", firebaseid }
            });

            return user;
        }
    }
}
