using CoinJar.Domain.Dtos;
using CoinJar.Domain.Entities;
using CoinJar.Domain.Exceptions;
using CoinJar.Domain.Services;
using System;
using System.Threading.Tasks;

namespace CoinJar.Domain.Commands
{
    public class DepositCommand: ICommand
    {
        public string UserName { get; set; }
        public ICoin Coin { get; set; }
    }

    public class DepositCommandHandler : ICommandHandler<DepositCommand>
    {
        private readonly IRepository<Jar> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DepositCommandHandler(IRepository<Jar> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DepositCommand command)
        {
            var jar = await _repository.GetOneAsync(x=>x.UserName == command.UserName);
            if (jar == null)
                throw new ValidateException("No jar is associated with this username");
            jar.AddCoin(command.Coin);
            _repository.Update(jar);
            await _unitOfWork.SaveAsync();
        }
    }
}
