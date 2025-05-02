using Core.Controllers;
using Elearning.BL.Interfaces;
using Elearning.Models;

namespace Elearning.Controllers
{
    public class ChapterController : BaseController<Chapter>
    {
        public ChapterController(IChapterBL chapterBL) : base(chapterBL)
        {
        }
    }
}
