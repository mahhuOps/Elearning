using Elearning.BL.Interfaces;
using Elearning.Models;
using Core.Controllers;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using FirebaseService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Core;
using Elearning.BL.Implements;
using Microsoft.AspNetCore.Authorization;

namespace Elearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : BaseController<Elearning.Models.Promotion>
    {
        public PromotionController(IBaseBL<Elearning.Models.Promotion> baseBL) : base(baseBL)
        {
        }
    }
}
