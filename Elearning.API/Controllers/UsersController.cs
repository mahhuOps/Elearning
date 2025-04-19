using Elearning.Interfaces;
using Elearning.Models;
using Core.Controllers;
using Core.Interfaces;
using Core.Models;

namespace Elearning.Controllers
{
    public class UsersController : BaseController<User>
    {
        public UsersController(IBaseBL<User> baseBL) : base(baseBL)
        {
        }
    }
}
