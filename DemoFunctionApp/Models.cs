using System;
using Dapper.Contrib.Extensions;

namespace DemoFunctionApp
{
    [Table("Wallet")]
    internal class WalletAggregate
    {
        [ExplicitKey]
        public int WalletId { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    [Table("Transactions")]
    public class TransactionAggregate
    {
        [ExplicitKey]
        public int TransactionId { get; set; }
        public double Amount { get; set; }
        public TransactionDirection Direction { get; set; }
        public int WalletId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public enum TransactionDirection
    {
        Credit = 1,
        Debit = 2
    }

    public class TransactionViewModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Direction { get; set; }
        public int Account { get; set; }
    }

    public class ResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
