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
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountsController(IAccountService accountService) 
            => this.accountService = accountService
               ?? throw new ArgumentNullException(nameof(accountService));

      
        [HttpGet]
        public async Task<ActionResult<List<AccountModel>>> GetAll()
        {
            var accountList = await accountService.GetAllAsync();
            var accountModelList = accountList.Select(account => (AccountModel)account);
            return Ok(accountModelList);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserModel user)
        {
            await accountService.CreateAsync(user.ToEntity());
            return Created(string.Empty, null);
        }
    }
}
