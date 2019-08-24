using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZipPay.Users.Api.Models;
using ZipPay.Users.BusinessService;

namespace ZipPay.Users.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService) 
            => this.userService = userService 
               ?? throw new ArgumentNullException(nameof(userService));

        [HttpGet]
        public async Task<ActionResult<List<UserModel>>> GetAll()
        {
            var userList = await userService.GetAllAsync();
            var userModelList = userList.Select(user => (UserModel)user);
            return Ok(userModelList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetById([FromRoute]Guid id)
        {
            var user = await userService.GetByIdAsync(id);
            return Ok((UserModel)user);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserModel user)
        {
            await userService.CreateAsync(user.ToEntity());
            return CreatedAtAction(nameof(GetById), new {id=user.Id},user);
        }
    }
}
