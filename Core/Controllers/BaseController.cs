using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Common.Constants;

namespace Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        protected IBaseBL<T> _baseBL;

        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        [HttpGet]
        public virtual async Task<ServiceResponse> Get()
        {
            var result = new ServiceResponse();

            result.Data = await _baseBL.GetAll();

            return result;
        }

        [HttpGet("{id}")]
        public virtual async Task<ServiceResponse> GetByID(int id)
        {
            var result = new ServiceResponse();

            result.Data = await _baseBL.GetByID(id);

            return result;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleType.ADMIN)]
        [AllowAnonymous]
        [HttpPost("save")]
        public virtual async Task<ServiceResponse> Save([FromBody] T model)
        {
            return await _baseBL.Save(model);
        }

        [HttpGet("search")]
        public virtual async Task<ServiceResponse> Search(string keyword)
        {
            var result = new ServiceResponse();

            result.Data = await _baseBL.Search(keyword);

            return result;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleType.ADMIN)]
        [HttpDelete("{id}")]
        public virtual async Task<ServiceResponse> Delete(string id)
        {
            return await _baseBL.Delete(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleType.ADMIN)]
        [HttpPut("update-field/{id}")]
        public virtual async Task<ServiceResponse> UpdateField([FromBody] ModelUpdateField model, int id)
        {
            return await _baseBL.UpdateField(model, id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleType.ADMIN)]
        [HttpPost("save-list")]
        public virtual async Task<ServiceResponse> SaveList([FromBody] List<T> models)
        {
            return await _baseBL.SaveList(models);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleType.ADMIN)]
        [HttpPost("delete-multi")]
        public virtual async Task<ServiceResponse> DeleteMulti([FromBody] List<int> ids)
        {
            return await _baseBL.DeleteMulti(ids);
        }
    }
}
