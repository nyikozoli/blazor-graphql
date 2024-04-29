using Microsoft.EntityFrameworkCore;

namespace GraphQLWithEFCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasMany(blog => blog.Posts)
            .WithOne(post => post.Blog);

        for (int i = 1; i <= 10; i++)
        {
            modelBuilder.Entity<Blog>().HasData(new Blog { Id = i, Url = $"http://example.com/{i}" });

            for (int j = 1; j <= 1000; j++)
            {
                modelBuilder.Entity<Post>().HasData(new Post
                    { Id = (i - 1) * 10_000 + j, BlogId = i, Title = $"Post {j}", Content = $"Content {j}" });
            }
        }

        modelBuilder.Entity<Blog>().HasData(new Blog { Id = 11, Url = "http://example.com/11" });

        for (int j = 1; j <= 10_000; j++)
        {
            modelBuilder.Entity<Post>().HasData(new Post
                { Id = 10 * 10_000 + j, BlogId = 11, Title = $"Post {j}", Content = $"Content {j}" });
        }
    }
}

public class Blog
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
}

[QueryType]
public static class Query
{
    [UsePaging(IncludeTotalCount = true)]
    [UseSorting]
    [UseFiltering]
    // [UseProjection]
    public static IQueryable<Blog> GetBlogs(ApplicationDbContext dbContext)
        => dbContext.Blogs.Include(blog => blog.Posts);
}