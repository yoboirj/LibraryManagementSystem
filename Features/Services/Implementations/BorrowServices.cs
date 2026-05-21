using LibraryManagementSystem.Features.Data;
using LibraryManagementSystem.Features.Data.Models;
using LibraryManagementSystem.Features.Repositories.Intefaces;
using LibraryManagementSystem.Features.Services.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Features.Services.Implementations
{
    public class BorrowServices : IBorrowServices
    {
        private readonly IBorrowRepository _repo;
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public BorrowServices(IBorrowRepository repo, IDbContextFactory<AppDbContext> contextFactory)
        {
            _repo = repo;
            _contextFactory = contextFactory;
        }

        public async Task<List<Borrow>> GetBorrowsAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<List<Borrow>> GetBorrowsByMemberNameAsync(string memberName)
        {
            return await _repo.GetByMemberNameAsync(memberName);
        }

        public async Task CreateBorrowAsync(Borrow borrow)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Get the book with tracking
                var book = await context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == borrow.BookId);

                if (book == null)
                    throw new Exception("Book not found.");

                if (borrow.Quantity <= 0)
                    throw new Exception("Quantity must be greater than zero.");

                // CRITICAL VALIDATION - Check if enough copies are available
                if (book.Quantity < borrow.Quantity)
                {
                    throw new Exception($"? Cannot borrow {borrow.Quantity} copy(s). Only {book.Quantity} copy(s) available.");
                }

                // Decrease the book quantity
                book.Quantity -= borrow.Quantity;

                // Create borrow record
                borrow.BorrowedAt = DateTimeOffset.Now;
                borrow.DueDate = DateTimeOffset.Now.AddDays(14);
                borrow.IsReturned = false;
                borrow.ReturnedQuantity = 0;

                if (string.IsNullOrEmpty(borrow.Title))
                    borrow.Title = book.Title;
                if (string.IsNullOrEmpty(borrow.Author))
                    borrow.Author = book.Author?.Name ?? "Unknown";

                await context.Borrows.AddAsync(borrow);

                // Save all changes
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ReturnBorrowAsync(int borrowId, int returnQuantity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var borrow = await context.Borrows.FindAsync(borrowId);

                if (borrow == null)
                    throw new Exception("Borrow record not found.");

                if (borrow.IsReturned)
                    throw new Exception("This book has already been fully returned.");

                var remainingToReturn = borrow.Quantity - borrow.ReturnedQuantity;

                if (returnQuantity <= 0)
                    throw new Exception("Return quantity must be greater than 0.");

                if (returnQuantity > remainingToReturn)
                    throw new Exception($"Cannot return {returnQuantity} book(s). Only {remainingToReturn} book(s) remain.");

                // Get the book and add the quantity back
                var book = await context.Books.FindAsync(borrow.BookId);
                if (book == null)
                    throw new Exception("Book not found.");

                borrow.ReturnedQuantity += returnQuantity;
                book.Quantity += returnQuantity;  // Add back to available copies

                if (borrow.ReturnedQuantity >= borrow.Quantity)
                {
                    borrow.IsReturned = true;
                    borrow.ReturnedAt = DateTimeOffset.Now;
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateBorrowAsync(Borrow borrow)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var existingBorrow = await context.Borrows.AsNoTracking().FirstOrDefaultAsync(b => b.Id == borrow.Id);
                if (existingBorrow == null)
                    throw new Exception("Borrow record not found.");

                var book = await context.Books.FirstOrDefaultAsync(b => b.Id == borrow.BookId);
                if (book == null)
                    throw new Exception("Book not found.");

                // If quantity changed, adjust book quantity
                if (existingBorrow.Quantity != borrow.Quantity)
                {
                    int diff = borrow.Quantity - existingBorrow.Quantity;
                    if (book.Quantity < diff)
                    {
                        throw new Exception($"Cannot borrow {borrow.Quantity} copy(s). Only {book.Quantity + existingBorrow.Quantity} copy(s) total available.");
                    }
                    book.Quantity -= diff;
                }

                context.Borrows.Update(borrow);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteBorrowAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var borrow = await context.Borrows.FindAsync(id);
                if (borrow != null)
                {
                    // Restore the quantity if not fully returned
                    if (!borrow.IsReturned)
                    {
                        int remainingQuantity = borrow.Quantity - borrow.ReturnedQuantity;
                        var book = await context.Books.FindAsync(borrow.BookId);
                        if (book != null)
                        {
                            book.Quantity += remainingQuantity;
                        }
                    }

                    context.Borrows.Remove(borrow);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Borrow>> GetOverdueBorrowsAsync()
        {
            var all = await GetBorrowsAsync();
            return all.Where(b => !b.IsReturned && b.DueDate < DateTimeOffset.Now).ToList();
        }

        public async Task<Dictionary<string, int>> GetMostBorrowedBooksAsync(int count)
        {
            var all = await GetBorrowsAsync();
            return all.GroupBy(b => b.Title)
                      .OrderByDescending(g => g.Sum(b => b.Quantity))
                      .Take(count)
                      .ToDictionary(g => g.Key, g => g.Sum(b => b.Quantity));
        }
    }
}