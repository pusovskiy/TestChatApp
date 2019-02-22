using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TestChatApp.Models
{
    public class UserContext : DbContext
    {
        public UserContext() :
            base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserContext, Migrations.Configuration>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FriendConnection> FriendConnections { get; set; }
        public DbSet<Dialogue> Dialogues { get; set; }  
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}