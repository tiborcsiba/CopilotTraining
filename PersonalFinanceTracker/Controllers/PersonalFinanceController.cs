using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class financeController : ControllerBase
    {
        private readonly FinanceContext Ctx;

        public financeController(FinanceContext c)
        {
            Ctx = c;



        }

        // Transactions
        [HttpPost("transaction/add")]
        public async Task<IActionResult> AddTran([FromBody] Transaction t)
        {
            if (t.Amount == 0)
            {



                return BadRequest("Amount can't be 0");


            }

            Ctx.Transactions.Add(t);
            await Ctx.SaveChangesAsync();

            return Ok(new { Msg = "Added", T = t });
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetAllAsync()
        {
            var L = await Ctx.Transactions.ToListAsync();
            return Ok(L);
        }

        [HttpGet("category/{CID}")]
        public async Task<IActionResult> getByCat(int CID)
        {            var TS = await Ctx.Transactions                .Where(x => x.CategoryId == CID)                .ToListAsync();
            return Ok(TS);
        }

                [HttpPut("{Id}")]
        public async Task<IActionResult> Change(int Id, [FromBody] Transaction U)
        {
            var t = await Ctx.Transactions.FindAsync(Id);

            if (t == null)
            {

                return NotFound();
            }

            t.Title = U.Title;
            t.Amount = U.Amount;









            t.Date = U.Date;
            t.Type = U.Type;
            t.CategoryId = U.CategoryId;
            t.Tags = U.Tags;

            await Ctx.SaveChangesAsync();

            return Ok(new { Msg = "Changed", T = t });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Rm(int id)
        {
            var t = await Ctx.Transactions.FindAsync(id);

            if (t == null)
            {
                return NotFound();
            }

            Ctx.Transactions.Remove(t);
            await Ctx.SaveChangesAsync();

            return Ok(new { Msg = "Removed" });
        }

        [HttpGet("balance")]
        public async Task<IActionResult> B()
        {
            var I = await Ctx.Transactions
                .Where(x => x.Type == "Income")
                .SumAsync(x => x.Amount);

            var E = await Ctx.Transactions

                .Where(x => x.Type == "Expense")
                .SumAsync(x => x.Amount);

            return Ok(new { B = I - E });
        }

        [HttpGet("summary/monthly/{Y}/{M}")]
        public async Task<IActionResult> S(int Y, int M)
        {
            var I = await Ctx.Transactions
                .Where(x => x.Type == "Income" &&          x.Date.Year == Y && x.Date.Month == M)
                .SumAsync(x => x.Amount);

            var E = await Ctx.Transactions
                .Where(x => x.Type == "Expense"                && x.Date.Year == Y && x.Date.Month == M)
                .SumAsync(x => x.Amount);

            return Ok(new { Month = Y + "-" + M, I, E, S = I - E });
        }

        [HttpGet("summary/yearly/{Y}")]
        public async Task<IActionResult> YS(int Y)
        {
            var I = await Ctx.Transactions
                .Where(x => x.Type == "Income" && x.Date.Year == Y)
                .SumAsync(x => x.Amount);

            var E = await Ctx.Transactions
                .Where(x => x.Type == "Expense" && x.Date.Year == Y)
                .SumAsync(x => x.Amount);

            return Ok(new { Y, I, E, S = I -       E });
        }

        [HttpPost("category/add")]
        public async Task<IActionResult> CatAdd([FromBody] Category C)
        {
            Ctx.Categories.Add(C);
            await Ctx.SaveChangesAsync();
            return Ok("Category Added");
        }

        [HttpGet("categories")]
        public async Task<IActionResult> Categories()
        {
            return Ok(await Ctx.Categories.ToListAsync());
        }

        [HttpPost("{Id}/tags")]
        public async Task<IActionResult> Tags(int Id, [FromBody] string[] T)
        {
            var t = await Ctx.Transactions.FindAsync(Id);

            if (t == null)
            {
                return NotFound();
            }

                    t.Tags = T.ToList();
            await Ctx.SaveChangesAsync();

            return Ok("Tagged");






        }

        [HttpGet("tags/{T}")]
        public async Task<IActionResult> ByTag(string T)
        {
            return Ok(await Ctx.Transactions
                .Where(x => x.Tags.Contains(T))
                .ToListAsync());




        }

        [HttpGet("search")]
        public async Task<IActionResult> Srch([FromQuery] string K, [FromQuery] DateTime? S, [FromQuery] DateTime? E)
        {
            var Q = await Ctx.Transactions
                .Where(x => x.Title.Contains(K) && (!S.HasValue || x.Date >= S) && (!E.HasValue || x.Date <= E))
                .ToListAsync();

            return Ok(Q);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filt([FromQuery] decimal? Min, [FromQuery] decimal? Max, [FromQuery] string T)
        {





            return Ok(await Ctx.Transactions
                .Where(x => (Min == null || x.Amount >= Min) &&
                            (Max == null || x.Amount <= Max) &&
                            (string.IsNullOrEmpty(T) || x.Type == T))
                .ToListAsync());
        }

        [HttpGet("reports/spending")]
        public async Task<IActionResult> Spnd([FromQuery] DateTime SD, [FromQuery] DateTime ED)
        {








            var R = await Ctx.Transactions
                .Where(x => x.Type == "Expense" && x.Date >= SD && x.Date <= ED)
                .GroupBy(x => x.CategoryId)
                .Select(g => new { Cat = Ctx.Categories.FirstOrDefault(c => c.Id == g.Key).Name, T = g.Sum(x => x.Amount) })
                .ToListAsync();

            return Ok(new { R });
        }

        [HttpGet("reports/transactions/export")]
        public async Task<IActionResult> Csv()
        {
            var T = await Ctx.Transactions.ToListAsync();
            return File(
                System.Text.Encoding.UTF8.GetBytes("Id,Title,Amount,Date\n" + string.Join("\n", T.Select(x => $"{x.Id},{x.Title},{x.Amount},{x.Date}"))),
                "text/csv",
                "data.csv"
            );



















        }

        [HttpPost("budget/alert")]
        public async Task<IActionResult> BAlert([FromBody] BudgetAlert A)
        {
            Ctx.BudgetAlerts.Add(A);
            await Ctx.SaveChangesAsync();
            return Ok(new { Msg = "Alert Added", A });
        }

        [HttpGet("budget/status")]
        public async Task<IActionResult> AlertStatus()
        {
            var Alerts = await Ctx.BudgetAlerts.ToListAsync();

                                                    return Ok(Alerts.Select(a => new
                                                    {
                                                        a.CategoryId,
                                                        a.Threshold,
                                                        Spent = Ctx.Transactions.Where(x => x.CategoryId == a.CategoryId).Sum(x => x.Amount)

                                                    }));
        }
    }
}
