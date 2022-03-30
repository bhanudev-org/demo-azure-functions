using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DemoFunctionApp
{
    public class AccountDetailsFunction
    {
        private readonly ITransactionService _transactionService;

        public AccountDetailsFunction(ITransactionService transactionService)
            => _transactionService = transactionService;


        [FunctionName("AccountDetails")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("AccountDetails HTTP trigger function processed a request.");

            try
            {
                var data = await JsonSerializer.DeserializeAsync<TransactionViewModel>(req.Body);
                if (data == null)
                {
                    return new BadRequestObjectResult(new ResponseViewModel { Message = "Invalid data sent"});
                }
                var result = await _transactionService.ExecuteAsync(data);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                log.LogError(e, "Error occurred while processing a transaction");
                return new BadRequestObjectResult(new ResponseViewModel() { Message = "Error occurred" });
            }
        }
    }
}
