using Elearning.Interfaces;
using Elearning.Models;
using Core.Controllers;

namespace Elearning.Controllers
{
    public class CourseSessionsController : BaseController<CourseSession>
    {
        public CourseSessionsController(ICourseSessionBL coursesessionBL) : base(coursesessionBL)
        {
        }
    }
}
