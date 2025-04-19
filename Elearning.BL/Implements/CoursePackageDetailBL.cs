using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.Interfaces;
using Elearning.Models;

namespace Elearning.Implements
{
    public class CoursePackageDetailBL : BaseBL<CoursePackageDetail>, ICoursePackageDetailBL
    {
        public CoursePackageDetailBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
