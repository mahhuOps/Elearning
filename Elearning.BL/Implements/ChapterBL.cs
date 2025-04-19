using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.Interfaces;
using Elearning.Models;

namespace Elearning.Implements
{
    public class ChapterBL : BaseBL<Chapter>, IChapterBL
    {
        public ChapterBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
