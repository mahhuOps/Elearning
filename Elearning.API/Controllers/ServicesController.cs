using Elearning.Interfaces;
using Elearning.Models;
using Core.Controllers;

namespace Elearning.Controllers
{
    public class ServicesController : BaseController<Service>
    {
        public ServicesController(IServiceBL serviceBL) : base(serviceBL)
        {
        }
    }
}
