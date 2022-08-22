using Faith.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Faith.Server.Data
{
    public class FaithContext : DbContext
    {
        public DbSet<User> FUsers { get; set; }
        public DbSet<TimeLine> FTimeLines { get; set; }
        public DbSet<Post> FPosts { get; set; }
        public DbSet<Reaction> FReactions { get; set; }

        public FaithContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Faith.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            const string DateFormat = "dd-MM-yyyy";

            builder.Entity<User>()
                .HasKey(u => u.UserID);
            builder.Entity<User>()
                .HasMany(m => m.Youngsters)
                .WithOne();
            builder.Entity<User>()
                .HasOne(y => y.TimeLine)
                .WithOne()
                .HasForeignKey<TimeLine>("UserID").IsRequired();

            builder.Entity<TimeLine>()
                .HasKey(t => t.TimeLineID);
            builder.Entity<TimeLine>()
                .HasMany(t => t.Posts)
                .WithOne();

            builder.Entity<Post>()
                .HasKey(p => p.PostID);
            builder.Entity<Post>()
                .HasMany(p => p.Reactions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Post>().Property(e => e.Date)
                .HasConversion(
                    dateValue => dateValue.ToString(DateFormat, CultureInfo.InvariantCulture),
                    stringValue => DateTime.ParseExact(stringValue, DateFormat, CultureInfo.InvariantCulture)
                )
                .IsRequired()
                .IsUnicode(false) // arbitrary
                .HasMaxLength(10); // arbitrary

            builder.Entity<Reaction>()
                .HasKey(r => r.ReactionID);

            base.OnModelCreating(builder);
        }
    }
}
