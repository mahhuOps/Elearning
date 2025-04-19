using Elearning.Interfaces;
using Elearning.Models;
using Core.Controllers;

namespace Elearning.Controllers
{
    public class LessonsController : BaseController<Lesson>
    {
        public LessonsController(ILessonBL lessonBL) : base(lessonBL)
        {
        }
    }
}
