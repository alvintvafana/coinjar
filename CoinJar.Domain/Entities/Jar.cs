using CoinJar.Domain.Dtos;
using System;
using System.Collections.Generic;

namespace CoinJar.Domain.Entities
{
    public interface ICoinJar
    {
        void AddCoin(ICoin coin);
        decimal GetTotalAmount();
        void Reset();
    }

    public class Jar : Entity<Guid> , ICoinJar
    {
        public decimal Balance { get; private set; }
        public decimal Volume { get; set; }
        public DateTime UpdatedOn { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string UserName { get; private set; }
        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();

        const decimal JARVOLUME = 42;
        public Jar()
        {}
        public Jar(string userName)
        {
            UserName = userName;
            CreatedOn = DateTime.Now;
        }

        public void AddCoin(ICoin coin)
        {
            Volume += coin.Volume;
            if(Volume > JARVOLUME)
            {
                throw new InvalidOperationException("Jar is now full!");
            }

            Balance += coin.Amount;
            UpdatedOn = DateTime.Now;
            Transactions.Add(new Transaction(coin.Amount, Balance, Volume,"Deposit"));
        }

        public decimal GetTotalAmount()
        {
            return Balance;
        }

        public void Reset()
        {
            Volume = 0;
            Balance = 0;
            UpdatedOn = DateTime.Now;
            Transactions.Add(new Transaction(0, Balance, Volume, "Reset"));
        }
    }
}
