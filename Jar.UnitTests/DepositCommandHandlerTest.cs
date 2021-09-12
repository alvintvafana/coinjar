using CoinJar.Domain.Commands;
using CoinJar.Domain.Dtos;
using CoinJar.Domain.Services;
using Moq;
using System;
using Xunit;

namespace Jar.UnitTests
{
    public class DepositCommandHandlerTest
    {
        private readonly IRepository<CoinJar.Domain.Entities.Jar> _repositoryMock;
        public DepositCommandHandlerTest()
        {
            _repositoryMock = new MockBookRepository<CoinJar.Domain.Entities.Jar>();
        }

        [Fact]
        public async void DepositSuccess()
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

            var depositCommandHandler = new DepositCommandHandler(_repositoryMock, mockUnitOfWork.Object);

            var coin = new Coin
            {
                Amount = 1,
                Volume = 0.2m
            };

            var command = new DepositCommand
            {
                UserName = "testUser",
                Coin = coin
            };

            await depositCommandHandler.HandleAsync(command);
            Assert.Contains(db.Table.Values, a => a.UserName == command.UserName && a.Balance == 3 && a.Volume == 2.2m);
        }
        [Fact]
        public async void DepositFail()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(a => a.SaveAsync());

            var depositCommandHandler = new DepositCommandHandler(_repositoryMock, mockUnitOfWork.Object);

            var coin = new Coin
            {
                Amount = 1,
                Volume = 0.2m
            };

            var command = new DepositCommand
            {
                UserName = "testUser",
                Coin = coin
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => depositCommandHandler.HandleAsync(command));
        }
    }
}
 