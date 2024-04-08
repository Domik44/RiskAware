using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class CommentSeed
    {
        public static void Seed(AppDbContext context)
        {
            var commentsToBeAdded = new Comment[]
            {
                new()
                {
                    Text = "Tenhle projekt je Bomba!!!",
                    UserId = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2",
                    RiskProjectId = 1
                }
            };

            foreach (var comment in commentsToBeAdded)
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
