using Core.Implements;
using DatabaseService.Interfaces;
using Elearning.Interfaces;
using Elearning.Models;

namespace Elearning.Implements
{
    public class LessonBL : BaseBL<Lesson>, ILessonBL
    {
        public LessonBL(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
