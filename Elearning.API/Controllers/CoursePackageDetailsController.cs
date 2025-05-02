using Elearning.BL.Interfaces;
using Elearning.Models;
using Core.Controllers;

namespace Elearning.Controllers
{
    public class CoursePackageDetailsController : BaseController<CoursePackageDetail>
    {
        public CoursePackageDetailsController(ICoursePackageDetailBL coursepackagedetailBL) : base(coursepackagedetailBL)
        {
        }
    }
}
