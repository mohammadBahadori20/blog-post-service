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
            b.ToTable("Blogposts");

            b.HasKey(x => x.BlogPostId);
            b.Property(x => x.BlogPostId)
                .HasMaxLength(36)
                .IsRequired()
                .ValueGeneratedNever();
            
            b.Property(x => x.AuthorId).HasMaxLength(36).IsRequired(false);
            b.Property(x => x.Title).HasMaxLength(100).IsRequired(false);
            b.Property(x => x.Description).HasMaxLength(100).IsRequired(false);
            b.Property(x => x.PublishedAt).IsRequired(false);

           
            b.HasMany(x => x.Comments)
                .WithOne(c => c.Blogpost)    
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Comment>(c =>
        {
            c.ToTable("Comments");

            c.HasKey(x => x.CommentId);
            c.Property(x => x.CommentId)
                .HasMaxLength(36)
                .IsRequired()
                .ValueGeneratedNever();
            
            c.Property(x => x.BlogPostId).HasMaxLength(36).IsRequired();
            c.Property(x => x.AuthorId).HasMaxLength(36).IsRequired(false);
            c.Property(x => x.Content).IsRequired().HasMaxLength(1000);
            c.Property(x => x.CreatedAt).IsRequired(false);

            c.HasOne(x => x.Parent)
                .WithMany(x => x.Replies)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict); 
           
            c.HasIndex(x => x.BlogPostId);
        });

        base.OnModelCreating(modelBuilder);


    }
}