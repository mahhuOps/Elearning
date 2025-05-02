using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;

namespace Elearning.BL.Implements
{
    public class LessonBL : BaseBL<Lesson>, ILessonBL
    {
        public LessonBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
