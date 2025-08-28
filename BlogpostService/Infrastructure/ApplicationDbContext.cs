using BlogpostService.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogpostService.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base()
    {
    }
    
    public DbSet<Blogpost> Blogposts { get; set; }
    public DbSet<Author> Authors { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Blogposts)
            .WithOne() // uni directional relation
            .HasForeignKey("AuthorId");

        // Blogpost owns Comments
        modelBuilder.Entity<Blogpost>()
            .OwnsMany(b => b.Comments, cb =>
            {
                cb.WithOwner().HasForeignKey("BlogPostId");
                cb.Property<Guid>("Id");
                cb.HasKey("Id");

                // Comment owns Author (nested)
                cb.HasOne(a => a.Author)
                    .WithMany(a => a.Comments)
                    .HasForeignKey("AuthorId");

                // Comment owns Replies (recursive)
                cb.OwnsMany(c => c.Replies, rb =>
                {
                    rb.WithOwner().HasForeignKey("ParentCommentId");
                    rb.Property<Guid>("Id");
                    rb.HasKey("Id");

                    // Replies also own their Author
                    rb.HasOne(a => a.Author)
                        .WithMany(a => a.Comments)
                        .HasForeignKey("AuthorId");
                });
            });
    }
}