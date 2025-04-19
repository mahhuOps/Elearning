using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.Interfaces;
using Elearning.Models;

namespace Elearning.Implements
{
    public class CourseSessionBL : BaseBL<CourseSession>, ICourseSessionBL
    {
        public CourseSessionBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
