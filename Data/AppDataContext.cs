using Microsoft.EntityFrameworkCore;
using MM.Models;

namespace MM.Data;

class AppDataContext : DbContext {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Data/Database.db");
    }

    public DbSet<Clientes> Clientes {get; set;}
    public DbSet<Produtos> Produtos {get; set;}
    public DbSet<Vendas> Vendas {get; set;}
}