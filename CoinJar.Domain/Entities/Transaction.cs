using System;

namespace CoinJar.Domain.Entities
{
    public class Transaction : Entity<Guid>
    {
        public Transaction()
        {

        }
        public decimal Amount { get; private set; }
        public decimal Balance { get; set; }
        public string Description { get; private set; }
        public decimal Volume { get; set; }
        public DateTime CreatedOn { get; private set; }

        public Transaction(decimal amount, decimal balance, decimal volume, string description)
        {
            Amount = amount;
            Description = description;
            CreatedOn = DateTime.Now;
            Volume = volume;
            Balance = balance;
        }
    }
}
