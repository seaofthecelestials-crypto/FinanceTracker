using ASP.NetLearning.Data;
using ASP.NetLearning.Models;
using ASP.NetLearning.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ASP.NetLearning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public HomeController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            decimal totalDeposits = await _context.Deposits.SumAsync(d => d.Amount);
            decimal totalSpent = await _context.Transactions.SumAsync(t => t.Amount);
            decimal remainingBalance = totalDeposits - totalSpent;

            ViewBag.TotalDeposits = totalDeposits;
            ViewBag.TotalSpent = totalSpent;
            ViewBag.RemainingBalance = remainingBalance;

            var categoryData = await _context.Transactions
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) })
                .ToListAsync();

            ViewBag.CategoryNames = categoryData.Select(d => d.Category).ToList();
            ViewBag.CategoryTotals = categoryData.Select(d => d.Total).ToList();

            decimal percentageSpent = totalDeposits == 0 ? 0 : totalSpent / totalDeposits * 100;

            ViewBag.PercentageSpent = percentageSpent;

            decimal progressWidth = percentageSpent > 100 ? 100 : percentageSpent;
            ViewBag.ProgressWidth = progressWidth;

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Contact contact)
        {
            if(ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                try
                {
                    var emailBody = $@"
                        <h2>New Contact Form Submission</h2>
                        <p><strong>Name:</strong> {contact.Name}</p>
                        <p><strong>Email:</strong> {contact.Email}</p>
                        <p><strong>Subject:</strong> {contact.Subject}</p>
                        <p><strong>Message:</strong> {contact.Message}</p>
                        <p><em>Sent on: {contact.CreatedAt}</em></p>";

                    await _emailService.SendEmailAsync("seaofthecelestials@gmail.com", $"Finance Tracker Contact:{contact.Subject}", emailBody);
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"Your message has been saved, but there was an issue sending the email notification: {ex.Message}";
                }
                return RedirectToAction(nameof(Contact));
            }
            return View(contact);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id?? HttpContext.TraceIdentifier});
        }
    }
} 