using Microsoft.EntityFrameworkCore;
using MinimalAPICatalogo.Models;

namespace MinimalAPICatalogo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configurando Pk e outros valores da tabela Categoria
            modelBuilder.Entity<Categoria>()
                .HasKey(x => x.CategoriaId);
            modelBuilder.Entity<Categoria>()
                .Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<Categoria>()
                .Property(x => x.Descricao)
                .HasMaxLength(150)
                .IsRequired();

            //Tabela Produtos
            modelBuilder.Entity<Produto>()
                .HasKey(x => x.ProdutoId);
            modelBuilder.Entity<Produto>()
                .Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<Produto>()
                .Property(x => x.Descricao)
                .HasMaxLength(150);
            modelBuilder.Entity<Produto>()
                .Property(x => x.Imagem)
                .HasMaxLength(100);

            //Precisão da prop Preco
            modelBuilder.Entity<Produto>()
                .Property(x => x.Preco)
                .HasPrecision(14, 2);

            //Configuração do Relacionamento
            modelBuilder.Entity<Produto>()
                .HasOne<Categoria>(x => x.Categoria)
                .WithMany(x => x.Produtos)
                .HasForeignKey(x => x.CategoriaId);
        }
    }
}
