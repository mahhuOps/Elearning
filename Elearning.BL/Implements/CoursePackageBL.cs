using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;

namespace Elearning.BL.Implements
{
    public class CoursePackageBL : BaseBL<CoursePackage>, ICoursePackageBL
    {
        public CoursePackageBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
