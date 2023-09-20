using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.EntityFrameworkCore;
using RaxRot.Blog.Data;
using RaxRot.Blog.Models.Domain;

namespace RaxRot.Blog.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogPostLikeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await _dbContext.BlogPostLikes.AddAsync(blogPostLike);
            await _dbContext.SaveChangesAsync();

            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
           return await _dbContext.BlogPostLikes
                .Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
           return await _dbContext.BlogPostLikes
                .CountAsync(x => x.BlogPostId == blogPostId);   
        }
    }
}
