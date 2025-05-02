using Elearning.BL.Interfaces;
using Elearning.Models;
using Core.Controllers;

namespace Elearning.Controllers
{
    public class CoursePackagesController : BaseController<CoursePackage>
    {
        public CoursePackagesController(ICoursePackageBL coursepackageBL) : base(coursepackageBL)
        {
        }
    }
}
