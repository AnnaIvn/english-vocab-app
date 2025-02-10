using EnglishVocabApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using EnglishVocabApp.Models;

namespace EnglishVocabApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>     // added User 
    {
        public DbSet<User> Users {  get; set; }
        //public DbSet<Type> Types { get; set; }
        //public DbSet<Word> Words { get; set; }
        //public DbSet<Folder> Folders { get; set; }
        //public DbSet<FoldersUsers> FoldersUsers { get; set; }
        //public DbSet<WordsFolders> WordsFolders { get; set; }

        // write all dbsets here

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // write all one-to-many relationships here
            Builder.Entity<Word>()
                .HasOne(w => w.Type)
                .WithMany(t => t.Words)
                .HasForeignKey(w => w.TypeId);

            // write all many-to-many relationships here
            Builder.Entity<WordsFolders>()
                .HasKey(wf => new { wf.WordId, wf.FolderId });

            Builder.Entity<WordsFolders>()
                .HasOne(wf => wf.Word)
                .WithMany(w => w.WordsFolders);

            Builder.Entity<WordsFolders>()
                .HasOne(wf => wf.Folder)
                .WithMany(f => f.WordsFolders);
        }
    }
}
