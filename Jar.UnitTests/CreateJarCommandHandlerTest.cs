using CoinJar.Domain.Commands;
using CoinJar.Domain.Dtos;
using CoinJar.Domain.Exceptions;
using CoinJar.Domain.Services;
using Moq;
using System;
using Xunit;

namespace Jar.UnitTests
{
    public class CreateJarCommandHandlerTest
    {
        private readonly IRepository<CoinJar.Domain.Entities.Jar> _repositoryMock;
        public CreateJarCommandHandlerTest()
        {
            _repositoryMock = new MockBookRepository<CoinJar.Domain.Entities.Jar>();
        }

        [Fact]
        public async void AddJarSuccess()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(a => a.SaveAsync());
            var createJarCommandHandler = new CreateJarCommandHandler(_repositoryMock, mockUnitOfWork.Object);

            var coin = new Coin
            {
                Amount = 1,
                Volume = 0.2m
            };

            var command = new CreateJarCommand
            {
                UserName = "testUser",
                Coin = coin
            };

            await createJarCommandHandler.HandleAsync(command);

            var db = _repositoryMock as MockBookRepository<CoinJar.Domain.Entities.Jar>;

            Assert.Single(db.Table);
            Assert.Contains(db.Table.Values, a => a.UserName == command.UserName);
        }
        [Fact]
        public async void AddJarFail()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(a => a.SaveAsync());
            var db = _repositoryMock as MockBookRepository<CoinJar.Domain.Entities.Jar>;
            db.Table.Add(Guid.NewGuid(), new CoinJar.Domain.Entities.Jar("testUser"));

            var createJarCommandHandler = new CreateJarCommandHandler(_repositoryMock, mockUnitOfWork.Object);

            var coin = new Coin
            {
                Amount = 1,
                Volume = 0.2m
            };

            var command = new CreateJarCommand
            {
                UserName = "testUser",
                Coin = coin
            };

            await Assert.ThrowsAsync<ValidateException>(() => createJarCommandHandler.HandleAsync(command));
        }
    }
}
 