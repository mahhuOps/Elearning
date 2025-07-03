using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;

namespace Elearning.BL.Implements
{
    public class CourseBL : BaseBL<Course>, ICourseBL
    {
        public CourseBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
