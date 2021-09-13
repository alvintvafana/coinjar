using CoinJar.Domain.Dtos;
using CoinJar.Domain.Entities;
using CoinJar.Domain.Exceptions;
using CoinJar.Domain.Services;
using System;
using System.Threading.Tasks;

namespace CoinJar.Domain.Commands
{
    public class CreateJarCommand: ICommand
    {
        public string UserName { get; set; }
        public ICoin Coin { get; set; }
    }

    public class CreateJarCommandHandler : ICommandHandler<CreateJarCommand>
    {
        private readonly IRepository<Jar> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateJarCommandHandler(IRepository<Jar> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(CreateJarCommand command)
        {
            var jar = await _repository.GetOneAsync(x => x.UserName == command.UserName);
            if (jar != null)
                throw new ValidateException("Jar associated with this username already exists");
            jar = new Jar(command.UserName);
            jar.AddCoin(command.Coin);
            await _repository.InsertAsync(jar);
            await _unitOfWork.SaveAsync();
        }
    }
}
