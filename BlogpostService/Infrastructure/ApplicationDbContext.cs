using BlogpostService.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogpostService.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base()
    {
    }
    
    public DbSet<Blogpost> Blogposts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Blogpost>(b =>
        {
            b.HasKey(bp => bp.BlogPostId);

            b.OwnsMany(bp => bp.Comments, cb =>
            {
                cb.WithOwner().HasForeignKey("BlogPostId");        
                cb.ToTable("BlogPostComments");                  
                
                //assigning commentId as primary key
                cb.HasKey("CommentId");

                // map the FK to BlogPostId as required
                cb.Property<Guid>("BlogPostId");

                // Owned collection of replies (nested)
                cb.OwnsMany(c => c.Replies, rb =>
                {
                    rb.WithOwner().HasForeignKey("ParentCommentId");   
                    rb.ToTable("BlogPostCommentReplies");              

                    // shadow key for reply row
                    rb.Property<Guid>("ReplyId");
                    rb.HasKey("ReplyId");

                    // include the ParentCommentId column for replies
                    rb.Property<Guid>("ParentCommentId");
                });
            });
        });

    }
}