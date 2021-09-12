using CoinJar.Domain.Dtos;
using CoinJar.Domain.Entities;
using CoinJar.Domain.Services;
using System;
using System.Threading.Tasks;

namespace CoinJar.Domain.Commands
{
    public class ResetCommand: ICommand
    {
        public string UserName { get; set; }
    }

    public class ResetCommandHandler : ICommandHandler<ResetCommand>
    {
        private readonly IRepository<Jar> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ResetCommandHandler(IRepository<Jar> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ResetCommand command)
        {
            var jar = await _repository.GetOneAsync(x=>x.UserName == command.UserName);
            if (jar == null)
                throw new InvalidOperationException("No jar is associated with this username");
            jar.Reset();
            _repository.Update(jar);
            await _unitOfWork.SaveAsync();
        }
    }
}
