using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.Interfaces;
using Elearning.Models;

namespace Elearning.Implements
{
    public class ServiceBL : BaseBL<Service>, IServiceBL
    {
        public ServiceBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
