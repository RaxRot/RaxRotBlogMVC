using Microsoft.AspNetCore.Identity;

namespace RaxRot.Blog.Repositories
{
    public interface IUserRepository
    {
       Task<IEnumerable<IdentityUser>> GetAll();
    }
}
