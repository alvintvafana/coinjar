using CoinJar.Api.Dtos;
using CoinJar.Api.Settings;
using CoinJar.Domain.Commands;
using CoinJar.Domain.Dtos;
using CoinJar.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinJar.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JarController : ControllerBase
    {
        private readonly IOptions<HashSet<CoinSettings>> _optionCoins;
        private readonly IMediator _mediator;

        public JarController(IOptions<HashSet<CoinSettings>> optionCoins, IMediator mediator)
        {
            _optionCoins = optionCoins;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateJarDto dto)
        {
            var deposit = _optionCoins.Value.FirstOrDefault(x => x.Denomination == dto.Denomination);
            if (deposit == null)
                return BadRequest("Coin denomination doesnot exist");

            var coin = new Coin
            {
                Amount = deposit.Denomination * dto.Quantity,
                Volume = deposit.Volume * dto.Quantity
            };

            try
            {
                var command = new CreateJarCommand
                {
                    Coin = coin,
                    UserName = dto.UserName
                };
                await _mediator.DispatchAsync(command);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> Update(string username, [FromBody] DepositDto dto)
        {
            var deposit = _optionCoins.Value.FirstOrDefault(x => x.Denomination == dto.Denomination);
            if (deposit == null)
                return BadRequest("Coin denomination doesnot exist");

            var coin = new Coin
            {
                Amount = deposit.Denomination * dto.Quantity,
                Volume = deposit.Volume * dto.Quantity
            };

            try
            {
                var command = new DepositCommand
                {
                    Coin = coin,
                    UserName = username
                };
                await _mediator.DispatchAsync(command);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var query = new BalanceQuery
            {
                UserName = username
            };
            var balance = await _mediator.DispatchAsync(query);
            return Ok(balance);
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            try
            {
                var command = new ResetCommand
                {
                    UserName = username
                };
                await _mediator.DispatchAsync(command);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Account successfully reset");
        }
    }
}
