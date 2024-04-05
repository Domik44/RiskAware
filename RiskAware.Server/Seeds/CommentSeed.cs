using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class CommentSeed
    {
        public static void Seed(AppDbContext context)
        {
            var CommentsToBeAdded = new Comment[]
            {
                new()
                {
                    Id = Guid.Parse("0adb54cb-4efc-4d84-b138-db7d3da5510c"),
                    Text = "Tenhle projekt je Bomba!!!",
                    UserId = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2",
                    ProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea")
                }
            };

            foreach (var comment in CommentsToBeAdded)
            {
                if(!context.Comments.Any(c => c.Id == comment.Id))
                {
                    context.Comments.Add(comment);
                }
            }
            context.SaveChanges();
        }
    }
}
