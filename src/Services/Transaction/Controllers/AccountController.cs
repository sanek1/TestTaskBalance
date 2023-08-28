namespace Transaction.WebApi.Controllers
{
    using Transaction.WebApi.Models;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Transaction.Framework.Domain;
    using Transaction.Framework.Services.Interface;
    using Transaction.Framework.Types;
    using Transaction.WebApi.Services;
    using Transaction.Framework.DTO;

    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IRabbitMqService _mqService;

        public AccountController(ITransactionService transactionService, IIdentityService identityService, IMapper mapper, IRabbitMqService mqService)
        {
            _transactionService = transactionService;
            _identityService = identityService;
            _mapper = mapper;
            _mqService = mqService;
        }

        [HttpGet("GetBalance")]
        public async Task<IActionResult> Balance([FromHeader] string currency = "RUB")
        {
            var accountNumber = _identityService.GetIdentity().AccountNumber;
            var transactionResult = await _transactionService.GetBalance(accountNumber, currency);
            return Ok(_mapper.Map<AccountSummaryDTO>(transactionResult)); 
        }

        [HttpGet("GetTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var accountNumber = _identityService.GetIdentity().AccountNumber;
            var transactionResult = await _transactionService.GetTransactions(accountNumber);
            return Ok(transactionResult);
        }

        [HttpPost("SetBalance")]
        public async Task<IActionResult> SetBalance([FromBody] AccountSummary AccountSummary)
        {
            var transactionResult = await _transactionService.SetBalance(AccountSummary);
            return Ok(_mapper.Map<AccountSummaryDTO>(transactionResult));// Ok(_mapper.Map<AccountSummary>(transactionResult));
        }

        [Route("[action]/{message}")]
        [HttpGet]
        public IActionResult SendMessage(string message)
        {
            _mqService.SendMessage(message);

            return Ok("Сообщение отправлено");
        }

    }
}
