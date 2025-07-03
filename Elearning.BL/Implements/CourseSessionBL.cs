using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;

namespace Elearning.BL.Implements
{
    public class CourseSessionBL : BaseBL<CourseSession>, ICourseSessionBL
    {
        public CourseSessionBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
