using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class TransactionsController : Controller
    {
        private readonly IStockTransactionService _transactionService;

        /// <summary>
        /// Initializes a new instance of the TransactionsController.
        /// </summary>
        /// <param name="transactionService">The transaction service, injected by the dependency injection container.</param>
        public TransactionsController(IStockTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// GET: /Transactions
        /// Displays a list of all transactions using an optimized data transfer object.
        /// </summary>
        /// <returns>A view containing a list of transaction cards.</returns>
        public async Task<IActionResult> Index()
        {
            var transactionCards = await _transactionService.GetAllTransactionCardsAsync();

            return View(transactionCards);
        }

        /// <summary>
        /// GET: /Transactions/Details/5
        /// Displays the full details for a single transaction.
        /// </summary>
        /// <param name="id">The ID of the transaction to display.</param>
        /// <returns>A view containing the detailed transaction information, or a NotFound result.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            // Basic validation: An ID must be provided.
            if (id == null)
            {
                return BadRequest("A transaction ID must be provided.");
            }

            var transaction = await _transactionService.GetTransactionByIdAsync(id.Value);

            // If the service returns null, it means no transaction with that ID was found.
            if (transaction == null)
            {
                return NotFound($"No transaction found with ID {id.Value}.");
            }

            return View(transaction);
        }
    }
}