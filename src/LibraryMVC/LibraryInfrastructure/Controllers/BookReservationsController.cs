using LibraryDomain.Model;
using LibraryInfrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class BookReservationsController : Controller
{
    private readonly LibraryContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BookReservationsController(LibraryContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Reserve(int bookId, int days)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var book = await _context.Books.FindAsync(bookId);
        if (book == null)
            return NotFound();

        var reservation = new BookReservation
        {
            UserId = user.Id,
            BookId = bookId,
            DueDate = DateTime.UtcNow.AddDays(days),
            Status = "Заброньована"
        };

        _context.BookReservations.Add(reservation);

        var loan = new ClientsBook
        {
            BookId = bookId,
            ClientId = user.Id,
            BorrowingDate = DateOnly.FromDateTime(DateTime.Now),
            ReturnDate = null
        };

        _context.ClientsBooks.Add(loan);

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Books");
    }

    [Authorize(Roles = "Librarian")]
    [HttpPost]
    public async Task<IActionResult> Lend(int reservationId)
    {
        var reservation = await _context.BookReservations.FindAsync(reservationId);
        if (reservation == null)
            return NotFound();

        reservation.Status = "Позичена";
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Books");
    }

    [Authorize(Roles = "Librarian")]
    [HttpPost]
    public async Task<IActionResult> CheckOverdue()
    {
        var overdueReservations = await _context.BookReservations
            .Where(r => r.Status == "Позичена" && r.DueDate < DateTime.UtcNow)
            .ToListAsync();

        foreach (var reservation in overdueReservations)
        {
            reservation.Status = "Прострочена";
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Books");
    }

    [Authorize(Roles = "Librarian")]
    [HttpPost]
    public async Task<IActionResult> ExpireReservations()
    {
        var expiredReservations = await _context.BookReservations
            .Where(r => r.Status == "Заброньована" && r.DueDate < DateTime.UtcNow)
            .ToListAsync();

        foreach (var reservation in expiredReservations)
        {
            reservation.Status = "Доступна";
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Books");
    }
}
