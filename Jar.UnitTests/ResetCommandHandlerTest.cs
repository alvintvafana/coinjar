using CoinJar.Domain.Commands;
using CoinJar.Domain.Dtos;
using CoinJar.Domain.Services;
using Moq;
using System;
using Xunit;

namespace Jar.UnitTests
{
    public class ResetCommandHandlerTest
    {
        private readonly IRepository<CoinJar.Domain.Entities.Jar> _repositoryMock;
        public ResetCommandHandlerTest()
        {
            _repositoryMock = new MockBookRepository<CoinJar.Domain.Entities.Jar>();
        }

        [Fact]
        public async void ResetSuccess()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(a => a.SaveAsync());
            var db = _repositoryMock as MockBookRepository<CoinJar.Domain.Entities.Jar>;
            var jar = new CoinJar.Domain.Entities.Jar("testUser");
            jar.AddCoin(new Coin
            {
                Amount = 2,
                Volume = 2m
            });

            db.Table.Add(Guid.NewGuid(), jar);

            var commandHandler = new ResetCommandHandler(_repositoryMock, mockUnitOfWork.Object);

            var command = new ResetCommand
            {
                UserName = "testUser",
            };

            await commandHandler.HandleAsync(command);
            Assert.Contains(db.Table.Values, a => a.UserName == command.UserName && a.Balance == 0 && a.Volume == 0m);
        }
        [Fact]
        public async void ResetFail()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(a => a.SaveAsync());

            var commandHandler = new ResetCommandHandler(_repositoryMock, mockUnitOfWork.Object);

            var command = new ResetCommand
            {
                UserName = "testUser",
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => commandHandler.HandleAsync(command));
        }
    }
}
 