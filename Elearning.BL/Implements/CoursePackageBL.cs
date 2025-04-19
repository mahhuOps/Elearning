using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.Interfaces;
using Elearning.Models;

namespace Elearning.Implements
{
    public class CoursePackageBL : BaseBL<CoursePackage>, ICoursePackageBL
    {
        public CoursePackageBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
