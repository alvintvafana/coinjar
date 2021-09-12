using CoinJar.Domain.Commands;
using CoinJar.Domain.Dtos;
using CoinJar.Domain.Queries;
using CoinJar.Domain.Services;
using Moq;
using System;
using Xunit;

namespace Jar.UnitTests
{
    public class BalanceQueryHandlerTest
    {
        private readonly IRepository<CoinJar.Domain.Entities.Jar> _repositoryMock;
        public BalanceQueryHandlerTest()
        {
            _repositoryMock = new MockBookRepository<CoinJar.Domain.Entities.Jar>();
        }

        [Fact]
        public async void GetBalance()
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

            var queryHandler = new BalanceQueryHandler(_repositoryMock);

            var query = new BalanceQuery
            {
                UserName = "testUser",
            };

            var result = await queryHandler.HandleAsync(query);
            Assert.Equal(2m, result);
        }
    }
}
 