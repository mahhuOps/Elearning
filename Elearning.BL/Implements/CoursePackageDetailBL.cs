using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;

namespace Elearning.BL.Implements
{
    public class CoursePackageDetailBL : BaseBL<CoursePackageDetail>, ICoursePackageDetailBL
    {
        public CoursePackageDetailBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
