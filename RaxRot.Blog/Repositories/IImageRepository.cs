﻿namespace RaxRot.Blog.Repositories
{
    public interface IImageRepository
    {
        Task<string>UploadAsync(IFormFile file);
    }
}
