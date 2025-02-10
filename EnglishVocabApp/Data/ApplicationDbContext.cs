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

        // write all dbsets here

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // write all one-to-many relationships here

            // write all many-to-many relationships here
        }
    }
}
