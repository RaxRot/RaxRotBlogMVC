namespace RaxRot.Blog.Models.ViewModels
{
    public class DeleteTagRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
