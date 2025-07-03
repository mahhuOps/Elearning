using Common.Constants;
using Core.Controllers;
using Core.Models;
using Elearning.BL.Interfaces;
using Elearning.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Elearning.Controllers
{
    public class ChapterController : BaseController<Chapter>
    {
        public ChapterController(IChapterBL chapterBL) : base(chapterBL)
        {
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<ServiceResponse> GetByID(int id)
        {
            var permission = await _baseBL.CheckPermission(id);

            if (!permission) return new ServiceResponse
            {
                Data = null,
                ErrorCode = Core.Enums.ErrorCodeEnum.NoPermission,
                Success = false
            };

            return await base.GetByID(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleType.ADMIN)]
        public override Task<ServiceResponse> Get()
        {
            return base.Get();
        }
    }
}
