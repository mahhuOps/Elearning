using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.Interfaces;
using Elearning.Models;

namespace Elearning.Implements
{
    public class CourseBL : BaseBL<Course>, ICourseBL
    {
        public CourseBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
