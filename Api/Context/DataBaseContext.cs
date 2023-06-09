using Api.Entities;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Api.Context
{
    public class DataBaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<FavNote> FavNotes { get; set; }
        public DbSet<SharedNote> SharedNotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost;port=3306;database=proyecto;user=root;password=";
            var connection = new MySqlConnection(connectionString);

            optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connectionString));
        }
    }
}
