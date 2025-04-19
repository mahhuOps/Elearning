using Elearning.Interfaces;
using Elearning.Models;
using Core.Controllers;

namespace Elearning.Controllers
{
    public class CoursesController : BaseController<Course>
    {
        public CoursesController(ICourseBL courseBL) : base(courseBL)
        {
        }
    }
}
