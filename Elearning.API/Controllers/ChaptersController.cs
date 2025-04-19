using Core.Controllers;
using Elearning.Interfaces;
using Elearning.Models;

namespace Elearning.Controllers
{
    public class ChaptersController : BaseController<Chapter>
    {
        public ChaptersController(IChapterBL chapterBL) : base(chapterBL)
        {
        }
    }
}
