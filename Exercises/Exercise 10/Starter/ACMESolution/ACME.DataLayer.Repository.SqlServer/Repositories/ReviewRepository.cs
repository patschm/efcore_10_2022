using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ACME.DataLayer.Repository.SqlServer;

// TODO 1: Open the Test Explorer and run all test.
// They should fail all
public class ReviewRepository : IReviewRepository
{
    private readonly ShopDatabaseContext _context;

    public ReviewRepository(ShopDatabaseContext context) 
    {
        _context = context;
    }

    public async Task<int> SaveAsync()
    {
        // TODO 2: Implement this method to save data to the database
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
        // TODO 3: Implement this method until the corrsponding test succeeds
        return await _context.Reviews.Where(expression).ToListAsync();
    }
    public async Task<IEnumerable<Review>> GetAllAsync(int page = 1, int count = 10)
    {
        // TODO 4: Implement this method until the corrsponding test succeeds
        // Note. page = 1 is the first set
        return await _context.Reviews.Skip((page - 1) * count).Take(count).ToArrayAsync();
    }
    public async Task<Review> GetByIdAsync(long id)
    {
        // TODO 5: Implement this method until the corrsponding test succeeds
        return await _context.Reviews.FirstAsync(r=>r.Id == id);
    }
    public async Task InsertAsync(Review entity)
    {
        // TODO 6: Implement this method until the corrsponding test succeeds
        await _context.Reviews.AddAsync(entity);
    }
    public async Task UpdateAsync(Review entity)
    {
        // TODO 7: Implement this method until the corrsponding test succeeds
        var dbEntity = await _context.Reviews.FindAsync(entity.Id);
        if (dbEntity != null)
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
    }
    public async Task DeleteAsync(Review entity)
    {
        // TODO 8: Implement this method until the corrsponding test succeeds
        var dbEntity = await _context.Reviews.FindAsync(entity.Id);
        if (dbEntity != null)
            _context.Remove(entity);
    }
}
