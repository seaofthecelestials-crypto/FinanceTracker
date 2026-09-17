using Microsoft.AspNetCore.Mvc;
using ASP.NetLearning.Data;
using Microsoft.EntityFrameworkCore;
using ASP.NetLearning.Models;

namespace ASP.NetLearning.Controllers
{
    public class DepositController : Controller
    {
        private readonly ApplicationDbContext _deposit;

        public DepositController(ApplicationDbContext deposit)
        {
            _deposit = deposit;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _deposit.Deposits.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposit = await _deposit.Deposits.FirstOrDefaultAsync(m => m.Id == id);
            if (deposit == null)
            {
                return NotFound();
            }
            return View(deposit);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Deposit deposit)
        {
            deposit.Date = DateTime.SpecifyKind(deposit.Date, DateTimeKind.Utc);
            if (ModelState.IsValid)
            {
                _deposit.Deposits.Add(deposit);
                await _deposit.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deposit);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposit = await _deposit.Deposits.FindAsync(id);
            if (deposit == null)
            {
                return NotFound();
            }
            return View(deposit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Deposit deposit)
        {
            if (id != deposit.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _deposit.Update(deposit);
                    await _deposit.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(deposit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(deposit);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposit = await _deposit.Deposits.FirstOrDefaultAsync(m => m.Id == id);
            if (deposit == null)
            {
                return NotFound();
            }
            return View(deposit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deposit = await _deposit.Deposits.FindAsync(id);
            _deposit.Deposits.Remove(deposit);
            await _deposit.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _deposit.Deposits.Any(e => e.Id == id);
        }
    }
}


