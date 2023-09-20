﻿using RaxRot.Blog.Models.Domain;

namespace RaxRot.Blog.Repositories
{
    public interface IBlogPostLikeRepository
    {
       Task<int>GetTotalLikes(Guid blogPostId);
       Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);

       Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);
    }
}
