using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill{Name = "Fireball", Damage = 50, SkillId = 1},
                new Skill{Name = "Lightning Strike", Damage = 40, SkillId = 2},
                new Skill{Name = "Boulder Throw", Damage = 30, SkillId = 3}
            );
        }
    }
}