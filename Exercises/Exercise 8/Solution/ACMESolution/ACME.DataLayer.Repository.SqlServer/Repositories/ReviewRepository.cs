using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ACME.DataLayer.Repository.SqlServer;

public class ReviewRepository : IReviewRepository
{
    private readonly ShopDatabaseContext _context;

    public ReviewRepository(ShopDatabaseContext context) 
    {
        _context = context;
    }

    public async Task<int> SaveAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return 0;
    }
    public async Task<IEnumerable<Review>> FindAsync(Expression<Func<Review, bool>> expression)
    {
        return await _context.Reviews.Where(expression).ToListAsync();
    }
    public async Task<IEnumerable<Review>> GetAllAsync(int page = 1, int count = 10)
    {
        return await _context.Reviews.Skip((page - 1) * count).Take(count).ToArrayAsync();
    }
    public async Task<Review> GetByIdAsync(long id)
    {
        return await _context.Reviews.FirstAsync(r=>r.Id == id);
    }
    public async Task InsertAsync(Review entity)
    {
        await _context.Reviews.AddAsync(entity);
    }
    public async Task UpdateAsync(Review entity)
    {
        var dbEntity = await _context.Reviews.FindAsync(entity.Id);
        if (dbEntity != null)
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
    }
    public async Task DeleteAsync(Review entity)
    {
        var dbEntity = await _context.Reviews.FindAsync(entity.Id);
        if (dbEntity != null)
            _context.Remove(entity);
    }
}
