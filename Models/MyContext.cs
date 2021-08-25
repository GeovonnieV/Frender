using Microsoft.EntityFrameworkCore;

namespace Frender.Models
{
    // the MyContext class representing a session with our MySQL 
    // database allowing us to query for or save data
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        // the "Monsters" table name will come from the DbSet variable name
        public DbSet<User> Users { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().HasMany(m => m.Friends).WithMany(p => p.FriendsWith).Map(w => w.ToTable("Friendship").MapLeftKey("FriendId").MapRightKey("FriendWithId"));
           /*
		    modelBuilder.Entity<Friendship>()
				.HasOne( pt => pt.Friend)
				.WithMany(ptt => ptt.Friends)
				.HasForeignKey(pt => pt.FriendWithId);

			modelBuilder.Entity<Friendship>()
				.HasOne( pt => pt.FriendWith)
				.WithMany( t => t.Friends)
				.HasForeignKey( pt => pt.FriendId);
				*/

			base.OnModelCreating(modelBuilder);
        }
    }
}