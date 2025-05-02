using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;

namespace Elearning.BL.Implements
{
    public class ServiceBL : BaseBL<Service>, IServiceBL
    {
        public ServiceBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
