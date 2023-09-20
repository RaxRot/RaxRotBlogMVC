using Microsoft.EntityFrameworkCore;
using RaxRot.Blog.Data;
using RaxRot.Blog.Models.Domain;

namespace RaxRot.Blog.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BlogPostCommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await _dbContext.BlogPostComments.AddAsync(blogPostComment);
            await _dbContext.SaveChangesAsync();

            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
           return await _dbContext.BlogPostComments
                .Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
