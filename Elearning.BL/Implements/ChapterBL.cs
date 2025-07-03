using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;

namespace Elearning.BL.Implements
{
    public class ChapterBL : BaseBL<Chapter>, IChapterBL
    {
        public ChapterBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
