using System;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;

namespace DemoFunctionApp
{
    public interface ITransactionService
    {
        Task<ResponseViewModel> ExecuteAsync(TransactionViewModel transactionDto);
    }

    internal class TransactionService : ITransactionService
    {
        private readonly string _connectionString;

        public TransactionService() => _connectionString = Environment.GetEnvironmentVariable("SqlServer");

        public async Task<ResponseViewModel> ExecuteAsync(TransactionViewModel transactionDto)
        {
            var result = new ResponseViewModel();
            var direction = transactionDto.Direction?.Trim().ToUpperInvariant() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transactionDto.Direction) || direction is not ("DEBIT" or "CREDIT") || transactionDto.Id == 0)
            {
                result.Message = "Invalid transaction";
                return result;
            }

            await using var conn = new SqlConnection(_connectionString);
            var dbTransaction = await conn.QueryFirstOrDefaultAsync<TransactionAggregate>(
                            "SELECT * FROM Transactions WHERE TransactionId=@TransactionId", new { TransactionId = transactionDto.Id });
            if (dbTransaction != null)
            {
                result.Message = "Transaction already executed, please send unique Transaction ID.";
                return result;
            }

            var transaction = new TransactionAggregate
            {
                TransactionId = transactionDto.Id,
                Amount = transactionDto.Amount,
                CreatedBy = 1, // In real-time application, user id from session or token should be used
                CreatedOn = DateTime.UtcNow,
                Direction = direction == "DEBIT" ? TransactionDirection.Debit : TransactionDirection.Credit,
                WalletId = transactionDto.Account
            };

            var wallet = await conn.QueryFirstOrDefaultAsync<WalletAggregate>(
                            "SELECT * FROM Wallet WHERE WalletId=@WalletId", new { transaction.WalletId });

            if (wallet == null)
            {
                result.Message = "Account (Wallet) not found";
                return result;
            }

            switch (direction)
            {
                case "DEBIT":
                    if (wallet.Balance >= transaction.Amount)
                    {
                        wallet.Balance -= transaction.Amount;
                        wallet.ModifiedBy = 1; // In real-time application, user id from session or token should be used
                        wallet.ModifiedOn = DateTime.UtcNow;

                        result.Success = await conn.UpdateAsync(wallet);
                        result.Message = result.Success ? "Funds debited from the wallet" : "Transaction failed, please try again";
                    }
                    else
                    {
                        result.Message = "Insufficient funds, please credit funds to your wallet.";
                    }
                    break;
                case "CREDIT":
                    wallet.Balance += transaction.Amount;
                    wallet.ModifiedBy = 1; // In real-time application, user id from session or token should be used
                    wallet.ModifiedOn = DateTime.UtcNow;

                    result.Success = await conn.UpdateAsync(wallet);
                    result.Message = result.Success ? "Funds credited to the wallet" : "Transaction failed, please try again";
                    break;
                default:
                    break;
            }

            if (result.Success)
            {
                await conn.InsertAsync(transaction);
            }

            return result;
        }
    }
}
